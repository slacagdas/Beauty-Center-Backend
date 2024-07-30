using webapbackend.Models;

namespace webapbackend.Interface
{
    public interface IAppointment
    {
        ICollection<Appointment> GetAppointments();
        Appointment GetAppointment(int id);
        IEnumerable<Appointment> GetAppointmentsByUserId(int userId);
        bool AppointmentExists(int id);
        bool AppointmentExists(string ProductName);
        bool CreateAppointment(Appointment appointment);
        bool UpdateAppointment(Appointment appointment);
        bool DeleteAppointment(Appointment appointment);
        bool Save();
    }
}
