using System;

#if(UNITY)
using UnityEngine;
#endif
namespace Model.UnityOyun.Assets.Model
{
    public enum SocketMessageType
    {
        LoginRequest,
        LoginResponse,
        GetObjects,
        ErrorResponse,
    }

    // İstemci tarafından gönderilen mesaj model

    [Serializable]
    public class SocketMessage
    {
        public string? MessType { get; set; }
        public string? Data { get; set; }
    }

    [Serializable]
    public abstract class MessageContentBase
    {
        // Base class properties
    }

    [Serializable]
    public class LoginRequest : MessageContentBase
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    [Serializable]
    public class LoginResponse : MessageContentBase
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    [Serializable]
    public class ErrorResponse : MessageContentBase
    {
        public string? Message { get; set; }
    }

    [Serializable]
    public class ResponseMessage : MessageContentBase
    {
        public string? Action { get; set; }
        public string? Data { get; set; }
    }

}
