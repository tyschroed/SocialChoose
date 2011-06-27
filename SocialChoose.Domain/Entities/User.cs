using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Nen.Security.Cryptography;

namespace SocialChoose.Domain.Entities
{
    public class User : Base
    {
        [StringLength(50),Required]
        public string CookieId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [StringLength(50)]
        public string PasswordHash { get; set; }
        [StringLength(50)]
        public string PasswordSalt { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string FacebookToken { get; set; }
        public DateTime? FacebookExpiresDateTime { get; set; }
        public decimal FacebookId { get; set; }

        /// <summary>
        /// Verifies candiate password against that which is stored.
        /// </summary>
        /// <param name="candidatePassword"></param>
        /// <returns></returns>
        public bool CheckPassword(string candidatePassword)
        {
            if (string.IsNullOrEmpty(candidatePassword))
                return false;

            return Hash.HmacSha1(PasswordSalt, candidatePassword) == PasswordHash;
        }

        public static User Load(int id)
        {
            return DatabaseConnection.GetOpenConnection().Query<User>("SELECT TOP 1 * FROM [User] WHERE Id = @UserId",new {UserId = id}).FirstOrDefault();
        }

        public static User Create()
        {
            var conn = DatabaseConnection
            .GetOpenConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO [User] (CookieId) VALUES (NULL);select scope_identity()";
            cmd.CommandType = System.Data.CommandType.Text;

            var id = cmd.ExecuteScalar();
            conn.Close();

            var user = new User();
            user.Id = int.Parse(id.ToString());
            return user;
        }

        public void Update() 
        {
            var result = DatabaseConnection
                        .GetOpenConnection()
                        .Query(@"UPDATE [User] SET CookieId = @CookieId, 
                                            UserName = @UserName, 
                                            PasswordHash = @PasswordHash, 
                                            PasswordSalt = @PasswordSalt,
                                            Name = @Name, 
                                            FacebookToken = @FacebookToken,
                                            FacebookExpiresDateTime = @FacebookExpires,
                                            FacebookId = @FacebookId,
                                            ModifiedDate = @ModifiedDate
                                            WHERE Id = @UserId",
                                    new { UserId = Id, CookieId = CookieId, UserName = UserName, PasswordHash = PasswordHash, PasswordSalt = PasswordSalt, Name = Name, FacebookToken = FacebookToken, FacebookExpires = FacebookExpiresDateTime, FacebookId = FacebookId, ModifiedDate = DateTime.Now });
        }
    }
}
