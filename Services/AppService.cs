using Microsoft.Extensions.Options;
using SyncTrelloTFS.DTO;
using SyncTrelloTFS.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncTrelloTFS.Services
{
    public interface IAppService
    {
        void Run(string[] args);
    }

    class AppService : IAppService
    {
        private readonly AppSettings _config;
        private readonly ITrelloApi _trelloApi;
        private readonly ITFSApi _tfsApi;
        private readonly ITrelloToTFS _trelloToTFS;
        private readonly ITrelloReportService _trelloReportService;

        public AppService(IOptions<AppSettings> config,
             ITrelloApi trelloApi,
             ITFSApi tfsApi,
             ITrelloToTFS trelloToTFS,
             ITrelloReportService trelloReportService)
        {
            _config = config.Value;
            _trelloApi = trelloApi;
            _tfsApi = tfsApi;
            _trelloToTFS = trelloToTFS;
            _trelloReportService = trelloReportService;
        }

        public void Run(string[] args)
        {
            string process = args[0];
            if (process.Equals("Report"))
                _trelloReportService.RunWeeklyStatusReport();
            else
                _trelloToTFS.RunTFSTrelloSync();
        }

    }
}
