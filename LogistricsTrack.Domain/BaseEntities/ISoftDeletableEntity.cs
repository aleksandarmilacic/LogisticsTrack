namespace LogisticsTrack.Domain.BaseEntities
{
    public interface ISoftDeletableEntity
    {
        DateTime? HasBeenDeleted { get; }

        bool IsSoftDeleted();
        void SoftDelete();
    }
}