using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Lion.XDroid.Control;
using Lion.XDroid.Libs;

namespace Lion.XDroid
{
    [Activity(Label = "MainActivity", Theme = "@style/DefaultTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        MainPagerAdapter mainPagerAdapter;

        int prev_tab_position = 0;

        TabLayout main_tab_layout;
        ViewPager main_view_pager;

        Color selected_color = Color.ParseColor("#3296DC");
        Color unselect_color = Color.ParseColor("#9c9c9c");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            main_tab_layout = FindViewById<TabLayout>(Resource.Id.tl_main);
            main_view_pager = FindViewById<ViewPager>(Resource.Id.vp_main);

            this.InitTabLayout();
            this.InitEvent();
            this.InitAdMob();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (this.mainPagerAdapter.IsAnonymous(prev_tab_position) == false)
            {
                var app_cache = new AppPreferences(this.ApplicationContext);
                var _token = app_cache.UserTokenKey;
                if (AppCommon.SNG.CheckExpired(this.ApplicationContext, _token) == false)
                {
                    var _fragment = (IPagerFragment)this.mainPagerAdapter.GetItem(prev_tab_position);
                    _fragment.Initialize();
                }
                else
                    prev_tab_position = 0;
            }

            var tab = main_tab_layout.GetTabAt(prev_tab_position);
            tab.Select();

            main_view_pager.CurrentItem = prev_tab_position;
        }

        private void InitAdMob()
        {
            MobileAds.Initialize(this.ApplicationContext, GetString(Resource.String.banner_ad_unit_id));
            var mAdView = FindViewById<AdView>(Resource.Id.adView);
            AdRequest adRequest = (new AdRequest.Builder()).Build();
            mAdView.LoadAd(adRequest);
        }

        private void TabColorToggle(int tab_position, bool selected)
        {
            var _color = selected_color;
            if (selected == false)
                _color = unselect_color;

            var _tab = main_tab_layout.GetTabAt(tab_position);
            {
                var _text = _tab.CustomView.FindViewById<TextView>(Resource.Id.tv_tab);
                _text.SetTextColor(_color);

                var _image = _tab.CustomView.FindViewById<ImageView>(Resource.Id.iv_tab);
                _image.SetColorFilter(_color, PorterDuff.Mode.SrcIn);
            }
        }

        private void InitEvent()
        {
            main_tab_layout.TabSelected += (sender, e) =>
            {
                TabColorToggle(prev_tab_position, false);

                //Log.Debug(this.GetType().Name, $"InitEvent ({prev_tab_position}) => ({e.Tab.Position})");
                prev_tab_position = e.Tab.Position;

                if (this.mainPagerAdapter.IsAnonymous(e.Tab.Position) == false)
                {
                    var app_cache = new AppPreferences(this.ApplicationContext);
                    var _token = app_cache.UserTokenKey;
                    if (AppCommon.SNG.CheckExpired(this.ApplicationContext, _token) == true)
                    {
                        AppCommon.SNG.JumpLoginActivity(this.ApplicationContext);
                        return;
                    }
                }

                main_view_pager.CurrentItem = e.Tab.Position;

                var _fragment = (IPagerFragment)this.mainPagerAdapter.GetItem(e.Tab.Position);
                _fragment.Initialize();

                TabColorToggle(e.Tab.Position, true);
            };
        }

        private void InitTabLayout()
        {
            this.mainPagerAdapter = new MainPagerAdapter(this.SupportFragmentManager);
            {
                var _winner = new ResultPagerFragment();
                {
                    this.mainPagerAdapter.AddFragment(_winner, "��÷���", true);
                }

                var _number = new NumPagerFragment();
                {
                    this.mainPagerAdapter.AddFragment(_number, "��ȣ����", false);
                }

                var _store = new StorePagerFragment();
                {
                    this.mainPagerAdapter.AddFragment(_store, "�ζ��Ǹ���", true);
                }

                var _setting = new SettingPagerFragment();
                {
                    this.mainPagerAdapter.AddFragment(_setting, "����", false);

                    _setting.Refresh += (s, e) =>
                    {
                        TabColorToggle(prev_tab_position, false);

                        this.OnResume();
                    };
                }
            }

            main_view_pager.Adapter = this.mainPagerAdapter;

            main_tab_layout.SetupWithViewPager(main_view_pager);

            this.SetupTabIcons(main_tab_layout);
        }

        private void SetupTabIcons(TabLayout tablayout)
        {
            View view0 = LayoutInflater.From(this).Inflate(Resource.Layout.main_tab, null);
            {
                var iv_tab0 = view0.FindViewById<ImageView>(Resource.Id.iv_tab);
                var tv_tab0 = view0.FindViewById<TextView>(Resource.Id.tv_tab);
                tv_tab0.SetText(this.mainPagerAdapter.GetPageTitle(0));
                iv_tab0.SetImageResource(Resource.Drawable.main1);
                tv_tab0.SetTextColor(this.selected_color);
                iv_tab0.SetColorFilter(this.selected_color, PorterDuff.Mode.SrcIn);
                tablayout.GetTabAt(0).SetCustomView(view0);
            }

            View view1 = LayoutInflater.From(this).Inflate(Resource.Layout.main_tab, null);
            {
                var iv_tab1 = view1.FindViewById<ImageView>(Resource.Id.iv_tab);
                var tv_tab1 = view1.FindViewById<TextView>(Resource.Id.tv_tab);
                tv_tab1.SetText(this.mainPagerAdapter.GetPageTitle(1));
                iv_tab1.SetImageResource(Resource.Drawable.main2);
                tv_tab1.SetTextColor(this.unselect_color);
                iv_tab1.SetColorFilter(this.unselect_color, PorterDuff.Mode.SrcIn);
                tablayout.GetTabAt(1).SetCustomView(view1);
            }

            View view2 = LayoutInflater.From(this).Inflate(Resource.Layout.main_tab, null);
            {
                var iv_tab2 = view2.FindViewById<ImageView>(Resource.Id.iv_tab);
                var tv_tab2 = view2.FindViewById<TextView>(Resource.Id.tv_tab);
                tv_tab2.SetText(this.mainPagerAdapter.GetPageTitle(2));
                iv_tab2.SetImageResource(Resource.Drawable.main3);
                tv_tab2.SetTextColor(this.unselect_color);
                iv_tab2.SetColorFilter(this.unselect_color, PorterDuff.Mode.SrcIn);
                tablayout.GetTabAt(2).SetCustomView(view2);
            }

            View view3 = LayoutInflater.From(this).Inflate(Resource.Layout.main_tab, null);
            {
                var iv_tab3 = view3.FindViewById<ImageView>(Resource.Id.iv_tab);
                var tv_tab3 = view3.FindViewById<TextView>(Resource.Id.tv_tab);
                tv_tab3.SetText(this.mainPagerAdapter.GetPageTitle(3));
                iv_tab3.SetImageResource(Resource.Drawable.main4);
                tv_tab3.SetTextColor(this.unselect_color);
                iv_tab3.SetColorFilter(this.unselect_color, PorterDuff.Mode.SrcIn);
                tablayout.GetTabAt(3).SetCustomView(view3);
            }
        }
    }
}