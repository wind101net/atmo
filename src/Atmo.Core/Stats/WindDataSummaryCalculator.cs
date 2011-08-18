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
			IgnoreZeroValuesForWeibullCalculation = false;
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

		public bool IgnoreZeroValuesForWeibullCalculation { get; set; }

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

		private const double ZeroReplace = 0.001;

		private void FinalizeWeibull() {
			_beta = 2.0;
			_theta = 0;

			// get a quick frequency sum
			var frequencyTotal = 0.0;
			foreach(var reading in _speedLookup.Values) {
				if (IgnoreZeroValuesForWeibullCalculation && reading.Speed == 0) {
					continue;
				}
				frequencyTotal += reading.Frequency;
			}

			double betaMin = 0.0;
			double betaMax = 2;
			// find the max bound (betaMax must be > 1 to start)
			for (int algorithmIterations = 0; algorithmIterations < _maxAlgorithmIterations; algorithmIterations++) {
				double betaDelta = CalculateWeibullFitBetterBetaDelta(frequencyTotal, betaMax);
				if (betaDelta > 0 && !Double.IsInfinity(betaDelta) && !Double.IsNaN(betaDelta)) {
					betaMax *= betaMax;
				}else {
					break;
				}
			}

			// now search between min and max for the best value
			double betaCurrent = Double.NaN;
			for (int algorithmIterations = 0; algorithmIterations < _maxAlgorithmIterations; algorithmIterations++) {
				betaCurrent = (betaMax + betaMin) / 2.0;
				double betaDelta = CalculateWeibullFitBetterBetaDelta(frequencyTotal, betaCurrent);
				if(betaDelta < 0) {
					betaMax = betaCurrent;
				}else if(betaDelta > 0) {
					betaMin = betaCurrent;
				}else {
					break;
				}
			}

			_beta = betaCurrent;

			// calc theta
			foreach(var reading in _speedLookup.Values) {
				var speed = reading.Speed;
				if (speed <= 0) {
					if (IgnoreZeroValuesForWeibullCalculation) {
						continue;
					}
					speed = ZeroReplace;
				}
				_theta += (Math.Pow(speed, _beta) / frequencyTotal) * reading.Frequency;
			}
			_theta = Math.Pow(_theta, 1.0 / _beta);

			// find probability density
			var betaMinusOne = _beta - 1.0;
			var thetaToTheBeta = Math.Pow(_theta, _beta);
			var weibullSum = 0.0;
			foreach (var reading in _speedLookup.Values) {
				var speed = reading.Speed;
				if (speed <= 0) {
					speed = ZeroReplace;
				}
				var density = ((_beta * Math.Pow(speed, betaMinusOne)) / thetaToTheBeta) * Math.Exp(-Math.Pow((speed / _theta), _beta));
				reading.Weibull = density;
				weibullSum += density;

			}

			// to density percentages
			if (0 != weibullSum) {
				foreach (var reading in _speedLookup.Values) {
					reading.Weibull /= weibullSum;
				}
			}
			_weibullCalcNeeded = false;
		}

		private double CalculateWeibullFitBetterBetaDelta(double sum, double beta) {
			double partCSum = 0;
			double partDSum = 0;
			double partESum = 0;

			foreach (var reading in _speedLookup.Values) {
				var speed = reading.Speed;
				if (speed <= 0) {
					if (IgnoreZeroValuesForWeibullCalculation) {
						continue;
					}
					speed = ZeroReplace;
				}

				var logSpeed = Math.Log(speed);
				var speedToTheBetaOccurrences = Math.Pow(speed, beta) * reading.Frequency;
				partCSum += speedToTheBetaOccurrences * logSpeed;
				partDSum += speedToTheBetaOccurrences;
				partESum += logSpeed * (reading.Frequency / sum);

			}

			var betaDelta = partESum - ((partCSum / partDSum) - (1.0 / beta));
			return betaDelta;
		}

		[Obsolete]
		private double WeibullFitBetterBeta(double sum, double beta) {
			return CalculateWeibullFitBetterBetaDelta(sum, beta)
				+ beta;
		}

		public double CalculateWeibullAverage() {

			const double a = 4.4077244121442000;
			const double b = -9.6053431539960600;
			const double c = 11.6090395731482000;
			const double d = -8.0669467702440900;
			const double f = 3.3392518484921100;
			const double g = -0.7573045379983490;
			const double h = 0.0735641628942245;

			double betaInv = 1.0 + (1.0 / Beta);
			double betaInv2 = betaInv * betaInv;
			double betaInv3 = betaInv2 * betaInv;
			double betaInv4 = betaInv2 * betaInv2;
			double betaInv5 = betaInv3 * betaInv2;
			double betaInv6 = betaInv3 * betaInv3;
			double gamma = a
						   + (b * betaInv)
						   + (c * betaInv2)
						   + (d * betaInv3)
						   + (f * betaInv4)
						   + (g * betaInv5)
						   + (h * betaInv6);

			return Theta * gamma;
		}



	}
}
