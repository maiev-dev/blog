namespace DatabaseApi.Model.Entities;

public record ApiUser(string ApiKey)
{
    public Guid Id { get; set; }
}