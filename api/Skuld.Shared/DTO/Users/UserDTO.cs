using Skuld.Shared.DTO.Enum;

namespace Skuld.Shared.DTO.Users
{
    public class UserDTO
    {
        public decimal UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Role Role { get; set; }
    }
}
