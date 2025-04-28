using Microsoft.Maui.Controls;
using TaskMaster.Views;

namespace TaskMaster;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Enregistrer les routes pour la navigation
		Routing.RegisterRoute("task/details", typeof(TaskDetailPage));
		Routing.RegisterRoute("task/edit", typeof(TaskEditPage));
		Routing.RegisterRoute("task/new", typeof(TaskEditPage));
	}
}
