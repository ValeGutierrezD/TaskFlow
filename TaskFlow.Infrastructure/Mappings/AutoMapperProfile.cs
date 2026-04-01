using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Usuario
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<CrearUsuarioDto, Usuario>();

            // Proyecto
            CreateMap<Proyecto, ProyectoDto>();
            CreateMap<CrearProyectoDto, Proyecto>();
            CreateMap<ActualizarProyectoDto, Proyecto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Tarea
            CreateMap<Tarea, TareaDto>();
            CreateMap<CrearTareaDto, Tarea>();
            CreateMap<ActualizarTareaDto, Tarea>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}