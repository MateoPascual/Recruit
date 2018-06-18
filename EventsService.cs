using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class EventsService : IEventsService
    {
        readonly IDataProvider dataProvider;

        public EventsService(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public List<Event> GetAll()
        {
            List<Event> results = new List<Event>();

            dataProvider.ExecuteCmd(
                "Events_GetAll",
                inputParamMapper: null,
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    Event eventModel = new Event();
                    eventModel.Id = (int)reader["Id"];
                    eventModel.Name = (string)reader["Name"];
                    eventModel.StartDate = reader["StartDate"] as DateTime? ?? default(DateTime);
                    eventModel.EndDate = reader["EndDate"] as DateTime? ?? default(DateTime);
                    eventModel.Description = reader["Description"] as string ?? "";
                    eventModel.Link = reader["Link"] as string ?? "";
                    eventModel.Logo = reader["Logo"] as string ?? "";
                    eventModel.IsOngoing = (bool)reader["IsOngoing"];
                    eventModel.Organizer = reader["Organizer"] as string ?? "";
                    eventModel.CreatedBy = (int)reader["CreatedBy"];
                    eventModel.ModifiedBy = (int)reader["ModifiedBy"];
                    eventModel.DateCreated = (DateTime)reader["DateCreated"];
                    eventModel.DateModified = (DateTime)reader["DateModified"];
                    eventModel.Private = (bool)reader["Private"];

                    results.Add(eventModel);
                });
            return results;
        }

        public Event GetById(EventGetByIdRequest idModel)
        {
            Event eventModel = new Event();
            
            dataProvider.ExecuteCmd(
                "Events_GetById",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Id", idModel.Id);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    eventModel.Id = (int)reader["Id"];
                    eventModel.Name = (string)reader["Name"];
                    eventModel.StartDate = reader["StartDate"] as DateTime? ?? default(DateTime);
                    eventModel.EndDate = reader["EndDate"] as DateTime? ?? default(DateTime);         
                    eventModel.Description = reader.GetSafeString("Description") as string ?? "";
                    eventModel.Link = reader["Link"] as string ?? "";
                    eventModel.Logo = reader["Logo"] as string ?? "";
                    eventModel.IsOngoing = (bool)reader["IsOngoing"];
                    eventModel.Organizer = reader["Organizer"] as string ?? "";
                    eventModel.CreatedBy = (int)reader["CreatedBy"];
                    eventModel.ModifiedBy = (int)reader["ModifiedBy"];
                    eventModel.DateCreated = (DateTime)reader["DateCreated"];
                    eventModel.DateModified = (DateTime)reader["DateModified"];
                    eventModel.Private = (bool)reader["Private"];
                });
            return eventModel;
        }

        public int Create(EventCreateRequest createModel)
        {
            int id = 0;
            dataProvider.ExecuteNonQuery(
                "Events_Create",
                inputParamMapper: (parameters) =>
                {
                    parameters.AddWithValue("@Name", createModel.Name);
                    parameters.AddWithValue("@StartDate", createModel.StartDate ?? (object)DBNull.Value);
                    parameters.AddWithValue("@EndDate", createModel.EndDate ?? (object)DBNull.Value);
                    parameters.AddWithValue("@Description", createModel.Description);
                    parameters.AddWithValue("@Logo", createModel.Logo);
                    parameters.AddWithValue("@Link", createModel.Link);
                    parameters.AddWithValue("@IsOngoing", createModel.IsOngoing);
                    parameters.AddWithValue("@Organizer", createModel.Organizer);
                    parameters.AddWithValue("@CreatedBy", createModel.CreatedBy);
                    parameters.AddWithValue("@ModifiedBy", createModel.ModifiedBy);
                    parameters.AddWithValue("@Private", createModel.Private);

                    parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                },
                returnParameters: (parameters) =>
                {
                    id = (int)parameters["@Id"].Value;
                });

            return id;
        }

        public void Delete(EventDeleteRequest deleteModel)
        {
            dataProvider.ExecuteNonQuery(
                "Events_Delete",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Id", deleteModel.Id);
                },
                returnParameters: null);
        }

        public void Update(EventUpdateRequest updateModel)
        {
            dataProvider.ExecuteNonQuery(
                "Events_Update",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Id", updateModel.Id);
                    parameters.AddWithValue("@Name", updateModel.Name);
                    parameters.AddWithValue("@StartDate", updateModel.StartDate ?? (object)DBNull.Value);
                    parameters.AddWithValue("@EndDate", updateModel.EndDate ?? (object)DBNull.Value);
                    parameters.AddWithValue("@Description", updateModel.Description);
                    parameters.AddWithValue("@Logo", updateModel.Logo);
                    parameters.AddWithValue("@Link", updateModel.Link);
                    parameters.AddWithValue("@IsOngoing", updateModel.IsOngoing);
                    parameters.AddWithValue("@Organizer", updateModel.Organizer);
                    parameters.AddWithValue("@CreatedBy", updateModel.CreatedBy);
                    parameters.AddWithValue("@ModifiedBy", updateModel.ModifiedBy);
                    parameters.AddWithValue("@Private", updateModel.Private);
                },
                returnParameters: null);
        }

        public List<Event> GetByUserID(int userID)
        {
            List<Event> results = new List<Event>();

            dataProvider.ExecuteCmd(
                "Get_EventsByUserID",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Id", userID);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                 {
                     Event eventModel = new Event();
                     eventModel.Id = (int)reader["Id"];
                     eventModel.Name = (string)reader["Name"];
                     eventModel.StartDate = reader["StartDate"] as DateTime? ?? default(DateTime);
                     eventModel.EndDate = reader["EndDate"] as DateTime? ?? default(DateTime);
                     eventModel.Description = reader["Description"] as string ?? "";
                     eventModel.Link = reader["Link"] as string ?? "";
                     eventModel.Logo = reader["Logo"] as string ?? "";
                     eventModel.IsOngoing = (bool)reader["IsOngoing"];
                     eventModel.Organizer = reader["Organizer"] as string ?? "";
                     eventModel.CreatedBy = (int)reader["CreatedBy"];
                     eventModel.ModifiedBy = (int)reader["ModifiedBy"];
                     eventModel.DateCreated = (DateTime)reader["DateCreated"];
                     eventModel.DateModified = (DateTime)reader["DateModified"];
                     eventModel.Private = (bool)reader["Private"];

                     results.Add(eventModel);
                 });
            return results;
        }

        public List<Event> GetPublicEvents()
        {
            List<Event> results = new List<Event>();

            dataProvider.ExecuteCmd(
                "Events_GetAllPublic",
                inputParamMapper: null,
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    Event eventModel = new Event();
                    eventModel.Id = (int)reader["Id"];
                    eventModel.Name = (string)reader["Name"];
                    eventModel.StartDate = reader["StartDate"] as DateTime? ?? default(DateTime);
                    eventModel.EndDate = reader["EndDate"] as DateTime? ?? default(DateTime);
                    eventModel.Description = reader["Description"] as string ?? "";
                    eventModel.Link = reader["Link"] as string ?? "";
                    eventModel.Logo = reader["Logo"] as string ?? "";
                    eventModel.IsOngoing = (bool)reader["IsOngoing"];
                    eventModel.Organizer = reader["Organizer"] as string ?? "";
                    eventModel.CreatedBy = (int)reader["CreatedBy"];
                    eventModel.ModifiedBy = (int)reader["ModifiedBy"];
                    eventModel.DateCreated = (DateTime)reader["DateCreated"];
                    eventModel.DateModified = (DateTime)reader["DateModified"];
                    eventModel.Private = (bool)reader["Private"];

                    results.Add(eventModel);
                });
            return results;
        }
    }
}
