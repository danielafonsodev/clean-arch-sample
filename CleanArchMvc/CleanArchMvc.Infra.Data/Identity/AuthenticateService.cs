﻿using CleanArchMvc.Domain.Account;
using Microsoft.AspNetCore.Identity;

namespace CleanArchMvc.Infra.Data.Identity;

public class AuthenticateService : IAuthenticate
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public AuthenticateService(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task<bool> AuthenticateAsync(string email, string password)
    {
        var result = await signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
        return result.Succeeded;
    }

    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await userManager.CreateAsync(applicationUser, password);
        if (result.Succeeded)
        {
            await signInManager.SignInAsync(applicationUser, isPersistent: false);
        }
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
