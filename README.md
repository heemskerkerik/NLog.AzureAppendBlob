# NLog.AzureAppendBlob
An NLog target using Microsoft Azure Storage Append-only Blobs

## Test App ##
The NLog.AzureAppendBlob.Test project is a console program that is preconfigured to use the 'AzureAppendBlob' target. To avoid publishing credentials, the connection string points to the Storage Emulator. **However**, the current Azure SDK is not yet compatible with append-only blobs. To test it, you'll have to create an Azure storage account and a blob container.