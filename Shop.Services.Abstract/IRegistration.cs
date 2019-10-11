using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Services.Abstract
{
    public interface IRegistration
    {
        void Register(string accaunt);
        bool CheckCode(int code);
    }
}
