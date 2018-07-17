using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using SyncTrelloTFS.DTO;
using SyncTrelloTFS.Services;
using SyncTrelloTFS.Services.HelperObjects;

namespace SyncTrelloTFS.Business
{
    public interface ITrelloReportService
    {
        void RunWeeklyStatusReport();
    }

    public class TrelloReportService : ITrelloReportService
    {
        public void RunWeeklyStatusReport()
        {
            //Run Weekly Report
            //Cards created

            //Cards
        }
    }

}
