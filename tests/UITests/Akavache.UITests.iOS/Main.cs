using Akavache.Sqlite3;
using UIKit;

namespace Akavache.UITests.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Akavache.BlobCache.ApplicationName = "Testing";
            typeof(SQLiteEncryptedBlobCache).ToString();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
