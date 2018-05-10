using BurningBox.GranTurismoSport.Strategie.BusinessModels;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.Services.Contracts
{
    public interface IStrategieResolver
    {
        StrategieResult Resolve(IRaceDefinition raceDefinition);
    }
}