using System.IO;
using Microsoft.Azure.WebJobs;

namespace RssWebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called test-queue-1.
        public static void ProcessQueueMessage([QueueTrigger("test-queue-1")] string message, TextWriter log)
        {
            log.WriteLine(message);

            new FileRepository(log).UpdateFile(message);
        }
    }
}
