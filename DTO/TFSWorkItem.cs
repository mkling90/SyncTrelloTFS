using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncTrelloTFS.DTO
{
    public class TFSWorkItem
    {
        public string id;
        public TFSFields fields;
    }

    public class TFSFields
    {
        [JsonProperty("System.WorkItemType")]
        public string WorkItemType { get; set; }

        [JsonProperty("System.State")]
        public string State { get; set; }

        [JsonProperty("System.AssignedTo")]
        public string AssignedTo { get; set; }
    }

    public class PatchWorkItem
    {
        public string op { get; set; }
        public string path { get; set; }
        public string value { get; set; }

        public static PatchWorkItem CreateSinglePatchWorkItem(string path, string value)
        {
            PatchWorkItem item = new PatchWorkItem()
            {
                op = "add",
                path = path,
                value = value
            };
            return item;
        }
        public static PatchWorkItem CreateCommittedWorkItemPatch()
        {
            return CreateSinglePatchWorkItem("/fields/System.State", "Committed");
        }

        public static PatchWorkItem CreateReadyForTestWorkItemPatch()
        {
            return CreateSinglePatchWorkItem("/fields/System.State", "Ready for Test");
        }

        public static PatchWorkItem CreateNewWorkItemPatch()
        {
            return CreateSinglePatchWorkItem("/fields/System.State", "New");
        }
    }
}
