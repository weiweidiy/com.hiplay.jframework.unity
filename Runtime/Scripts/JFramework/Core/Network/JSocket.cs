using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public class JSocket : JBaseSocket
    {
        private Socket _socket;
        private string _host;
        private int _port;
        private bool _isOpen;

        public override bool IsOpen { get => _isOpen; set => _isOpen = value; }

        public override void Init(string url, string token = null)
        {
            var parts = url.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out _port))
            {
                OnError(this, "Invalid url format. Use host:port");
                return;
            }
            _host = parts[0];
        }

        public override void Open()
        {
            try
            {
                if (_socket != null)
                {
                    Close();
                }
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_host, _port);
                _isOpen = true;
                OnOpen(this);
            }
            catch (Exception ex)
            {
                _isOpen = false;
                OnError(this, ex.Message);
            }
        }

        public override void Close()
        {
            try
            {
                _isOpen = false;
                _socket?.Shutdown(SocketShutdown.Both);
                _socket?.Close();
                _socket = null;
                OnClosed(this, SocketStatusCodes.NormalClosure, "Closed by user");
            }
            catch (Exception ex)
            {
                OnError(this, ex.Message);
            }
        }

        public override async Task<byte[]> Send(byte[] data)
        {
            if (_socket == null || !_isOpen)
            {
                OnError(this, "Socket is not open.");
                return null;
            }
            try
            {
                await _socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
                var buffer = new byte[4096];
                int bytesRead = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                if (bytesRead > 0)
                {
                    var responseData = new byte[bytesRead];
                    Array.Copy(buffer, responseData, bytesRead);
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                OnError(this, ex.Message);
            }
            return null;
        }

        public override async Task<string> Send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            var response = await Send(data);
            return response != null ? Encoding.UTF8.GetString(response) : null;
        }

        public override Task RPCVoid(string method, object param = null, TimeSpan? timeout = null)
        {
            throw new NotImplementedException("没有实现RPC调用");
        }

        public override Task<TResponse> RPC<TResponse>(string method, object param = null, TimeSpan? timeout = null)
        {
            throw new NotImplementedException("没有实现RPC调用");
        }

     
    }
}
