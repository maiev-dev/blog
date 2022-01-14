using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DatabaseApi.Model.Entities;

public record class Article(Guid Creator, string Header,
                            string Abstract, string Text)
{
    public Guid Id { get; init; }
    public List<Keyword> Keywords { get; set; }
}
//TODO: Keywords