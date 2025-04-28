using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class TaskEditPage : ContentPage
    {
        private readonly TaskEditViewModel _viewModel;

        public TaskEditPage(TaskEditViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadDataCommand.Execute(null);
        }
    }
} 