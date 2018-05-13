using System;
using System.Collections.Generic;
using System.Linq;
using BurningBox.GranTurismoSport.Strategie.BusinessModels;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;
using BurningBox.GranTurismoSport.Strategie.Services.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.Services
{
    public class StrategieResolver : IStrategieResolver
    {
        public IStrategieResult Resolve(IRaceDefinition raceDefinition)
        {
            var tireTypes = raceDefinition.TiresDefinitions.Select(t => t.TiresType).ToList();
            var numberOfTiresType = tireTypes.Distinct().Count();
            if (numberOfTiresType != raceDefinition.TiresDefinitions.Count)
            {
                throw new InvalidOperationException("TiresDefinions is invalid, TiresType must be distinct");
            }


            GetTiresSequence(raceDefinition.TiresDefinitions, 3);

            var strategieResults = new List<StrategieResult>();
            var fuelConsumptionPerLap = 100 / raceDefinition.NumberOfLapsWithFullFuel;

            foreach (var startTiresType in tireTypes)
            {
                foreach (PitStrategie pitStrategie in Enum.GetValues(typeof(PitStrategie)))
                {
                    var strategie = new StrategieResult
                                    {
                                        StartTiresType = startTiresType,
                                        PitStops = new List<IPitStop>(),
                                        RaceTime = TimeSpan.Zero
                                    };
                    strategieResults.Add(strategie);
                    RunLaps(raceDefinition, startTiresType, fuelConsumptionPerLap, pitStrategie, strategie);
                }

                // run
            }

            StrategieResult result = null;
            switch(raceDefinition.RaceMode)
            {
                case RaceMode.Race:
                    result = strategieResults.OrderBy(r => r.RaceTime).First();
                    break;
                case RaceMode.Endurance:
                    result = strategieResults.OrderBy(r => r.PitStops.Count).First();
                    break;
            }

            return result;
        }

        private void RunLaps(IRaceDefinition raceDefinition, TiresType startTiresType, double fuelConsumptionPerLap, PitStrategie pitStrategie, StrategieResult strategie)
        {
            var startTireDefinition = raceDefinition.TiresDefinitions.First(t => t.TiresType == startTiresType);

            var currentFuelRate = 100.0;
            var currentTiresRate = 100.0;
            var currentLapNumber = 0;
            var tireConsumptionPerLap = 100.0 / startTireDefinition.OptimalNumberOfLaps;
            var needRefuel = false;
            var needChangeTires = false;

            do
            {
                currentLapNumber++;
                currentFuelRate -= fuelConsumptionPerLap;
                currentTiresRate -= tireConsumptionPerLap;

                if (currentFuelRate <= 20)
                {
                    needRefuel = true;
                }

                if (currentTiresRate <= 20)
                {
                    needChangeTires = true;
                }

                if (needChangeTires || needRefuel)
                {
                    switch(pitStrategie)
                    {
                        case PitStrategie.OnlyNeeded:
                            break;
                        case PitStrategie.FuelEachTime:
                            needRefuel = true;
                            break;
                        case PitStrategie.TiresEachTime:
                            needChangeTires = true;
                            break;
                        case PitStrategie.FuelAndTiresEachTime:
                            needRefuel = true;
                            needChangeTires = true;
                            break;
                    }


                    strategie.PitStops.Add(new PitStop
                                           {
                                               Refuel = needRefuel,
                                               ChangeTires = needChangeTires,
                                               LapNumber = currentLapNumber,
                                               TiresType = startTireDefinition.TiresType,
                                           });

                    if (needChangeTires)
                    {
                        strategie.RaceTime = strategie.RaceTime.Add(raceDefinition.TiresChangeDuration);
                        needChangeTires = false;
                        currentTiresRate = 100.0;
                    }

                    if (needRefuel)
                    {
                        strategie.RaceTime = strategie.RaceTime.Add(GetFuelFillingTime(raceDefinition, currentFuelRate));
                        needRefuel = false;
                        currentFuelRate = 100;
                    }

                    strategie.RaceTime = strategie.RaceTime.Add(raceDefinition.TimeLostForPitStop);
                }

                strategie.RaceTime = strategie.RaceTime.Add(startTireDefinition.AverageLapTime);
            } while (!IsDone(raceDefinition, strategie, currentLapNumber));
        }

        private bool IsDone(IRaceDefinition raceDefinition, StrategieResult strategie, int currentLapNumber)
        {
            var done = false;
            switch(raceDefinition.RaceMode)
            {
                case RaceMode.Race:
                    if (currentLapNumber == raceDefinition.NumberOfLaps)
                    {
                        done = true;
                    }

                    break;
                case RaceMode.Endurance:
                    if (strategie.RaceTime >= raceDefinition.RaceDuration)
                    {
                        done = true;
                    }

                    break;
            }

            return done;
        }

        private TimeSpan GetFuelFillingTime(IRaceDefinition raceDefinition, double currentFuelRate)
        {
            // [s/%]
            var fillingSpeed = raceDefinition.FuelFillingDuration.TotalSeconds / raceDefinition.FuelToFillInPercent;

            return TimeSpan.FromSeconds((100 - currentFuelRate) * fillingSpeed);
        }


        private void GetTiresSequence(List<ITiresDefinition> tiresDefinitions, int sequenceSize)
        {
            var initialTires = tiresDefinitions.Select(t => t.TiresType).OrderBy(t => t).ToList();

            var tiresTypes = new Dictionary<int, TiresType>();

            var i = 0;
            foreach (var tiresType in initialTires)
            {
                tiresTypes.Add(i++, tiresType);
            }

            var sequences = new List<int[]>
                            {
                                new int[sequenceSize]
                            };

            var last = sequences[0];

            var indexValues = new Dictionary<int, int>();
            for (int j = 0; j < sequenceSize; j++)
            {
                indexValues.Add(j, 0);
            }

            do
            {
            } while (Increments(indexValues, sequenceSize, initialTires.Count-1, sequences, ref last));
        }

        private bool Increments(Dictionary<int, int> indexValues, int sequenceSize, int maxValue, List<int[]> sequences, ref int[] last)
        {
            
            for (var i = 0; i < sequenceSize; i++)
            {
                if (last[i]<maxValue)
                {
                    indexValues[i]++;
                    break;
                }
            }

            var nbFull = 0;
            last = new int[sequenceSize];
            sequences.Add(last);
            for (var i = 0; i < sequenceSize; i++)
            {
                last[i] = indexValues[i];
                if (last[i] == maxValue)
                {

                    nbFull++;
                }
            }

            if (nbFull == sequenceSize)
            {
                return false;
            }
            
            return true;
        }
    }
}