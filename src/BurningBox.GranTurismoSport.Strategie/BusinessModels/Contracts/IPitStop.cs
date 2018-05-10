namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IPitStop
    {
        int LapNumber { get; set; }
        int OptimistLapNumbers { get; set; }
        bool ChangeTires { get; set; }
        bool Refuel { get; set; }
        TireType TireType { get; set; }
    }
}