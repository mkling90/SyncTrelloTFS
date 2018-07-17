using System;
using System.Collections.Generic;
using System.Text;

namespace SyncTrelloTFS.Services.HelperObjects
{
    public sealed class TrelloListNames
    {
        private readonly String name;
        private readonly int value;
        public static readonly TrelloListNames DEVELOPMENTCOMPLETE = new TrelloListNames(1, "Development Complete");
        public static readonly TrelloListNames READYFORTEST = new TrelloListNames(2, "Ready For Test (In Build)");
        public static readonly TrelloListNames LAUNCHPAD = new TrelloListNames(3, "Launchpad");

        private TrelloListNames(int value, String name)
        {
            this.name = name;
            this.value = value;
        }

        public override String ToString()
        {
            return name;
        }
    }
}
