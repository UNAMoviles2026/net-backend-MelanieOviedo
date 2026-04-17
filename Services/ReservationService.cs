using DTOs.Requests;
using DTOs.Responses;
using reservations_api.Mappers;
using reservations_api.Repositories;
using reservations_api.Models.Entities;


namespace reservations_api.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ReservationResponse> CreateAsync(CreateReservationRequest request)
    {
        if(request.StartTime >= request.EndTime)
        {
            throw new InvalidOperationException("StartTime must be before endTime.");
        }

        var existingReservations = await _reservationRepository.GetByClassroomAndDateAsync(request.ClassroomId, request.Date);

        if(HasOverLap(request.StartTime, request.EndTime, existingReservations))
        {
            throw new InvalidOperationException("Time conflicts with another reservation.");
        }

        var reservation = ReservationMapper.ToEntity(request);
        var createdReservation = await _reservationRepository.AddAsync(reservation);

        return ReservationMapper.ToResponse(createdReservation);
    }
    public async Task DeleteAsync(Guid id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
        {
            throw new KeyNotFoundException("Reservation not found.");
        }

        await _reservationRepository.DeleteAsync(reservation);
    }

    public async Task<List<ReservationResponse>> GetByDateAsync(DateOnly date)
    {
        var reservations = await _reservationRepository.GetByDateAsync(date);
        return reservations.Select(ReservationMapper.ToResponse).ToList();
    }


    private bool HasOverLap(TimeOnly startTime, TimeOnly endTime, List<Reservation> existingReservations)
    {
        return existingReservations.Any(r => 
            (startTime < r.EndTime && endTime > r.StartTime)
        );
    }
}