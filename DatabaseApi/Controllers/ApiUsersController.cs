using DatabaseApi.Model;
using DatabaseApi.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers;
[ApiController]
[Route("/api/[controller]")]
public class ApiUsersController : ControllerBase
{
    private ApiUserService _apiUserService;

    public ApiUsersController(ApiUserService apiUserService)
    {
        _apiUserService = apiUserService;
    }

    [HttpGet]
    [Route("create")]
    public async Task<IActionResult> Create()
    {
        var createdUser = await _apiUserService.CreateApiUser();
        return new JsonResult(createdUser);
    }

    [HttpGet]
    [Route("get_all")]
    public async Task<IActionResult> GetAll(string apiKey)
    {
        if (await _apiUserService.UserHasRights(apiKey, Right.All))
        {
            var allUsers = await _apiUserService.GetAll();
            return new JsonResult(allUsers);
        }
        return new StatusCodeResult(403);
    }
    
    [HttpGet]
    [Route("get_rights")]
    public async Task<IActionResult> GetUserRights(Guid userId)
    {
        var userRights = await _apiUserService.GetUserRights(userId);
        return new JsonResult(userRights);
    }

    [HttpPost]
    [Route("grant")]
    public async Task<IActionResult> Grant(string apiKey, 
                                            ApiUserRight right)
    {
        if (await _apiUserService.UserHasRights(apiKey, Right.All))
        {
            await _apiUserService.GrantRightsToUser(right.ApiUserId, right.Right);
            return new StatusCodeResult(201);
        }
        return new StatusCodeResult(403);
    }

    [HttpGet]
    [Route("register_su")]
    public async Task<IActionResult> CreateSuperUser()
    {
        var createdUser = await _apiUserService.CreateApiUserWithRights(Right.View,
                                                                        Right.Insert, 
                                                                        Right.Update, 
                                                                        Right.Delete, 
                                                                        Right.All);
        return new JsonResult(createdUser);
    }
    
}