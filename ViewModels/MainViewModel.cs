using Picart.Services.UserAppThemeSettingsService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Picart.ViewModels
{
    internal partial class MainViewModel(IUserAppThemeSettingsService userAppThemeSettingsService) : ViewModelBase
    {
        private readonly IUserAppThemeSettingsService _userAppThemeSettingsService = userAppThemeSettingsService;

        public bool IsDarkTheme
        {
            get => Application.Current?.RequestedTheme == AppTheme.Dark;
            set
            {
                _userAppThemeSettingsService.SetUserAppTheme(value ? AppTheme.Dark : AppTheme.Light);
                OnPropertyChanged();
            }
        }
    }
}
