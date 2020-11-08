using GoRestClient.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace GoRestClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private UserModel _selectedUser = null;
        private ObservableCollection<UserModel> _usersCollection = new ObservableCollection<UserModel>();

        public MainWindowViewModel()
        {
            SearchCommand = new DelegateCommand(Search);
        }

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public ObservableCollection<UserModel> UsersCollection
        {
            get => _usersCollection;
            set => SetProperty(ref _usersCollection, value);
        }

        public DelegateCommand SearchCommand { get; }

        private void Search()
        {
            UsersCollection.Add(new UserModel { Id = 1, Name = "Dummy 1", Email = "1@dummy.com", Gender= Gender.Female, Status = true, Created = DateTime.Now, Updated = DateTime.Now });
            UsersCollection.Add(new UserModel { Id = 2, Name = "Dummy 2", Email = "2@dummy.com", Gender= Gender.Male, Status = false, Created = DateTime.Now, Updated = DateTime.Now });
            UsersCollection.Add(new UserModel { Id = 3, Name = "Dummy 3", Email = "3@dummy.com", Gender= Gender.Female, Status = false, Created = DateTime.Now, Updated = DateTime.Now });
            UsersCollection.Add(new UserModel { Id = 4, Name = "Dummy 4", Email = "4@dummy.com", Gender= Gender.Male, Status = true, Created = DateTime.Now, Updated = DateTime.Now });
        }
    }
}
