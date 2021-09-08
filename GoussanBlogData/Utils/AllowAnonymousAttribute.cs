namespace GoussanBlogData.Utils
{
    /// <summary>
    /// Here modifications to the AllowAnonymous Attribute is handled
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    { }
}
