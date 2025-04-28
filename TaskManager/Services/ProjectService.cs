using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster.Services
{
    public interface IProjectService
    {
        Task<List<Projet>> GetAllProjectsAsync();
        Task<Projet> GetProjectByIdAsync(int projectId);
        Task<bool> CreateProjectAsync(Projet project);
        Task<bool> UpdateProjectAsync(Projet project);
        Task<bool> DeleteProjectAsync(int projectId);
    }

    public class ProjectService : IProjectService
    {
        private readonly TaskManagerDbContext _dbContext;

        public ProjectService(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Projet>> GetAllProjectsAsync()
        {
            return await _dbContext.Projets
                .ToListAsync();
        }

        public async Task<Projet> GetProjectByIdAsync(int projectId)
        {
            return await _dbContext.Projets
                .Include(p => p.TachesProjets)
                    .ThenInclude(tp => tp.Tache)
                        .ThenInclude(t => t.Auteur)
                .Include(p => p.TachesProjets)
                    .ThenInclude(tp => tp.Tache)
                        .ThenInclude(t => t.Realisateur)
                .FirstOrDefaultAsync(p => p.IdProjet == projectId);
        }

        public async Task<bool> CreateProjectAsync(Projet project)
        {
            try
            {
                await _dbContext.Projets.AddAsync(project);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateProjectAsync(Projet project)
        {
            try
            {
                var existingProject = await _dbContext.Projets.FindAsync(project.IdProjet);
                if (existingProject == null) return false;

                existingProject.NomProjet = project.NomProjet;
                existingProject.DescriptionProjet = project.DescriptionProjet;

                _dbContext.Projets.Update(existingProject);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            try
            {
                var project = await _dbContext.Projets.FindAsync(projectId);
                if (project == null) return false;

                _dbContext.Projets.Remove(project);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 