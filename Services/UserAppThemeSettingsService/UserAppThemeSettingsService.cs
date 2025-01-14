namespace Picart.Services.UserAppThemeSettingsService;

internal class UserAppThemeSettingsService() : IUserAppThemeSettingsService
{
    private const string UserAppThemeKey = "UserAppTheme";

    public AppTheme LoadUserAppTheme() => Enum.Parse<AppTheme>(Preferences.Get(UserAppThemeKey, AppTheme.Unspecified.ToString()));

    public void ReloadUserAppTheme() => SetUserAppTheme(LoadUserAppTheme());

    public void SetUserAppTheme(string? themeName)
    {
        Enum.TryParse(typeof(AppTheme), themeName, out var appTheme);
        SetUserAppTheme((AppTheme?)appTheme);
    }

    public void SetUserAppTheme(AppTheme? appTheme)
    {
        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = appTheme ?? AppTheme.Unspecified;
        }
        Preferences.Set(UserAppThemeKey, appTheme.ToString());
    }

    public void ToggleUserAppTheme()
    {
        if (Application.Current is not null)
        {
            var requestedTheme = Application.Current.RequestedTheme;
            SetUserAppTheme(requestedTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark);
        }
    }
}
