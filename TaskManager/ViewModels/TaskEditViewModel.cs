using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Services;

namespace TaskMaster.ViewModels
{
    [QueryProperty(nameof(TaskId), "id")]
    public partial class TaskEditViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private int _taskId;

        [ObservableProperty]
        private string _titre;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private DateTime _echeance;

        [ObservableProperty]
        private string _statut;

        [ObservableProperty]
        private string _priorite;

        [ObservableProperty]
        private string _categorie;

        [ObservableProperty]
        private int _idRealisateur;

        [ObservableProperty]
        private ObservableCollection<Utilisateur> _utilisateurs;

        [ObservableProperty]
        private Utilisateur _selectedRealisateur;

        public ObservableCollection<string> Statuts { get; } = new ObservableCollection<string>
        {
            "à faire", "en cours", "terminée", "annulée"
        };

        public ObservableCollection<string> Priorites { get; } = new ObservableCollection<string>
        {
            "basse", "moyenne", "haute", "critique"
        };

        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>
        {
            "perso", "travail", "projet", "autre"
        };

        public TaskEditViewModel(ITaskService taskService, IAuthService authService)
        {
            _taskService = taskService;
            _authService = authService;
            Utilisateurs = new ObservableCollection<Utilisateur>();
            Echeance = DateTime.Now.AddDays(7);
            Statut = "à faire";
            Priorite = "moyenne";
            Categorie = "travail";
            Title = "Nouvelle Tâche";
        }

        partial void OnTaskIdChanged(int value)
        {
            LoadDataAsync().ConfigureAwait(false);
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            IsBusy = true;

            try
            {
                // Charger la liste des utilisateurs
                var tasks = await _taskService.GetAllTasksAsync();
                var utilisateurs = tasks
                    .SelectMany(t => new[] { t.Auteur, t.Realisateur })
                    .GroupBy(u => u.IdUtilisateur)
                    .Select(g => g.First())
                    .ToList();

                Utilisateurs.Clear();
                foreach (var user in utilisateurs)
                {
                    Utilisateurs.Add(user);
                }

                // Si c'est une édition, charger la tâche existante
                if (TaskId > 0)
                {
                    var task = await _taskService.GetTaskByIdAsync(TaskId);
                    if (task != null)
                    {
                        Titre = task.Titre;
                        Description = task.Description;
                        Echeance = task.Echeance;
                        Statut = task.Statut;
                        Priorite = task.Priorite;
                        Categorie = task.Categorie;
                        IdRealisateur = task.IdRealisateur;
                        SelectedRealisateur = Utilisateurs.FirstOrDefault(u => u.IdUtilisateur == IdRealisateur);
                        Title = "Modifier Tâche";
                    }
                }
                else
                {
                    // Pour une nouvelle tâche, le réalisateur est par défaut l'utilisateur actuel
                    var currentUser = await _authService.GetCurrentUserAsync();
                    IdRealisateur = currentUser.IdUtilisateur;
                    SelectedRealisateur = currentUser;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors du chargement: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(Titre) || string.IsNullOrWhiteSpace(Description))
            {
                await Shell.Current.DisplayAlert("Erreur", "Veuillez remplir tous les champs obligatoires", "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                var task = new Tache
                {
                    Titre = Titre,
                    Description = Description,
                    Echeance = Echeance,
                    Statut = Statut,
                    Priorite = Priorite,
                    Categorie = Categorie,
                    IdRealisateur = SelectedRealisateur?.IdUtilisateur ?? IdRealisateur,
                    Auteur = currentUser, // Correction pour définir l'Auteur
                    Realisateur = SelectedRealisateur ?? currentUser // Correction pour définir le Réalisateur
                };

                bool success;
                if (TaskId > 0)
                {
                    task.IdTache = TaskId;
                    success = await _taskService.UpdateTaskAsync(task);
                }
                else
                {
                    success = await _taskService.CreateTaskAsync(task);
                }

                if (success)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible de sauvegarder la tâche", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de la sauvegarde: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
} 