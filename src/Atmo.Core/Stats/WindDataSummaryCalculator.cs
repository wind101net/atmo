// ================================================================================
//
// Atmo 2
// Copyright (C) 2011  BARANI DESIGN
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// 
// Contact: Jan Barani mailto:jan@baranidesign.com
//
// ================================================================================

using System;
using System.Collections.Generic;
using Atmo.Units;

namespace Atmo.Stats {
	public class WindDataSummaryCalculator<T> where T:IReadingsSummary {

		private double _beta;
		private double _theta;
		private readonly double _speedStep;
		private readonly double _speedHalfStep;
		//private readonly List<WindSpeedFrequency> _speedFrequency;
		private bool _weibullCalcNeeded;
		//private readonly List<WindDirectionEnergy> _directionEnergy;
		//private double _minSpeed;
		//private double _maxSpeed;
		private readonly int _maxAlgorithmIterations;
		private readonly double _angleStep;
		private readonly double _angleHalfStep;

		private readonly Dictionary<double, WindSpeedFrequency> _speedLookup;
		private readonly Dictionary<double, WindDirectionEnergy> _directionLookup;

		public WindDataSummaryCalculator() {
			_weibullCalcNeeded = true;
			_beta = 0;
			_theta = 0;
			_speedStep = 0.5;
			_speedHalfStep = _speedStep/2;
			_maxAlgorithmIterations = 64;
			_angleStep = 22.5 / 2.0;
			_angleHalfStep = _angleStep/2;

			_speedLookup = new Dictionary<double, WindSpeedFrequency>();

			int directionCounts = (int) (360.0/_angleStep);
			_directionLookup = new Dictionary<double, WindDirectionEnergy>(directionCounts);
			for(int i = 0; i < directionCounts; i++) {
				var dir = i*_angleStep;
				_directionLookup.Add(dir, new WindDirectionEnergy(dir, 0, 0));
			}

		}

		public void Process(T readings) {

			// wind dir stuff
			if(null != readings.Mean) {
				var speed = readings.Mean.WindSpeed;
				if (!Double.IsNaN(speed)) {
					var energy = speed*speed;
					var power = energy*speed;
					foreach (var set in readings.GetWindDirectionCounts()) {
						if (0 == set.Value || Double.IsNaN(set.Key)) {
							continue;
						}
						var dirSlot = UnitUtility.WrapDegree(((int) ((set.Key + _angleHalfStep)/_angleStep))*_angleStep);
						WindDirectionEnergy windDirEnrg;
						var modEnergy = power*set.Value;
						if (_directionLookup.TryGetValue(dirSlot, out windDirEnrg)) {
							windDirEnrg.Frequency += set.Value;
							windDirEnrg.Power += modEnergy;
						}
					}
				}
			}

			// wind speed stuff
			foreach(var set in readings.GetWindSpeedCounts()) {
				if (0 == set.Value || Double.IsNaN(set.Key)) {
					continue;
				}
				var speedBucket = ((int)((set.Key + _speedHalfStep) / _speedStep)) * _speedStep;
				WindSpeedFrequency windSpeedFreq;
				if(_speedLookup.TryGetValue(speedBucket, out windSpeedFreq)) {
					windSpeedFreq.Frequency += set.Value;
				}else {
					windSpeedFreq = new WindSpeedFrequency(speedBucket, set.Value);
					_speedLookup.Add(speedBucket, windSpeedFreq);
				}
				_weibullCalcNeeded = true;
			}
		}

		public List<WindDirectionEnergy> WindDirectionEnergyData {
			get {
				double energySum = 0;
				double frequencySum = 0;
				foreach(var record in _directionLookup.Values) {
					energySum += record.Power;
					frequencySum += record.Frequency;
				}
				var result = new List<WindDirectionEnergy>(_directionLookup.Count);
				foreach (var record in _directionLookup.Values) {
					result.Add(new WindDirectionEnergy(record.Direction, record.Frequency / frequencySum, record.Power / energySum));
				}
				return result;
			}
		}

		public List<WindSpeedFrequency> WindSpeedFrequencyData {
			get {
				FinalizeWeibullIfNeeded();
				return new List<WindSpeedFrequency>(_speedLookup.Values);
			}
		}

		public double Beta {
			get {
				FinalizeWeibullIfNeeded();
				return _beta;
			}
		}

		public double Theta {
			get {
				FinalizeWeibullIfNeeded();
				return _theta;
			}
		}

		private void FinalizeWeibullIfNeeded() {
			if(_weibullCalcNeeded) {
				FinalizeWeibull();
			}
		}

		private void FinalizeWeibull() {
			_beta = 2.0;
			_theta = 0;
			var frequencyTotal = 0.0;
			foreach(var reading in _speedLookup.Values) {
				frequencyTotal += reading.Frequency;
			}
			for (int algorithmIterations = 0; algorithmIterations < _maxAlgorithmIterations; algorithmIterations++) {
				_beta = WeibullFitBetterBeta(frequencyTotal, _beta);
				;
			}
			foreach(var reading in _speedLookup.Values) {
				const double delta = 0.0;
				var speed = reading.Speed;
				if (speed <= 0) {
					speed = 0.001;
				}
				speed -= delta;
				var occurrences = reading.Frequency;
				var speedToTheBeta = Math.Pow(speed, _beta);
				double localTheta = (speedToTheBeta/frequencyTotal)*occurrences;
				_theta += localTheta;
				//_theta += (Math.Pow(reading.Speed, _beta) / frequencyTotal) * reading.Frequency;
			}
			_theta = Math.Pow(_theta, 1.0 / _beta);
			var thetaToTheBeta = Math.Pow(_theta, _beta);
			double weibullSum = 0;
			foreach(var reading in _speedLookup.Values) {
				const double delta = 0.0;
				var speed = reading.Speed;
				if (speed <= 0) {
					speed = 0.001;
				}
				speed -= delta;

				reading.Weibull =
					(_beta * Math.Pow(speed, _beta - 1.0))
					/
					(thetaToTheBeta * Math.Pow(Math.E, -Math.Pow(speed / _theta, _beta)))
				;
				weibullSum += reading.Weibull;
			}

			var correction = 1.0 / weibullSum;
			var correctedSum = correction*frequencyTotal;
			foreach (var reading in _speedLookup.Values) {
				reading.Weibull *= correctedSum;
			}
			_weibullCalcNeeded = false;
		}

		private double WeibullFitBetterBeta(double sum, double beta) {

			double betaMinusOne = beta - 1;
			double delta = 0;

			double partCSum = 0;
			double partDSum = 0;
			double partESum = 0;
			double thetaSum = 0;

			foreach (var reading in _speedLookup.Values) {
				var speed = reading.Speed;
				if(speed <= 0) {
					speed = 0.001;
				}
				speed -= delta;
				var inverseSpeed = 1.0/speed;
				var occurrences = reading.Frequency;
				var occurrenceBeta = occurrences*beta;
				var logSpeed = Math.Log(speed);
				var occurrenceRatio = occurrences / sum;
				var speedToTheBeta = Math.Pow(speed, beta);

				var partC = speedToTheBeta * logSpeed * occurrences;
				var partD = speedToTheBeta * occurrences;
				var partE = (logSpeed / sum) * occurrences;

				var thetaCalc = speedToTheBeta*occurrenceRatio;

				partCSum += partC;
				partDSum += partD;
				partESum += partE;
				thetaSum += thetaCalc;

			}

			var betaDelta = partESum - ((partCSum / partDSum) - (1.0 / beta));
			var newBeta = beta + betaDelta;
			return newBeta;

			
			/*double cSum = 0;
			double dSum = 0;
			double eSum = 0;
			foreach (var reading in _speedLookup.Values) {

				var speedLog = 0 == reading.Speed ? 0.001 : Math.Log(reading.Speed);
				var speedToTheBeta = Math.Pow(reading.Speed, beta);

				var d = speedToTheBeta * reading.Frequency;
				
				if (!Double.IsNaN(d)) {
					dSum += d;
					var c = d * speedLog;
					if (!Double.IsNaN(c)) {
						cSum += c;
					}
				}

				var e = (speedLog / sum) * reading.Frequency;
				if (!Double.IsNaN(e) && !Double.IsNegativeInfinity(e) && !Double.IsPositiveInfinity(e)) {
					eSum += e;
				}
			}
			var betaDelta = eSum - (cSum / dSum) + (1.0 / beta);
			return beta + betaDelta;*/

		}



	}
}
