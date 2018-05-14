namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IPitStop
    {
        int LapNumber { get; set; }
        bool ChangeTires { get; set; }
        bool Refuel { get; set; }
        TiresType TiresType { get; set; }
        double FuelState { get; set; }
        double TiresState { get; set; }
    }
}