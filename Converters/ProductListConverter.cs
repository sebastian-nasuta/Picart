using OpenAI.Chat;
using Picart.Models;
using Picart.Services.OpenAISettingsService;
using System.Text.Json;

namespace Picart.Converters
{
    internal class ProductListConverter(IOpenAISettingsService openAISettingsService) : IProductListConverter
    {
        private readonly string _apiKey = openAISettingsService.LoadOpenAIApiKey();

        public async Task<IEnumerable<ProductGroup>> ConvertAsync(string rawList, List<string> productGroupNames)
        {
            try
            {
                if (!productGroupNames.Contains("INNE"))
                {
                    productGroupNames.Add("INNE");
                }

                var systemMessage = new SystemChatMessage(
                    ChatMessageContentPart.CreateTextPart("You are a converter who converts the given raw product list into a grouped list."),
                    ChatMessageContentPart.CreateTextPart($"""
                        ~RULES~

                        - Extract individual products from the list and assign them to the appropriate product groups.
                        - Return the list of products in the form of product groups in JSON array format.
                        - Do not add any other text, return ONLY JSON with the list of products.
                        - If you are not sure where to assign a given product, add it to the "INNE" group.
                        - Use only the given category names, NEVER create new ones.
                        """),
                    ChatMessageContentPart.CreateTextPart("""
                        ~EXAMPLE~
                        
                        USER:
                        productGroups = "Owoce i warzywa; Jogurty; Sery; Wędliny"
                        rawListInRandomOrder = "jogurt grecki; ser żółty; koń; jabłka; ser pleśniowy; skyr; mozarella; kabanosy; parówki; jogurt naturalny; banany; jogurt owocowy; pomidory; krakowska sucha; ogórki; ziemniaki"
                        
                        ASSISTANT:
                        {
                          "ProductGroups": [
                            {
                              "Name": "Owoce i warzywa",
                              "Products": [
                                { "Name": "jabłka", "Checked": false },
                                { "Name": "banany", "Checked": false },
                                { "Name": "pomidory", "Checked": false },
                                { "Name": "ogórki", "Checked": false },
                                { "Name": "ziemniaki", "Checked": false }
                              ]
                            },
                            {
                              "Name": "Jogurty",
                              "Products": [
                                { "Name": "skyr", "Checked": false },
                                { "Name": "jogurt grecki", "Checked": false },
                                { "Name": "jogurt naturalny", "Checked": false },
                                { "Name": "jogurt owocowy", "Checked": false }
                              ]
                            },
                            {
                              "Name": "Sery",
                              "Products": [
                                { "Name": "ser żółty", "Checked": false },
                                { "Name": "ser pleśniowy", "Checked": false },
                                { "Name": "mozarella", "Checked": false }
                              ]
                            },
                            {
                              "Name": "Wędliny",
                              "Products": [
                                { "Name": "krakowska sucha", "Checked": false },
                                { "Name": "kabanosy", "Checked": false },
                                { "Name": "parówki", "Checked": false }
                              ]
                            },
                            {
                              "Name": "INNE",
                              "Products": [
                                { "Name": "koń", "Checked": false }
                              ]
                            }
                          ]
                        }
                        """));
                var userMessage = new UserChatMessage(
                    ChatMessageContentPart.CreateTextPart("USER:"),
                    ChatMessageContentPart.CreateTextPart($"productGroups = \"{string.Join("; ", productGroupNames)}\""),
                    ChatMessageContentPart.CreateTextPart($"rawListInRandomOrder = \"{rawList}\""));

                var client = new ChatClient(OpenAIModels.gpt_3_5_turbo, _apiKey);
                var completion = await client.CompleteChatAsync(
                    [systemMessage, userMessage],
                    new ChatCompletionOptions()
                    {
                        ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat(),
                    });

                var result = completion.Value.Content[0].Text;

                var deserializedResult = JsonSerializer.Deserialize<ProductGroupCollection>(result)
                    ?? throw new Exception($"An error occurred while deserializing the product list. Result: {result}");

                return deserializedResult.ProductGroups;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show a message to the user)
                throw new Exception($"An error occurred while converting the product list: {ex.Message}");
            }
        }
    }
}
