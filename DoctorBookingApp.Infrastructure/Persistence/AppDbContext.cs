using DoctorBookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoctorBookingApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
