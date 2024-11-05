using AutoMapper;
using PepeWeb.Data.DTO;
using PepeWeb.Data.Models;

namespace PepeWeb.Data.Mapper
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<Table, TableDTO>();
            CreateMap<Field, FieldDTO>();
            CreateMap<Value, ValueDTO>();
        }
    }
}
