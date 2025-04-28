using Microsoft.Maui;
using Microsoft.Maui.Controls;
using TaskMaster.Services;
using TaskMaster.ViewModels;

namespace TaskMaster;

public partial class App : Application
{
	private readonly IAuthService _authService;

	public App(IAuthService authService)
	{
		InitializeComponent();
		_authService = authService;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Check if user is already authenticated
		if (_authService.IsAuthenticated)
		{
			return new Window(new AppShell());
		}
		else
		{
			MainPage mainPage = Handler.MauiContext.Services.GetService<MainPage>();
			return new Window(mainPage);
		}
	}
}