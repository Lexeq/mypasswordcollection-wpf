using PasswordStorage;
using System.Collections.Generic;

namespace MPC
{

    class SearchHelper
    {
        private IList<PasswordItem> _collection;

        private string _searchString;

        private int _currentIndex;

        private bool _firstPass;

        public bool AutoReset { get; set; }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value.ToLower();
                Reset();
            }
        }

        public SearchHelper(IList<PasswordItem> collection)
        {
            _collection = collection;
            Reset();
        }

        public int FindNext()
        {
            if (_firstPass)
            {
                while (_currentIndex < _collection.Count)
                {
                    if (_collection[_currentIndex].Site.ToLower().StartsWith(_searchString))
                    {
                        _currentIndex++;
                        return _currentIndex - 1;
                    }
                    _currentIndex++;
                }
                _firstPass = false;
                _currentIndex = 0;
            }
            if (!_firstPass)
            {
                while (_currentIndex < _collection.Count)
                {
                    var siteLower = _collection[_currentIndex].Site.ToLower();
                    if (siteLower.Contains(_searchString) && !siteLower.StartsWith(_searchString))
                    {
                        _currentIndex++;
                        return _currentIndex - 1;
                    }
                    _currentIndex++;
                }
            }
           if(AutoReset)
                Reset();
            return -1;
        }

        public void Reset()
        {
            _currentIndex = 0;
            _firstPass = true;
        }
    }

}
