using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Services;

namespace TaskMaster.ViewModels
{
    public partial class TasksViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableCollection<Tache> _tasks;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _filterStatus;

        [ObservableProperty]
        private string _filterPriority;

        [ObservableProperty]
        private string _filterCategory;

        public TasksViewModel(ITaskService taskService, IAuthService authService)
        {
            _taskService = taskService;
            _authService = authService;
            Tasks = new ObservableCollection<Tache>();
            Title = "Tâches";
        }

        [RelayCommand]
        private async Task LoadTasksAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsRefreshing = true;

            try
            {
                Tasks.Clear();
                var user = await _authService.GetCurrentUserAsync();
                var tasks = await _taskService.GetTasksByUserAsync(user.IdUtilisateur);

                // Appliquer les filtres si nécessaires
                if (!string.IsNullOrEmpty(FilterStatus))
                    tasks = tasks.Where(t => t.Statut == FilterStatus).ToList();

                if (!string.IsNullOrEmpty(FilterPriority))
                    tasks = tasks.Where(t => t.Priorite == FilterPriority).ToList();

                if (!string.IsNullOrEmpty(FilterCategory))
                    tasks = tasks.Where(t => t.Categorie == FilterCategory).ToList();

                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception
                await Shell.Current.DisplayAlert("Erreur", $"Impossible de charger les tâches: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task AddTaskAsync()
        {
            await Shell.Current.GoToAsync("task/new");
        }

        [RelayCommand]
        private async Task TaskSelectedAsync(Tache task)
        {
            if (task == null)
                return;

            await Shell.Current.GoToAsync($"task/details?id={task.IdTache}");
        }

        [RelayCommand]
        private async Task FilterTasksAsync()
        {
            await LoadTasksAsync();
        }

        [RelayCommand]
        private void ClearFilters()
        {
            FilterStatus = string.Empty;
            FilterPriority = string.Empty;
            FilterCategory = string.Empty;
            LoadTasksCommand.Execute(null);
        }

        [RelayCommand]
        private async Task Logout()
        {
            _authService.Logout();
            
            try 
            {
                // Essayer d'abord la navigation Shell si disponible
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("//login");
                }
                else
                {
                    // Si Shell n'est pas disponible, redémarrer avec MainPage
                    var mainPage = Application.Current.Handler.MauiContext.Services.GetService<MainPage>();
                    Application.Current.MainPage = mainPage;
                }
            }
            catch (Exception)
            {
                // En cas d'échec, créer directement une nouvelle instance de MainPage
                var mainPage = Application.Current.Handler.MauiContext.Services.GetService<MainPage>();
                Application.Current.MainPage = mainPage;
            }
        }

        public async Task OnAppearing()
        {
            await LoadTasksAsync();
        }
    }
} 