using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SILDMS.Model.SecurityModule;

namespace SILDMS.DataAccessInterface.Server
{
    public interface IServerDataService
    {
        List<SEC_Server> GetServers(string id, string action, out string errorNumber);
        string ManipulateServer(SEC_Server server, string action, out string errorNumber);
    }
}
