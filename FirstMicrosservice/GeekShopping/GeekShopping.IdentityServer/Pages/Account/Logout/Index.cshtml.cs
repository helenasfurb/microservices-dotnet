using Duende.IdentityModel;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeekShopping.IdentityServer.Pages.Logout;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;

    [BindProperty]
    public string? LogoutId { get; set; }

    public Index(IIdentityServerInteractionService interaction, IEventService events)
    {
        _interaction = interaction;
        _events = events;
    }

    public async Task<IActionResult> OnGetAsync(string? logoutId, CancellationToken ct)
    {
        LogoutId = logoutId;

        var showLogoutPrompt = LogoutOptions.ShowLogoutPrompt;

        if (User.Identity?.IsAuthenticated != true)
        {
            showLogoutPrompt = false;
        }
        else
        {
            var context = await _interaction.GetLogoutContextAsync(LogoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                showLogoutPrompt = false;
            }
        }

        if (showLogoutPrompt == false)
        {
            return await OnPostAsync(ct);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            LogoutId ??= await _interaction.CreateLogoutContextAsync();

            await HttpContext.SignOutAsync();

            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            Telemetry.Metrics.UserLogout(idp);

            if (idp != null && idp != Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider)
            {
                if (await HttpContext.GetSchemeSupportsSignOutAsync(idp))
                {
                    var url = Url.Page("/Account/Logout/Loggedout", new { logoutId = LogoutId });

                    return SignOut(new AuthenticationProperties { RedirectUri = url }, idp);
                }
            }
        }

        return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
    }
}
