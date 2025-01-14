namespace Picart.Services.UserAppThemeSettingsService;

public interface IUserAppThemeSettingsService
{
    AppTheme LoadUserAppTheme();
    void ReloadUserAppTheme();
    void SetUserAppTheme(AppTheme? appTheme);
    void ToggleUserAppTheme();
}