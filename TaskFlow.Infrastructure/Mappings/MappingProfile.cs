using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using System.Globalization;

namespace TaskFlow.Infrastructure.Mappings
{
    // Convertidores personalizados para fechas
    public class DateTimeToStringConverter : IValueConverter<DateTime, string>
    {
        public string Convert(DateTime source, ResolutionContext context)
        {
            return source.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }

    public class StringToDateTimeConverter : IValueConverter<string, DateTime>
    {
        public DateTime Convert(string source, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentException("La fecha no puede estar vacía");

            source = source.Trim()
                .Replace("a. m.", "AM").Replace("p. m.", "PM")
                .Replace("a.m.", "AM").Replace("p.m.", "PM")
                .Replace("am", "AM").Replace("pm", "PM");

            string[] formats = {
                "dd-MM-yyyy", "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy hh:mm:ss tt",
                "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy hh:mm:ss tt",
                "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd hh:mm:ss tt"
            };

            if (DateTime.TryParseExact(source, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            if (DateTime.TryParse(source, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return result;

            throw new FormatException($"No se pudo convertir la fecha '{source}' a DateTime.");
        }
    }

    // Perfil principal de mapeo
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

            // Tarea con convertidores de fecha (DateTime <-> string)
            CreateMap<Tarea, TareaDto>()
                .ForMember(dest => dest.FechaVencimiento,
                    opt => opt.ConvertUsing<DateTimeToStringConverter, DateTime>());
            CreateMap<TareaDto, Tarea>()
                .ForMember(dest => dest.FechaVencimiento,
                    opt => opt.ConvertUsing<StringToDateTimeConverter, string>());

            // Comentario
            CreateMap<Comentario, ComentarioDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : null));
        }
    }
}

