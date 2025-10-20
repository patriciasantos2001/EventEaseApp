using Blazored.LocalStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Services
{
    public class AttendanceService
    {
        private readonly ILocalStorageService _localStorage;
        private const string StorageKey = "attendances";

        public AttendanceService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        private async Task<List<Attendance>> GetAllAsync()
        {
            var attendances = await _localStorage.GetItemAsync<List<Attendance>>(StorageKey);
            return attendances ?? new List<Attendance>();
        }

        public async Task RegisterAttendanceAsync(int eventId, string username)
        {
            var attendances = await GetAllAsync();

            // Avoid duplicate attendance
            if (!attendances.Any(a => a.EventId == eventId && a.Username == username))
            {
                attendances.Add(new Attendance
                {
                    EventId = eventId,
                    Username = username
                });

                await _localStorage.SetItemAsync(StorageKey, attendances);
            }
        }

        public async Task<List<Attendance>> GetEventAttendancesAsync(int eventId)
        {
            var attendances = await GetAllAsync();
            return attendances.Where(a => a.EventId == eventId).ToList();
        }

        public async Task<List<Attendance>> GetUserAttendancesAsync(string username)
        {
            var attendances = await GetAllAsync();
            return attendances.Where(a => a.Username == username).ToList();
        }

        public async Task<bool> IsUserAttendingAsync(int eventId, string username)
        {
            var attendances = await GetAllAsync();
            return attendances.Any(a => a.EventId == eventId && a.Username == username);
        }
    }
}
