using AutoMapper;
using PetCareApp.Core.Application.Interfaces;
using PetCareApp.Core.Domain.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class GenericService<Entity, Dto> : IGenericService<Entity,Dto>
        where Dto : class
        where Entity : class
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositorio<Entity> _genericRepository;

        public GenericService(IGenericRepositorio<Entity> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        public virtual async Task<Dto?> CreateAsync(Dto dto)
        {
            Entity entity = _mapper.Map<Entity>(dto);
            Entity createdEntity = await _genericRepository.AddAsync(entity);
            if(createdEntity == null)
                return null;
            return _mapper.Map<Dto>(createdEntity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            await _genericRepository.RemoveAsync(id);
            return true;
        }

        public virtual async Task<List<Dto?>> GetAllAsync()
        {
            var listEntities = await _genericRepository.GetAllAsync();
            var listDtos = _mapper.Map<List<Dto?>>(listEntities);
            return listDtos;

        }

        public virtual async Task<Dto?> GetByIdAsync(int id)
        {
            var Entity = await _genericRepository.GetByIdAsync(id);
            var Dto = _mapper.Map<Dto>(Entity);
            return Dto;
        }

        public virtual async Task<Dto?> UpdateAsync(Dto dto, int id)
        {
            Entity Entity = _mapper.Map<Entity>(dto);
            Entity? updatedEntity = await _genericRepository.UpdateAsync(id, Entity);
            if (updatedEntity == null)
                return null;
            return _mapper.Map<Dto>(updatedEntity);

        }
    }
}
