using System.Collections.Generic;
using BurningBox.GranTurismoSport.Strategie.BusinessModels;

namespace BurningBox.GranTurismoSport.Strategie.Services
{
    public class TiresProvider
    {
        private readonly List<TiresType> _tiresTypes;
        private int _currentIndex;


        public TiresProvider(List<TiresType> tiresTypes)
        {
            _tiresTypes = tiresTypes;
        }

        public TiresType GetNext()
        {
            var result = _tiresTypes[_currentIndex];
            _currentIndex++;
            if (_currentIndex == _tiresTypes.Count)
            {
                _currentIndex = 0;
            }

            return result;
        }
    }
}