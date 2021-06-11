using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Enums
{
    public enum UserLoginStatus : int
    {
        Success = 0,

        WrongPassword = 1,

        AccountNotFound = 2,
    }
}
