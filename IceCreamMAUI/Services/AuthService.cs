﻿using IceCreamMAUI.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IceCreamMAUI.Services
{
    public class AuthService
    {
        private const string AuthKey = "AuthKey";

        public LoggedInUser? User { get;private set; }
        public string? Token { get; private set; }

        public void Signin(AuthResponseDto dto)
        {
            var serialized = JsonSerializer.Serialize(dto);
            Preferences.Default.Set(AuthKey, serialized);
            (User, Token) = dto;
        }

        public void Initialize()
        {
            if(Preferences.Default.ContainsKey(AuthKey))
            {
                var serialized = Preferences.Default.Get<string>(AuthKey, null);
                if(string.IsNullOrEmpty(serialized))
                {
                    Preferences.Default.Remove(AuthKey);
                }
                else
                {
                    (User, Token) = JsonSerializer.Deserialize<AuthResponseDto>(serialized)!;
                }
            }
        }

        public void Signout()
        {
            Preferences.Default.Remove(AuthKey);
            (User, Token) = (null,null);
        }
    }
}
