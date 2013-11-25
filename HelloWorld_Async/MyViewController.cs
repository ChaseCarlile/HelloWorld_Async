using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading.Tasks;
using System.Collections.Generic;
using MonoTouch.Dialog;

namespace HelloWorld_Async
{
    public class MyViewController : UIViewController
    {
        UIButton button1;
        UIButton button2;
        UIButton button3;
        float buttonWidth = 150;
        float buttonHeight = 50;

        public UIPopoverController DetailPopover { get; set; }
        public ModalContentViewController DetailModal { get; set; }
        public CustomModalViewController DetailCustom { get; set; }

        public MyViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.Frame = UIScreen.MainScreen.Bounds;
            View.BackgroundColor = UIColor.White;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            float middle = (View.Frame.Width / 2 - buttonWidth / 2);
            float offset = 200;

            button1 = UIButton.FromType(UIButtonType.RoundedRect);
            button1.Frame = new RectangleF(middle - offset, 10, buttonWidth, buttonHeight);
            button1.SetTitle("Popover Works", UIControlState.Normal);
            button1.TouchUpInside += (object sender, EventArgs e) => ShowPopoverAsync(button1);
            button1.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin;

            button2 = UIButton.FromType(UIButtonType.RoundedRect);
            button2.Frame = new RectangleF(middle, 10, buttonWidth, buttonHeight);
            button2.SetTitle("Modal Works", UIControlState.Normal);
            button2.TouchUpInside += (object sender, EventArgs e) => ShowModalAsync(button2);
            button2.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin;

            button3 = UIButton.FromType(UIButtonType.RoundedRect);
            button3.Frame = new RectangleF(middle + offset, 10, buttonWidth, buttonHeight);
            button3.SetTitle("Custom Broken", UIControlState.Normal);
            button3.TouchUpInside += (object sender, EventArgs e) => ShowCustomAsync(button3);
            button3.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin;

            View.AddSubview(button1);
            View.AddSubview(button2);
            View.AddSubview(button3);
        }

        public async void ShowPopoverAsync(NSObject sender)
        {
            // Set the sender to a UIButton.
            UIButton tappedButton = (UIButton)sender;

            var content = new PopoverContentViewController();
            DetailPopover = new UIPopoverController(content);
            DetailPopover.PopoverContentSize = new SizeF(375, 375);
            DetailPopover.PresentFromRect(tappedButton.Frame, View, UIPopoverArrowDirection.Any, true);

            // await! control returns to the caller
            var result = await WaitAsync();

            content.WebViewContent = result;
        }

        public async void ShowModalAsync(NSObject sender)
        {
            // Set the sender to a UIButton.
            UIButton tappedButton = (UIButton)sender;

            DetailModal = new ModalContentViewController();
            DetailModal.ModalPresentationStyle = MonoTouch.UIKit.UIModalPresentationStyle.FormSheet;

            this.PresentViewController(DetailModal, true, null);

            // await! control returns to the caller
            var result = await WaitAsync();

            DetailModal.WebViewContent = result;
        }

        public async void ShowCustomAsync(NSObject sender)
        {
            // Set the sender to a UIButton.
            UIButton tappedButton = (UIButton)sender;

            DetailCustom = new CustomModalViewController("Custom Modal");

            var doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Plain, (s, e) =>
            {
                DetailCustom.Dismiss(s, e);
            });
            DetailCustom.SetRightBarButton(doneButton);

            var size = new SizeF
            {
                Height = 500, Width = 400
            };
            var view = new UIView(new RectangleF(0, 0, size.Width, size.Height));

            UIWebView webView = new UIWebView(new RectangleF(0, 0, size.Width, size.Height));
            string directory = System.IO.Path.Combine(NSBundle.MainBundle.BundlePath, "Documentation/");
            string html = "<!DOCTYPE html><html><body><h1>{0}</h1></body></html>";
            webView.LoadHtmlString(string.Format(html, "Loading . . . "), new NSUrl(directory, true));
            view.Add(webView);

            var sections = new List<Section> { new Section(view) };
            DetailCustom.AddSections(sections);
            this.View.Add(DetailCustom.View);
            DetailCustom.Present();

            // await! control returns to the caller
            var result = await WaitAsync();

            webView.LoadHtmlString(string.Format(html, result), new NSUrl(directory, true));
        }

        public async Task<string> WaitAsync()
        {
            await Task.Delay(2000);

            return "Finished";
        }
    }
}

