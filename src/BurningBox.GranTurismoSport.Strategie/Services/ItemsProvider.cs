using System.Collections.Generic;
using BurningBox.GranTurismoSport.Strategie.BusinessModels;

namespace BurningBox.GranTurismoSport.Strategie.Services
{
    public class ItemsProvider<TItem>
    {
        private readonly List<TItem> _tiresTypes;
        private int _currentIndex;


        public ItemsProvider(List<TItem> tiresTypes)
        {
            _tiresTypes = tiresTypes;
        }

        public TItem GetNext()
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