using Microsoft.Extensions.Configuration;

namespace Picart.Services.OpenAISettingsService;

internal class OpenAISettingsService(IConfiguration configuration) : IOpenAISettingsService
{
    public string LoadOpenAIApiKey()
    {
        var apiKey = Preferences.Get("OpenAI:ApiKey", null);
        if (string.IsNullOrEmpty(apiKey))
        {
            apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new ArgumentNullException("OpenAI:ApiKey is missing in the configuration file.");

            if (!string.IsNullOrEmpty(apiKey))
                SaveOpenAIApiKey(apiKey);
        }
        return apiKey;
    }

    public void SaveOpenAIApiKey(string apiKey) => Preferences.Set("OpenAI:ApiKey", apiKey);
}
