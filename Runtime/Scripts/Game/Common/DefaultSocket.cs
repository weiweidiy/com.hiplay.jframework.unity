using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace JFramework.Unity
{
    public class DefaultSocket : IJSocket
    {
        private Socket _socket;
        private string _url;
        private string _host;
        private int _port;
        private string _token;
        private bool _isOpen;
        private Thread _receiveThread;

        public bool IsOpen => _isOpen;

        public event Action<IJSocket> onOpen;
        public event Action<IJSocket, SocketStatusCodes, string> onClosed;
        public event Action<IJSocket, string> onError;
        public event Action<IJSocket, byte[]> onBinary;

        public void Init(string url, string token = null)
        {
            _url = url;
            _token = token;

            // 解析 host:port
            // 支持格式: "127.0.0.1:12345"
            var parts = url.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out _port))
            {
                onError?.Invoke(this, "Invalid url format. Use host:port");
                return;
            }
            _host = parts[0];
        }

        public void Open()
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
                onOpen?.Invoke(this);

                // 启动接收线程
                _receiveThread = new Thread(ReceiveLoop);
                _receiveThread.IsBackground = true;
                _receiveThread.Start();
            }
            catch (Exception ex)
            {
                _isOpen = false;
                onError?.Invoke(this, ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                _isOpen = false;
                _socket?.Shutdown(SocketShutdown.Both);
                _socket?.Close();
                _socket = null;
                onClosed?.Invoke(this, SocketStatusCodes.NormalClosure, "Closed by user");
            }
            catch (Exception ex)
            {
                onError?.Invoke(this, ex.Message);
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                if (_socket != null && _isOpen)
                {
                    _socket.Send(data);
                }
                else
                {
                    onError?.Invoke(this, "Socket is not open.");
                }
            }
            catch (Exception ex)
            {
                onError?.Invoke(this, ex.Message);
            }
        }

        private void ReceiveLoop()
        {
            var buffer = new byte[4096];
            try
            {
                while (_isOpen && _socket != null)
                {
                    int bytesRead = _socket.Receive(buffer);
                    if (bytesRead > 0)
                    {
                        var data = new byte[bytesRead];
                        Array.Copy(buffer, data, bytesRead);
                        onBinary?.Invoke(this, data);
                    }
                    else
                    {
                        // 连接关闭
                        _isOpen = false;
                        onClosed?.Invoke(this, SocketStatusCodes.NormalClosure, "Remote closed");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _isOpen = false;
                onError?.Invoke(this, ex.Message);
                onClosed?.Invoke(this, SocketStatusCodes.NormalClosure, "Receive error");
            }
        }
    }
}