using AutoMapper;
using System;
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
            CreateMap<CrearUsuarioDto, Usuario>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            // Proyecto
            CreateMap<Proyecto, ProyectoDto>();
            CreateMap<CrearProyectoDto, Proyecto>();
            CreateMap<ActualizarProyectoDto, Proyecto>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Tarea
            CreateMap<Tarea, TareaDto>();
            CreateMap<CrearTareaDto, Tarea>();
            CreateMap<ActualizarTareaDto, Tarea>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Comentario
            CreateMap<Comentario, ComentarioDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : null));
        }
    }
}

