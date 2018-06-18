using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data.Providers;
using Sabio.Services.Interfaces;


namespace Sabio.Services
{
    public class ConfigFromDatabase : IConfiguration
    {
        public class ConfigValue
        {
            public string Value { get; set; }
            public bool IsPrivate { get; set; }
        }

        readonly Dictionary<string, ConfigValue> allValues = new Dictionary<string, ConfigValue>();

        public string AdminEmailKey => Get("AdminEmailKey");
        public string SendGridKey => Get("SendGridKey");
        public string AmazonSecretKey => Get("AwsSecretKey");
        public string AmazonAccessKey => Get("AwsAccessKey");
        //^^^^^^^^ the string needs to match the name in the table on sql======= email sender servic
        //put key in webconfigurationservice table
        public ConfigFromDatabase(IDataProvider dataProvider)
        { 
            dataProvider.ExecuteCmd(
                "WebConfiguration_GetAll",
                inputParamMapper: null,
                singleRecordMapper: (reader, resultSetNumber) =>
                {
                    ConfigValue configValue = new ConfigValue();
                    configValue.Value = (string)reader["value"];
                    configValue.IsPrivate = (bool)reader["private"];

                    allValues.Add((string)reader["name"], configValue);
                });
        }

        public Dictionary<string, string> GetAllPublic()
        {
            return allValues
                .Where(kv => !kv.Value.IsPrivate)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value.Value
                );
        }

        string Get(string name)
        {
            return allValues[name].Value;
        }
    }
}
