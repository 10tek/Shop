using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Services.Abstract
{
    public interface IRegistration
    {
        int SendCode(string phoneNumber);
    }
}
