using webapbackend.Data;
using webapbackend.Models;

namespace webapbackend
{
    public class Seed
    {
            private readonly DataContext _dataContext;

            public Seed(DataContext context)
            {
                _dataContext = context;
            }

            public void SeedDataContext()
            {
                // verilerini ekle
                if (!_dataContext.Users.Any())
                {
                    var categories = new List<Category>()
                {
                    new Category() { Name = "Lazer Epilasyon" },
                    new Category() { Name = "Cilt Bakımı" },
                    new Category() { Name = "Kozmetik Uygulamalar" },
                    new Category() { Name = "El ve Ayak Bakımı" },
                    new Category() { Name = "Bölgesel İncelme ve G5" }
                };

                    var products = new List<Product>()
                {
                    new Product() { Name = "Kadın Lazer Epilasyon", Category = categories[0] },
                    new Product() { Name = "Erkek Lazer Epilasyon", Category = categories[0] },
                    new Product() { Name = "Cilt Bakımı", Category = categories[1] }
                };

                    var users = new List<User>()
                {
                    new User()
                    {
                        Name = "Sıla",
                        Surname = "Çağdaş",
                        Email = "sila.cagdas@tedu.edu.tr",
                        Password = "123456",
                        Address = "Ankara",
                        BirthDate = new DateTime(2003, 8, 15),
                        PhoneNumber="05435931168"
                    }
                };

                    var appointments = new List<Appointment>()
                {
                    new Appointment()
                    {
                        Date = new DateTime(2024, 8, 1),
                        User = users[0] 
                    }
                };

                    _dataContext.Categories.AddRange(categories);
                    _dataContext.Products.AddRange(products);
                    _dataContext.Users.AddRange(users);
                    _dataContext.Appointments.AddRange(appointments);

                    _dataContext.SaveChanges();
                }
            }
        }
    }
