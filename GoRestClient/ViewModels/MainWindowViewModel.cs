using GoRestClient.Models;
using Prism.Mvvm;

namespace GoRestClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private UserModel _selectedUser = null;
        public UserModel SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public MainWindowViewModel()
        {
            SelectedUser = new UserModel { Name = "Dummy" };
        }
    }
}
