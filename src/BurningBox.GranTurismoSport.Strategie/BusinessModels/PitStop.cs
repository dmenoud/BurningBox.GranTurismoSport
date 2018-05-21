using BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts;

namespace BurningBox.GranTurismoSport.Strategie.BusinessModels
{
    public class PitStop : IPitStop
    {
        public int LapNumber { get; set; }
        public bool ChangeTires { get; set; }
        public int Refuel { get; set; }
        public TiresType TiresType { get; set; }
        public double FuelState { get; set; }
        public double TiresState { get; set; }
    }
}