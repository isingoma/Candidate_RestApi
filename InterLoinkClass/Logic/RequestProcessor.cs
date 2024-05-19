using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InterLoinkClass.Logic
{
    public class RequestProcessor
    {
        HttpListener listener;
        public string url = "http://localhost:8969/candidateregistration/apiv1/";

        public void ProcessTraffic()
        {
            try
            {
                listener = new HttpListener();

                listener.Prefixes.Add(url);


                listener.Start();
                Console.WriteLine("************************************************");
                Console.WriteLine("Listening For an HTTP Request...");
                Console.WriteLine("************************************************");


                while (true)
                {
                    try
                    {

                        HttpListenerContext context = listener.GetContext();
                        Thread workerThread = new Thread(new ParameterizedThreadStart(HandleRequest));
                        workerThread.Start(context);

                        //HandleRequest(context);
                    }
                    catch (Exception ex)
                    {
                        //log Errors into a file;
                        Console.WriteLine(ex.Message);

                    }
                }
            }
            catch (Exception ex)
            {
                //log Errors into a file;
                Console.WriteLine(ex.Message);
                
            }
        }

        private string getIp(HttpListenerContext context)
        {
            string _custIP = null;
            try
            {

                _custIP = context.Request.RemoteEndPoint.ToString();
                string[] _custIPList = _custIP.Split(':');

                _custIP = _custIPList[0];


            }
            catch (Exception ex)
            {
                _custIP = "";
            }
            return _custIP;

        }
        private void HandleRequest(object httpContext)
        {
            try
            {
                //pick up the request
                HttpListenerContext context = (HttpListenerContext)httpContext;
                string requesttitme = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");
                string request = (new StreamReader(context.Request.InputStream).ReadToEnd());

                Console.WriteLine();
                Console.WriteLine("Time In:" + DateTime.Now);
                Console.WriteLine(".........................Request Made.........................");
                Console.WriteLine(request);

                string IpToWhitelist = "";
                IpToWhitelist = getIp(context);

                //process the request
                string jsonresp = ProcessRequest(request, IpToWhitelist);
                //return the response
                byte[] buf = Encoding.ASCII.GetBytes(jsonresp);
                context.Response.ContentLength64 = buf.Length;
                context.Response.OutputStream.Write(buf, 0, buf.Length);
                string Responsetitme = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");
                Console.WriteLine();
                Console.WriteLine("Time Out:" + DateTime.Now);
                Console.WriteLine(".........................Response to Request......................");
                Console.WriteLine(jsonresp);

                string whatToLog = Environment.NewLine
                     + "Request Recieved: >> "
                     + requesttitme
                     + context.Request.RemoteEndPoint
                     + Environment.NewLine
                     + Environment.NewLine
                     + Environment.NewLine
                     + "Response Sent: "
                     + Responsetitme
                     + Environment.NewLine
                     + jsonresp
                     + Environment.NewLine
                     + "------------------------------------------------"
                     + Environment.NewLine;
                if (!string.IsNullOrEmpty(request))
                {
                    whatToLog = Environment.NewLine
                    + "Request Recieved: >> "
                    + requesttitme
                    + context.Request.RemoteEndPoint
                    + Environment.NewLine
                    + request
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Response Sent: "
                    + Responsetitme
                    + Environment.NewLine
                    + jsonresp
                    + Environment.NewLine
                    + "------------------------------------------------"
                    + Environment.NewLine;
                }

                //log response sent
                //log.Info(whatToLog);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public string ProcessRequest(string request, string Ipaddress)
        {
            //log request recieved
            string jsonresp = "";

            Logic bll = new Logic();
            try
            {

                dynamic req = JObject.Parse(request);

                string requestType = GetRequestType(request);

                if (requestType.ToUpper().Equals("CREATECANDIDATE"))
                {
                    StudentDetails deserializedTransaction = JsonConvert.DeserializeObject<StudentDetails>(request);
                    StudentDetailsResponse postresp = bll.CreateCandidate(deserializedTransaction, Ipaddress);
                    jsonresp = JsonConvert.SerializeObject(postresp, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                }
                else if (requestType.ToUpper().Equals("UPDATECANDIDATE"))
                {
                    StudentDetails deserializedTransaction = JsonConvert.DeserializeObject<StudentDetails>(request);
                    StudentDetailsResponse postresp = bll.UpdateCandidate(deserializedTransaction, Ipaddress);
                    jsonresp = JsonConvert.SerializeObject(postresp, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                }
                else if (requestType.ToUpper().Equals("QUERYCANDIDATE"))
                {
                    StudentDetails deserializedTransaction = JsonConvert.DeserializeObject<StudentDetails>(request);
                    StudentDetailsResponse postresp = bll.QueryCandidate(deserializedTransaction, Ipaddress);
                    jsonresp = JsonConvert.SerializeObject(postresp, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                }
                else
                {
                    jsonresp = OperationNotSupportedYetResponse("UNKOWN REQUEST TYPE SENT: " + requestType);
                }

            }
            catch (Exception ex)
            {
                jsonresp = OperationNotSupportedYetResponse("SOMETHING WENT WRONG");
            }

            return jsonresp;
        }

        private string GetRequestType(string requestXml)
        {
            string requestType = "";

            dynamic req = JObject.Parse(requestXml);

            requestType = req.RequestType;


            if (requestType.ToUpper() == "CREATECANDIDATE")
            {
                requestType = "CreateCandidate";
            }
            else if (requestType.ToUpper() == "UPDATECANDIDATE")
            {
                requestType = "UpdateCandidate";
            }
            else if(requestType.ToUpper() == "QUERYCANDIDATE")
            {
                requestType = "QueryCandidate";
            }
            else
            {
                requestType = "UNKNOWN";
            }
            return requestType;
        }

        //Change this to json
        private string OperationNotSupportedYetResponse(string error)
        {
            string xmlResponse = "{" +
            "StatusDescription: " + error +
            ",StatusCode: 100" +
        "}";

            return xmlResponse;
        }
    }
}
