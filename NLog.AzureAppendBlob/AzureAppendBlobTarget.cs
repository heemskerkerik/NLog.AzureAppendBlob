using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Common;

namespace NLog.AzureAppendBlob
{
    [Target("AzureAppendBlob")]
    public sealed class AzureAppendBlobTarget: TargetWithLayout
    {
        [RequiredParameter]
        public Layout ConnectionString { get; set; }

        [RequiredParameter]
        public Layout Container { get; set; }

        [RequiredParameter]
        public Layout BlobName { get; set; }

        private CloudBlobClient _client;
        private CloudBlobContainer _container;
        private CloudAppendBlob _blob;
        private string _connectionString;

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            _client = null;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var connectionString = ConnectionString.Render(logEvent);

            if (_client == null || !string.Equals(_connectionString, connectionString, System.StringComparison.OrdinalIgnoreCase))
            {
                _client = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
                InternalLogger.Debug("Initialized connection to {0}", connectionString);
            }

            string containerName = Container.Render(logEvent);
            string blobName = BlobName.Render(logEvent);

            if (_container == null || _container.Name != containerName)
            {
                _container = _client.GetContainerReference(containerName);
                InternalLogger.Debug("Got container reference to {0}", containerName);

                if(_container.CreateIfNotExists())
                {
                    InternalLogger.Debug("Created container {0}", containerName);
                }

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
                        InternalLogger.Debug("Created blob: {0}", blobName);
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
