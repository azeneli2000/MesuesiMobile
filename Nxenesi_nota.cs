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

namespace App7
{
   public class Nxenesi_nota
    {
        public Int64 id_shkolla { get; set; }
        public Int64 id_mesuesi { get; set; }
        public string nr_amza { get; set; }
        public string cikli { get; set; }
        public string lenda { get; set; }
        public string viti_shkollor { get; set; }
        public string vleresimi { get; set; }
        public string data { get; set; }
        public string mom { get; set; }
        public string shkrim { get; set; }
        public string projekt { get; set; }
        public string provim { get; set; }
        public string mungese { get; set; }

        public string klasa { get; set; }
        public string indeksi { get; set; }
    }
}