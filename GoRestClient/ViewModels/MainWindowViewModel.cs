using GoRestClient.Models;
using GoRestClient.Models.Enums;
using GoRestClient.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GoRestClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IUserService _userService;
        private UserModel _selectedUser;
        private ObservableCollection<UserModel> _usersCollection;

        public MainWindowViewModel(IUserService userService)
        {
            _userService = userService;
            _usersCollection = new ObservableCollection<UserModel>();
            SearchCommand = new DelegateCommand(async () => await Search());
        }

        public IEnumerable<Gender> GendersOptions => Enum.GetValues(typeof(Gender)).Cast<Gender>();

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

        private async Task Search()
        {
            var searchResult = await _userService.Search();
            UsersCollection.AddRange(searchResult);
        }
    }
}
