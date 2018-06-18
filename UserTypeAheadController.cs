using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Models.Requests;
using Sabio.Models.Responses;
using Sabio.Services;


namespace Sabio.Web.Controllers
{
    public class UserTypeAheadController : ApiController
    {
        readonly IUserTypeAheadService UserTypeAheadService;

        public UserTypeAheadController(IUserTypeAheadService UserTypeAhead)
        {
            this.UserTypeAheadService = UserTypeAhead;
        }

        [Route("api/users/search"), HttpPost]
        public HttpResponseMessage GetUserByName(UserTypeAheadInput UserIdModal)
        {

            if (UserIdModal == null)
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

            List<UserTypeAheadRequest> eventmodel = UserTypeAheadService.GetUserByName(UserIdModal);

            ItemsResponse<UserTypeAheadRequest> itemsResponse = new ItemsResponse<UserTypeAheadRequest>();
            itemsResponse.Items = eventmodel;

            return Request.CreateResponse(HttpStatusCode.OK, itemsResponse);
        }
    }
}
