using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UnityOyun.Assets.Model
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // İstemci tarafından gönderilen mesaj model
    public class SocketMessage
    {
        public string Action { get; set; } = "";
        public string Data { get; set; } = "";
    }

    // Sunucunun gönderdiği yanıt model
    public class ResponseMessage
    {
        public string Action { get; set; } = "";
        public string Data { get; set; } = "";
    }
}
