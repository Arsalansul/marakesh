using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Marakesh.Server.Model
{
    [Serializable]
    public class FirstRequest
    {
        public RequestType RequestType { get; set; }

        public static int GetSize()
        {
#warning rewrite this! use separate sizeof class?
            var resp = new FirstRequest();
            var json = JsonConvert.SerializeObject(resp);
            return Encoding.UTF8.GetBytes(json).Length;
        }
    }

    [Serializable]
    public class PlayerCountRequest
    {
        public RequestType RequestType { get; set; } = RequestType.PlayerCount;
        public static int GetSize()
        {
#warning rewrite this! use separate sizeof class?
            var resp = new FirstRequest();
            var json = JsonConvert.SerializeObject(resp);
            return Encoding.UTF8.GetBytes(json).Length;
        }
    }

    public enum RequestType
    {
        PlayerCount = 1,
    }
}
