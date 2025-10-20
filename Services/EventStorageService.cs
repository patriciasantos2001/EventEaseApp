using Blazored.LocalStorage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventEase.Services
{
    public class EventStorageService
    {
        private readonly ILocalStorageService _localStorage;
        private const string StoragePrefix = "events_";

        public EventStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        private static string GetKey(string username) => $"{StoragePrefix}{username}";

        public async Task<List<Event>> GetEventsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new();

            var events = await _localStorage.GetItemAsync<List<Event>>(GetKey(username));
            return events ?? new();
        }

        public async Task SaveEventAsync(string username, Event newEvent)
        {
            if (string.IsNullOrWhiteSpace(username))
                return;

            var events = await GetEventsAsync(username);

            if (newEvent.Id == 0)
                newEvent.Id = events.Count > 0 ? events[^1].Id + 1 : 1;

            newEvent.CreatedBy = username;
            events.Add(newEvent);

            await _localStorage.SetItemAsync(GetKey(username), events);
        }

        public async Task DeleteEventAsync(string username, int id)
        {
            var events = await GetEventsAsync(username);
            var eventToRemove = events.Find(e => e.Id == id);
            if (eventToRemove != null)
            {
                events.Remove(eventToRemove);
                await _localStorage.SetItemAsync(GetKey(username), events);
            }
        }

        public async Task ClearAllAsync(string username)
        {
            await _localStorage.RemoveItemAsync(GetKey(username));
        }

         public async Task<List<Event>> GetAllEventsAsync()
        {
            var allEvents = new List<Event>();
            var keys = await _localStorage.KeysAsync(); // requires Blazored.LocalStorage v4+

            foreach (var key in keys)
            {
                if (key.StartsWith(StoragePrefix))
                {
                    var events = await _localStorage.GetItemAsync<List<Event>>(key);
                    if (events != null)
                        allEvents.AddRange(events);
                }
            }

            // Optional: sort newest first
            return allEvents.OrderByDescending(e => e.Date).ToList();
        }
    }
}
