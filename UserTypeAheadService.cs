using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services;

namespace Sabio.Web
{
    public class UserTypeAheadService : IUserTypeAheadService
    {
        readonly IDataProvider dataProvider;

        public UserTypeAheadService(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public List<UserTypeAheadRequest> GetUserByName(UserTypeAheadInput requestName)
        {
            string holder = Utils.ConvertStringToLikeExpression(requestName.Name);

            List<UserTypeAheadRequest> results = new List<UserTypeAheadRequest>();

            dataProvider.ExecuteCmd(
                "User_Type_Ahead_Search",
                inputParamMapper: parameters =>
                {
                    parameters.AddWithValue("@Name", holder);
                    parameters.AddWithValue("@UserTypeId", requestName.UserTypeId ?? (object)DBNull.Value);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    UserTypeAheadRequest eventmodal = new UserTypeAheadRequest();
                    eventmodal.UserName = (string)reader["FullName"];
                    eventmodal.Id = (int)reader["Id"];
                    results.Add(eventmodal);
                });
            return results;
        }
    }
}