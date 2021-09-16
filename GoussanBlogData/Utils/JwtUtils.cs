
using GoussanBlogData.Models;
using GoussanBlogData.Models.DatabaseModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GoussanBlogData.Utils
{
    /// <summary>
    /// Interface that exports utilities accessible by the rest of the application
    /// </summary>
    public interface IJwtUtils
    {
        /// <summary>
        /// Generate a JWT Token for the User Input
        /// </summary>
        /// <param name="user"></param>
        /// <returns>JWT Token</returns>
        public string GenerateToken(User user);
        /// <summary>
        /// Validates a JWT Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns>ID of User extracted from JWT Token</returns>
        public string ValidateToken(string token);
    }

    /// <summary>
    /// Class Method that defines JWT Utility functions
    /// </summary>

    public class JwtUtils : IJwtUtils
    {
        private readonly string _config;

        /// <summary>
        /// Extract Secret Key from configuration
        /// </summary>
        public JwtUtils()
        {
            _config = Config.Secret;
        }

        /// <summary>
        /// Generate a new JWT Token and Sign it for the specific user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Signed JWT Token</returns>

        public string GenerateToken(User user)
        {
            // generate token that is valid for 1 day
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Validates the JWT Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns>User ID extracted from the Token</returns>
        public string ValidateToken(string token)
        {
            if (token == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }

}
