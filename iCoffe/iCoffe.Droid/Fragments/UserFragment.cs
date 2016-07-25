using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDiag = System.Diagnostics;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Actions;

using iCoffe.Shared;
using iCoffe.Droid.Adapters;

namespace iCoffe.Droid.Fragments
{
    public class UserFragment : Fragment, IOnMapReadyCallback
    {
        User user;

        MapView mapView;
        GoogleMap map;

        ListView bonusListView;
        UserBonusAdapter bonusAdapter;
        IList<BonusOffer> bonuses;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            user = new User() { City = @"<No City>", FirstName = @"<No First>", LastName = @"<No Last>" };

            ISharedPreferences prefs = Activity.GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
            string userSer = prefs.GetString(SignInActivity.C_USER, string.Empty);
            if (!string.IsNullOrEmpty(userSer))
            {
                user = Data.DeserializeUser(userSer);
            }

            //return base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.UserFragment, container, false);
            bonusListView = view.FindViewById<ListView>(Resource.Id.ufListView);

            view.FindViewById<TextView>(Resource.Id.ufUserNameTV).Text = user.LastName + @" " + user.FirstName;
            view.FindViewById<Button>(Resource.Id.ufExitB).Click += ExitButton_Click;

            mapView = view.FindViewById<MapView>(Resource.Id.ufUserMap);
            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this); //this is important

            return view;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.UiSettings.ZoomControlsEnabled = true;  // GetUiSettings().setZoomControlsEnabled(true);
            RecreateMarkers();
        }

        public void RecreateMarkers()
        {
            if (map != null)
            {
                map.Clear();

                //LatLng pos = new LatLng(54.974362, 73.418061);
                LatLng pos = new LatLng(54.9748227, 73.4099986);
                map.AddMarker(new MarkerOptions().SetPosition(pos).SetTitle(@"����")); // addMarker(new MarkerOptions().position(/*some location*/));

                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(pos, 12)); // moveCamera(CameraUpdateFactory.newLatLngZoom(/*some location*/, 10));
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SDiag.Debug.Print("UserAvatar_Click called");

            ISharedPreferences prefs = Activity.GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(MainActivity.C_ACCESS_TOKEN, string.Empty);
            editor.Apply();
            StartActivity(new Intent(Activity, typeof(SignInActivity)));
        }

        public override void OnPause()
        {
            base.OnPause();
            mapView.OnPause();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            mapView.OnDestroy();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView.OnSaveInstanceState(outState);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView.OnLowMemory();
        }

        public override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();

            RecreateAdapter();
        }

        public void RecreateAdapter()
        {
            // get data
            bonuses = Data.Offers.Take(5).ToList();

            // create our adapter
            bonusAdapter = new UserBonusAdapter(Activity, bonuses);

            //Hook up our adapter to our ListView
            Activity.RunOnUiThread(() => bonusListView.Adapter = bonusAdapter);
        }

    }
}