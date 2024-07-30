using Microsoft.EntityFrameworkCore;
using webapbackend.Data;
using webapbackend.Interface;
using webapbackend.Models;

namespace webapbackend.Repository
{
    public class AppointmentRepository : IAppointment
    {
        private readonly DataContext _context;

        public AppointmentRepository(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<Appointment> GetAppointmentsByUserId(int userId)
        {
            return _context.Appointments.Include(a => a.Product).Where(a => a.UserId == userId).ToList();
        }
        public bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(c => c.Id == id);
        }
        public bool AppointmentExists(string Name)
        {
            return _context.Appointments.Any(c => c.ProductName == Name);
        }


        public bool CreateAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            return Save();
        }

        public bool DeleteAppointment(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            return Save();
        }

        public ICollection<Appointment> GetAppointments()
        {
            return _context.Appointments.Include(a => a.Product).ToList();
        }

        public Appointment GetAppointment(int id)
        {
            return _context.Appointments.FirstOrDefault(e => e.Id == id);
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0;
            }
            catch (Exception ex)
            {
                // Log exception details
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public bool UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            return Save();
        }
    }
}