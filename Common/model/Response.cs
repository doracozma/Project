using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Common
{
    public class Response : Dictionary<string, Object>
    {

        public Response() : base(new Dictionary<string, Object>())
        {
        }


        public void add(string valueName, Object value)
        {
            this.Add(valueName, value);
        }

        public Object get(string valueName)
        {
            return this.GetValueOrDefault(valueName, null);
        }

    }
}