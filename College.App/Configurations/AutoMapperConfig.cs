using AutoMapper;
using College.App.Data;
using College.App.Models;

namespace College.App.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Define your object-object mapping configurations here
            CreateMap<Student, StudentDTO>().ReverseMap();
        }
    }
}
