namespace DatabaseApi.Model.Entities;

public record ApiUserRight(Guid ApiUserId, Right Right)
{
    public Guid Id { get; init; }
}