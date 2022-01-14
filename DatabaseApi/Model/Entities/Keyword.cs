namespace DatabaseApi.Model.Entities;

public record class Keyword(string Value)
{
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
}