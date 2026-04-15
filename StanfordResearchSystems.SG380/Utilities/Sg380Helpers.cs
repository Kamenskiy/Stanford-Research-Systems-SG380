using System.Net.Sockets;

namespace StanfordResearchSystems.SG380.Utilities
{
    /// <summary>
    /// Helper
    /// </summary>
    public static class Sg380Helpers
    {
        /// <summary>
        /// Проверяет доступность генератора по указанному IP и порту (синхронно).
        /// </summary>
        /// <param name="ipAddress">IP-адрес</param>
        /// <param name="port">TCP-порт (по умолчанию 5025)</param>
        /// <param name="timeoutMs">Таймаут в миллисекундах</param>
        public static bool Ping(string ipAddress, int port = 5025, int timeoutMs = 2000)
        {
            using var client = new TcpClient();
            var result = client.BeginConnect(ipAddress, port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(timeoutMs);
            if (success)
            {
                client.EndConnect(result);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверяет доступность генератора по указанному IP и порту (асинхронно).
        /// </summary>
        /// <param name="ipAddress">IP-адрес</param>
        /// <param name="port">TCP-порт (по умолчанию 5025)</param>
        /// <param name="timeoutMs">Таймаут в миллисекундах</param>
        public static async Task<bool> PingAsync(string ipAddress, int port = 5025, int timeoutMs = 2000)
        {
            using var client = new TcpClient();
            var connectTask = client.ConnectAsync(ipAddress, port);
            var timeoutTask = Task.Delay(timeoutMs);
            var completedTask = await Task.WhenAny(connectTask, timeoutTask);
            if (completedTask == connectTask)
            {
                await connectTask; // ensure exception if any
                return true;
            }
            return false;
        }
    }
}