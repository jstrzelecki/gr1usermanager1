using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dapper;
using ivgr1UserManager.Helpers;
using ivgr1UserManager.Models;
using ivgr1UserManager.Services;
using Npgsql;

namespace ivgr1UserManager.ViewModels;

public partial class UserViewModel : ObservableObject
{
    //private readonly UserRepository _userRepository;
    private readonly UserService _userService;

    [ObservableProperty] private string _firstName;
    [ObservableProperty] private string _lastName;
    [ObservableProperty] private string _email;

    [ObservableProperty] private bool _isViaEmailSelected;
    [ObservableProperty] private bool _isViaSmsSelected;
    [ObservableProperty] private bool _isNoUpdatesSelected;
    [ObservableProperty] private bool _isChecked;

    [ObservableProperty] private string _selectedAccountType;

    public ObservableCollection<string> AccountTypes { get; } =
    [
        "Standard", "Premium", "Business"
    ];
    
    public ObservableCollection<User> Users { get; } = new();
    
    public ICommand SaveCommand { get; }
    public ICommand LoadCommand { get; }

    public UserViewModel(UserService userService)
    {
        _userService = userService;
        /*_userRepository = new UserRepository("""
                                             Host=localhost;
                                             Port=5432;
                                             Username=js;
                                             Password=postgres;
                                             Database=users1_db;
                                             """);*/
        SelectedAccountType = AccountTypes[0];
        IsChecked = true;
        SaveCommand = new AsyncRelayCommand(SaveUser);
        LoadCommand = new AsyncRelayCommand(LoadUsers);
        //_userRepository.InitDB();
        // LoadUsers();
    }

    private async Task LoadUsers()
    {
        try
        {
            var usersFromDb = await _userService.GetAllUsersAsync();
            Users.Clear();
            Users.AddRange(usersFromDb);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
        /*Users.Clear();
        foreach (var user in _userRepository.GetAllUsers())
        {
            Users.Add(user);
        }*/
    }

    private async Task SaveUser()
    {
        var user = new User
        {
            FirstName = FirstName.Trim(),
            LastName = LastName.Trim(),
            Email = Email.Trim(),
            NotificationPreference = IsViaEmailSelected ? "Email" :
                                     IsViaSmsSelected ? "SMS" :
                                     "None",
            AccountType = SelectedAccountType,
            IsTermsAccepted = IsChecked,
            
        };
        try
        {
            await _userService.AddUserAsync(user);
            Users.Add(user);
            ClearFields();
            /*//_userRepository.AddUser(user);
            Users.Add(user);
            ClearFields();*/
        }
        catch (PostgresException ex) when (ex.SqlState == "23505") // wartość unikalna
        {
            Console.WriteLine("błąd, użytkownik z takim mejlem już istnieje");
        }
        catch (PostgresException ex) when (ex.SqlState == "23502") // null
        {   
            Console.WriteLine("Wszystkie pola powinny być wypełnione");
        }
      //  LoadUsers();
    }

    private void ClearFields()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        IsViaEmailSelected = false;
        IsViaSmsSelected = false;
        IsNoUpdatesSelected = true;
        SelectedAccountType = AccountTypes[0];
        IsChecked = true;
    }
}