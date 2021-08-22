
namespace GoussanBlogData.Models;
public class User
{
    public string Id { get; set; }
    public string Created { get; set; }
    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string Email { get; set; }
    public Media Medias { get; set; }
}
