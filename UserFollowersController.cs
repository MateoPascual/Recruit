using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Models.Responses;
using Sabio.Services;
using Sabio.Services.Interfaces;

namespace Sabio.Web.Controllers
{
    public class UserFollowersController : ApiController
    {
        readonly IUserFollowers userFollowerService;
           
        public UserFollowersController(IUserFollowers userFollowerService)
        {
            this.userFollowerService = userFollowerService;
        }

        [Route("api/User/Followers"), HttpPost]
        public HttpResponseMessage GetFollowers(UserFollowersID Id)
        {
            if (Id == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotAcceptable,
                    "No data was sent to the server.");
            };

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    ModelState);
            };

            List<UserFollowers> userFollowers = userFollowerService.GetUserFollowers(Id);

            ItemsResponse<UserFollowers> itemsResponse = new ItemsResponse<UserFollowers>();
            itemsResponse.Items = userFollowers;

            return Request.CreateResponse(HttpStatusCode.OK, itemsResponse);
        }
    }
}
