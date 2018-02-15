using System.Configuration;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace RssWebJob
{
    public class FileRepository
    {
        private TextWriter _log;
        private string _connectionString;

        public FileRepository(TextWriter log)
        {
            _log = log;

            _connectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;
            _log.WriteLine($"connectionString: {_connectionString}");
        }

        private const string FileShareName = "test-file-share-1";

        public void UpdateFile(string name)
        {
            _log.WriteLine($"Parameters: {name}");

            if (!EnsureConnectionStringValid())
            {
                return;
            }

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference(FileShareName);

            // Ensure that the share exists.
            if (share.Exists())
            {
                // Get a reference to the root directory for the share.
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                CloudFile file = rootDir.GetFileReference(name);

                file.UploadText("File content.");
            }
        }

        private bool EnsureConnectionStringValid()
        {
            return !string.IsNullOrEmpty(_connectionString);
        }
    }
}
