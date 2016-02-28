using Microsoft.Azure;
using System.Configuration;

namespace NLog.AzureAppendBlob
{
	public class ConfigManager
	{
		private readonly string _connectionStringKey;

		public ConfigManager(string connectionStringKey)
		{
			_connectionStringKey = connectionStringKey;
		}

		public string GetStorageAccountConnectionString()
		{
			//First, check cloud service config
			var connectionStringValue = CloudConfigurationManager.GetSetting(_connectionStringKey);

			if (!string.IsNullOrEmpty(connectionStringValue))
				return connectionStringValue;

			//No cloud config setting found, check connection local config
			var connectionString = ConfigurationManager.ConnectionStrings[_connectionStringKey];

			if (connectionString != null)
				connectionStringValue = connectionString.ConnectionString;

			//No matching connection string found using the value specified as a key,
			// use as actual connection string
			return connectionStringValue;
		}
	}
}

