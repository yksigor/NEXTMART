using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Security
{
    public class Token
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }

        public Token(bool Authenticated, string Created, string Expiration, string AccessToken, string Message)
        {
            this.Authenticated = Authenticated;
            this.Created = Created;
            this.Expiration = Expiration;
            this.AccessToken = AccessToken;
            this.Message = Message;
        }

        public Token() { }
    }
}
