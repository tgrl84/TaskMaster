using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class TaskDetailPage : ContentPage
    {
        private readonly TaskDetailViewModel _viewModel;

        public TaskDetailPage(TaskDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
} 