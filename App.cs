using Microsoft.Extensions.Options;
using SyncTrelloTFS.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncTrelloTFS
{
    public class App
    {
        private readonly IAppService _appService;
        private readonly AppSettings _config;

        public App(IAppService appService, IOptions<AppSettings> config)
        {
            _appService = appService;
            _config = config.Value;
        }

        public void Run(string[] args)
        {
            _appService.Run(args);

        }
    }
}
