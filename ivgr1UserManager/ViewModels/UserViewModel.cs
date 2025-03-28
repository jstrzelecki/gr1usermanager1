using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dapper;
using ivgr1UserManager.Models;
using Npgsql;

namespace ivgr1UserManager.ViewModels;

public partial class UserViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    

    [ObservableProperty] private string _firstName;
    [ObservableProperty] private string _lastName;
    [ObservableProperty] private string _email;

    public ObservableCollection<User> Users { get; } = new();
    
    public ICommand SaveCommand { get; }
    public ICommand LoadCommand { get; }

    public UserViewModel()
    {
        _userRepository = new UserRepository("""
                                             Host=localhost;
                                             Port=5432;
                                             Username=js;
                                             Password=postgres;
                                             Database=users1_db;
                                             """);
        
        SaveCommand = new RelayCommand(SaveUser);
        LoadCommand = new RelayCommand(LoadUsers);
        _userRepository.InitDB();
        // LoadUsers();
    }

    private void LoadUsers()
    {
        Users.Clear();
        foreach (var user in _userRepository.GetAllUsers())
        {
            Users.Add(user);
        }
    }

    private void SaveUser()
    {
        var user = new User
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email
        };
        try
        {
            _userRepository.AddUser(user);
            Users.Add(user);
            ClearFields();
        }
        catch (PostgresException ex) when (ex.SqlState == "23505") // wartość unikalna
        {
            Console.WriteLine("błąd, użytkownik z takim mejlem już istnieje");
        }
        catch (PostgresException ex) when (ex.SqlState == "23502") // null
        {   
            Console.WriteLine("Wszystkie pola powinny być wypełnione");
        }
        LoadUsers();
    }

    private void ClearFields()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }
}