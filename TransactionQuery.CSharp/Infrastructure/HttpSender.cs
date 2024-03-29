﻿using System;
using System.Text;
using System.Net;
using System.IO;

namespace TransactionQuery.CSharp
{
    public class HttpSender
    {
        public string Send(string data, string url, string action)
        {
            string result = string.Empty;

            var byteData = Encoding.ASCII.GetBytes(data);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (!String.IsNullOrEmpty(action))
            {
                webRequest.Headers.Add("SOAPAction", action);
            }
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.ContentLength = data.Length;

            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
            }

            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
            asyncResult.AsyncWaitHandle.WaitOne();
            
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }

            return result;
        }
    }
}
