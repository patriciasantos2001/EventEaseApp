using System.ComponentModel.DataAnnotations;

namespace EventEase
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, ErrorMessage = "Event name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location can't be longer than 200 characters")]
        public string Location { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty; // ← NEW FIELD
    }
}
