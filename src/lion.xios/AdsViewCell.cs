using System;

using Foundation;
using UIKit;
using Google.MobileAds;

namespace Lion.XiOS
{
    public partial class AdsViewCell : UITableViewCell
    {

        public AdsViewCell(IntPtr handle) : base(handle)
        {
        }
        public void SetSelectedAnimated(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
        }
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
