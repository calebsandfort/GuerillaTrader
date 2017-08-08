using Abp.Application.Services;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuerillaTrader.Entities;
using GuerillaTrader.Shared;
using GuerillaTrader.Shared.SqlExecuter;

namespace GuerillaTrader.Services
{
    public class AppServiceBase : ApplicationService
    {
        #region Properties
        public readonly ISqlExecuter _sqlExecuter;
        public readonly IConsoleHubProxy _consoleHubProxy;
        public readonly IBackgroundJobManager _backgroundJobManager;
        public readonly IObjectMapper _objectMapper;
        #endregion

        #region Constructor
        public AppServiceBase(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, IBackgroundJobManager backgroundJobManager, IObjectMapper objectMapper)
        {
            _sqlExecuter = sqlExecuter;
            _consoleHubProxy = consoleHubProxy;
            _backgroundJobManager = backgroundJobManager;
            _objectMapper = objectMapper;
        }
        #endregion
    }
}
