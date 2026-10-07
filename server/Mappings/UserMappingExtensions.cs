using Nat20Server.Entities;
using Nat20Server.DTOs;

namespace Nat20Server.Mappings
{
    public static class UserMappingExtensions
    {
        public static UserPublicDto ToPublicDto(this User user)
        {
            return new UserPublicDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
