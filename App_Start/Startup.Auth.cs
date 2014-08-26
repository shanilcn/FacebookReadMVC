using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using FacebookReadMVC.Models;
using Microsoft.Owin.Security.Facebook;
using System.Threading.Tasks;

namespace FacebookReadMVC
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            
                var facebookOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions
                {
                    AppId = "1430700287164125",
                    AppSecret = "ee985ca3401facc9bb1c719807a13c22",
                    Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider
                    {
                        OnAuthenticated = (context) =>
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, XmlSchemaString, "Facebook"));
                            foreach (var x in context.User)
                            {
                                var claimType = string.Format("urn:facebook:{0}", x.Key);
                                string claimValue = x.Value.ToString();
                                if (!context.Identity.HasClaim(claimType, claimValue))
                                    context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, XmlSchemaString, "Facebook"));

                            }
                            return Task.FromResult(0);
                        }
                    }
                };
                facebookOptions.Scope.Add("email");
                //facebookOptions.Scope.Add("user_groups");
                facebookOptions.Scope.Add("read_stream");
                //facebookOptions.Scope.Add("publish_actions");
                app.UseFacebookAuthentication(facebookOptions);



                //FacebookAuthenticationOptions options = new FacebookAuthenticationOptions();
                //options.AppId = "1430700287164125";
                //options.AppSecret = "ee985ca3401facc9bb1c719807a13c22";
                //options.Scope.Add("user_groups");
                //options.Scope.Add("read_stream");
                //options.Scope.Add("publish_actions");

                //app.UseFacebookAuthentication(options);


                //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                //{
                //    ClientId = "",
                //    ClientSecret = ""
                //});            
        }
        const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";
    }
}