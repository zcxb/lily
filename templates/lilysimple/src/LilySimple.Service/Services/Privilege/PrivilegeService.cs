using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LilySimple.Services.Privilege
{
    public class PrivilegeService : ServiceBase
    {
        private readonly ILogger<PrivilegeService> _logger;

        public PrivilegeService(ILogger<PrivilegeService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> CheckPermission(int userId, string permissionName)
        {
            // TODO

            return false;
        }
    }
}
