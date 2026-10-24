namespace Bookly.Domain.Entities.Base;

public abstract class SoftDeletableEntity : BaseEntity
{
    public DateTime? DeletedAt { get; private set; }
    public void Delete() => DeletedAt = DateTime.UtcNow;
    public void Restore() => DeletedAt = null;
}
