using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskFlow.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<UsuarioDto, Usuario>();

            CreateMap<Proyecto, ProyectoDto>();
            CreateMap<CrearProyectoDto, Proyecto>();

            CreateMap<Tarea, TareaDto>();
            CreateMap<CrearTareaDto, Tarea>();
        }
    }
}
