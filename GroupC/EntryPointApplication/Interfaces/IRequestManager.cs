﻿using IdentityServer.Models;

namespace EntryPointApplication.Interfaces
{
    interface IRequestManager
    {
        Task<HttpResponseMessage> AuthenticateAsync(Users users);

        bool ValidateCurrentToken(string token);
    }
}