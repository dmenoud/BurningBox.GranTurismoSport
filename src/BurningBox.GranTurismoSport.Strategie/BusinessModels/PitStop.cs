using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class PitStop : IPitStop
    {
        public int LapsNumber { get; set; }
        public bool ChangeTires { get; set; }
        public bool Refuel { get; set; }
        public TireType TireType { get; set; }
    }
}