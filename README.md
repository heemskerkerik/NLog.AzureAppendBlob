# NLog.AzureAppendBlob
An NLog target using Microsoft Azure Storage Append Blobs. See the [Azure Storage Team Blog](http://blogs.msdn.com/b/windowsazurestorage/archive/2015/08/04/microsoft-azure-storage-release-append-blob-new-azure-file-service-features-and-client-side-encryption-general-availability.aspx) for more information about Append Blobs.

## Usage ##
Install the [NLog.AzureAppendBlob](https://www.nuget.org/packages/NLog.AzureAppendBlob/) package from NuGet. If you use NLog 4.x or higher, NLog will automatically load the extension assembly. Otherwise, put the following in your NLog configuration:

    <nlog>
        <extensions>
            <add assembly="NLog.AzureBlobStorage" />
        </extensions>
    </nlog>

### Target configuration ###
The target's type name is ``AzureAppendBlob``.

* **connectionString** - The connection string for the storage account to use. Consult the Azure Portal to retrieve this.
* **container** - (layout) The name of the blob container where logs will be placed. Must exist.
* **blobName** - (layout) Name of the blob to write to. Will be created when it does not exist.
* **layout** - (layout) Text to write.

### Sample ###
    <target type="AzureAppendBlob" 
            name="Azure" 
            layout="${longdate} ${level:uppercase=true} - ${message}" 
            connectionString="UseDevelopmentStorage=true;" 
            container="logtest" 
            blobName="${date:format=yyyy-MM-dd}.log" />

See the [NLog Wiki](https://github.com/NLog/NLog) for more information about configuring NLog.

### Building ###
The project is a .NET 4.0 project, but it uses C# 6.0 features. If you wish to build it yourself, you'll need Visual Studio 2015 or Microsoft Build Tools 2015.

### Test App ###
NLog.AzureAppendBlob.Test is a console program that is preconfigured to use the ``AzureAppendBlob`` target. To avoid publishing credentials, the connection string points to the Storage Emulator. **However**, the current Azure SDK (2.7) is not yet compatible with append blobs. If you run it as is, it will silently fail and write nothing. To test it, you'll have to create an Azure storage account and a blob container.