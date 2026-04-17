using System.ComponentModel.DataAnnotations;

namespace DTOs.Requests
{
    public class GetReservationsRequest
    {
        [Required]
        public DateOnly Date { get; set; }
    }
}