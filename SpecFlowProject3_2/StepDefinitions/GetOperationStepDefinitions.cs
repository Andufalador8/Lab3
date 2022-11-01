using NUnit.Framework;
using RestSharp;
using System;
using System.Security.Policy;
using TechTalk.SpecFlow;

namespace SpecFlowProject3_2.StepDefinitions
{
    [Binding]
    public class GetOperationStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;

        public GetOperationStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
        [Given(@"the god name - ""([^""]*)""")]
        public void GivenTheGodName_(string ganga)
        {
            scenarioContext["url"] = "https://api-rv.herokuapp.com/rv/v2/meta/god/" + ganga;
        }

        [When(@"make request")]
        public void WhenMakeRequest()
        {
            var client = new RestClient(scenarioContext["url"].ToString());

            var request = new RestRequest();

            request.AddHeader("accept", "application/json");

            var response = client.Get(request);

            scenarioContext["Result"] = response.Content.ToString();
        }

        [Then(@"got the json file")]
        public void ThenGotTheJsonFile()
        {
            var result = scenarioContext["Result"];
            Assert.That(result, Is.EqualTo("[\n  {\n    \"mandal\": 10, \n    \"meter\": \"Jagati\", \n    \"sukta\": 75, \n    \"sungby\": \"Sindhukshit Praiyamedh\", \n    \"sungbycategory\": \"human male\", \n    \"sungfor\": \"Ganga\", \n    \"sungforcategory\": \"divine female\"\n  }\n]\n"));
        }
    }
}
