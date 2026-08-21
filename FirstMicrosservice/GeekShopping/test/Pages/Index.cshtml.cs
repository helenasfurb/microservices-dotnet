using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace test.Pages.Home
{
    [AllowAnonymous]
    public class Index : PageModel
    {
    }
}
