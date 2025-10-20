using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace EventEase.Services
{
    public class UserSessionService
    {
        private readonly ILocalStorageService _localStorage;
        private const string StorageKey = "user_session";

        public event Action? OnSessionChanged;

        public bool IsLoggedIn => _currentUser is not null;
        public UserSession? CurrentUser => _currentUser;

        private UserSession? _currentUser;

        public UserSessionService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        /// <summary>
        /// Initialize the session from local storage when the app loads.
        /// </summary>
        public async Task InitializeAsync()
        {
            _currentUser = await _localStorage.GetItemAsync<UserSession>(StorageKey);
        }

        /// <summary>
        /// Log in a user and persist their session.
        /// </summary>
        public async Task LoginAsync(string username)
        {
            _currentUser = new UserSession
            {
                Username = username,
                LoginTime = DateTime.UtcNow
            };

            await _localStorage.SetItemAsync(StorageKey, _currentUser);
            OnSessionChanged?.Invoke();
        }

        /// <summary>
        /// Log out the user and clear the session.
        /// </summary>
        public async Task LogoutAsync()
        {
            _currentUser = null;
            await _localStorage.RemoveItemAsync(StorageKey);
            OnSessionChanged?.Invoke();
        }
    }

    public class UserSession
    {
        public string Username { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
    }
}
