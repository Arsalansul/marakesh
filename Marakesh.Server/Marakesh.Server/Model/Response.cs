using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Marakesh.Server.Model
{
    [Serializable]
    public class Response
    {
        [JsonIgnore]
        private int size;

        public Response()
        {
            
        }

        public static int GetSize()
        {
#warning rewrite this! use separate sizeof class?
            var resp = new Response();
            var json = JsonConvert.SerializeObject(resp);
            return Encoding.UTF8.GetBytes(json).Length;
        }
        public ResponseType responseType { get; set; }
        public int PlayerCount { get; set; }
    }

    public enum ResponseType
    {
        PlayerCount = 1,
    }
}
