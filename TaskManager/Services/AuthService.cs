using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string firstname, string lastname, string email, string password);
        Task<Utilisateur> GetCurrentUserAsync();
        bool IsAuthenticated { get; }
        void Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly TaskManagerDbContext _dbContext;
        private Utilisateur _currentUser;

        public AuthService(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsAuthenticated => _currentUser != null;

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email && u.MotDePasse == password);

            if (user != null)
            {
                _currentUser = user;
                return true;
            }

            return false;
        }

        public async Task<bool> RegisterAsync(string firstname, string lastname, string email, string password)
        {
            // Vérifier si l'email existe déjà
            var existingUser = await _dbContext.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser != null)
            {
                return false;
            }

            // Créer nouvel utilisateur
            var newUser = new Utilisateur
            {
                Prenom = firstname,
                Nom = lastname,
                Email = email,
                MotDePasse = password // Note: Dans un vrai projet, utiliser un hachage sécurisé
            };

            await _dbContext.Utilisateurs.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            _currentUser = newUser;
            return true;
        }

        public async Task<Utilisateur> GetCurrentUserAsync()
        {
            return _currentUser;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
} 