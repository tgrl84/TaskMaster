using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class TasksPage : ContentPage
    {
        private readonly TasksViewModel _viewModel;

        public TasksPage(TasksViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }
    }
} 