using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnimusTest.Controls
{
    public class AuthController
    {

        public static bool isLoggedIn()
        {
            // Тут має бути логіка для перевірки, чи користувач авторизований
            // Наприклад, перевірка наявності токена в локальному сховищі
            string token = File.Exists("E://AUTHTOKEN.txt")
                ? File.ReadAllText("E://AUTHTOKEN.txt")
                : null;
            return !string.IsNullOrEmpty(token);
        }

        public static async Task<bool> Login(string username, string password)
        {
            var loginSuccess = await LoginAsync(username, password);
            if (loginSuccess)
            {
                Console.WriteLine("Login successful!");
                return true;
            }
            else
            {
                Console.WriteLine("Login failed.");
                return false;
            }
        }

        private static async Task<bool> LoginAsync(string username, string password)
        {

            HttpClient _httpClient = new HttpClient();

            var loginUrl = "http://localhost:3000/api/login";

            var loginData = new
            {
                username,
                password
            };

            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(loginUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    string token = responseJson?.token;

                    if (!string.IsNullOrEmpty(token))
                    {
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                        //зробити запис токена в локальне сховище
                        File.WriteAllText("E://AUTHTOKEN.txt", token);


                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Login failed: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return false;
        }

        public static void Register(string username, string password)
        {
            // Тут має бути логіка для реєстрації нового користувача
            // Наприклад, збереження логіна та пароля в базі даних
            Console.WriteLine("User registered successfully!");
        }

        public static void Logout()
        {
            // Тут має бути логіка для виходу з системи
            // Наприклад, очищення сесії користувача
            Console.WriteLine("User logged out successfully!");
        }
    }
}
