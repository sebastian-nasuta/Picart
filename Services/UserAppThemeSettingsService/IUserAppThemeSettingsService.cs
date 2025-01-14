namespace Picart.Services.UserAppThemeSettingsService;

public interface IUserAppThemeSettingsService
{
    void ReloadUserAppTheme();
    void SetUserAppTheme(string? themeName);
    void SetUserAppTheme(AppTheme? appTheme);
    AppTheme? LoadUserAppTheme();
}