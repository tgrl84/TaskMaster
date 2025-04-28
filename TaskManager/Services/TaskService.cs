using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster.Services
{
    public interface ITaskService
    {
        Task<List<Tache>> GetAllTasksAsync();
        Task<List<Tache>> GetTasksByUserAsync(int userId);
        Task<List<Tache>> GetTasksByStatusAsync(string status);
        Task<List<Tache>> GetTasksByPriorityAsync(string priority);
        Task<List<Tache>> GetTasksByCategoryAsync(string category);
        Task<Tache> GetTaskByIdAsync(int taskId);
        Task<bool> CreateTaskAsync(Tache task);
        Task<bool> UpdateTaskAsync(Tache task);
        Task<bool> DeleteTaskAsync(int taskId);
        Task<List<SousTache>> GetSubTasksAsync(int taskId);
        Task<bool> AddSubTaskAsync(SousTache subTask);
        Task<bool> UpdateSubTaskAsync(SousTache subTask);
        Task<bool> DeleteSubTaskAsync(int subTaskId);
        Task<bool> AddCommentAsync(Commentaire comment);
        Task<List<Commentaire>> GetCommentsAsync(int taskId);
        Task<List<HistoriqueModification>> GetHistoryAsync(int taskId);
        Task<bool> AddTaskToProjectAsync(int taskId, int projectId);
        Task<List<Tache>> GetTasksByProjectAsync(int projectId);
    }

    public class TaskService : ITaskService
    {
        private readonly TaskManagerDbContext _dbContext;
        private readonly IAuthService _authService;

        public TaskService(TaskManagerDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<List<Tache>> GetAllTasksAsync()
        {
            return await _dbContext.Taches
                .Include(t => t.Auteur)
                .Include(t => t.Realisateur)
                .ToListAsync();
        }

        public async Task<List<Tache>> GetTasksByUserAsync(int userId)
        {
            return await _dbContext.Taches
                .Where(t => t.IdRealisateur == userId)
                .Include(t => t.Auteur)
                .Include(t => t.Realisateur)
                .ToListAsync();
        }

        public async Task<List<Tache>> GetTasksByStatusAsync(string status)
        {
            return await _dbContext.Taches
                .Where(t => t.Statut == status)
                .Include(t => t.Auteur)
                .Include(t => t.Realisateur)
                .ToListAsync();
        }

        public async Task<List<Tache>> GetTasksByPriorityAsync(string priority)
        {
            return await _dbContext.Taches
                .Where(t => t.Priorite == priority)
                .Include(t => t.Auteur)
                .Include(t => t.Realisateur)
                .ToListAsync();
        }

        public async Task<List<Tache>> GetTasksByCategoryAsync(string category)
        {
            return await _dbContext.Taches
                .Where(t => t.Categorie == category)
                .Include(t => t.Auteur)
                .Include(t => t.Realisateur)
                .ToListAsync();
        }

        public async Task<Tache> GetTaskByIdAsync(int taskId)
        {
            return await _dbContext.Taches
                .Include(t => t.Auteur)
                .Include(t => t.Realisateur)
                .Include(t => t.SousTaches)
                .Include(t => t.Commentaires)
                    .ThenInclude(c => c.Auteur)
                .Include(t => t.TachesEtiquettes)
                    .ThenInclude(te => te.Etiquette)
                .Include(t => t.HistoriqueModifications)
                    .ThenInclude(h => h.Auteur)
                .FirstOrDefaultAsync(t => t.IdTache == taskId);
        }

        public async Task<bool> CreateTaskAsync(Tache task)
        {
            try
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null) return false;

                task.IdAuteur = currentUser.IdUtilisateur;
                task.Auteur = currentUser;
                task.DateCreation = DateTime.Now;

                await _dbContext.Taches.AddAsync(task);
                
                // Ajouter une entrée dans l'historique
                var history = new HistoriqueModification
                {
                    IdAuteur = currentUser.IdUtilisateur,
                    Auteur = currentUser,
                    DateModification = DateTime.Now,
                    DescriptionModif = "Création de la tâche",
                    Tache = task
                };
                
                await _dbContext.HistoriqueModifications.AddAsync(history);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTaskAsync(Tache task)
        {
            try
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null) return false;

                var existingTask = await _dbContext.Taches.FindAsync(task.IdTache);
                if (existingTask == null) return false;

                // Mettre à jour les propriétés
                existingTask.Titre = task.Titre;
                existingTask.Description = task.Description;
                existingTask.Echeance = task.Echeance;
                existingTask.Statut = task.Statut;
                existingTask.Priorite = task.Priorite;
                existingTask.IdRealisateur = task.IdRealisateur;
                existingTask.Categorie = task.Categorie;

                _dbContext.Taches.Update(existingTask);
                
                // Ajouter une entrée dans l'historique
                var history = new HistoriqueModification
                {
                    IdTache = task.IdTache,
                    Tache = existingTask,
                    IdAuteur = currentUser.IdUtilisateur,
                    Auteur = currentUser,
                    DateModification = DateTime.Now,
                    DescriptionModif = "Modification de la tâche"
                };
                
                await _dbContext.HistoriqueModifications.AddAsync(history);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            try
            {
                var task = await _dbContext.Taches.FindAsync(taskId);
                if (task == null) return false;

                _dbContext.Taches.Remove(task);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<SousTache>> GetSubTasksAsync(int taskId)
        {
            return await _dbContext.SousTaches
                .Where(st => st.IdTacheParent == taskId)
                .ToListAsync();
        }

        public async Task<bool> AddSubTaskAsync(SousTache subTask)
        {
            try
            {
                await _dbContext.SousTaches.AddAsync(subTask);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateSubTaskAsync(SousTache subTask)
        {
            try
            {
                _dbContext.SousTaches.Update(subTask);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSubTaskAsync(int subTaskId)
        {
            try
            {
                var subTask = await _dbContext.SousTaches.FindAsync(subTaskId);
                if (subTask == null) return false;

                _dbContext.SousTaches.Remove(subTask);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddCommentAsync(Commentaire comment)
        {
            try
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null) return false;

                comment.IdAuteur = currentUser.IdUtilisateur;
                comment.Auteur = currentUser;
                comment.DateCommentaire = DateTime.Now;

                await _dbContext.Commentaires.AddAsync(comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Commentaire>> GetCommentsAsync(int taskId)
        {
            return await _dbContext.Commentaires
                .Where(c => c.IdTache == taskId)
                .Include(c => c.Auteur)
                .OrderByDescending(c => c.DateCommentaire)
                .ToListAsync();
        }

        public async Task<List<HistoriqueModification>> GetHistoryAsync(int taskId)
        {
            return await _dbContext.HistoriqueModifications
                .Where(h => h.IdTache == taskId)
                .Include(h => h.Auteur)
                .OrderByDescending(h => h.DateModification)
                .ToListAsync();
        }

        public async Task<bool> AddTaskToProjectAsync(int taskId, int projectId)
        {
            try
            {
                var taskProject = new TacheProjet
                {
                    IdTache = taskId,
                    Tache = await _dbContext.Taches.FindAsync(taskId) ?? throw new InvalidOperationException("Tâche introuvable."),
                    IdProjet = projectId,
                    Projet = await _dbContext.Projets.FindAsync(projectId) ?? throw new InvalidOperationException("Projet introuvable.")
                };

                await _dbContext.TachesProjets.AddAsync(taskProject);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Tache>> GetTasksByProjectAsync(int projectId)
        {
            return await _dbContext.TachesProjets
                .Where(tp => tp.IdProjet == projectId)
                .Include(tp => tp.Tache)
                    .ThenInclude(t => t.Auteur)
                .Include(tp => tp.Tache)
                    .ThenInclude(t => t.Realisateur)
                .Select(tp => tp.Tache)
                .ToListAsync();
        }
    }
} 