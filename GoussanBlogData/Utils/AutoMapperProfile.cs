
using AutoMapper;
using GoussanBlogData.Models.DatabaseModels;
using GoussanBlogData.Models.UserModels;

namespace GoussanBlogData.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User => AuthResponse
            CreateMap<User, AuthResponse>();

            // CreateUser => User
            CreateMap<Createuser, User>();

            // UpdateRequest => User
            CreateMap<UpdateRequest, User>().ForAllMembers(x => x.Condition((src, dest, prop) =>
                {
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }));
        }
    }

}
