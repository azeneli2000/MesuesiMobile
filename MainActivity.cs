using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System;
using Android.Content;
using System.Net.Http;


using System.Net.Http.Headers;




using System.Web;
using Android.Views.InputMethods;
using System.Threading;
using AndroidHUD;
using Plugin.Connectivity;
using Android.Net;

namespace App7
{
    [Activity(Label = "Mesuesi", MainLauncher = true, Icon = "@drawable/iconandi")]
   
    public class MainActivity : Activity
    {
        public static bool internetConnectionCheck(Activity CurrentActivity)
        {
            bool Connected = false;
            ConnectivityManager connectivity = (ConnectivityManager)CurrentActivity
                .GetSystemService(Context.ConnectivityService);
            if (connectivity != null)
            {
                NetworkInfo[] info = connectivity.GetAllNetworkInfo();
                if (info != null) for (int i = 0; i < info.Length; i++)
                        if (info[i].GetState() == NetworkInfo.State.Connected)
                        {

                            Connected = true;
                        }
                        else { }
            }
            else
            {

                Connected = false;

            }
            return Connected;
        }






        private List<string> myitems;
        private ListView lv;
        private Button hyrje;
       
        protected override void OnCreate(Bundle bundle)
        {
           
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
        
          
            TextView user = FindViewById<EditText>(Resource.Id.username);
            TextView pas = FindViewById<EditText>(Resource.Id.pass);
        
            user.Text = Settings.GeneralSettings;
            if (user.Text!="")
            pas.RequestFocus();
          
            hyrje = FindViewById<Button>(Resource.Id.login);
            hyrje.Click += Hyrje_Click;

        }
       
        private async void Hyrje_Click(object sender, EventArgs e)
        {
         
            bool conn = internetConnectionCheck(this);
            if(!conn)
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Nuk jeni te lidhur me internet !", MaskType.Clear, TimeSpan.FromSeconds(2));
                return;
            }

            TextView user = FindViewById<EditText>(Resource.Id.username);
            string perd = user.Text;
            if (Settings.GeneralSettings == ""||Settings.GeneralSettings != user.Text)
                Settings.GeneralSettings = perd;
            TextView pas = FindViewById<EditText>(Resource.Id.pass);

            var caller = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Mesuesi/?id=" + perd);
            List<string> mesuesi = new List<string>();
            List<string> shkolla_emri = new List<string>();
            List<string> lendet = new List<string>();
            //********
            AndHUD.Shared.Show(this, "Prisni...", 30, MaskType.Clear);
            Task<List<string>> task1 = new Task<List<string>>(caller.Getmesuesi);
            task1.Start();
            //mesuesi = caller.Getmesuesi();
            mesuesi = await task1;
            AndHUD.Shared.Show(this, "Prisni...", 50, MaskType.Clear);
            if (mesuesi.Count > 0)
            {
                string password = mesuesi[4].ToString();
                if (pas.Text.Trim() == password)
                {
                    tempdata.emri_mesuesi = mesuesi[0].ToString() + " " + mesuesi[1].ToString(); ;
                    tempdata.id_shkolla = mesuesi[2].ToString();
                    tempdata.id_mesuesi = mesuesi[3].ToString();
                    var caller1 = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Shkolla/?id=" + tempdata.id_shkolla);
                    //************************************
                    AndHUD.Shared.Show(this, "Prisni...", 100, MaskType.Clear);
                    Task<List<string>> task2 = new Task<List<string>>(caller1.Getshkolla);
                    task2.Start();
                    shkolla_emri = await task2;
                    //shkolla_emri = caller1.Getshkolla();
                   
                    tempdata.emr_shkolla = shkolla_emri[0];
                   

                    AndHUD.Shared.Dismiss();
                    var intent = new Intent(this, typeof(Lenda));
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    AndHUD.Shared.ShowErrorWithStatus(this, "Fjalekalim i gabuar !", MaskType.Clear,TimeSpan.FromSeconds(2));
                    pas.Text = "";
                   // AndHUD.Shared.Dismiss();
                }
            }
            else
                user.Text = "";
            pas.Text = "";
            }

    
        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
               string s = myitems[e.Position].ToString();
               // tempdata.emri = s;
                var intent = new Intent(this, typeof(HedhjeNotash));
                StartActivity(intent);
            
        }
    }
}

