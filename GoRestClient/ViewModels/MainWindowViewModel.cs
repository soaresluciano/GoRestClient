using GoRestClient.Core;
using GoRestClient.Models;
using GoRestClient.Services;
using GoRestClient.Services.Models;
using GoRestClient.Services.Models.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GoRestClient.ViewModels
{
    public class MainWindowViewModel : BindableBase, IDisposable
    {
        private readonly IUserService _userService;
        private readonly IStatusManager _statusManager;
        private UserModel _selectedUser;
        private ObservableCollection<UserModel> _usersCollection;
        private ObservableCollection<string> _statusList;
        private PaginationModel _pagination;
        
        public MainWindowViewModel(
            IUserService userService,
            IStatusManager statusManager)
        {
            _userService = userService;
            _statusManager = statusManager;
            _statusManager.OnNewStatusReceived += StatusManager_OnNewStatusReceived;
            _usersCollection = new ObservableCollection<UserModel>();
            _statusList = new ObservableCollection<string>();
            SearchCommand = new DelegateCommand(async () => await Search(1));
            InsertCommand = new DelegateCommand(async () => await Insert());
            UpdateCommand = new DelegateCommand(async () => await Update());
            DeleteCommand = new DelegateCommand(async () => await Delete());
            CreateNewCommand = new DelegateCommand(ClearSelectedUser);
            FirstPageCommand =
                new DelegateCommand(async () => await Search(1), CanExecuteGoBackPage)
                    .ObservesProperty(() => Pagination);
            PreviousPageCommand = 
                new DelegateCommand(async () => await Search(--Pagination.Page), CanExecuteGoBackPage)
                    .ObservesProperty(() => Pagination);
            NextPageCommand = 
                new DelegateCommand(async () => await Search(++Pagination.Page), CanExecuteGoForwardPage)
                    .ObservesProperty(() => Pagination);
            LastPageCommand =
                new DelegateCommand(async () => await Search(Pagination.Pages), CanExecuteGoForwardPage)
                    .ObservesProperty(() => Pagination);

            ClearSelectedUser();
        }

        public void Dispose()
        {
            _statusManager.OnNewStatusReceived -= StatusManager_OnNewStatusReceived;
            _userService.Dispose();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Gender> GendersOptions => Enum.GetValues(typeof(Gender)).Cast<Gender>();

        public string NameFilter { get; set; }

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

        public PaginationModel Pagination
        {
            get => _pagination;
            set => SetProperty(ref _pagination, value);
        }

        public ObservableCollection<string> StatusList
        {
            get => _statusList;
            set => SetProperty(ref _statusList, value);
        }

        public DelegateCommand SearchCommand { get; }
        public DelegateCommand InsertCommand { get; }
        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand CreateNewCommand { get; }
        public DelegateCommand FirstPageCommand { get; }
        public DelegateCommand PreviousPageCommand { get; }
        public DelegateCommand NextPageCommand { get; }
        public DelegateCommand LastPageCommand { get; }

        private async Task Search(uint resultPage)
        {
            var searchResult = await _userService.Search(NameFilter, resultPage);
            UsersCollection.Clear();
            UsersCollection.AddRange(searchResult.Records);
            Pagination = searchResult.Pagination;
            ClearSelectedUser();
        }

        private async Task Insert()
        {
            var newUser = await _userService.Create(SelectedUser);
            UsersCollection.Add(newUser);
            SelectedUser = newUser;
        }

        private async Task Update()
        {
            await _userService.Update(SelectedUser);
        }

        private async Task Delete()
        {
            await _userService.Delete(SelectedUser.Id);
            UsersCollection.Remove(SelectedUser);
            ClearSelectedUser();
        }

        private void ClearSelectedUser() => SelectedUser = new UserModel();

        private bool CanExecuteGoBackPage() => Pagination?.Page > 1;
        
        private bool CanExecuteGoForwardPage() => Pagination?.Page < Pagination?.Pages;

        private void StatusManager_OnNewStatusReceived(object s, string statusMessage)
        {
            StatusList.Add(statusMessage);
        }
    }
}
