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
using System.Threading.Tasks;

namespace App7
{
    [Activity(Label = "Lenda")]
    public class Lenda : Activity
    {
        private List<string> klasa = new List<string>();
        private List<string> indeksi = new List<string>();
        private List<string> lendet = new List<string>();
        private Spinner sp1;
        private Spinner sp2;
        private Spinner sp3;
        private TextView tv_shkolla;
        private TextView tv_mesuesi;
        private Button vazhdo;

        public string gjej_vitin()
        {
            if (DateTime.Now.Month >= 7)
                return (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            else
                return
                 (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
        }
        //public override void OnBackPressed()
        //{
        //    base.OnBackPressed();
        //    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        //}
        protected  override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Lenda);
            //TextView emritxt = FindViewById<TextView>(Resource.Id.textView1);
            //emritxt.Text = tempdata.emri;


            tempnota pozicioni = new tempnota();


            //shkolla
            tv_shkolla = FindViewById<TextView>(Resource.Id.textView14);
            tv_shkolla.Text = "Shkolla : " + tempdata.emr_shkolla;
            //mesuesi
            tv_mesuesi= FindViewById<TextView>(Resource.Id.textView54);
            tv_mesuesi.Text = "Mesuesi : " + tempdata.emri_mesuesi;
            //klasa
            sp1 = FindViewById<Spinner>(Resource.Id.spinner1);
            sp1.ItemSelected += new EventHandler< AdapterView.ItemSelectedEventArgs >(sp1_itemselected);
            var caller_klasa = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/MesuesiKlasa/?id_mesuesi="+tempdata.id_mesuesi);
            //AndHUD.Shared.Show(this, "Prisni...", 50, MaskType.Clear);
            //Task<List<string>> task1 = new Task<List<string>>(caller_klasa.Get_mesuesit_klasat);
            //task1.Start();
            //klasa = await task1;

            klasa = caller_klasa.Get_mesuesit_klasat();
            ArrayAdapter <string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, klasa);
            sp1.Adapter = adapter;
            tempdata.klasa = klasa[0];
            //butoni vazhdo

           
            
            //indeksi
            sp2 = FindViewById<Spinner>(Resource.Id.spinner2);
            sp2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(sp2_itemselected);
            var caller_indeksi = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/MesuesiIndeksi/?id_mesuesi=" + tempdata.id_mesuesi+"&klasa=" + klasa[0]);
            indeksi = caller_indeksi.Get_mesuesit_indekset();
            ArrayAdapter<string> adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, indeksi);
            sp2.Adapter = adapter1;
            tempdata.indeksi = indeksi[0];
            //mbush lendet
           
            var caller_lendet = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/MesuesiLenda/?id_mesuesi=" + tempdata.id_mesuesi + "&klasa=" + klasa[0]+"&indeksi="+indeksi[0]);
            //AndHUD.Shared.Show(this, "Prisni...", 100, MaskType.Clear);
            //Task <List<string>> task2 = new Task<List<string>>(caller_lendet.Get_mesuesit_lendet);
            //task2.Start();
            //lendet = await task2;


            lendet = caller_lendet.Get_mesuesit_lendet();
            sp3 = FindViewById<Spinner>(Resource.Id.spinner3);
            sp3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(sp3_itemselected);
            ArrayAdapter<string> adapter_l = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, lendet);
            sp3.Adapter = adapter_l;
            vazhdo = FindViewById<Button>(Resource.Id.vazhdo);
            vazhdo.Click += Vazhdo_click;
            AndHUD.Shared.Dismiss();        
        }

        private void Vazhdo_click(object sender, EventArgs e)
        {
            bool conn = Internet.internetConnectionCheck(this);
            if (!conn)
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Nuk jeni te lidhur me internet !", MaskType.Clear, TimeSpan.FromSeconds(2));
                return;
            }


            tempnota.vleresimi.Clear();
           for(int i=0;i<=60;i++)
            {
                tempnota.vleresimi.Add("");

            }              
           var intent = new Intent(this, typeof(Nxenesit));
            StartActivity(intent);
        }

        private void sp3_itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner lenda = (Spinner)sender;
            tempdata.emri_lenda = lenda.GetItemAtPosition(e.Position).ToString();
        }

        //indeksi
        private void sp2_itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner indeksi = (Spinner)sender;
            tempdata.indeksi = indeksi.GetItemAtPosition(e.Position).ToString();
            //mbush lendet
            var caller_lendet = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/MesuesiLenda/?id_mesuesi=" + tempdata.id_mesuesi + "&klasa=" + tempdata.klasa + "&indeksi=" + tempdata.indeksi);
            lendet = caller_lendet.Get_mesuesit_lendet();
            ArrayAdapter<string> adapter_l = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, lendet);
            sp3.Adapter = adapter_l;
            vazhdo = FindViewById<Button>(Resource.Id.vazhdo);
            if (lendet.Count == 0)
            {
                vazhdo.Enabled = false;

            }
            else
            {
                vazhdo.Enabled = true;
            }
        }
        //klasa
        private void sp1_itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner klasa = (Spinner)sender;
            tempdata.klasa =  klasa.GetItemAtPosition(e.Position).ToString();
            if(Convert.ToInt16( tempdata.klasa) < 10)
            {
                tempdata.cikli = "True"; 
            }
            else
            {
                tempdata.cikli = "False";
            }

            //mbush indekset
            var caller_indeksi = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/MesuesiIndeksi/?id_mesuesi=" + tempdata.id_mesuesi + "&klasa=" + tempdata.klasa);
            indeksi = caller_indeksi.Get_mesuesit_indekset();
            ArrayAdapter<string> adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, indeksi);
            sp2.Adapter = adapter1;

            //mbush lendet
            var caller_lendet = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/MesuesiLenda/?id_mesuesi=" + tempdata.id_mesuesi + "&klasa=" + tempdata.klasa + "&indeksi=" + tempdata.indeksi);
            lendet = caller_lendet.Get_mesuesit_lendet();
            ArrayAdapter<string> adapter_l = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, lendet);
            sp3.Adapter = adapter_l;

            vazhdo = FindViewById<Button>(Resource.Id.vazhdo);
            if (lendet.Count == 0)
            {
                vazhdo.Enabled = false;
                
            }
            else
            {
                vazhdo.Enabled = true;
            }
        }
    }
}