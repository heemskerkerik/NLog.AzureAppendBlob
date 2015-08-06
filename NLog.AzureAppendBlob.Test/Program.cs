using System;

namespace NLog.AzureAppendBlob.Test
{
	internal class Program
	{
		private static void Main()
		{
			var logger = LogManager.GetCurrentClassLogger();

			logger.Trace("Hello!");
			logger.Debug("This is");
			logger.Info("NLog using");
			logger.Warn("Append blobs in");
			logger.Error("Windows Azure");
			logger.Fatal("Storage.");

			try
			{
				throw new NotSupportedException();
			}
			catch (Exception ex)
			{
				logger.Error("This is an expected exception.", ex);
			}
		}
	}
}
