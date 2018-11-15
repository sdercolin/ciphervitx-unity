using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public class MessageManager
{
    readonly List<Message> history = new List<Message>();
    readonly List<RemoteRequest> waitingRequests = new List<RemoteRequest>();
    readonly IService service;
    String Url;

    public MessageManager(IService service)
    {
        this.service = service;
    }

    public async Task<bool> Connect(String url)
    {
        LogUtils.Log("Start connecting: " + url);
        if (await service.Connect(url))
        {
            Url = url;
            Task.Run(() => Listen().Wait()).Forget();
            return true;
        }
        return false;
    }

    public void Send(Message message)
    {
        Task.Run(() =>
        {
            if (!service.Connected)
            {
                service.Connect(Url).Wait();
            }
            service.Send(message.ToString()).Wait();
        });
    }

    async Task Listen()
    {
        LogUtils.Log("Successfully connected, start listening... ");
        while (service.Connected)
        {
            LogUtils.Log("Receiving...");
            var data = await service.Receive();
            LogUtils.Log("Received: " + data);
            var message = Message.FromString(data);
            if (message != null)
            {
                LogUtils.Log("Converted to Message: " + message.GetType().FullName);
                history.Add(message);
                message.SendBySelf = false;
                Game.DoMessage(message);
            }
            else
            {
                var request = RemoteRequest.FromString(data);
                if (request != null)
                {
                    LogUtils.Log("Converted to RemoteRequest: " + request.GetType().FullName);
                    if (request.Response != null)
                    {
                        var oriRequest = waitingRequests.Find(it => it.Guid == request.Guid);
                        oriRequest.Response = request.Response;
                        waitingRequests.Remove(oriRequest);
                    }
                }
            }
            Thread.Sleep(100);
        }
    }

    public async Task<RemoteRequest> Request(RemoteRequest request)
    {
        if (!service.Connected)
        {
            await service.Connect(Url);
        }
        await service.Send(request.ToString());
        waitingRequests.Add(request);
        LogUtils.Log("Requested: " + request.GetType().FullName);
        while (request.Response == null)
        {
            Thread.Sleep(200);
        }
        return request;
    }
}
