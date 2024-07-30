using webapbackend.Models;

namespace webapbackend.Dto
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }
        public int UserId { get; set; }
       
        public string ProductName { get; set; }
        public int CategoryId { get; set; }  
        public int ProductId { get; set; }
      
    }
}
