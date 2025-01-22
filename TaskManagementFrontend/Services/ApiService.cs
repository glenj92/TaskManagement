using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using TaskManagementFrontend.Models;

namespace TaskManagementFrontend.Services
{

    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
       

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> Login(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(); // Devuelve el token
            }

            throw new Exception("Login failed");
        }

        // Método para registrar un nuevo usuario
        public async Task<bool> Register(string name, string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}/auth/register", new
            {
                Name = name,
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Registration failed: {errorContent}");
        }

        public async Task<List<TaskDto>> GetTasks(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_configuration["ApiBaseUrl"]}/tasks");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TaskDto>>();
            }

            throw new Exception("Failed to fetch tasks");
        }

        public async Task<bool> CreateTask(int id, string title, string description, string status, int? assigneduserid, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}/tasks", new
            {
                Title = title,
                Description = description,
                Status = status,
                AssignedUserId = assigneduserid
            });

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception("Failed to create task");
        }

        public async Task<bool> UpdateTask(int id, string title, string description, string status, int? assigneduserid, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"{_configuration["ApiBaseUrl"]}/tasks/{id}", new
            {
                Title = title,
                Description = description,
                Status = status,
                AssignedUserId = assigneduserid
            });

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception("Failed to update task");
        }

        public async Task<bool> DeleteTask(int taskId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{_configuration["ApiBaseUrl"]}/tasks/{taskId}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception("Failed to delete task");
        }




    }

  

}