# ServerlessChatRoom
A memo for the practice using Azure SignalR &amp; Azure Functions to build a serverless chat room. Have broadcast, echo and DM features.

## Workflow Overview
1) When a user joins the chat room, a http request is sent to trigger the **negotiate** http trigger function and get the SignalR URL and access token.
2) The client side starts connection with SignalR with the previous URL and access token.
3) For broadcasting a message, client side sends a http request to trigger the **broadcast** http trigger function, gets the SignalRMessage containing target method name and arguments that a hub can call.
4) For echo and DM, it works the same as broadcasting as long as the user is authenticated by userId when starts negotiate at the beginning.
