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
using Android.Net;

using Plugin.Connectivity;



namespace App7
{
    public class Internet
    {
        //        NetworkStatus internetStatus = Reachability.InternetConnectionStatus();

        //if(!Reachability.IsHostReachable("http://google.com")) {
        //    // Put alternative content/message here
        //}
        //else
        //{
        //    // Put Internet Required Code here
        //}

        //      if (CrossConnectivity.Current.IsConnected) 
        //        {  
        //    // your logic...  
        //} else {  
        //    // write your code if there is no Internet available  
        //} 

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



    }
}
