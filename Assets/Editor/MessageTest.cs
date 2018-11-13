using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MessageTest
{

    [Test]
    public void CloneTest()
    {
        var message1 = new EmptyMessage();
        var clone1 = message1.Clone();
        Assert.IsTrue(clone1 is EmptyMessage);
        //string message1Json = message1.ToString();
        //LogUtils.Log(message1Json);

        Game.Initialize();
        Game.SetTestMode();
        var card = new Card00001(Game.Player);
        var targets = new List<Card>() { card };
        var message2 = new MoveMessage()
        {
            Targets = targets,
            Reason = card.sk1
        };
        var clone2 = message2.Clone() as MoveMessage;
        Assert.IsTrue(clone2 is MoveMessage);
        Assert.IsTrue(clone2.Reason == card.sk1);
        Assert.IsFalse(clone2.Targets == targets);
        Assert.IsTrue(clone2.Targets.SequenceEqual(targets));
        //string message2Json = message2.ToString();
        //LogUtils.Log(message2Json);

        var message3 = new DeployMessage()
        {
            Targets = targets,
            ActionedList = new List<bool>() { true },
            ToFrontFieldList = new List<bool>() { false },
            Reason = null
        };
        var clone3 = message3.Clone() as DeployMessage;
        Assert.IsTrue(clone3 is DeployMessage);
        Assert.IsTrue(clone3.Reason == null);
        Assert.IsFalse(clone3.Targets == targets);
        Assert.IsTrue(clone3.Targets.SequenceEqual(targets));
        Assert.IsTrue(clone3.ActionedList[0]);
        Assert.IsFalse(clone3.ToFrontFieldList[0]);
        clone3.ActionedList[0] = false;
        Assert.IsFalse(clone3.ActionedList[0]);
        Assert.IsTrue(message3.ActionedList[0]);
        //string message3Json = message3.ToString();
        //LogUtils.Log(message2Json);
    }

    [Test]
    public void ParseTest(){
        Game.Initialize();
        Game.SetTestMode();
        var card = CardFactory.CreateCard(1, Game.Player);
        Game.Player.Deck.AddCard(card);
        var targets = new List<Card>() { card };
        var message = new DeployMessage()
        {
            Targets = targets,
            ActionedList = new List<bool>() { true },
            ToFrontFieldList = new List<bool>() { false },
            Reason = null
        };
        var messageString = message.ToString();
        var messageParsed = Message.FromString(messageString) as DeployMessage;
        Assert.NotNull(messageParsed);
        Assert.AreEqual(message.Targets.Count, messageParsed.Targets.Count);
    }
}
