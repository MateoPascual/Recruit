using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class ActivityCalcService : IActivityCalcService
    {
        readonly IDataProvider dataProvider;

        public ActivityCalcService(IDataProvider dataprovider)
        {
            this.dataProvider = dataprovider;
        }
        
        public List<ActivityCalc> GetUserActivity(ActivityCalcGetId UserID)
        {
            List<ActivityCalc> userActivity = new List<ActivityCalc>();

            dataProvider.ExecuteCmd(
                "ActivityCalc",
                inputParamMapper: param =>
                {
                    param.AddWithValue("Id", UserID.Id);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    ActivityCalc activities = new ActivityCalc();
                    activities.MonthAndDate = (string)reader["MonthAndDate"];
                    activities.NumberOfPosts = (int)reader["NumberOfPosts"];

                    userActivity.Add(activities);
                });
            return userActivity;
        }
    }
}
