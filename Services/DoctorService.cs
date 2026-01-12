using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Services;

public class DoctorService
{
    private readonly EhrDbContext _context;

    public DoctorService(EhrDbContext context)
    {
        _context = context;
    }

    // Fetch ONLY available doctors
    public async Task<List<Doctor>> GetAvailableDoctorsAsync()
    {
        return await _context.Doctors
            .Where(d => d.IsAvailable)
            .OrderBy(d => d.FirstName)
            .ThenBy(d => d.LastName)
            .ToListAsync();
    }

    // Validate doctor availability before booking
    public async Task<bool> IsDoctorAvailableAsync(int doctorId)
    {
        return await _context.Doctors
            .AnyAsync(d => d.DoctorId == doctorId && d.IsAvailable);
    }
}
