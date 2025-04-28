using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Pomelo.EntityFrameworkCore.MySql;
using TaskMaster.Services;
using TaskMaster.ViewModels;
using TaskMaster.Views;

namespace TaskMaster {

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Configuration de EF Core avec MySQL
            builder.Services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseMySql("Server=localhost;Port=3306;Database=taskmaster;User=root;",
                new MySqlServerVersion(new Version(8, 0, 32))));

            // Enregistrer les services
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ITaskService, TaskService>();
            builder.Services.AddSingleton<IProjectService, ProjectService>();

            // Enregistrer les ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<TasksViewModel>();
            builder.Services.AddTransient<TaskDetailViewModel>();
            builder.Services.AddTransient<TaskEditViewModel>();

            // Enregistrer les vues
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<TasksPage>();
            builder.Services.AddTransient<TaskDetailPage>();
            builder.Services.AddTransient<TaskEditPage>();

            // Vérification de la connexion à la base de données
            using (var scope = builder.Services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();
                try
                {
                    dbContext.Database.OpenConnection();
                    Console.WriteLine("Connexion à la base de données réussie.");
                    dbContext.Database.Migrate();
                    dbContext.Database.CloseConnection();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Échec de la connexion à la base de données : {ex.Message}");
                }
            }

            return builder.Build();
        }
    }
}