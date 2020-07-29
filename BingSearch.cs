using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using Microsoft.Azure.CognitiveServices.Search.VideoSearch;
using Microsoft.Azure.CognitiveServices.Search.VideoSearch.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Net.Mime;

namespace QnABot
{

    public class BingSearch
    {
        public ITurnContext context { get; set; }
        public BingSearch(ITurnContext context)
        {
            this.context = context;
        }

        public async Task WebSearch(string subscriptionKey, string query)
        {
            var client = new WebSearchClient(new Microsoft.Azure.CognitiveServices.Search.WebSearch.ApiKeyServiceClientCredentials(subscriptionKey));
            try
            {
                var webResults = client.Web.SearchAsync(query).Result;
               
                if (webResults != null)
                {
                    
                    await context.SendActivityAsync("Searching in web for " + query);
                    await context.SendActivityAsync(webResults.WebPages.Value[0].Url);
                    await context.SendActivityAsync(webResults.WebPages.Value[1].Url);
                }
            }
            catch (Exception ex)
            {
                await context.SendActivityAsync("Could not retrieve image results due to:" + ex.Message);
            }
        }

        public async Task ImageSearch(string subscriptionKey, string query)
        {
            var client = new ImageSearchClient(new Microsoft.Azure.CognitiveServices.Search.ImageSearch.ApiKeyServiceClientCredentials(subscriptionKey));
            try
            {
                var imageResults = client.Images.SearchAsync(query).Result;
                if(imageResults!=null)
                {
                    var activity = MessageFactory.Attachment(new Attachment[]
                    {
                        new Attachment{ContentUrl=imageResults.Value[0].ContentUrl,ContentType="image/png"},
                        new Attachment{ContentUrl=imageResults.Value[1].ContentUrl,ContentType="image/png"},
                        new Attachment{ContentUrl=imageResults.Value[2].ContentUrl,ContentType="image/png"}
                    });
                    await context.SendActivityAsync("Searching for images related to " + query);
                    await context.SendActivityAsync(activity);
                }
            }
            catch (Exception ex)
            {
                await context.SendActivityAsync("Could not retrieve image results due to:" + ex.Message);
            }
        }

        public  async Task VideoSearch(string subscriptionKey, string query)
        {
            var client = new VideoSearchAPI(new Microsoft.Azure.CognitiveServices.Search.VideoSearch.ApiKeyServiceClientCredentials(subscriptionKey));
           // var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(subscriptionKey));
            try
            {
                var videoResults = client.Videos.SearchAsync(query).Result;
                if(videoResults!=null)
                {
                    var activity = MessageFactory.Attachment(new Attachment[]
                        {
                        new Attachment { ContentUrl=videoResults.Value[0].ContentUrl,ContentType="video/mp4"},
                        new Attachment { ContentUrl = videoResults.Value[1].ContentUrl, ContentType = "video/mp4" },
                        new Attachment { ContentUrl = videoResults.Value[2].ContentUrl, ContentType = "video/mp4" }
                    });
                    await context.SendActivityAsync("Searching for videos related to " + query);
                    await context.SendActivityAsync(activity);
                }
                
            }
            catch(Exception ex)
            {
                await context.SendActivityAsync("Could not retrieve video results due to:" + ex.Message);
            }
        }
        
        
    }
}
