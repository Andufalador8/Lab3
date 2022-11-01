using NUnit.Framework;
using RestSharp;
using System;
using System.Runtime.Serialization.Json;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowProject3.StepDefinitions
{
    [Binding]
    public class DeleteOperationStepDefinitions
    {
        public static String Serialize(Object data)
        {
            var serializer = new DataContractJsonSerializer(data.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, data);

            return Encoding.UTF8.GetString(ms.ToArray());
        }
        private readonly ScenarioContext scenarioContext;

        public DeleteOperationStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
        [Given(@"""([^""]*)"" of the file")]
        public void GivenOfTheFile(string id)
        {
            scenarioContext["id"] = id;
        }

        [Given(@"make token")]
        public void GivenMakeToken()
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

            token = token.Substring(10,15);

            scenarioContext["token"] = token;
        }

        [When(@"request send")]
        public void WhenRequestSend()
        {
            var client = new RestClient("https://restful-booker.herokuapp.com/booking/" + scenarioContext["id"].ToString());

            var request = new RestRequest();

            request.AddHeader("Accept", "application/json");

            request.AddHeader("Cookie", "token=" + scenarioContext["token"].ToString());

            var response = client.Delete(request);

            scenarioContext["response"] = response.Content.ToString();
        }

        [Then(@"Created")]
        public void ThenCreated()
        {
            var result = scenarioContext["response"];
            Assert.That(result, Is.EqualTo("Created"));
        }
    }
}
