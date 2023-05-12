using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NugetMoodReboot.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using NugetMoodReboot.Interfaces;

namespace APIMoodReboot.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryUsers repositoryUsers;

        public UsersController(IRepositoryUsers repositoryUsers)
        {
            this.repositoryUsers = repositoryUsers;
        }

        [HttpGet("{pattern}")]
        public async Task<ActionResult<List<Tuple<string, int>>>> SearchUsers(string pattern)
        {
            return await this.repositoryUsers.SearchUsersAsync(pattern);
        }

        [HttpGet]
        public ActionResult<AppUser> Profile()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claim.Value;
            AppUser user = JsonConvert.DeserializeObject<AppUser>(jsonUser);

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult> Profile(UpdateProfileApiModel updateProfile)
        {
            if (updateProfile.Image != null)
            {
                await this.repositoryUsers.UpdateUserBasicsAsync(updateProfile.UserId, updateProfile.Username, updateProfile.FirstName, updateProfile.LastName, updateProfile.Image.Name);
                return NoContent();
            }

            await this.repositoryUsers.UpdateUserBasicsAsync(updateProfile.UserId, updateProfile.Username, updateProfile.FirstName, updateProfile.LastName);
            return NoContent();
        }

        [HttpPut("{userId}/{token}")]
        public async Task<ActionResult> ApproveUserEmail(int userId, string token)
        {
            UserAction? userAction = await this.repositoryUsers.FindUserActionAsync(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                }
                else
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    await this.repositoryUsers.ApproveUserAsync(userId);
                }
            }
            return NoContent();
        }

        #region CHANGE EMAIL

        [HttpPost("{userId}/{token}/{email}")]
        public async Task<ActionResult> ChangeEmail(int userId, string token, string email)
        {
            UserAction? userAction = await this.repositoryUsers.FindUserActionAsync(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    return NoContent();
                }
                else
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    await this.repositoryUsers.UpdateUserEmailAsync(userId, email);
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> RequestChangeEmail()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claim.Value;
            AppUser user = JsonConvert.DeserializeObject<AppUser>(jsonUser);

            string token = await this.repositoryUsers.CreateUserActionAsync(user.Id);
            return Ok(token);
        }

        #endregion

        #region CHANGE PASSWORD

        [HttpPost("{userId}/{token}/{password}")]
        public async Task<ActionResult> ChangePassword(int userId, string token, string password)
        {
            UserAction? userAction = await this.repositoryUsers.FindUserActionAsync(userId, token);

            if (userAction != null)
            {
                DateTime limitDate = userAction.RequestDate.AddHours(24);
                // Expired request - passed 24hrs
                if (DateTime.Now > limitDate)
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    return NoContent();
                }
                else
                {
                    await this.repositoryUsers.RemoveUserActionAsync(userAction);
                    await this.repositoryUsers.UpdateUserPasswordAsync(userId, password);
                }
            }
            return NoContent();
        }

        //[AuthorizeUsers]
        [HttpPost]
        public async Task<ActionResult> RequestChangePassword()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claim.Value;
            AppUser user = JsonConvert.DeserializeObject<AppUser>(jsonUser);

            string token = await this.repositoryUsers.CreateUserActionAsync(user.Id);
            return Ok(token);
        }

        #endregion
    }
}
