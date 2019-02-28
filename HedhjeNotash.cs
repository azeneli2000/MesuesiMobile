using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Globalization;
using AndroidHUD;
using System.Text.RegularExpressions;

namespace App7
{
  
    [Activity(Label = "HedhjeNotash")]

    public class HedhjeNotash : Activity

    {
    


        public string gjej_vitin()
        {
           

            if (DateTime.Now.Month >= 7)
                return (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            else
                return
                 (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
        }
        public string mungese ()
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            if (noatatxt.Text == "m")
                return "True";
            else
                return "False";

        }
        public string momentale()
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            if (noatatxt.Text != "m")
                return "True";
            else
                return "False";

        }

        //public override void OnBackPressed()
        //{
        //    base.OnBackPressed();
        //    Finish();
        //    //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        //}
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.prova);
            tempdata.mom = "True";
            tempdata.shkrim = "False";
            tempdata.projekt = "False";
            //emri
           
            TextView emritxt = FindViewById<TextView>(Resource.Id.textView1);
            emritxt.Text = Regex.Replace( tempdata.emri_nx.ToString(), "\\d+","").TrimEnd();
            //Lenda
            TextView lendatxt = FindViewById<TextView>(Resource.Id.textView2);
            lendatxt.Text = tempdata.emri_lenda+ " :  ";
            //klasa
            TextView klasatxt = FindViewById<TextView>(Resource.Id.textView3);
            klasatxt.Text = tempdata.klasa + tempdata.indeksi + "   ";

            Switch sw_mom = FindViewById<Switch>(Resource.Id.switch1);
            Switch sw_shkrim = FindViewById<Switch>(Resource.Id.switch2);
            Switch sw_Projekt = FindViewById<Switch>(Resource.Id.switch3);

            sw_mom.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if(e.IsChecked)
                {
                    sw_shkrim.Checked = false;
                    sw_Projekt.Checked = false;
                 }

            };

            sw_shkrim.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (e.IsChecked)
                {
                    sw_mom.Checked = false;
                    sw_Projekt.Checked = false;
                    tempdata.mom = "False";
                    tempdata.shkrim = "True";
                    tempdata.projekt = "False";
                }

            };

            sw_Projekt.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (e.IsChecked)
                {
                    sw_shkrim.Checked = false;
                    sw_mom.Checked = false;
                    tempdata.mom = "False";
                    tempdata.shkrim = "False";
                    tempdata.projekt = "True";
                }

            };







            Button nota4 = FindViewById<Button>(Resource.Id.button1);
            nota4.Click  += nota4_Click;



            Button nota5 = FindViewById<Button>(Resource.Id.button2);
            nota5.Click += nota5_Click;


            Button nota6 = FindViewById<Button>(Resource.Id.button3);
            nota6.Click += nota6_Click;

            Button nota7 = FindViewById<Button>(Resource.Id.button4);
            nota7.Click += nota7_Click;

            Button nota8 = FindViewById<Button>(Resource.Id.button5);
            nota8.Click += nota8_Click;

            Button nota9 = FindViewById<Button>(Resource.Id.button6);
            nota9.Click += nota9_Click;

            Button nota10 = FindViewById<Button>(Resource.Id.button7);
            nota10.Click += nota10_Click;


            Button notam = FindViewById<Button>(Resource.Id.button8);
            notam.Click += notam_Click;


            Button notaOK = FindViewById<Button>(Resource.Id.button9);
            notaOK.Click += notaOK_Click;
        }

        private async void notaOK_Click(object sender, EventArgs e)
        {
            TextView emritxt = FindViewById<TextView>(Resource.Id.textView1);
            emritxt.Text = Regex.Replace(tempdata.emri_nx.ToString(), "\\d+", "").TrimEnd();
            bool conn = Internet.internetConnectionCheck(this);
            if (!conn)
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Nuk jeni te lidhur me internet !", MaskType.Clear, TimeSpan.FromSeconds(2));
               
                return;
            }
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            if (noatatxt.Text == "")
                return;
            Nxenesi_nota nota = new Nxenesi_nota();
            nota.id_shkolla =Convert.ToInt64( tempdata.id_shkolla);
            nota.id_mesuesi = Convert.ToInt64(tempdata.id_mesuesi);
            nota.nr_amza = tempdata.amza;
            nota.cikli = tempdata.cikli;
            nota.lenda = tempdata.emri_lenda;
            nota.viti_shkollor = gjej_vitin();
            nota.vleresimi = noatatxt.Text;
            nota.data = DateTime.Now.ToString();
            nota.mom = tempdata.mom;
            nota.shkrim = tempdata.shkrim;
            nota.projekt = tempdata.projekt;
            nota.mungese = mungese();
            nota.provim = "False";
            nota.klasa = tempdata.klasa;
            nota.indeksi = tempdata.indeksi;
            //var caller4 = new restSharpCaller(" http://localhost:6349/api/Nota/");
            
            var caller4 = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Nota/");

            //***********************
            AndHUD.Shared.Show(this, "Prisni...", 0, MaskType.Clear);

            Task<int> task1 = new Task<int>(()=>caller4.Create(nota));
            task1.Start();
            int j = await task1;
            //caller4.Create(nota);
            //gjen userin per te derguar noten me notification
            List<string> perd_l = new List<string>();
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
            AndHUD.Shared.Show(this, "Prisni...", 30, MaskType.Clear);
            //******************************************************
            var caller_user = new restSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/UserAmza/?nr_amza=" + nota.nr_amza + "&cikli=" + nota.cikli + "&id_shkolla="+tempdata.id_shkolla);
            Task<List<string>> task2 = new Task<List<string>>(caller_user.GetUserData);
            task2.Start();
            List<string> tmp = await task2;
            AndHUD.Shared.Show(this, "Prisni...", 100, MaskType.Clear,TimeSpan.FromSeconds(2));


            if (tmp.Count > 0)
            {
                string p = tmp[0].ToString();
                string datastring = tmp[1];//data e regjistrimit
                int dite_falas = Convert.ToInt32( caller_user.GetUserData()[2]);//ditet falas
                int dite_paguar = Convert.ToInt32(tmp[2]);//ditet paguar
                                                                           
                DateTime dateValue;
                DateTime d = DateTime.Now;


                if (DateTime.TryParseExact(datastring, formats, new CultureInfo("en-US"),DateTimeStyles.None,out dateValue))
                    d = dateValue.AddDays(dite_falas+dite_paguar);
                else
                {
                    //msgbox gabimi
                }

                //nqs userit i ka skaduar abonimi
                if (d >= DateTime.Now)
                {

                    var caller5 = new restSharpCaller("http://webapi320171012080657.azurewebsites.net/api/Notifications/?message=" + emritxt.Text + "  " + nota.lenda + " : " + nota.vleresimi + "&perdoruesi=" + p);
                    caller5.get_notification();

                }
                else //dergon nje otification paralajmerues
                {
                    var caller5 = new restSharpCaller("http://webapi320171012080657.azurewebsites.net/api/Notifications/?message=" + " Nxenesi : " + emritxt.Text + "ka marre note nqs doni te aktivizoni sherbin kontaktoni shkollen ose klikoni www.example.com " + "&perdoruesi=" + p);
                    caller5.get_notification();

                }







            }
                tempnota.vleresimi[tempnota.pozicioni] = noatatxt.Text;
            AndHUD.Shared.Dismiss();
            var intent = new Intent(this, typeof(Nxenesit));
            StartActivity(intent);
            Finish();
          

        }
        private void notam_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "m";
        }

        private void nota10_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "10";
        }

        private void nota9_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "9";
        }

        private void nota8_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "8";
        }

        private void nota7_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "7";
        }

        private void nota6_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "6";
        }

        private void nota5_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "5";
        }

        private void nota4_Click(object sender, EventArgs e)
        {
            TextView noatatxt = FindViewById<TextView>(Resource.Id.textView4);
            noatatxt.Text = "4";
        }
    }
}