
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
