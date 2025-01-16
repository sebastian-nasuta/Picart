namespace Picart.Services.OpenAISettingsService;

public interface IOpenAISettingsService
{
    string LoadOpenAIApiKey();
    void SaveOpenAIApiKey(string apiKey);
}