using reservations_api.Models.Entities;
using reservations_api.Data;
using Microsoft.EntityFrameworkCore;

namespace reservations_api.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        return await _context.Reservations.FindAsync(id);
    }

    public async Task DeleteAsync(Reservation reservation)
    {
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Reservation>> GetByClassroomAndDateAsync(Guid classroomId, DateOnly date)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Where(r => r.ClassroomId == classroomId && r.Date == date)
            .ToListAsync();
    }

}