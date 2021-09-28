using System;
using System.Collections.Generic;
using System.Text;

namespace SPP1.Serialize
{
    public interface ISerializer
    {
        public string Serialize(object obj);
    }
}
