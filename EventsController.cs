using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Models.Responses;
using Sabio.Services.Interfaces;
using Sabio.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers
{
    public class EventsController : ApiController
    {
        readonly IEventsService eventsService;

        public EventsController(IEventsService eventsService)
        {
            this.eventsService = eventsService;
        }

        [Route("api/events"), HttpGet]
        public HttpResponseMessage GetAll()
        {
            List<Event> events = eventsService.GetAll();

            ItemsResponse<Event> itemsResponse = new ItemsResponse<Event>();
            itemsResponse.Items = events;

            return Request.CreateResponse(HttpStatusCode.OK, itemsResponse);
        }

        [Route("api/events/{id:int}"), HttpPost]
        public HttpResponseMessage GetById(int id, EventGetByIdRequest idModel)
        {
            
            if (idModel == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotAcceptable,
                    "No data was sent to the server.");
            };

            if (idModel.Id != id)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotAcceptable,
                    "The Id on the URL and data body does not match.");
            };

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    ModelState);
            };

            Event eventModel = eventsService.GetById(idModel);

            ItemResponse<Event> itemResponse = new ItemResponse<Event>();
            itemResponse.Item = eventModel;

            return Request.CreateResponse(HttpStatusCode.OK, itemResponse);
        }

        [Route("api/events"), HttpPost]
        public HttpResponseMessage Create(EventCreateRequest createModel)
        {
            if (createModel == null)
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

            ItemResponse<int> itemResponse = new ItemResponse<int>();
            itemResponse.Item = eventsService.Create(createModel);

            return Request.CreateResponse(HttpStatusCode.OK, itemResponse);
        }

        [Route("api/events/{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id, EventDeleteRequest deleteModel)
        {
            if (deleteModel == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotAcceptable,
                    "No data was sent to the server.");
            };

            if (deleteModel.Id != id)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotAcceptable,
                    "The Id on the URL and data body does not match.");
            };

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    ModelState);
            };

            eventsService.Delete(deleteModel);
            SuccessResponse successResponse = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, successResponse);
        }

        [Route("api/events/{id:int}"), HttpPut]
        public HttpResponseMessage Update(int id, EventUpdateRequest updateModel)
        {
            if (updateModel == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotAcceptable,
                    "No data was sent to the server.");
            };

            if (updateModel.Id != id)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotAcceptable,
                    "The Id on the URL and data body does not match.");
            };

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    ModelState);
            };

            eventsService.Update(updateModel);
            SuccessResponse successResponse = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, successResponse);
        }

        [Route("api/user/events"), HttpGet]
        public HttpResponseMessage GetByUserId()
        {
            int userId = User.Identity.GetId().Value;

            List<Event> eventsGetByUserIds = eventsService.GetByUserID(userId);

            ItemsResponse<Event> itemsResponse = new ItemsResponse<Event>();
            itemsResponse.Items = eventsGetByUserIds;

            return Request.CreateResponse(HttpStatusCode.OK, itemsResponse);
        }

        [Route("api/user/eventsPublic"), HttpGet]
        public HttpResponseMessage GetPublicEvents()
        {
            List<Event> eventsGetAllPublic = eventsService.GetPublicEvents();

            ItemsResponse<Event> itemsResponse = new ItemsResponse<Event>();
            itemsResponse.Items = eventsGetAllPublic;

            return Request.CreateResponse(HttpStatusCode.OK, itemsResponse);
        }
    }
}
