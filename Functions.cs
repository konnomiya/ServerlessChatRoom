using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace AzureSignalrTriggerFunction
{
    public static class Functions
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "chat", UserId = "{headers.x-ms-signalr-userid}")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("broadcast")]
        public static Task Broardcast(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            string name = req.Query["name"];
            string message = req.Query["message"];

            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "broadcastMessage",
                    Arguments = new[] { name, message }
                });
        }

        [FunctionName("senduser")]
        public static Task PrivateMessageToQueue(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequest req,
             [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            string name = req.Query["name"];
            string userId = req.Query["userId"];
            string message = req.Query["message"];

            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    UserId = userId,
                    Target = "echo",
                    Arguments = new[] { name, message }
                });
        }

    }
}
