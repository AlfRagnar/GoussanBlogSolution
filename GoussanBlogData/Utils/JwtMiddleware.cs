
using GoussanBlogData.Services;

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
            if(userId != null)
            {
                context.Items["User"] = await userService.GetUserAsync(userId);
            }

            await _next(context);
        }
    }

}
