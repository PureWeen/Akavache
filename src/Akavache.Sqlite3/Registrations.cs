using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using Splat;
using Akavache;

namespace Akavache.Sqlite3
{
    public class Registrations : IWantsToRegisterStuff
    {
        public void Register(IMutableDependencyResolver resolver)
        {
            // NB: We want the most recently registered fs, since there really 
            // only should be one 
            var fs = Locator.Current.GetService<IFilesystemProvider>();
            if (fs == null)
            {
                throw new Exception("Failed to initialize Akavache properly. Do you have a reference to Akavache.dll?");
            }

            Func<IBlobCache> localCacheFunc = () =>
            {
                fs.CreateRecursive(fs.GetDefaultLocalMachineCacheDirectory()).SubscribeOn(BlobCache.TaskpoolScheduler).Wait();
                return new SQLitePersistentBlobCache(Path.Combine(fs.GetDefaultLocalMachineCacheDirectory(), "blobs.db"), BlobCache.TaskpoolScheduler);
            };


            resolver.Register(localCacheFunc, typeof(IBlobCache), "LocalMachineCreate");

            Func<IBlobCache> userAccountFunc = () =>
            {
                fs.CreateRecursive(fs.GetDefaultRoamingCacheDirectory()).SubscribeOn(BlobCache.TaskpoolScheduler).Wait();
                return new SQLitePersistentBlobCache(Path.Combine(fs.GetDefaultRoamingCacheDirectory(), "userblobs.db"), BlobCache.TaskpoolScheduler);
            };

            resolver.Register(userAccountFunc, typeof(IBlobCache), "UserAccountCreate");

            Func<ISecureBlobCache> secureFunc = () =>
            {
                fs.CreateRecursive(fs.GetDefaultSecretCacheDirectory()).SubscribeOn(BlobCache.TaskpoolScheduler).Wait();
                return new SQLiteEncryptedBlobCache(Path.Combine(fs.GetDefaultSecretCacheDirectory(), "secret.db"), Locator.Current.GetService<IEncryptionProvider>(), BlobCache.TaskpoolScheduler);
            };

            resolver.Register(secureFunc, typeof(ISecureBlobCache), "Create");
        }
    }
}
