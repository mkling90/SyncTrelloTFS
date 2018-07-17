using Microsoft.Extensions.Options;
using SyncTrelloTFS.DTO;
using SyncTrelloTFS.Services;
using SyncTrelloTFS.Services.HelperObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncTrelloTFS.Business
{
    public interface ITrelloToTFS
    {
        void RunTFSTrelloSync();
    }

    public class TrelloToTFS : ITrelloToTFS
    {
        private readonly AppSettings _config;
        private readonly ITrelloApi _trelloApi;
        private readonly ITFSApi _tfsApi;

        public TrelloToTFS(IOptions<AppSettings> config,
             ITrelloApi trelloApi,
             ITFSApi tfsApi)
        {
            _config = config.Value;
            _trelloApi = trelloApi;
            _tfsApi = tfsApi;
        }

        public void RunTFSTrelloSync()
        {
            DevCompleteToCommitted();
            ReadyForTestToReadyForTest();
        }

        public void DevCompleteToCommitted()
        {
            // Get Trello cards that are in dev complete
            List<TrelloCard> devCompleteCards = _trelloApi.GetListCards(TrelloListNames.DEVELOPMENTCOMPLETE);
            //For each card, set the TFS status to 'committed'
            foreach (TrelloCard card in devCompleteCards)
            {
                //Get card id
                String tfsID = GetCardTFSId(card);
                TFSWorkItem wi = _tfsApi.GetWorkItem(tfsID);
                //Check the status in tfs.  The status should be "committed", if not, set it if possible,
                if (wi.fields.State != "Committed")
                {
                    //Can't go directly from Test -> Committed, have to set the status to new first
                    if (wi.fields.State == "Ready for Test")
                        _tfsApi.PatchWorkItem(wi.id, PatchWorkItem.CreateNewWorkItemPatch());
                    _tfsApi.PatchWorkItem(wi.id, PatchWorkItem.CreateCommittedWorkItemPatch());
                }
                
            }
        }

        public void ReadyForTestToReadyForTest()
        {
            // Get Trello cards that are in ready to test
            List<TrelloCard> rdyTestCards = _trelloApi.GetListCards(TrelloListNames.READYFORTEST);
            //For each card, set the TFS status to 'ready to test'
            foreach (TrelloCard card in rdyTestCards)
            {
                //Get card id
                String tfsID = GetCardTFSId(card);
                TFSWorkItem wi = _tfsApi.GetWorkItem(tfsID);
                //Check the status in tfs.  The status should be "committed", if not, set it if possible,
                if (wi.fields.State != "Ready for Test")
                {
                    _tfsApi.PatchWorkItem(wi.id, PatchWorkItem.CreateReadyForTestWorkItemPatch());
                }
            }
        }

        private string GetCardTFSId(TrelloCard card)
        {
            int start, end;
            if (card.name.Contains("Bug #"))
                start = card.name.IndexOf("Bug #") + 5;
            else
                start = card.name.IndexOf("Product Backlog Item #") + 22;
            end = card.name.IndexOf(":");
            return card.name.Substring(start, end - start);
        }
    }
}
