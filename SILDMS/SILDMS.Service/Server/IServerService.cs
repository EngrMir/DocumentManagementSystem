using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;
using SILDMS.Utillity;

namespace SILDMS.Service.Server
{
    public interface IServerService
    {
        ValidationResult GetServers(string id, string action, out List<SEC_Server> serverList);
        ValidationResult ManipulateServer(SEC_Server server, string action, out string status);
    }
}
