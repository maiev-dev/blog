using DatabaseApi.Model;
using DatabaseApi.Model.Entities;
using DatabaseApi.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace DatabaseApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ArticlesController : Controller
{
    private IRepository<Article> _articleRepository;
    private IRepository<Keyword> _keywordsRepository;
    private ApiUserService _apiUserService;
     
    public ArticlesController(IRepository<Article> articleRepository,
                                ApiUserService apiUserService, 
                                IRepository<Keyword> keywordsRepository)
    {
        _articleRepository = articleRepository;
        _apiUserService = apiUserService;
        _keywordsRepository = keywordsRepository;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(string apiKey, [FromBody]Article article)
    {
        if (await _apiUserService.UserHasRights(apiKey, Right.Insert))
        {
            var saveCandidate = article with
            {
                Creator = await _apiUserService.GetUserId(apiKey)
                
            };
            
            await _articleRepository.CreateAsync(saveCandidate);
            await _articleRepository.SaveChangesAsync();
            
            
            return new StatusCodeResult(201);
        }
        return new StatusCodeResult(403);
    }

    [HttpGet]
    [Route("get_all")]
    public async Task<IActionResult> GetAll(string apiKey, bool isShort)
    {
        if (await _apiUserService.UserHasRights(apiKey, Right.View))
        {
            var allArticles = await _articleRepository.GetAllAsync();
            if (isShort)
            {
                allArticles = allArticles.Select(x => new Article(x.Creator,
                                             x.Header, x.Abstract, string.Empty)
                {
                    Id = x.Id,
                    Keywords = x.Keywords
                });
            }
            return new JsonResult(allArticles);
        }

        return new StatusCodeResult(403);
    }

    [HttpGet]
    [Route("get/{id}")]
    public async Task<IActionResult> GetArticle(string apiKey, Guid id)
    {
        if (await _apiUserService.UserHasRights(apiKey, Right.View))
        {
            var article = await _articleRepository.FindAsync(id);
            return new JsonResult(article);
        }

        return new StatusCodeResult(403);
    }
    
    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> SearchArticleByKeywords(string apiKey, string[] keywords)
    {
        if (await _apiUserService.UserHasRights(apiKey, Right.View))
        {
            var allArticles = await _articleRepository.GetAllAsync();
            
            var filtered = allArticles
                .Where(x => FalseIfNull(
                    x.Keywords?.Any(k => keywords.Contains(k.Value)))
                );
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return new JsonResult(filtered);
        }

        return StatusCode(403);
    }

    private bool FalseIfNull(bool? value)
    {
        return value != null && value.Value;
    }
    
    
}

