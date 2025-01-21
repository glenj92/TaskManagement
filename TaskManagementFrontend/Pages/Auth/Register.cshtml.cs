using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;


public class RegisterModel : PageModel
{
    private readonly ApiService _apiService;

    [BindProperty]
    public RegisterRequest RegisterRequest { get; set; }

    [BindProperty]
    public string ConfirmPassword { get; set; }

    public string Message { get; set; }

    public RegisterModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public void OnGet()
    {
        // Método ejecutado al cargar la página de registro
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        // Verificar que las contraseñas coincidan
        if (RegisterRequest.Password != RegisterRequest.ConfirmPassword)
        {
            Message = "Passwords do not match.";
            return Page();
        }

        try
        {
            var isRegistered = await _apiService.Register(RegisterRequest.Name, RegisterRequest.Email, RegisterRequest.Password);

            if (isRegistered)
            {
                Message = "Registration successful! You can now log in.";
                return RedirectToPage("/Auth/Login");
            }
        }
        catch (System.Exception ex)
        {
            Message = $"Error: {ex.Message}";
        }

        return Page();
    }
}

public class RegisterRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; } // Nuevo campo para confirmar contraseña

}

