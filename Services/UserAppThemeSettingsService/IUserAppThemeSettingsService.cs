namespace Picart.Services.UserAppThemeSettingsService;

public interface IUserAppThemeSettingsService
{
    AppTheme LoadUserAppTheme();
    void ReloadUserAppTheme();
    void SetUserAppTheme(string? themeName);
    void SetUserAppTheme(AppTheme? appTheme);
    void ToggleUserAppTheme();
}