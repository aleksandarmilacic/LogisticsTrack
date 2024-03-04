using AutoMapper;
using LogisticsTrack.Database.Repository;
using Microsoft.EntityFrameworkCore; 
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.BaseEntities;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Domain.HelperModels;
using LogisticsTrack.Service.Extensions;

namespace LogisticsTrack.Service
{
    public abstract class GroundBaseService
    {
        // just because easier config for autofac registration of services
    }

    public abstract class BaseService<TEntity, TDTOModel, TDTOListModel, TBMOModel> : GroundBaseService, 
        IBaseService<TDTOModel, TDTOListModel, TBMOModel>
        where TEntity : Entity, ISoftDeletableEntity, IEntity, new()
        where TDTOModel : ModelBaseDTO
        where TDTOListModel : ModelBaseDTO
        where TBMOModel : ModelBaseBMO
    {
        protected readonly IMapper Mapper;

        protected IRepository<TEntity> EntityRepository { get; }

        public BaseService(IRepository<TEntity> entityRepository, IMapper mapper)
        {
            EntityRepository = entityRepository;
            Mapper = mapper;
        }


        #region IService


        public async virtual Task<PaginationList<IEnumerable<TDTOListModel>>> GetAllAsync(PaginationParameters paginateConfig = null)
        {
            var query = GetAllEntities().AsNoTracking();

            if (paginateConfig != null && paginateConfig.Query != null)
                foreach (var queryModel in paginateConfig.Query)
                {
                    query = GetSearchQuery(query, queryModel.Property, typeof(TEntity).Name, queryModel.Value);
                }


            var total = await query.CountAsync();

            query = query.Paginate(
                parameters: paginateConfig,
                typeof(TDTOListModel).Name);

            var entities = await query.ToListAsync().ConfigureAwait(false);
            var result = Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDTOListModel>>(entities);

            return new PaginationList<IEnumerable<TDTOListModel>>(result, total);
        }

        public async virtual Task<TDTOModel> GetAsync(Guid id)
        {
            var entity = await GetEntityByIdAsync(GetAllEntities().AsNoTracking(), id).ConfigureAwait(false);

            return Mapper.Map<TDTOModel>(entity);
        }

        public async virtual Task<TDTOModel> CreateAsync(TBMOModel model)
        {
            model.RejectIfInvalid();

            var entity = new TEntity();
            await UpdateEntity(entity, model, true);

            EntityRepository.Add(entity);
            await EntityRepository.SaveAsync().ConfigureAwait(false);

            entity = await GetEntityByIdAsync(GetAllEntities().AsNoTracking(), entity.Id).ConfigureAwait(false);

            return Mapper.Map<TDTOModel>(entity);
        }

        protected virtual System.Threading.Tasks.Task UpdateBulkEntity(TEntity entity, TBMOModel model, bool created)
        {
            return UpdateEntity(entity, model, created);
        }

        public async virtual Task<IEnumerable<TDTOListModel>> CreateBulkAsync(IEnumerable<TBMOModel> models)
        {
            models.RejectIfInvalid();

            var entities = new List<TEntity>();
            try
            {
                foreach (var model in models)
                {
                    var entity = new TEntity();
                    await UpdateBulkEntity(entity, model, true);

                    entities.Add(entity);
                }

                EntityRepository.AddMany(entities);
                await EntityRepository.SaveAsync().ConfigureAwait(false);

                var ids = entities.Select(x => x.Id);

                entities = await GetAllEntitiesBase().AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // TODO log it
            }

            // return ones that got created
            return Mapper.Map<IEnumerable<TDTOListModel>>(entities);
        }

        public async virtual Task<TDTOModel> UpdateAsync(Guid id, TBMOModel model)
        {
            model.RejectIfInvalid();

            var entity = await GetEntityByIdAsync(GetAllEntities(), id).ConfigureAwait(false);
            entity.RejectIfNotFound();

            await UpdateEntity(entity, model);

            await EntityRepository.SaveAsync().ConfigureAwait(false);

            entity = await GetEntityByIdAsync(GetAllEntities().AsNoTracking(), id).ConfigureAwait(false);

            return Mapper.Map<TDTOModel>(entity);
        }

        public async virtual Task DeleteAsync(Guid id)
        {
            var entity = await GetEntityByIdAsync(GetAllEntities(), id).ConfigureAwait(false);

            if (null == entity) return; // no need to delete anything

            entity.SoftDelete();

            await EntityRepository.SaveAsync().ConfigureAwait(false);
        }



        #endregion IService

        public IQueryable<TEntity> GetSearchQuery(IQueryable<TEntity> query, string propertyName, string mappedTotypeName, string queryString)
        {

            return SearchFilterInSqlService<TEntity>.GetSearchQuery(query, propertyName, mappedTotypeName, queryString);
        }

        protected virtual IQueryable<TEntity> GetAllEntitiesBase()
        {
            var query = EntityRepository.GetAll();


            return query;
        }


        protected virtual IQueryable<TEntity> GetAllEntities()
        {
            var query = GetAllEntitiesBase();
            return query;
        }

        protected virtual async Task<TEntity?> GetEntityByIdAsync(IQueryable<TEntity> query, Guid id)
        {
            return await query.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
        }


        protected async virtual Task UpdateEntity(TEntity entity, TBMOModel updateModel, bool created = false)
        {
            Mapper.Map<TBMOModel, TEntity>(updateModel, entity); 

            // this is used for additional logic in the derived classes
        }
    }
}
