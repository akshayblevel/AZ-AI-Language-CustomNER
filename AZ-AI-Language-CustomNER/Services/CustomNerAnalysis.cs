using AZ_AI_Language_CustomNER.Interfaces;
using AZ_AI_Language_CustomNER.Models;
using Azure;
using Azure.AI.TextAnalytics;

namespace AZ_AI_Language_CustomNER.Services
{
    public class CustomNerAnalysis : ICustomNerAnalysis
    {
        private readonly TextAnalyticsClient _client;

        public CustomNerAnalysis(IConfiguration config)
        {
            string endpoint = config["AzureLanguage:Endpoint"] ?? "";
            string apiKey = config["AzureLanguage:Key"] ?? "";
            var credential = new AzureKeyCredential(apiKey);

            _client = new TextAnalyticsClient(new Uri(endpoint), credential);
        }
        public async Task<List<CustomEntityResponse>> CustomNerAsync(List<string> documents)
        {
            string projectName = "akkicustomner";
            string deploymentName = "akkicustomnerdeployment";

            var actions = new TextAnalyticsActions
            {
                RecognizeCustomEntitiesActions = new List<RecognizeCustomEntitiesAction>
                {
                    new RecognizeCustomEntitiesAction(projectName, deploymentName)
                }
            };

            var operation = await _client.StartAnalyzeActionsAsync(documents, actions);
            await operation.WaitForCompletionAsync();

            var results = new List<CustomEntityResponse>();

            await foreach (var docsInPage in operation.Value)
            {
                foreach (var docResult in docsInPage.RecognizeCustomEntitiesResults)
                {
                    foreach (var doc in docResult.DocumentsResults)
                    {
                        if (doc.HasError)
                        {
                            results.Add(new CustomEntityResponse
                            {
                                Id = doc.Id,
                                Entities = new List<CustomEntity>
                            {
                                new CustomEntity { Category = "Error", Text = doc.Error.Message }
                            }
                            });
                        }
                        else
                        {
                            results.Add(new CustomEntityResponse
                            {
                                Id = doc.Id,
                                Entities = doc.Entities.Select(e => new CustomEntity
                                {
                                    Category = e.Category.ToString(),
                                    Text = e.Text,
                                    ConfidenceScore = e.ConfidenceScore,
                                    Offset = e.Offset,
                                    Length = e.Length
                                }).ToList()
                            });
                        }
                    }
                }
            }

            return results;
        }
    }
}
