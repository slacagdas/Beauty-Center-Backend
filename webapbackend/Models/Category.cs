using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapbackend.Models
{
    public class Category
            {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string? Name { get; set; }
            public ICollection<Product>? Products { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
    }
