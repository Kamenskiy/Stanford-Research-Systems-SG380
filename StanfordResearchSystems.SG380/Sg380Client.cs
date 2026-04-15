using System.Net.Sockets;
using System.Text;

namespace StanfordResearchSystems.SG380
{
    /// <summary>
    /// Клиент для управления RF-генераторами SG382, SG384, SG386 по протоколу TCP/IP.
    /// </summary>
    public sealed partial class Sg380Client : IDisposable
    {
        private const int DefaultPort = 5025;
        private const int DefaultTimeoutMs = 5000;
        private const int ReceiveBufferSize = 1024;

        private TcpClient? _tcpClient;
        private NetworkStream? _stream;
        private readonly SemaphoreSlim _syncSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// IP-адрес генератора.
        /// </summary>
        public string IpAddress { get; }

        /// <summary>
        /// TCP-порт генератора.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Таймаут операций в миллисекундах. По умолчанию 5000.
        /// </summary>
        public int TimeoutMs { get; set; } = DefaultTimeoutMs;

        /// <summary>
        /// Указывает, установлено ли соединение.
        /// </summary>
        public bool IsConnected => _tcpClient?.Connected == true;

        /// <summary>
        /// Создаёт новый экземпляр клиента.
        /// </summary>
        /// <param name="ipAddress">IP-адрес генератора.</param>
        /// <param name="port">TCP-порт (по умолчанию 5025).</param>
        public Sg380Client(string ipAddress, int port = DefaultPort)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("IP-адрес не может быть пустым.", nameof(ipAddress));

            IpAddress = ipAddress;
            Port = port;
        }

        #region Connection Management

        /// <summary>
        /// Устанавливает соединение с генератором (синхронно).
        /// </summary>
        public void Connect()
        {
            if (IsConnected)
                return;

            _tcpClient = new TcpClient();
            _tcpClient.Connect(IpAddress, Port);
            _stream = _tcpClient.GetStream();
            _stream.ReadTimeout = TimeoutMs;
            _stream.WriteTimeout = TimeoutMs;
        }

        /// <summary>
        /// Устанавливает соединение с генератором (асинхронно).
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (IsConnected)
                return;

            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(IpAddress, Port, cancellationToken).ConfigureAwait(false);
            _stream = _tcpClient.GetStream();
            _stream.ReadTimeout = TimeoutMs;
            _stream.WriteTimeout = TimeoutMs;
        }

        /// <summary>
        /// Разрывает соединение.
        /// </summary>
        private void Disconnect()
        {
            _stream?.Dispose();
            _tcpClient?.Dispose();
            _stream = null;
            _tcpClient = null;
        }

        #endregion

        #region Low-level Communication

        private void EnsureConnected()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Соединение не установлено. Вызовите Connect() перед отправкой команд.");
        }

        /// <summary>
        /// Отправляет команду без ожидания ответа (синхронно).
        /// </summary>
        /// <param name="command">Строка команды (должна заканчиваться символом новой строки).</param>
        private void SendCommand(string command)
        {
            EnsureConnected();
            var data = Encoding.ASCII.GetBytes(command);
            _syncSemaphore.Wait();
            try
            {
                _stream?.Write(data, 0, data.Length);
                _stream?.Flush();
            }
            finally
            {
                _syncSemaphore.Release();
            }
        }

        /// <summary>
        /// Отправляет команду без ожидания ответа (асинхронно).
        /// </summary>
        /// <param name="command">Строка команды (должна заканчиваться символом новой строки).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        private async Task SendCommandAsync(string command, CancellationToken cancellationToken = default)
        {
            EnsureConnected();
            var data = Encoding.ASCII.GetBytes(command);
            await _syncSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                if (_stream != null)
                {
                    await _stream.WriteAsync(data.AsMemory(0, data.Length), cancellationToken).ConfigureAwait(false);
                    await _stream.FlushAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                _syncSemaphore.Release();
            }
        }

        /// <summary>
        /// Отправляет запрос и возвращает ответ (синхронно).
        /// </summary>
        /// <param name="query">Строка запроса (должна заканчиваться символом новой строки).</param>
        /// <returns>Строка ответа без завершающих символов.</returns>
        private string SendQuery(string query)
        {
            SendCommand(query);
            return ReadResponse();
        }

        /// <summary>
        /// Отправляет запрос и возвращает ответ (асинхронно).
        /// </summary>
        /// <param name="query">Строка запроса (должна заканчиваться символом новой строки).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Строка ответа без завершающих символов.</returns>
        public async Task<string> SendQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            await SendCommandAsync(query, cancellationToken).ConfigureAwait(false);
            return await ReadResponseAsync(cancellationToken).ConfigureAwait(false);
        }

        private string ReadResponse()
        {
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[ReceiveBufferSize];
                int bytes;
                while (_stream != null && (bytes = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytes);
                    if (ms.Length > 0 && ms.ToArray()[ms.Length - 1] == '\n')
                        break;
                }
                return Encoding.ASCII.GetString(ms.ToArray()).Trim();
            }
        }

        private async Task<string> ReadResponseAsync(CancellationToken cancellationToken)
        {
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[ReceiveBufferSize];
                int bytes;
                while (_stream != null && (bytes = await _stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken).ConfigureAwait(false)) > 0)
                {
                    ms.Write(buffer, 0, bytes);
                    if (ms.Length > 0 && ms.ToArray()[ms.Length - 1] == '\n')
                        break;
                }
                return Encoding.ASCII.GetString(ms.ToArray()).Trim();
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Освобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            _syncSemaphore.Dispose();
            Disconnect();
            //GC.SuppressFinalize(this);
        }

        #endregion
    }
}