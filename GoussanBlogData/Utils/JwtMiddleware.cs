
using GoussanBlogData.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace GoussanBlogData.Utils
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICosmosDbService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                var userObject = await userService.GetUserAsync(userId);
                context.Items["User"] = await userService.GetUserAsync(userId);
            }

            await _next(context);
        }
    }

}
