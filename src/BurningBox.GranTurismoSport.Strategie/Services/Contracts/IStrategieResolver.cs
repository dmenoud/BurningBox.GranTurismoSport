using BurningBox.GranTurismoSport.Strategie.BusinessModels;
using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;
using JetBrains.Annotations;

namespace BurningBox.GranTurismoSport.Strategie.Services.Contracts
{
    public interface IStrategieResolver
    {
        IStrategieResult Resolve([NotNull]IRaceDefinition raceDefinition);
    }
}