namespace EfCoreDemo.Domain;

public interface ISoftDeleteEntity
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}