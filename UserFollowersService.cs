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
    public class UserFollowersService : IUserFollowers
    {
        readonly IDataProvider dataProvider;

        public UserFollowersService(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public List<UserFollowers> GetUserFollowers(UserFollowersID Id)
        {
            List<UserFollowers> results = new List<UserFollowers>();

            dataProvider.ExecuteCmd(
                (Id.GetShortList)? "Users_FollowersTop5" : "Users_Followers",
                inputParamMapper: param =>
                {
                    param.AddWithValue("@Id", Id.Id);
                },
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    UserFollowers model = new UserFollowers();
                    model.Followers = (int)reader["Followers"];
                    model.ReceiverId = (int)reader["ReceiverId"];
                    model.AvatarUrl = (string)reader["AvatarUrl"];
                    model.FirstName = (string)reader["FirstName"];
                    model.LastName = (string)reader["LastName"];
                    model.Type = (string)reader["Type"];
                    results.Add(model);
                });
            return results;
        }
    }
}
