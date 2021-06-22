using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LilySimple.Shared.Enums
{
    public enum PermissionType : int
    {
        [Description("resource")]
        Resource = 1,

        [Description("action")]
        Action = 2,
    }
}
