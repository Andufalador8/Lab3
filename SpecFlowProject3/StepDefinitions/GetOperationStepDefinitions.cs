using NUnit.Framework;
using RestSharp;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowProject3.StepDefinitions
{
    [Binding]
    public class GetOperationStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;

        public GetOperationStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
        [Given(@"the (.*)")]
        public void GivenThe(int p0)
        {
            scenarioContext["id"] = p0.ToString();
        }

        [When(@"making a request")]
        public void WhenMakingARequest()
        {
            string url = "https://restful-booker.herokuapp.com/booking/";

            var client = new RestClient(url + scenarioContext["id"]);

            var request = new RestRequest();

            request.AddHeader("Accept", "application/json");

            var response = client.Get(request);

            scenarioContext["Result"] = response.Content.ToString();
        }

        [Then(@"get a json file by id")]
        public void ThenGetAJsonFileById()
        {
            var result = scenarioContext["Result"];
            Assert.That(result, Is.EqualTo("{\"firstname\":\"Alex\",\"lastname\":\"Morante Briones\",\"totalprice\":111,\"depositpaid\":true,\"bookingdates\":{\"checkin\":\"2018-01-01\",\"checkout\":\"2019-01-01\"},\"additionalneeds\":\"Breakfast\"}"));
        }
    }
}
