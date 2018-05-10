using System;
using System.Runtime.InteropServices;
using BurningBox.GranTurismoSport.Strategie.BusinessModels;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;
using BurningBox.GranTurismoSport.Strategie.Services.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.Services
{
    public class StrategieResolver : IStrategieResolver
    {
        public StrategieResult Resolve(IRaceDefinition raceDefinition)
        {
           
            var result = new StrategieResult();





            return result;

        }
    }
}