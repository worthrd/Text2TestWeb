using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Text2TestWebApp.Luis
{
    public class IntentManager
    {
        static string appId = "88c7ed3b-74a8-46a1-bf4f-1c466e424604";
        static string predictionKey = "81fdf4910cb94430881d728afeabd727";
        static string predictionEndpoint = "https://cognativeresource.cognitiveservices.azure.com/";


        public static async Task<String> GetIntent(string utterance)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // The request header contains your subscription key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", predictionKey);

            // The "q" parameter contains the utterance to send to LUIS
            queryString["query"] = utterance;

            // These optional request parameters are set to their default values
            queryString["verbose"] = "true";
            queryString["show-all-intents"] = "true";
            queryString["staging"] = "false";
            queryString["timezoneOffset"] = "0";

            var predictionEndpointUri = String.Format("{0}luis/prediction/v3.0/apps/{1}/slots/production/predict?{2}", predictionEndpoint, appId, queryString);

            var response = await client.GetAsync(predictionEndpointUri);

            var strResponseContent = await response.Content.ReadAsStringAsync();

            return strResponseContent.ToString();
        }

    }
}
