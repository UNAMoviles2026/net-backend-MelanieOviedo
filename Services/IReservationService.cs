using DTOs.Requests;
using DTOs.Responses;

namespace reservations_api.Services;

public interface IReservationService
{
    Task<ReservationResponse> CreateAsync(CreateReservationRequest request);
    Task DeleteAsync(Guid id);
    Task<List<ReservationResponse>> GetByDateAsync(DateOnly date);
    
}