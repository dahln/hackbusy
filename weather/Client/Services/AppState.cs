using Blazored.LocalStorage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace weather.Client.Services
{
    //https://chrissainty.com/3-ways-to-communicate-between-components-in-blazor/
    public class AppState
    {
        public event Action OnChange;

        private API _api;
        private ILocalStorageService _localStorage;

        public AppState(API api, ILocalStorageService localStorage)
        {
            _api = api;
            _localStorage = localStorage;
        }

        public bool UpdateAppState()
        {
            return true;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

    }
}
