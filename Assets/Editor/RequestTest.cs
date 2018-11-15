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
        player.Deck.AddCard(card1);
        player.Deck.AddCard(card2);
        player.Deck.AddCard(card3);
        var task = Request.Choose(new List<Card> { card1, card2, card3 }, rival);
        task.Wait();
        Assert.IsTrue(task.IsCompleted);
        Assert.IsNotNull(task.Result);
        Assert.AreEqual(3, task.Result.Count);
        Assert.AreSame(card1, task.Result[0]);
        Game.MessageManager = null;
    }

    class DummyService : IService
    {
        public bool Connected
        {
            get
            {
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
            await Task.Run(() =>
            {
                while (queue.Count == 0)
                {
                    Thread.Sleep(100);
                }
            });
            var requestString = queue.Dequeue();
            var request = RemoteRequest.FromString(requestString) as ChooseRemoteRequest<Card>;
            request.Response = request.Choices;
            return request.ToString();
        }

        public Task Send(string data)
        {
            queue.Enqueue(data);
            return Task.CompletedTask;
        }

        readonly Queue<string> queue = new Queue<string>();
    }
}
