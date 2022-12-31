using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.WebPubSub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace WebPubSubServer
{
    public static class WebPubSubConnector
    {
        [FunctionName(nameof(WebPubSubConnector))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req)
        {
            var user = req.Query["user"];
            if (user.ToString() == string.Empty)
            {
                return new BadRequestObjectResult("no user supplied");
            }

            var connectionString = Environment.GetEnvironmentVariable("PubSubKey");
            var serviceClient = new WebPubSubServiceClient(connectionString, "Hub");
            var url = await serviceClient.GetClientAccessUriAsync(DateTimeOffset.Now + TimeSpan.FromHours(2), user,
                new List<string> 
                    { 
                        "webpubsub.joinLeaveGroup",
                        "webpubsub.sendToGroup"
                    }, CancellationToken.None);
            return new OkObjectResult(url.ToString());
        }
    }
}
