using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task InitDB()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(@"
                    CREATE TABLE IF NOT EXISTS users (
                        id SERIAL PRIMARY KEY,
                        first_name VARCHAR(50) NOT NULL,
                        last_name VARCHAR(50) NOT NULL,
                        email VARCHAR(50) NOT NULL UNIQUE
                    )
        ");
        Console.WriteLine("Database initialized");
    }


    public async Task<List<User>> GetAllUsersAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var users = await connection.QueryAsync<User>("SELECT * FROM users");
        return users.AsList();
    }
// tu skończyłem !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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