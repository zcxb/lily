using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Shared.Enums
{
    public enum UserLoginStatus : int
    {
        Success = 0,

        WrongPassword = 1,

        AccountNotFound = 2,
    }
}
