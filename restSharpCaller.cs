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
using RestSharp;

namespace App7
{
    class restSharpCaller
    {
        public RestClient client;
        public  restSharpCaller(string baseUrl)
        {
            client = new RestClient(baseUrl);
        }

        public List<Nxenesit_klasa> GetNxenesi()
        {
            
            //var request = new RestRequest("", Method.GET);
            //var response = client.Execute<List<person>>(request);
            //return response.Data;

            var request = new RestRequest("", Method.GET);
          //  request.RequestFormat = DataFormat.Json;
           // request.AddParameter("application/json", "{\"klasa\":\"1\",\"indeksi\":\"A\",\"id_shkolla\":\"1\",\"viti_sh\":\"2016-2017\",\"cikli\":\"True\"}" , ParameterType.RequestBody);
            request.AddParameter("?klasa =1&" + "indeksi=A&" + "id_shkolla=1&" + "viti_sh = 2016-2017&" + "cikli=True", ParameterType.UrlSegment);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
           
            var response =  client.Execute<List<Nxenesit_klasa>>(request);

            return response.Data;
        }

        public List<string> Getmesuesi()
        {

            //var request = new RestRequest("", Method.GET);
            //var response = client.Execute<List<person>>(request);
            //return response.Data;

            var request = new RestRequest("", Method.GET);
            //  request.RequestFormat = DataFormat.Json;
            // request.AddParameter("application/json", "{\"klasa\":\"1\",\"indeksi\":\"A\",\"id_shkolla\":\"1\",\"viti_sh\":\"2016-2017\",\"cikli\":\"True\"}" , ParameterType.RequestBody);
           // request.AddParameter("?id=E.Azizi", ParameterType.UrlSegment);
            // request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = client.Execute<List<string>>(request);

            return response.Data;
        }

        public List<string> Getshkolla()
        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<string>>(request);
            return response.Data;
        }

        public List<string> Get_lendet()

        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<string>>(request);
            return response.Data;
        }
        
      
     //sherbejne per te nxjerre klasat,indekset dhe lendet korrespondente nje mesuesi
        // ***********
        public List<string> Get_mesuesit_klasat()

        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<string>>(request);
            return response.Data;
        }
        public List<string> Get_mesuesit_indekset()

        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<string>>(request);
            return response.Data;
        }

        public List<string> Get_mesuesit_lendet()

        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<string>>(request);
            return response.Data;
        }

        //************


        public int Create(Nxenesi_nota nota)
        {
            var request = new RestRequest("", Method.POST);
           request.AddJsonBody(nota);
            client.Execute(request);
            return 1;
        }
        public void get_notification()
        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute(request);
        }

       
        public List<string> GetUserData()
        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<string>>(request);
            return response.Data;
        }

      

        public void Update(int id, person product)
        {
            var request = new RestRequest("Products/" + id, Method.PUT);
            request.AddJsonBody(product);
            client.Execute(request);
        }

        public void Delete(int id)
        {
            var request = new RestRequest("Products/" + id, Method.DELETE);
            client.Execute(request);
        }
    }
}