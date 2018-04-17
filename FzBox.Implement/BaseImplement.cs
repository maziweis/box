using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzBox.Implement
{
    public abstract class BaseImplement
    {
        public abstract KingResponse ProcessRequest(KingRequest request);
    }
}
