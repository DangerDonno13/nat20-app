using Nat20Server.DTOs;
using Nat20Server.Entities;
using MongoDB.Driver;

namespace Nat20Server.Services
{
    /// <summary>
    /// Service for managing user-related operations, including retrieving and creating users in the database.
    /// </summary>
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<User>("Users");
        }

        /// <summary>
        /// Retrieves a user from the database based on their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The user if found, otherwise null.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new user in the database with the provided raw password, which is hashed before storage.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="rawPassword">The raw password to hash and store.</param>
        public async Task CreateUserAsync(User user, string rawPassword)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword);
            await _usersCollection.InsertOneAsync(user);
        }
    }
}
