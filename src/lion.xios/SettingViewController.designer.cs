// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Lion.XiOS
{
    [Register ("SettingViewController")]
    partial class SettingViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell cell1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell cell2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell cell3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell inboxCell { get; set; }

        private void ReleaseDesignerOutlets ()
        {
            if (cell1 != null) {
                cell1.Dispose ();
                cell1 = null;
            }

            if (cell2 != null) {
                cell2.Dispose ();
                cell2 = null;
            }

            if (cell3 != null) {
                cell3.Dispose ();
                cell3 = null;
            }

            if (inboxCell != null) {
                inboxCell.Dispose ();
                inboxCell = null;
            }
        }
    }
}