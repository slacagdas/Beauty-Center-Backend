using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapbackend.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? Date { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
       public string ProductName { get; set; }
        public Category Category { get; set; } // Category ile olan ilişki
        public Product Product { get; set; }   // Product ile olan ilişki
        public User? User { get; set; } // User ile olan ilişkiz
    }
}

