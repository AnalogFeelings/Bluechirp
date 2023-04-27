
using System;
using System.Threading.Tasks;
using Mastonet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TooterLib.Core;
using TooterLib.Helpers;

namespace TooterTests
{
    //[TestClass]
    //public class AuthTests
    //{
    //    [TestMethod]
    //    public async Task OAuthUrlCreationTest()
    //    {
    //        string[] testConstants = { "FediTest", "https://www.google.com", "tooter://mycallback" };

    //        APIConstants.SetConstants(testConstants[0], testConstants[1], testConstants[2]);

    //        const string testInstance = "mastodon.social";

    //        try
    //        {
    //            await AuthHelper.Instance.CreateOAuthUrlAsync(testInstance);
    //        }
    //        catch (Exception ex)
    //        {
    //            Assert.Fail(ex.Message);
    //        }
    //    }

    //    [TestMethod]
    //    public async Task FinishOAuthTest()
    //    {
    //        string[] testConstants = { "FediTest", "https://www.google.com", "tooter://mycallback" };

    //        APIConstants.SetConstants(testConstants[0], testConstants[1], testConstants[2]);

    //        const string testInstance = "mastodon.social";

    //        try
    //        {
    //            await AuthHelper.Instance.CreateOAuthUrlAsync(testInstance);
    //        }
    //        catch (Exception ex)
    //        {
    //            Assert.Fail(ex.Message);
    //        }

    //        var authClient = new AuthenticationClient(testInstance);
    //        if (authClient == null)
    //        {
    //            Assert.Fail("The auth client object is null!");
    //        }

    //        await authClient.CreateApp(testConstants[0], Scope.Read | Scope.Write | Scope.Follow, testConstants[1], testConstants[2]);

    //        var auth = await authClient.ConnectWithPassword(Constants.Constants.TestEmail, Constants.Constants.TestPassword);

    //        try
    //        {
    //            await AuthHelper.Instance.CompleteAuthAsync(auth);
    //        }
    //        catch (Exception ex)
    //        {
    //            Assert.Fail(ex.Message);
    //            throw;
    //        }
    //    }
    //}
}
