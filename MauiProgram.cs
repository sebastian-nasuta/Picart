using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Picart.Converters;
using Picart.Services.OpenAISettingsService;
using Picart.Services.UserAppThemeSettingsService;
using System.Reflection;

namespace Picart
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialSymbolsOutlined.ttf", "MSO");
                });

            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Picart.appsettings.json")
                ?? throw new FileNotFoundException("appsettings.json not found in the resources.");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);

            builder.Services.AddSingleton<IProductListConverter, ProductListConverter>();

            builder.Services.AddSingleton<IOpenAISettingsService, OpenAISettingsService>();
            builder.Services.AddSingleton<IUserAppThemeSettingsService, UserAppThemeSettingsService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
