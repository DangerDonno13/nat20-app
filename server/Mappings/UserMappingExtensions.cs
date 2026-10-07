using Nat20Server.Entities;
using Nat20Server.DTOs;

namespace Nat20Server.Mappings
{
    public static class UserMappingExtensions
    {
        /// <summary>
        /// Maps a User entity to a UserPublicDto, which is a data transfer object that contains public information about the user. This method is useful for exposing user information without revealing sensitive data such as passwords or internal identifiers.
        /// </summary>
        /// <param name="user">The User entity to map.</param>
        /// <returns>The mapped UserPublicDto.</returns>
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
