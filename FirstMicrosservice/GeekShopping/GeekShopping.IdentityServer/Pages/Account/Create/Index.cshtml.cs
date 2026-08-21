using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeekShopping.IdentityServer.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly TestUserStore _users;
    private readonly IIdentityServerInteractionService _interaction;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public Index(
        IIdentityServerInteractionService interaction,
        TestUserStore? users = null)
    {
        _users = users ?? throw new InvalidOperationException("Please call 'AddTestUsers(TestUsers.Users)' on the IIdentityServerBuilder in Startup or remove the TestUserStore from the AccountController.");

        _interaction = interaction;
    }

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        if (Input.Button != "create")
        {
            if (context != null)
            {
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                if (context.IsNativeClient())
                {
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl ?? "~/");
            }
            else
            {
                return Redirect("~/");
            }
        }

        if (_users.FindByUsername(Input.Username) != null)
        {
            ModelState.AddModelError("Input.Username", "Invalid username");
        }

        if (ModelState.IsValid)
        {
            var user = _users.CreateUser(Input.Username, Input.Password, Input.Name, Input.Email);

            var isuser = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.Username
            };

            await HttpContext.SignInAsync(isuser);

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl ?? "~/");
            }

            if (Url.IsLocalUrl(Input.ReturnUrl))
            {
                return Redirect(Input.ReturnUrl);
            }
            else if (string.IsNullOrEmpty(Input.ReturnUrl))
            {
                return Redirect("~/");
            }
            else
            {
                throw new ArgumentException("invalid return URL");
            }
        }

        return Page();
    }
}
