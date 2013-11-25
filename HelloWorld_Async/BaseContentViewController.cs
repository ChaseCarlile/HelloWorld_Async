using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace HelloWorld_Async
{
    public class BaseContentViewController : UIViewController
    {
        protected UIWebView webView = new UIWebView(new RectangleF(0, 0, 550, 700));
        protected string directory = System.IO.Path.Combine(NSBundle.MainBundle.BundlePath, "Documentation/");
        protected string html = "<!DOCTYPE html><html><body><h1>{0}</h1></body></html>";
        protected string _webViewContent;

        public string WebViewContent
        {
            get { return _webViewContent; }
            set
            {
                _webViewContent = value;
                webView.LoadHtmlString(string.Format(html, _webViewContent), new NSUrl(directory, true));
            }
        }

        public BaseContentViewController()
        {
            this.View.BackgroundColor = UIColor.Red;
            this.WebViewContent = "Loading . . . ";
            View.Add(webView);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

    }
}