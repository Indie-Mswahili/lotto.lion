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
    [Register ("PrizeViewController")]
    partial class PrizeViewController
    {
        [Outlet]
        UIKit.UIImageView imgNum1 { get; set; }

        [Outlet]
        UIKit.UIImageView imgNum2 { get; set; }

        [Outlet]
        UIKit.UIImageView imgNum3 { get; set; }

        [Outlet]
        UIKit.UIImageView imgNum4 { get; set; }

        [Outlet]
        UIKit.UIImageView imgNum5 { get; set; }

        [Outlet]
        UIKit.UIImageView imgNum6 { get; set; }

        [Outlet]
        UIKit.UIImageView imgNum7 { get; set; }

        [Outlet]
        UIKit.UIView internetView { get; set; }

        [Outlet]
        UIKit.UILabel lblDate { get; set; }

        [Outlet]
        UIKit.UILabel lblDate2 { get; set; }

        [Outlet]
        UIKit.UILabel lblNum1 { get; set; }

        [Outlet]
        UIKit.UILabel lblNum2 { get; set; }

        [Outlet]
        UIKit.UILabel lblNum3 { get; set; }

        [Outlet]
        UIKit.UILabel lblNum4 { get; set; }

        [Outlet]
        UIKit.UILabel lblNum5 { get; set; }

        [Outlet]
        UIKit.UILabel lblNum6 { get; set; }

        [Outlet]
        UIKit.UILabel lblNum7 { get; set; }

        [Outlet]
        UIKit.UILabel lblPrize { get; set; }

        [Outlet]
        UIKit.UILabel lblPrize2 { get; set; }

        [Outlet]
        UIKit.UILabel lblSeq { get; set; }

        [Outlet]
        UIKit.UILabel lblSeq2 { get; set; }

        [Outlet]
        UIKit.UIActivityIndicatorView myLoadIndicator { get; set; }

        [Outlet]
        UIKit.UITableView prizeTableView { get; set; }

        [Outlet]
        UIKit.UIView SeqView { get; set; }

        [Action ("btnCloseAction:")]
        partial void BtnCloseAction (UIKit.UIButton sender);

        [Action ("btnOkAction:")]
        partial void BtnOkAction (UIKit.UIButton sender);

        [Action ("btnTurnAction:")]
        partial void BtnTurnAction (UIKit.UIButton sender);

        private void ReleaseDesignerOutlets ()
        {
            if (imgNum1 != null) {
                imgNum1.Dispose ();
                imgNum1 = null;
            }

            if (imgNum2 != null) {
                imgNum2.Dispose ();
                imgNum2 = null;
            }

            if (imgNum3 != null) {
                imgNum3.Dispose ();
                imgNum3 = null;
            }

            if (imgNum4 != null) {
                imgNum4.Dispose ();
                imgNum4 = null;
            }

            if (imgNum5 != null) {
                imgNum5.Dispose ();
                imgNum5 = null;
            }

            if (imgNum6 != null) {
                imgNum6.Dispose ();
                imgNum6 = null;
            }

            if (imgNum7 != null) {
                imgNum7.Dispose ();
                imgNum7 = null;
            }

            if (internetView != null) {
                internetView.Dispose ();
                internetView = null;
            }

            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (lblDate2 != null) {
                lblDate2.Dispose ();
                lblDate2 = null;
            }

            if (lblNum1 != null) {
                lblNum1.Dispose ();
                lblNum1 = null;
            }

            if (lblNum2 != null) {
                lblNum2.Dispose ();
                lblNum2 = null;
            }

            if (lblNum3 != null) {
                lblNum3.Dispose ();
                lblNum3 = null;
            }

            if (lblNum4 != null) {
                lblNum4.Dispose ();
                lblNum4 = null;
            }

            if (lblNum5 != null) {
                lblNum5.Dispose ();
                lblNum5 = null;
            }

            if (lblNum6 != null) {
                lblNum6.Dispose ();
                lblNum6 = null;
            }

            if (lblNum7 != null) {
                lblNum7.Dispose ();
                lblNum7 = null;
            }

            if (lblPrize != null) {
                lblPrize.Dispose ();
                lblPrize = null;
            }

            if (lblPrize2 != null) {
                lblPrize2.Dispose ();
                lblPrize2 = null;
            }

            if (lblSeq != null) {
                lblSeq.Dispose ();
                lblSeq = null;
            }

            if (lblSeq2 != null) {
                lblSeq2.Dispose ();
                lblSeq2 = null;
            }

            if (myLoadIndicator != null) {
                myLoadIndicator.Dispose ();
                myLoadIndicator = null;
            }

            if (prizeTableView != null) {
                prizeTableView.Dispose ();
                prizeTableView = null;
            }

            if (SeqView != null) {
                SeqView.Dispose ();
                SeqView = null;
            }
        }
    }
}