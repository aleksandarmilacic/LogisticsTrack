using LogisticsTrack.Domain.BaseEntities;

namespace LogisticsTrack.Domain.DTOModels
{
    public class ModelBaseDTO : IDTO
    {
        public Guid Id { get; set; }
    }
}
