
using PetCareApp.Core.Application.Interfaces;

namespace PetCareApp.Core.Application.Services
{
    public class Logger : Ilogger
    {
        public void Info(string mensaje, string usuario = "")
        {
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} {(!string.IsNullOrEmpty(usuario)  ? $"Usuario:{usuario}" : "")} - {mensaje}");
        }

        public void Error(string mensaje, string usuario = "")
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} {(!string.IsNullOrEmpty(usuario) ? $"Usuario:{usuario}" : "")} - {mensaje}");
        }
    }
}
