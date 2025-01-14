namespace Picart.Services.UserAppThemeSettingsService;

internal class UserAppThemeSettingsService() : IUserAppThemeSettingsService
{
    public AppTheme? LoadUserAppTheme()
    {
        var savedTheme = Preferences.Get("UserAppTheme", AppTheme.Unspecified.ToString());
        Enum.TryParse(typeof(AppTheme), savedTheme, out var currentTheme);
        return (AppTheme?)currentTheme;
    }

    public void SetUserAppTheme(string? themeName)
    {
        Enum.TryParse(typeof(AppTheme), themeName, out var appTheme);
        SetUserAppTheme((AppTheme?)appTheme);
    }

    public void SetUserAppTheme(AppTheme? appTheme)
    {
        if (Application.Current is not null)
            Application.Current.UserAppTheme = appTheme ?? AppTheme.Unspecified;

        Preferences.Set("UserAppTheme", appTheme.ToString());
    }

    public void ReloadUserAppTheme() => SetUserAppTheme(LoadUserAppTheme());
}
