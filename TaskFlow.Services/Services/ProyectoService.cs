using AutoMapper;
using TaskFlow.Core.DTOs;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _proyectoRepo;
        private readonly IMapper _mapper;

        public ProyectoService(IProyectoRepository proyectoRepo, IMapper mapper)
        {
            _proyectoRepo = proyectoRepo;
            _mapper = mapper;
        }

        public async Task<ProyectoDto?> CrearProyecto(CrearProyectoDto dto)
        {
            var existe = await _proyectoRepo.ExisteNombreParaCreador(dto.Nombre, dto.CreadorId);
            if (existe) throw new Exception("Ya tienes un proyecto con ese nombre");

            var proyecto = _mapper.Map<Proyecto>(dto);
            await _proyectoRepo.Add(proyecto);
            // Agregar al creador como miembro admin
            await _proyectoRepo.AddMember(proyecto.Id, dto.CreadorId, "Admin"); 
            return _mapper.Map<ProyectoDto>(proyecto);
        }
    }
}
