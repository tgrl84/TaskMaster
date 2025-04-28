using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Services;

namespace TaskMaster.ViewModels
{
    [QueryProperty(nameof(TaskId), "id")]
    public partial class TaskDetailViewModel : BaseViewModel
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private int _taskId;

        [ObservableProperty]
        private Tache _task;

        [ObservableProperty]
        private string _newComment;

        [ObservableProperty]
        private ObservableCollection<SousTache> _subTasks;

        [ObservableProperty]
        private ObservableCollection<Commentaire> _comments;

        [ObservableProperty]
        private ObservableCollection<HistoriqueModification> _history;

        [ObservableProperty]
        private string _newSubTaskTitle;

        [ObservableProperty]
        private DateTime _newSubTaskDeadline;

        public TaskDetailViewModel(ITaskService taskService, IAuthService authService)
        {
            _taskService = taskService;
            _authService = authService;
            SubTasks = new ObservableCollection<SousTache>();
            Comments = new ObservableCollection<Commentaire>();
            History = new ObservableCollection<HistoriqueModification>();
            NewSubTaskDeadline = DateTime.Now.AddDays(7);
        }

        partial void OnTaskIdChanged(int value)
        {
            LoadTaskAsync().ConfigureAwait(false);
        }

        [RelayCommand]
        private async Task LoadTaskAsync()
        {
            if (TaskId <= 0)
                return;

            IsBusy = true;

            try
            {
                Task = await _taskService.GetTaskByIdAsync(TaskId);
                Title = Task.Titre;

                // Charger les sous-tâches
                SubTasks.Clear();
                var subTasks = await _taskService.GetSubTasksAsync(TaskId);
                foreach (var subTask in subTasks)
                {
                    SubTasks.Add(subTask);
                }

                // Charger les commentaires
                Comments.Clear();
                var comments = await _taskService.GetCommentsAsync(TaskId);
                foreach (var comment in comments)
                {
                    Comments.Add(comment);
                }

                // Charger l'historique
                History.Clear();
                var history = await _taskService.GetHistoryAsync(TaskId);
                foreach (var h in history)
                {
                    History.Add(h);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible de charger la tâche: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task UpdateTaskAsync()
        {
            if (Task == null)
                return;

            IsBusy = true;

            try
            {
                var success = await _taskService.UpdateTaskAsync(Task);
                if (success)
                {
                    await LoadTaskAsync();
                    await Shell.Current.DisplayAlert("Succès", "Tâche mise à jour avec succès", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible de mettre à jour la tâche", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de la mise à jour: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteTaskAsync()
        {
            if (Task == null)
                return;

            var confirm = await Shell.Current.DisplayAlert("Confirmation", 
                "Êtes-vous sûr de vouloir supprimer cette tâche ?", "Oui", "Non");

            if (!confirm)
                return;

            IsBusy = true;

            try
            {
                var success = await _taskService.DeleteTaskAsync(TaskId);
                if (success)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible de supprimer la tâche", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de la suppression: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddCommentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewComment))
                return;

            IsBusy = true;

            try
            {
                var comment = new Commentaire
                {
                    IdTache = TaskId,
                    Tache = Task,
                    Contenu = NewComment,
                    Auteur = await _authService.GetCurrentUserAsync() // Assuming _authService provides the current user
                };

                var success = await _taskService.AddCommentAsync(comment);
                if (success)
                {
                    NewComment = string.Empty;
                    await LoadTaskAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible d'ajouter le commentaire", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de l'ajout du commentaire: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddSubTaskAsync()
        {
            if (string.IsNullOrWhiteSpace(NewSubTaskTitle))
                return;

            IsBusy = true;

            try
            {
                var subTask = new SousTache
                {
                    IdTacheParent = TaskId,
                    TacheParent = Task,
                    Titre = NewSubTaskTitle,
                    Statut = "à faire",
                    Echeance = NewSubTaskDeadline
                };

                var success = await _taskService.AddSubTaskAsync(subTask);
                if (success)
                {
                    NewSubTaskTitle = string.Empty;
                    NewSubTaskDeadline = DateTime.Now.AddDays(7);
                    await LoadTaskAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible d'ajouter la sous-tâche", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de l'ajout de la sous-tâche: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task UpdateSubTaskStatusAsync(SousTache subTask)
        {
            if (subTask == null)
                return;

            IsBusy = true;

            try
            {
                // Changer le statut de la sous-tâche (à faire -> en cours -> terminée -> à faire)
                subTask.Statut = subTask.Statut switch
                {
                    "à faire" => "en cours",
                    "en cours" => "terminée",
                    "terminée" => "à faire",
                    _ => "à faire"
                };

                var success = await _taskService.UpdateSubTaskAsync(subTask);
                if (success)
                {
                    await LoadTaskAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible de mettre à jour la sous-tâche", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de la mise à jour: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteSubTaskAsync(SousTache subTask)
        {
            if (subTask == null)
                return;

            var confirm = await Shell.Current.DisplayAlert("Confirmation",
                "Êtes-vous sûr de vouloir supprimer cette sous-tâche ?", "Oui", "Non");

            if (!confirm)
                return;

            IsBusy = true;

            try
            {
                var success = await _taskService.DeleteSubTaskAsync(subTask.IdSousTache);
                if (success)
                {
                    await LoadTaskAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Impossible de supprimer la sous-tâche", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de la suppression: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 