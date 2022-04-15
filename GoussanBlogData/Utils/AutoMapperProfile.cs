
using AutoMapper;
using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.UserModels;

namespace GoussanBlogData.Utils
{
    /// <summary>
    /// Object Mapping is handled here
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Here we define the profiles for Object to Object mapping
        /// </summary>
        public AutoMapperProfile()
        {
            // User => AuthResponse
            CreateMap<User, AuthResponse>();

            // CreateUser => User
            CreateMap<CreateUser, User>();


        }
    }

}
