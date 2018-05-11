﻿namespace BurningBox.GranTurismoSport.Strategie.BusinessModels.Contracts
{
    public interface IPitStop
    {
        int LapsNumber { get; set; }
        bool ChangeTires { get; set; }
        bool Refuel { get; set; }
        TireType TireType { get; set; }
    }
}