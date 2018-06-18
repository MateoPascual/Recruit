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

namespace Sabio.Web.Controllers
{
    public class SchoolTypeAheadController : ApiController
    {
        readonly ISchoolTypeAheadService schooltypeAheadService;

        public SchoolTypeAheadController(ISchoolTypeAheadService typeAhead)
        {
            this.schooltypeAheadService = typeAhead;
        }

        [Route("api/schools/search"), HttpPost]
        public HttpResponseMessage GetByName(SchoolTypeAheadRequestName idModel)
        {

            if (idModel == null)
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

            List<SchoolTypeAheadRequest> eventmodel = schooltypeAheadService.GetByName(idModel);

            ItemsResponse<SchoolTypeAheadRequest> itemsResponse = new ItemsResponse<SchoolTypeAheadRequest>();
            itemsResponse.Items = eventmodel;

            return Request.CreateResponse(HttpStatusCode.OK, itemsResponse);
        }
    }
}
