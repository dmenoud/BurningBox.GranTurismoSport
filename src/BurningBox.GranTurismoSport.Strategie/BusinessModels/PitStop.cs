using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class PitStop : IPitStop
    {
        public int LapNumber { get; set; }
        public bool ChangeTires { get; set; }
        public bool Refuel { get; set; }
        public TiresType TiresType { get; set; }
    }
}