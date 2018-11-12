using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public class MessageManager
{
    readonly List<Message> history = new List<Message>();
    readonly IService service;

    public MessageManager(IService service)
    {
        this.service = service;
    }

    public async Task<bool> Connect(String url)
    {
        if (await service.Connect(url))
        {
            Task.Run(Listen).Start();
            return true;
        }
        return false;
    }

    public void Send(Message message)
    {
        Task.Run(() => service.Send(message));
    }

    Task Listen()
    {
        while (service.Connected)
        {
            Thread.Sleep(100);
        }
        return Task.CompletedTask;
    }

}
