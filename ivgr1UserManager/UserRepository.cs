using System;
using System.Collections.Generic;
using Dapper;
using ivgr1UserManager.Models;
using Npgsql;

namespace ivgr1UserManager;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InitDB()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS users (
                        id SERIAL PRIMARY KEY,
                        first_name VARCHAR(50) NOT NULL,
                        last_name VARCHAR(50) NOT NULL,
                        email VARCHAR(50) NOT NULL UNIQUE
                    )
        ");
    }


    public List<User> GetAllUsers()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return connection.Query<User>("SELECT * FROM users").AsList();
    }

    public void AddUser(User user)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        Console.WriteLine($"Adding user {user.FirstName} {user.LastName}");
        connection.Execute("""
                           Insert into users (first_name, last_name, email) 
                           values (@FirstName, @LastName, @Email) 
                           """, user);
                          
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}