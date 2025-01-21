using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    private readonly ApiService _apiService;

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public LoginModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            // Llama a la API de login
            var token = await _apiService.Login(Email, Password);

            // Guarda el token en la sesión o cookie
            HttpContext.Session.SetString("AuthToken", token);

            // Redirige al dashboard (o a otra página después de iniciar sesión)
            return RedirectToPage("/Tasks/Index");
        }
        catch (Exception ex)
        {
            // Muestra un mensaje de error si el login falla
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}
