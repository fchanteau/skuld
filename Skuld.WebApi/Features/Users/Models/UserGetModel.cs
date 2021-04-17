namespace Skuld.WebApi.Features.Users.Models
{
    public class UserGetModel
    {
        public decimal UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
