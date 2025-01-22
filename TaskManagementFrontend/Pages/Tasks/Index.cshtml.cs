using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementFrontend.Services; 
using TaskManagementFrontend.Models; 


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

    [TempData]
    public string AlertMessage { get; set; }

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

    public async Task<IActionResult> OnPostCreateAsync()
    {
        try
        {
            Token = HttpContext.Session.GetString("AuthToken");
            var isTasked =  await _apiService.CreateTask(NewTask, Token);
            if (isTasked)
            {
                Message = "Registration successful!";
                return RedirectToPage();
            }
        }
        catch (System.Exception ex)
        {
            Message = $"Error: {ex.Message}";
        }

         return RedirectToPage();

        
    }


     public async Task<IActionResult> OnPostUpdateAsync()
    {

        Token = HttpContext.Session.GetString("AuthToken");

        if (!string.IsNullOrEmpty(Token))
        {
            // var success = await _apiService.UpdateTask(EditTask.Id, EditTask.Title, EditTask.Description, EditTask.Status, EditTask.AssignedUserId, Token);
            var success = await _apiService.UpdateTask(EditTask, Token);

            if (success)
            {
                Message = "Updated successfuly!";
                return RedirectToPage();
            }
            else
            {   
                ModelState.AddModelError(string.Empty, "Error updating the task.");
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

