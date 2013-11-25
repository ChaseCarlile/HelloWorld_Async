using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace HelloWorld_Async
{
    public sealed class CustomModalViewController : UIViewController
    {
        public RectangleF ToolbarFrame
        {
            get { return _dialogViewController.NavigationController.Toolbar.Frame; }
        }

        public UINavigationController RootNavigationController { get; set; }

        private DialogViewController _dialogViewController;


        public CustomModalViewController(string title)
        {
            var root = new RootElement(title) { UnevenRows = true };

            if (_dialogViewController == null)
                _dialogViewController = new DialogViewController(UITableViewStyle.Plain, root);
            if (RootNavigationController == null)
                RootNavigationController = new UINavigationController(_dialogViewController);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, Dismiss);
            doneButton.Style = UIBarButtonItemStyle.Done;
            _dialogViewController.NavigationItem.RightBarButtonItem = doneButton;
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            if (RootNavigationController.View.Superview != null)
            {
                RootNavigationController.View.Superview.AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin;

                RootNavigationController.View.Superview.Bounds = new RectangleF(
                        0,
                        -ToolbarFrame.Height / 2,
                        RootNavigationController.View.Superview.Frame.Width - 20,
                        RootNavigationController.View.Superview.Frame.Height - ToolbarFrame.Height
                        );
            }
        }


        public void AddSections(List<Section> sections)
        {
            _dialogViewController.Root.Add(sections);
        }

        public void Present(/*callback*/)
        {
            RootNavigationController.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
            RootNavigationController.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
            PresentViewController(RootNavigationController, true, null);
        }

        public void Dismiss(object sender, EventArgs e)
        {
           DismissViewController(true, null);
           View.RemoveFromSuperview();
        }

        public void SetRightBarButton(UIBarButtonItem uiBarButtonItem)
        {
            _dialogViewController.NavigationItem.RightBarButtonItem = uiBarButtonItem;
        }

        public void SetLeftBarButton(UIBarButtonItem uiBarButtonItem)
        {
            _dialogViewController.NavigationItem.LeftBarButtonItem = uiBarButtonItem;
        }

    }
}
