using System;
using System.Collections.Generic;
using System.Text;

namespace SyncTrelloTFS
{
    public class AppSettings
    {
        // Trello Settings
        public string Title { get; set; }
        public string TrelloBaseUrl { get; set; }
        public string TrelloKey { get; set; }
        public string TrelloToken { get; set; }

        //TFS Settings
        public string TFSBaseUrl { get; set; }
        public string TFSPersonalAccessToken { get; set; }

    }
}
