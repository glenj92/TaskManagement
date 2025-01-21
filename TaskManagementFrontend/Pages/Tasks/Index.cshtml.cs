using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TasksModel : PageModel
{
    private readonly ApiService _apiService;

    [BindProperty]
    public List<TaskDto> Tasks { get; set; }

    [BindProperty]
    public TaskDto NewTask { get; set; }

    [BindProperty]
    public TaskDto EditTask { get; set; }

    [BindProperty]
    public string FilterStatus { get; set; }

    public string Token { get; set; }
    public string Message { get; set; }

    public TasksModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync(string filterStatus = null)
    {
        Token = HttpContext.Session.GetString("AuthToken");

        if (!string.IsNullOrEmpty(Token))
        {
            var allTasks = await _apiService.GetTasks(Token);
            FilterStatus = filterStatus;

            Tasks = string.IsNullOrEmpty(filterStatus)
                ? allTasks
                : allTasks.Where(t => t.Status == filterStatus).ToList();
        }
    }

    //   public async Task<IActionResult> OnGet()
    // {
    //     var token = HttpContext.Session.GetString("AuthToken");
    //     if (string.IsNullOrEmpty(token))
    //     {
    //         return RedirectToPage("/Auth/Login");
    //     }

    //     Tasks = await _apiService.GetTasks(token);
    //     return Page();
    // }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            Token = HttpContext.Session.GetString("AuthToken");
            var isTasked =  await _apiService.CreateTask(NewTask.Title, NewTask.Description, NewTask.Status, Token);

            if (isTasked)
            {
                Message = "Registration successful! You can now log in.";
                return RedirectToPage("/Tasks");
            }
        }
        catch (System.Exception ex)
        {
            Message = $"Error: {ex.Message}";
        }

        // return RedirectToPage();
        return Page();


        
    }

     // Actualizar tarea
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Token = HttpContext.Session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(Token))
            {
                // Usar ApiService para actualizar la tarea
                var success = await _apiService.UpdateTask(EditTask, Token);

                if (success)
                {
                    return RedirectToPage(); // Redirige para actualizar la lista
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar la tarea.");
                    return Page();
                }
            }

            return Unauthorized();
        }
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        Token = HttpContext.Session.GetString("AuthToken");
        await _apiService.DeleteTask(id, Token);

        return RedirectToPage();
    }
}


public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}


