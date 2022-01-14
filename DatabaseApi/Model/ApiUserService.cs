using DatabaseApi.Model.Entities;
using DatabaseApi.Model.Repositories;

namespace DatabaseApi.Model;

public class ApiUserService
{
    private IRepository<ApiUser> _apiUsersRepository;
    private IRepository<ApiUserRight> _apiUserRightsRepository;
    public ApiUserService(IRepository<ApiUser> apiUserRepository, 
                     IRepository<ApiUserRight> apiUserRightsRepository)
    {
        _apiUserRightsRepository = apiUserRightsRepository;
        _apiUsersRepository = apiUserRepository;
    }

    public async Task<ApiUser> CreateApiUser()
    {
        return await CreateApiUserWithRights(Right.View);
    }

    public async Task<Guid> GetUserId(string apiKey)
    {
        var allUsers = await _apiUsersRepository.GetAllAsync();
        var user = allUsers.FirstOrDefault(x => x.ApiKey == apiKey);
        return user.Id;
    }

    public async Task<ApiUser> CreateApiUserWithRights(params Right[] rights)
    {
        Guid apiKey = Guid.NewGuid();
        var existUsers = await _apiUsersRepository.GetAllAsync();
        var existApiKeys = existUsers.Select(x => x.ApiKey);
        while (existApiKeys.Contains(apiKey.ToString()))
        {
            apiKey = Guid.NewGuid();
        }
        var user = new ApiUser(apiKey.ToString());
        await _apiUsersRepository.CreateAsync(user);
        foreach (var right in rights)
        {
            var userRight = new ApiUserRight(user.Id, right);
            await _apiUserRightsRepository.CreateAsync(userRight);
        }
        await _apiUsersRepository.SaveChangesAsync();
        await _apiUserRightsRepository.SaveChangesAsync();
        return user;
    }
    
    public async Task<IEnumerable<ApiUser>> GetAll()
    {
        return await _apiUsersRepository.GetAllAsync();
    }

    public async Task<bool> UserHasRights(string apiKey, params Right[] rights)
    {
        var userId = await GetUserId(apiKey);
        var userRights = await GetUserRights(userId);
        return rights.All(x => userRights.Contains(x));
    }

    public async Task<IEnumerable<Right>> GetUserRights(Guid userId)
    {
        var allRights = await _apiUserRightsRepository.GetAllAsync();
        var userRights = allRights
                                         .Where(x => x.ApiUserId == userId)
                                         .Select(x => x.Right);
        return userRights.ToList();
    }

    public async Task GrantRightsToUser(Guid to, params Right[] rights)
    {
        var userRights = await GetUserRights(to);
        foreach (var right in rights)
        {
            if (userRights.Contains(right) == false)
            {
                var apiUserRight = new ApiUserRight(to, right);
                await _apiUserRightsRepository.CreateAsync(apiUserRight);
            }
        }
        await _apiUserRightsRepository.SaveChangesAsync();
    }
}