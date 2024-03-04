namespace LogisticsTrack.Domain.BaseEntities;
public abstract class SoftDeletableEntity : Entity, ISoftDeletableEntity
{
    public DateTime? HasBeenDeleted { get; private set; }

    public void SoftDelete()
    {
        HasBeenDeleted = DateTime.UtcNow;
    }

    public bool IsSoftDeleted() => HasBeenDeleted.HasValue;
}
