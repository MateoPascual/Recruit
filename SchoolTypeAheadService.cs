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
    public class SchoolTypeAheadService : ISchoolTypeAheadService
    {
        readonly IDataProvider dataProvider;

        public SchoolTypeAheadService(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public List<SchoolTypeAheadRequest> GetByName(SchoolTypeAheadRequestName requestName)
        {
            string holder = Utils.ConvertStringToLikeExpression(requestName.Name);

            List<SchoolTypeAheadRequest> results = new List<SchoolTypeAheadRequest> ();

            dataProvider.ExecuteCmd(
                "School_Type_Ahead_Search",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Name", holder);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    SchoolTypeAheadRequest eventmodal = new SchoolTypeAheadRequest();
                    eventmodal.SchoolName = (string)reader["SchoolName"];
                    eventmodal.Id = (int)reader["Id"];
                    results.Add(eventmodal);
                });
            return results;
        }
    }
}