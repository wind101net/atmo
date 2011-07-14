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
			_speedStep = 1.0;
			_speedHalfStep = _speedStep/2;
			//_speedFrequency = new List<WindSpeedFrequency>();
			//_directionEnergy = new List<WindDirectionEnergy>();
			_speedLookup = new Dictionary<double, WindSpeedFrequency>();
			_directionLookup = new Dictionary<double, WindDirectionEnergy>();
			//_validSpeeds = new List<double>();
			//_minSpeed = _maxSpeed = Double.NaN;
			_maxAlgorithmIterations = 16;
			_angleStep = 22.5;
			_angleHalfStep = _angleStep/2;
		}

		public void Process(T readings) {

			// wind dir stuff
			if(null != readings.Mean && !Double.IsNaN(readings.Mean.WindSpeed)) {
				var speed = readings.Mean.WindSpeed;
				var power = speed*speed;
				var energy = power*speed;
				foreach (var set in readings.GetWindDirectionCounts()) {
					if (Double.IsNaN(set.Key)) {
						continue;
					}
					var dirSlot = UnitUtility.WrapDegree(((int)((set.Key + _angleHalfStep) / _angleStep)) * _angleStep);
					WindDirectionEnergy windDirEnrg;
					if(_directionLookup.TryGetValue(dirSlot, out windDirEnrg)) {
						windDirEnrg.Frequency += set.Value;
						windDirEnrg.Energy += energy;
					}else {
						windDirEnrg = new WindDirectionEnergy(dirSlot, set.Value, energy);
						_directionLookup.Add(dirSlot, windDirEnrg);
					}
				}

			}

			// wind speed stuff
			foreach(var set in readings.GetWindSpeedCounts()) {
				if (Double.IsNaN(set.Key)) {
					continue;
				}
				var speedBucket = ((int)((set.Key + _speedHalfStep) / _speedStep)) * _speedStep;
				WindSpeedFrequency windSpeedFreq;
				if(_speedLookup.TryGetValue(speedBucket, out windSpeedFreq)) {
					windSpeedFreq.Frequency += set.Value;
				}else {
					windSpeedFreq = new WindSpeedFrequency(speedBucket, set.Value, 0);
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
					energySum += record.Energy;
					frequencySum += record.Frequency;
				}
				var result = new List<WindDirectionEnergy>(_directionLookup.Count);
				foreach (var record in _directionLookup.Values) {
					result.Add(new WindDirectionEnergy(record.Direction, record.Frequency / frequencySum, record.Energy / energySum));
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
			//var delta = 0.0;
			var sum = 0.0;
			//var rawSum = 0.0;
			_beta = 2.0;
			_theta = 0;
			foreach(var reading in _speedLookup.Values) {
				sum += reading.Frequency;
			}
			for (int algorithmIterations = 0; algorithmIterations < _maxAlgorithmIterations; algorithmIterations++) {
				_beta = WeibullFitBetterBeta(sum, _beta);
			}
			foreach(var reading in _speedLookup.Values){
				_theta += (Math.Pow(reading.Speed, _beta) / sum) * reading.Frequency;
			}
			_theta = Math.Pow(_theta, 1.0 / _beta);
			var thetaToTheBeta = Math.Pow(_theta, _beta);
			foreach(var reading in _speedLookup.Values) {
				var speed = reading.Speed;
				reading.Weibull =
					_beta
					* (Math.Pow(speed, _beta - 1.0) / thetaToTheBeta)
					* Math.Pow(Math.E, -Math.Pow(speed / _theta, _beta))
				;
			}
			double weibullSum = 0;
			foreach (var reading in _speedLookup.Values) {
				weibullSum += reading.Weibull;
			}
			var correction = 1.0 / weibullSum;
			var correctedSum = correction*sum;
			foreach (var reading in _speedLookup.Values) {
				reading.Weibull *= correctedSum;
			}
			_weibullCalcNeeded = false;
		}

		private double WeibullFitBetterBeta(double sum, double beta) {
			double cSum = 0;
			double dSum = 0;
			double eSum = 0;
			foreach (var reading in _speedLookup.Values) {

				var speedLog = Math.Log(reading.Speed);
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
			return beta + betaDelta;
		}



	}
}
