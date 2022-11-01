using NUnit.Framework;
using RestSharp;
using System;
using System.ComponentModel.Design.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowProject3.StepDefinitions
{
    [Binding]
    public class UpdateStepDefinitions
    {
        public static String Serialize(Object data)
        {
            var serializer = new DataContractJsonSerializer(data.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, data);

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        private readonly ScenarioContext scenarioContext;

        public UpdateStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
        [Given(@"""([^""]*)""")]
        public void Given(string id)
        {
            scenarioContext["id"] = id;
        }

        [When(@"make request")]
        public void WhenMakeRequest()
        {
            var client_t = new RestClient("https://restful-booker.herokuapp.com/auth");

            var request_t = new RestRequest();

            request_t.AddHeader("Accept", "application/json");

            MyJsonDictionary<String, Object> account = new MyJsonDictionary<String, Object>();
            account["username"] = "admin";
            account["password"] = "password123";

            request_t.AddJsonBody(Serialize(account));

            var response_t = client_t.Post(request_t);

            var token = response_t.Content.ToString();

            token = token.Substring(10, 15);

            scenarioContext["token"] = token;

            //

            MyJsonDictionary<String, Object> booking = new MyJsonDictionary<String, Object>();

            booking["firstname"] = "Nazar";
            booking["lastname"] = "Trukhan";
            booking["totalprice"] = 543;
            booking["depositpaid"] = true;
            MyJsonDictionary<String, Object> bookingdates = new MyJsonDictionary<String, Object>();
            booking["bookingdates"] = bookingdates;
            bookingdates["checkin"] = "2017-05-06";
            bookingdates["checkout"] = "2018-05-06";
            booking["additionalneeds"] = "Massage";

            scenarioContext["jsonFile"] = Serialize(booking);


            scenarioContext["jsonFile"] = Serialize(booking);

            string url = "https://restful-booker.herokuapp.com/booking/";

            var client = new RestClient(url + scenarioContext["id"].ToString());

            var request = new RestRequest();

            request.AddHeader("Accept", "application/json");

            request.AddHeader("Cookie", "token=" + scenarioContext["token"].ToString());

            request.AddJsonBody(scenarioContext["jsonFile"]);

            var response = client.Patch(request);

            scenarioContext["result"] = response.Content.ToString();

        }

        [Then(@"done")]
        public void ThenDone()
        {
            var result = scenarioContext["result"];
            Assert.That(result, Is.Not.Null);
        }
    }
}
