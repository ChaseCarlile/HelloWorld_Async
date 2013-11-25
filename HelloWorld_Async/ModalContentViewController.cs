using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace HelloWorld_Async
{
    public class ModalContentViewController : BaseContentViewController 
    { 
        protected UITapGestureRecognizer _dismissRecognizer;

        public ModalContentViewController()
            : base()
        {
            this.View.BackgroundColor = UIColor.Clear;
            _dismissRecognizer = new UITapGestureRecognizer(OnTapOutside);
            _dismissRecognizer.NumberOfTapsRequired = 1u;
            _dismissRecognizer.CancelsTouchesInView = false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.View.AutosizesSubviews = true;
            View.Window.AddGestureRecognizer(_dismissRecognizer);
        }

        private void OnTapOutside(UITapGestureRecognizer recogniser)
        {
            if (recogniser.State == UIGestureRecognizerState.Ended)
            {
                var window = View.Window;
                DismissViewController(true, () => window.RemoveGestureRecognizer(_dismissRecognizer));
            }
        }
    }
}