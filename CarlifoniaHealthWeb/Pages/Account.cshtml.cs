using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarlifoniaHealthWeb.Pages;

public class AccountModel : PageModel
{
    public void OnGet()
    {
    }

    public async Task OnGetLogin()
    {
        // Indicate here where Auth0 should redirect the user after a login.
        // Note that the resulting absolute Uri must be added to the
        // **Allowed Callback URLs** settings for the app.
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri("/")
            .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    }

   
    public async Task OnGetLogout()
    {
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            // Indicate here where Auth0 should redirect the user after a logout.
            // Note that the resulting absolute Uri must be added to the
            // **Allowed Logout URLs** settings for the app.
            .WithRedirectUri(Url.Page("Index"))
            .Build();

        await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
