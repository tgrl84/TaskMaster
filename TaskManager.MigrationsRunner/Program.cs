// See htusing Microsoft.EntityFrameworkCore;
using TaskMaster; // ← Assure-toi que c'est bien le namespace de ton DbContext
using Microsoft.EntityFrameworkCore;
Console.WriteLine("Lancement de la migration...");

// Configure la chaîne de connexion MySQL
var connectionString = "server=localhost;user=root;password=ton_mot_de_passe;database=taskmanager_db"; // ⬅️ à adapter

// Configure les options EF Core pour MySQL
var optionsBuilder = new DbContextOptionsBuilder<TaskManagerDbContext>();
optionsBuilder.UseMySql(
    connectionString,
    new MySqlServerVersion(new Version(8, 0, 34)) // ⬅️ adapte la version à ton serveur
);

// Exécute la migration
using var context = new TaskManagerDbContext(optionsBuilder.Options);

Console.WriteLine("Application de la migration à la base de données...");
context.Database.Migrate();

Console.WriteLine("Migration terminée avec succès !");
