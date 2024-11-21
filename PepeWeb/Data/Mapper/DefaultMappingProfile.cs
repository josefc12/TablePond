using AutoMapper;
using TablePond.Data.DTO;
using TablePond.Data.Models;

namespace TablePond.Data.Mapper
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {
            CreateMap<Table, TableDTO>();
            CreateMap<Field, FieldDTO>();
            CreateMap<Value, ValueDTO>();
            CreateMap<Invitation, InvitationDTO>();

            CreateMap<TableDTO, Table>();
            CreateMap<FieldDTO, Field>();
            CreateMap<ValueDTO, Value>();
            CreateMap<InvitationDTO, Invitation>();
        }
    }
}
