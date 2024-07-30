namespace webapbackend.Dto
{
    public class RegisterResponse
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string? UserEmail { get; set; }
        public string? UserAddress { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int UserId { get; set; }
        public DateTime UserBirthDate { get; set; }
        public string Token { get; set; }
        public bool IsAdmin { get; set; }
    }
}
