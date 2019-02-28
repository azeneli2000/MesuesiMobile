using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidHUD;

namespace App7
{
    [Activity(Label = "Nxenesit")]


    public class Nxenesit : Activity
    {
        public string gjej_vitin()
        {
            if (DateTime.Now.Month >= 7)
                return (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            else
                return
                 (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
        }
        private TextView tv_klasa;

        private ListView lv_nxenesit;
        private ListView lv_amza;

        private List<Nxenesit_klasa> nx_list = new List<Nxenesit_klasa>();
        private List<string> emri = new List<string>();
        private List<string> amza = new List<string>();

        //public override void OnBackPressed()
        //{
        //    base.OnBackPressed();
        //    Finish();
        //    //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        //}
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Nxenesit);
           


            tv_klasa = FindViewById<TextView>(Resource.Id.textView1);
            tv_klasa.Text = "Klasa : " + tempdata.klasa + tempdata.indeksi;
            lv_nxenesit = FindViewById<ListView>(Resource.Id.listView1);
            //lv_amza = FindViewById<ListView>(Resource.Id.listView2);
            var caller4 = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Nxenesi/?klasa=" + tempdata.klasa + "&indeksi=" + tempdata.indeksi + "&id_shkolla=" + tempdata.id_shkolla + "&viti_sh=" + gjej_vitin() + "&cikli=" + tempdata.cikli);
            nx_list = caller4.GetNxenesi();
            int i = 0;  int j = 0; // i indeksi i listview j indeksi i listes se listes se vlersimeve
            int p;

            foreach (Nxenesit_klasa nx in nx_list)

            {

                emri.Add(nx.emri + "     " + tempnota.vleresimi[i]);
                amza.Add(nx.nr_amza);
                i++;

            }
            ArrayAdapter<string> adapter_lv1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, emri);
            lv_nxenesit.Adapter = adapter_lv1;
            ArrayAdapter<string> adapter_lv2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, amza);
            //lv_amza.Adapter = adapter_lv2;
            lv_nxenesit.ItemClick  += Lv_nx_ItemClick;

        }

        private void Lv_nx_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            bool conn = Internet.internetConnectionCheck(this);
            if (!conn)
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Nuk jeni te lidhur me internet !", MaskType.Clear, TimeSpan.FromSeconds(2));
                return;
            }
            string emri_nx = emri[e.Position].ToString();
            string nr_am = amza[e.Position].ToString();
            tempdata.emri_nx = emri_nx;
            tempdata.amza = nr_am;
            
            tempnota.pozicioni=e.Position; 
            
            var intent = new Intent(this, typeof(HedhjeNotash));
            StartActivity(intent);
            Finish();
        }


    }
}