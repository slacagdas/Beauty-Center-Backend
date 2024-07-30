﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapbackend.Models
{
    public class User
    {
       
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Surname { get; set; }
            public string? Password { get; set; }
            public string? Email { get; set; }
            public string? Address { get; set; }
            public string? PhoneNumber { get; set; }
    public bool isAdmin { get; set; }
            public DateTime BirthDate { get; set; }
            public ICollection<Appointment>? Appointments { get; set; }//one to many

        }
    }

