
using GoussanBlogData.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace GoussanBlogData.Utils
{
    /// <summary>
    /// JWT Middleware that handles JWT Token Verification and uses it to create a User Context for the API whenever a request comes in
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Queue System 
        /// </summary>
        /// <param name="next"></param>
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Task that gets invoked whenever a new request comes in. 
        /// It validates the JWT Token to ensure that the request has proper authorization
        /// And it assigns retrieves user information from Cosmos DB and stores it in a User Context that is accessible at runtime by the API
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        /// <param name="jwtUtils"></param>

        public async Task Invoke(HttpContext context, ICosmosDbService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var userId = jwtUtils.ValidateToken(token);
                if (userId != null)
                {
                    var userObject = await userService.GetUserAsync(userId);
                    context.Items["User"] = userObject;
                }
            }

            await _next(context);
        }
    }

}
