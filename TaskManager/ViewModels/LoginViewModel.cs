using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Services;

namespace TaskMaster.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private bool _isSignUpMode;

        [ObservableProperty]
        private string _errorMessage;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Connexion";
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Veuillez remplir tous les champs";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var result = await _authService.LoginAsync(Email, Password);
                if (result)
                {
                    // Naviguer vers l'écran principal
                    if (Shell.Current != null)
                    {
                        await Shell.Current.GoToAsync("//tasks");
                    }
                    else
                    {
                        // Si nous ne sommes pas dans un contexte Shell, créer et définir AppShell comme Page principale
                        Application.Current.MainPage = new AppShell();
                    }
                }
                else
                {
                    ErrorMessage = "Identifiants incorrects. Veuillez réessayer.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur de connexion: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Veuillez remplir tous les champs";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var result = await _authService.RegisterAsync(FirstName, LastName, Email, Password);
                if (result)
                {
                    // Naviguer vers l'écran principal après l'inscription
                    if (Shell.Current != null)
                    {
                        await Shell.Current.GoToAsync("//tasks");
                    }
                    else
                    {
                        // Si nous ne sommes pas dans un contexte Shell, créer et définir AppShell comme Page principale
                        Application.Current.MainPage = new AppShell();
                    }
                }
                else
                {
                    ErrorMessage = "Cette adresse email est déjà utilisée.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur d'inscription: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void ToggleMode()
        {
            IsSignUpMode = !IsSignUpMode;
            Title = IsSignUpMode ? "Inscription" : "Connexion";
            ErrorMessage = string.Empty;
        }
    }
} 