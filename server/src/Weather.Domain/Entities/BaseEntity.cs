namespace Weather.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    // Helper method to update the timestamp automatically
    protected void RegisterUpdate()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Delete() 
    {
        IsDeleted = true;
        RegisterUpdate();
    }
}