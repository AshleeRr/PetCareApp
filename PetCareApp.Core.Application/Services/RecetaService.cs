using AutoMapper;
using PetCareApp.Core.Application.Dtos.RecetasDtos;
using PetCareApp.Core.Domain.Entities;
using PetCareApp.Core.Domain.Interfaces;

public class RecetaService : IRecetaService
{
    private readonly IRecetaRepository _recetaRepository;
    private readonly ICitaRepository _citasRepository;
    private readonly IMedicamentoRepository _medicamentoRepository;
    private readonly IMapper _mapper;

    public RecetaService(
        IRecetaRepository recetaRepository,
        ICitaRepository citasRepository,
        IMedicamentoRepository medicamentoRepository,
        IMapper mapper)
    {
        _recetaRepository = recetaRepository;
        _citasRepository = citasRepository;
        _medicamentoRepository = medicamentoRepository;
        _mapper = mapper;
    }
    public async Task<RecetaDto> CreateRecetaAsync(CreateRecetaDto dto)
    {
        // 1. Validar cita existe
        var cita = await _citasRepository.GetByIdAsync(dto.CitaId);
        if (cita == null)
            throw new Exception("La cita no existe.");
        var receta = _mapper.Map<Receta>(dto);
        receta.Fecha = DateTime.Now;

        receta = await _recetaRepository.AddAsync(receta);
        return _mapper.Map<RecetaDto>(receta);
    }

    public async Task<bool> AddMedicamentoToRecetaAsync(AddMedicamentoToRecetaDto dto)
    {
        var receta = await _recetaRepository.GetByIdAsync(dto.RecetaId);
        if (receta == null)
            throw new Exception("La receta no existe.");

        var medicamento = await _medicamentoRepository.GetByIdAsync(dto.MedicamentoId);
        if (medicamento == null)
            throw new Exception("El medicamento no existe.");

        var relacion = new RecetaMedicamento
        {
            RecetaId = dto.RecetaId,
            MedicamentoId = dto.MedicamentoId,
            DosisIndicada = dto.Dosis,
            DuracionTratamiento = dto.Duracion,
            Observaciones = dto.Observaciones
        };

        await _recetaRepository.AddMedicamentoToRecetaAsync(relacion);

        return true;
    }

    public async Task<List<RecetaDto>> GetRecetasByCitaAsync(int citaId)
    {
        var recetas = await _recetaRepository.GetByCitaIdAsync(citaId);
        return _mapper.Map<List<RecetaDto>>(recetas);
    }
}

