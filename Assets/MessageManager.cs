using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public class MessageManager
{
    readonly List<Message> history = new List<Message>();
    readonly IService service;
    String Url;

    public MessageManager(IService service)
    {
        this.service = service;
    }

    public async Task<bool> Connect(String url)
    {
        if (await service.Connect(url))
        {
            Url = url;
            var listeningTask = Task.Run(Listen);
            return true;
        }
        return false;
    }

    public void Send(Message message)
    {
        Task.Run(() => { 
            if(!service.Connected){
                service.Connect(Url).Wait();
            }
            service.Send(message).Wait();
        });
    }

    async Task Listen()
    {
        while (service.Connected)
        {
            var message = await service.Receive();
            history.Add(message);
            message.SendBySelf = false;
            Game.DoMessage(message);
            Thread.Sleep(100);
        }
    }

}
