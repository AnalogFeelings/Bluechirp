using System;
using Mastonet;
using Mastonet.Entities;

namespace TooterTests.Generators
{
    internal class ClientData
    {
        internal static (Auth token, AppRegistration registration) CreateFakeClientAuthObjects()
        {
            Auth token = new Auth { AccessToken = "testToken", TokenType = "testType",
                CreatedAt = "testDate"
            };

            AppRegistration registration = new AppRegistration
            {
                ClientId = "testClient",
                Id = 69420,
                ClientSecret = "testSecret",
                Instance = "testInstance",
                RedirectUri = "testMe",
                Scope = Scope.Read | Scope.Write | Scope.Follow
            };

            return (token, registration);
        }
    }
}