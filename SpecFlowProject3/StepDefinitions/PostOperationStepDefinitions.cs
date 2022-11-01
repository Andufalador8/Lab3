using NUnit.Framework;
using RestSharp;
using System;
using System.Runtime.Serialization.Json;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowProject3.StepDefinitions
{
    [Binding]
    public class PostOperationStepDefinitions
    {
        public static String Serialize(Object data)
        {
            var serializer = new DataContractJsonSerializer(data.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, data);

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        private readonly ScenarioContext scenarioContext;

        public PostOperationStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
        [Given(@"JsonFile")]
        public void GivenJsonFile()
        {
            
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
        }

        [When(@"make a request")]
        public void WhenMakeARequest()
        {
            string url = "https://restful-booker.herokuapp.com/booking";

            var client = new RestClient(url);

            var request = new RestRequest();

            request.AddHeader("Accept", "application/json");

            request.AddJsonBody(scenarioContext["jsonFile"]);

            var response = client.Post(request);

            scenarioContext["result"] = response.Content.ToString();
        }

        [Then(@"assert")]
        public void ThenAssert()
        {
            var result = scenarioContext["result"];
            Assert.That(result, Is.Not.Null);
        }
    }
    
}
