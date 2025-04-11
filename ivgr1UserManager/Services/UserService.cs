using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ivgr1UserManager.Models;

namespace ivgr1UserManager.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;   
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            return await _userRepository.GetAllUsersAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"GetAllUsers Error : {ex.Message}");
        }
    }

    public async Task AddUserAsync(User user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName)
                                                      || string.IsNullOrWhiteSpace(user.Email)
           )
        {
            throw new ArgumentException("Invalid User");
        }
        
        var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Email {user.Email} already exists");
        }

        try
        {
            int rowAffected = await _userRepository.AddUserAsync(user);
            if (rowAffected == 0)
            {
                throw new Exception($"Failed to add user {user.Email}");
            }
        }
        catch(Exception ex)
        {
            throw new Exception($"Adding user failed: {ex.Message}");
        }
    }
}