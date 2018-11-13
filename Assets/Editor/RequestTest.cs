using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

public class RequestTest
{

    [Test]
    public void ChooseRemoteRequestTest()
    {
        Game.Initialize();
        Game.SetTestMode();
        Game.MessageManager = new MessageManager(new DummyService());
        Game.MessageManager.Connect("").Wait();
        var player = Game.Player;
        var rival = Game.Rival;
        var card1 = CardFactory.CreateCard(1, player);
        var card2 = CardFactory.CreateCard(2, player);
        var card3 = CardFactory.CreateCard(3, player);
        var task = Request.Choose(new List<Card> { card1, card2, card3 }, rival);
        task.Wait(5000);
        Assert.IsTrue(task.IsCompleted);
        Assert.AreEqual(3, task.Result);
        Assert.AreSame(card1, task.Result[0]);
        Game.MessageManager = null;
    }

    class DummyService : IService
    {
        public bool Connected {
            get {
                return true;
            }
        }

        public Task<bool> Connect(string url)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Join(string roomId)
        {
            return Task.FromResult(true);
        }

        public async Task<string> Receive()
        {
            LogUtils.Log("Service Receive() called: " + Thread.CurrentThread.Name);
            var result = await Task.Run(() =>
            {
                while (queue.Count == 0)
                {
                    Thread.Sleep(100);
                    LogUtils.Log("Queue: " + queue.GetHashCode() + " count = " + queue.Count);
                }
                var requestString = queue.Dequeue();
                var request = RemoteRequest.FromString(requestString) as ChooseRemoteRequest<Card>;
                request.Response = request.Choices;
                return request.ToString();
            });
            return result;
        }

        public async Task Send(string data)
        {
            var sendTask = Task.Run(() =>
            {
                LogUtils.Log("Service Send() called: " + Thread.CurrentThread.Name);
                queue.Enqueue(data);
                LogUtils.Log("Enqueue: " + queue.GetHashCode() + " count = " + queue.Count);
            });
        }

        readonly Queue<string> queue = new Queue<string>();
    }
}

