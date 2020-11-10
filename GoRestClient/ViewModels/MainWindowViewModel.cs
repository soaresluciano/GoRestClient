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
            SearchResultModel searchResult;

            try
            {
                searchResult = await _userService.Search(NameFilter, resultPage);
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to fetch the search results.", e);
                RefreshRecords(null, null);
                return;
            }
            
            RefreshRecords(searchResult.Records, searchResult.Pagination);
            _statusManager.ReportInfo("Search results successfully fetched.");
        }

        private void RefreshRecords(IEnumerable<UserModel> records, PaginationModel pagination)
        {
            Pagination = pagination;
            UsersCollection.Clear();
            if (records != null) 
                UsersCollection.AddRange(records);
            ClearSelectedUser();
        }

        private async Task Insert()
        {
            UserModel newUser;
            try
            {
                newUser = await _userService.Create(SelectedUser);
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to create the new user record.", e);
                return;
            }
            
            UsersCollection.Add(newUser);
            SelectedUser = newUser;
            _statusManager.ReportInfo("User record successfully created.");
        }

        private async Task Update()
        {
            UserModel updatedUser;
            try
            {
                updatedUser = await _userService.Update(SelectedUser);
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to update the user information.", e);
                return;
            }

            SelectedUser = updatedUser;
            _statusManager.ReportInfo("User record successfully updated.");
        }

        private async Task Delete()
        {
            try
            {
                await _userService.Delete(SelectedUser.Id);
            }
            catch (Exception e)
            {
                _statusManager.ReportException("Failed to delete the user.", e);
                return;
            }
            
            UsersCollection.Remove(SelectedUser);
            ClearSelectedUser();
            _statusManager.ReportInfo("User record successfully deleted.");
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
