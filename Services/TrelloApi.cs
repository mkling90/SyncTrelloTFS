using Microsoft.Extensions.Options;
using RestSharp;
using SyncTrelloTFS.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SyncTrelloTFS.Services.HelperObjects;

namespace SyncTrelloTFS.Services
{
    public interface ITrelloApi
    {
        T Execute<T>(RestRequest request) where T : new();
        String GetKey();
        List<TrelloCard> GetListCards(TrelloListNames listName);
    }

    public class TrelloApi : ITrelloApi
    {
        private readonly AppSettings _config;
        


        public TrelloApi(IOptions<AppSettings> config)
        {
            _config = config.Value;
        }


        
        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(_config.TrelloBaseUrl);

            request.AddUrlSegment("keyId", _config.TrelloKey);
            request.AddUrlSegment("tokenId", _config.TrelloToken);

            var response = client.Execute(request);
            if(response.ErrorException != null)
            {
                //handle call exception
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response.Content);
            //return response;
        }

        public List<TrelloCard> GetListCards(TrelloListNames listName)
        {
            //Get the Current Development board
            String currentDevBoardId = GetCurrentDevelopmentBoardId();
            String listId = GetListId(currentDevBoardId, listName);

            var cardRequest = new RestRequest("1/lists/{listId}/cards?key={keyId}&token={tokenId}", Method.GET);
            cardRequest.AddUrlSegment("listId", listId);
            List<TrelloCard> listCards = Execute<List<TrelloCard>>(cardRequest);

            return listCards.Where(c => c.name.Contains("Bug #") || c.name.Contains("Product Backlog Item #")).ToList();
        }

        private string GetCurrentDevelopmentBoardId()
        {
            var boardRequest = new RestRequest("1/members/me/boards?key={keyId}&token={tokenId}", Method.GET);
            List<TrelloBoard> boards = Execute<List<TrelloBoard>>(boardRequest);
            foreach (TrelloBoard board in boards)
            {
                if (board.name.Equals("Exida Current Development"))
                    return board.id;
            }
            return String.Empty;
        }

        private string GetListId(string currentDevBoardId, TrelloListNames listName)
        {
            var listRequest = new RestRequest("1/boards/{currentDevBoardId}/lists?key={keyId}&token={tokenId}", Method.GET);
            listRequest.AddUrlSegment("currentDevBoardId", currentDevBoardId);
            List<TrelloList> trelloLists = Execute<List<TrelloList>>(listRequest);
            TrelloList list = trelloLists.FirstOrDefault(l => l.name.Equals(listName.ToString()));
            if (list != null)
                return list.id;
            return String.Empty;
        }

        public string GetKey()
        {
            return _config.TrelloKey;
        }

        
    }
}
