﻿using GoRestClient.Models;
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
        private PaginationModel _pagination;

        public MainWindowViewModel(IUserService userService)
        {
            _userService = userService;
            _usersCollection = new ObservableCollection<UserModel>();
            SearchCommand = new DelegateCommand(async () => await Search());
            InsertCommand = new DelegateCommand(async () => await Insert());
            UpdateCommand = new DelegateCommand(async () => await Update());
            DeleteCommand = new DelegateCommand(async () => await Delete());
            CreateNewCommand = new DelegateCommand(ClearSelectedUser);
            ClearSelectedUser();
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

        public DelegateCommand SearchCommand { get; }
        public DelegateCommand InsertCommand { get; }
        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand CreateNewCommand { get; }

        private async Task Search()
        {
            var searchResult = await _userService.Search(NameFilter);
            UsersCollection.Clear();
            UsersCollection.AddRange(searchResult.Records);
            Pagination = searchResult.Pagination;
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

        private void ClearSelectedUser()
        {
            SelectedUser = new UserModel();
        }
    }
}
