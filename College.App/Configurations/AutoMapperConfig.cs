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
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<RolePrivilege, RolePrivilegeDTO>().ReverseMap();
            //  CreateMap<UserRoleMapping, UserRoleMappingDTO>().ReverseMap();
            //  CreateMap<UserType, UserTypeDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<User, UserReadOnlyDTO>().ReverseMap();


        }
    }
}
