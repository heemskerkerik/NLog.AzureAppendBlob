using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace NLog.AzureAppendBlob
{
	[Target("AzureAppendBlob")]
	public sealed class AzureAppendBlobTarget: TargetWithLayout
	{
		[RequiredParameter]
		public string ConnectionString { get; set; }

		[RequiredParameter]
		public Layout Container { get; set; }

		[RequiredParameter]
		public Layout BlobName { get; set; }

		private CloudBlobClient _client;
		private CloudBlobContainer _container;
		private CloudAppendBlob _blob;

		protected override void InitializeTarget()
		{
			base.InitializeTarget();

			_client = CloudStorageAccount.Parse(ConnectionString)
			                             .CreateCloudBlobClient();
		}

		protected override void Write(LogEventInfo logEvent)
		{
			if (_client == null)
			{
				return;
			}

			string containerName = Container.Render(logEvent);
			string blobName = BlobName.Render(logEvent);

			if (_container == null || _container.Name != containerName)
			{
				_container = _client.GetContainerReference(containerName);
				_blob = null;
			}

			if (_blob == null || _blob.Name != blobName)
			{
				_blob = _container.GetAppendBlobReference(blobName);

				if (!_blob.Exists())
				{
					try
					{
						_blob.Properties.ContentType = "text/plain";
						_blob.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition());
					}
					catch (StorageException ex) when (ex.RequestInformation.HttpStatusCode == (int) HttpStatusCode.Conflict)
					{
						// to be expected
					}
				}
			}

			_blob.AppendText(Layout.Render(logEvent) + "\r\n", Encoding.UTF8);
		}
	}
}
