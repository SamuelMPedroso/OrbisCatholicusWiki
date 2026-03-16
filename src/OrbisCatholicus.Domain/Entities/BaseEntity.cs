namespace OrbisCatholicus.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public abstract class SoftDeletableEntity : BaseEntity
{
    public bool IsActive { get; set; } = true;
    public DateTime? DeactivatedAt { get; set; }

    public void Deactivate()
    {
        IsActive = false;
        DeactivatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        DeactivatedAt = null;
    }
}
