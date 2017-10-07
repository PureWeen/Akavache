using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using UIKit;

namespace Akavache.UITests.iOS
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.


            Akavache.BlobCache.ApplicationName = "Testing";
            BlobCache.LocalMachine.GetObject<object>("key")
                .Catch((KeyNotFoundException ke) => Observable.Return<object>(null))
                .SelectMany(result =>
                {
                    return BlobCache.LocalMachine.InsertObject<object>("key", new object());
                })
                .SelectMany(result =>
                {
                    return BlobCache.LocalMachine.GetObject<object>("key");
                })
                .SelectMany(result =>
                {
                    return BlobCache.LocalMachine.InsertObject<object>("key", new object());
                })
                .Subscribe();

        //    BlobCache.LocalMachine.Insert("key", new object())
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
