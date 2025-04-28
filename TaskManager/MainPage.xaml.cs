using Microsoft.Maui.Controls;
using TaskMaster.ViewModels;

namespace TaskMaster;

public partial class MainPage : ContentPage
{
	private readonly LoginViewModel _viewModel;

	public MainPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		
	}
	private void OnLoginButtonClicked(object sender, EventArgs e)
	{

	}
    private void OnSignUpButtonClicked(object sender, EventArgs e)
	{

	}
}

