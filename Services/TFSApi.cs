using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SyncTrelloTFS.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SyncTrelloTFS.Services
{
    public interface ITFSApi
    {
        T Execute<T>(RestRequest request) where T : new();
        TFSWorkItem GetWorkItem(string wid);
        bool PatchWorkItem(string wid, PatchWorkItem patchWorkItem);
    }

    public class TFSApi : ITFSApi
    {
        private readonly AppSettings _config;

        public TFSApi(IOptions<AppSettings> config)
        {
            _config = config.Value;
        }


        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(_config.TFSBaseUrl);
            client.Authenticator = new HttpBasicAuthenticator("", _config.TFSPersonalAccessToken);

            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                //handle call exception
            }
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public TFSWorkItem GetWorkItem(string wid)
        {
            var wiRequest = new RestRequest("exSILentiaProducts/_apis/wit/workitems/{wid}", Method.GET);
            wiRequest.AddUrlSegment("wid", wid);
            TFSWorkItem wiItem = Execute<TFSWorkItem>(wiRequest);

            return wiItem;
        }

        public bool PatchWorkItem(string wid, PatchWorkItem patchWorkItem)
        {
            var wiPatchRequest = new RestRequest("exSILentiaProducts/_apis/wit/workitems/{wid}?api-version=1.0", Method.PATCH);
            wiPatchRequest.AddUrlSegment("wid", wid);
            wiPatchRequest.AddHeader("Content-Type", "application/json-patch+json");
            wiPatchRequest.AddParameter("undefined", CreatePatchBody(patchWorkItem), ParameterType.RequestBody);
            TFSWorkItem wiItem = Execute<TFSWorkItem>(wiPatchRequest);
            return true;
        }

        private string CreatePatchBody(PatchWorkItem patchWorkItem)
        {
            String patchBody = $"  [{{\r\n\t\"op\":\"{patchWorkItem.op}\",\r\n\t\"path\":\"{patchWorkItem.path}\",\r\n\t\"value\":\"{patchWorkItem.value}\"\r\n\t}}]";
            return patchBody;
        }
    }
}
