using LogisticsTrack.Domain.BaseEntities;
using LogisticsTrack.Domain.BMOModels;
using LogisticsTrack.Domain.DTOModels;
using LogisticsTrack.Domain.HelperModels;

namespace LogisticsTrack.Service
{
    public interface IBaseService<TDTOModel, TDTOListModel, TBMOModel>
        where TDTOModel : class
        where TDTOListModel : class
        where TBMOModel : class
    {
        Task<TDTOModel> CreateAsync(TBMOModel model);
        Task<IEnumerable<TDTOListModel>> CreateBulkAsync(IEnumerable<TBMOModel> models);
        Task DeleteAsync(Guid id);
        Task<PaginationList<IEnumerable<TDTOListModel>>> GetAllAsync(PaginationParameters? paginateConfig = null);
        Task<TDTOModel> GetAsync(Guid id);
        Task<TDTOModel> UpdateAsync(Guid id, TBMOModel model);
    }
}