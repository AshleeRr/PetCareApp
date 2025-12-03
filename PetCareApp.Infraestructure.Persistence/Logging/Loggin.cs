using PetCareApp.Core.Application.Interfaces;
using System.Collections.Concurrent;

namespace PetCareApp.Infraestructure.Persistence.Logging
{
    public class Loggin: Ilogger
    {

        private static readonly Lazy<Loggin> _inst = new(() => new Loggin());
        public static Loggin Instance => _inst.Value;

        private readonly BlockingCollection<string> _queue = new();
        private readonly string _logPath;

        private Loggin()
        {
            var dir = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(dir);
            _logPath = Path.Combine(dir, "app.log");
            var t = new Thread(Writer) { IsBackground = true };
            t.Start();
        }

        private void Writer()
        {
            foreach (var msg in _queue.GetConsumingEnumerable())
            {
                File.AppendAllText(_logPath, msg + Environment.NewLine);
            }
        }

        public void Info(string mensaje, string usuario = "")
            => _queue.Add($"[INFO] {DateTime.UtcNow:o} {usuario} {mensaje}");

        public void Error(string mensaje, string usuario = "")
            => _queue.Add($"[ERROR] {DateTime.UtcNow:o} {usuario} {mensaje}");

        public void Warning(string mensaje, string usuario = "")
            => _queue.Add($"[Warning] {DateTime.UtcNow:o} {usuario} {mensaje}");
    }
}
