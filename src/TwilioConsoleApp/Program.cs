using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace TwilioTest
{
    class Program
    {
        public class Message
        {
            public string To { get; set; }
            public string From { get; set; }
            public string Body { get; set; }
            public string Status { get; set; }
        }
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            //1
            var request = new RestRequest("Accounts/AC9e37c3fee84384ee8ccfa7aaabf53905/Messages.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator("AC9e37c3fee84384ee8ccfa7aaabf53905", "add6ff176860f34fea3295e9a3533547");
            //2
            var response = new RestResponse();
            //3a
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            //4
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            foreach (var message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
            }
            Console.ReadLine();
        }

        //3b
        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}