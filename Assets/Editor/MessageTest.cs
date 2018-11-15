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

        Game.Initialize();
        Game.SetTestMode();
        var card = new Card00001(Game.Player);
        var targets = new List<Card> { card };
        var message2 = new MoveMessage
        {
            Targets = targets,
            Reason = card.sk1
        };
        var clone2 = message2.Clone() as MoveMessage;
        Assert.IsTrue(clone2 is MoveMessage);
        Assert.IsTrue(clone2.Reason == card.sk1);
        Assert.IsFalse(clone2.Targets == targets);
        Assert.IsTrue(clone2.Targets.SequenceEqual(targets));

        var message3 = new DeployMessage
        {
            Targets = targets,
            ActionedList = new List<bool> { true },
            ToFrontFieldList = new List<bool> { false },
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
    }

    [Test]
    public void ParseTest()
    {
        Game.Initialize();
        Game.SetTestMode();
        var card = CardFactory.CreateCard(1, Game.Player);
        Game.Player.Deck.AddCard(card);
        var targets = new List<Card> { card };
        card.Attach(new PowerBuff(null, 10));
        card.Attach(new CanNotAttack(null));
        var message = new DeployMessage
        {
            Targets = targets,
            ActionedList = new List<bool> { true },
            ToFrontFieldList = new List<bool> { false },
            Reason = null
        };
        var messageString = message.ToString();
        var messageParsed = Message.FromString(messageString) as DeployMessage;
        Assert.NotNull(messageParsed);
        Assert.AreEqual(message.Targets.Count, messageParsed.Targets.Count);
        Assert.AreEqual(message.Targets[0].Guid, messageParsed.Targets[0].Guid);
        Assert.AreEqual(messageString, messageParsed.ToString());
    }

    [Test]
    public void ParseCreateTest()
    {
        Game.Initialize();
        Game.SetTestMode();
        var card = CardFactory.CreateCard(1, Game.Player);
        Game.Player.Deck.AddCard(card);
        var targets = new List<Card> { card };
        var buff = new PowerBuff(null, 10);
        var subskill = new CanNotAttack(null);
        var message1 = new AttachItemMessage
        {
            Item = buff,
            Target = card
        }; // without Do();
        var message1String = message1.ToString();
        var message1Parsed = Message.FromString(message1String) as AttachItemMessage;
        Assert.NotNull(message1Parsed);
        Assert.AreNotSame(buff, message1Parsed.Item);
        Assert.AreEqual(buff.Guid, message1Parsed.Item.Guid);
    }
}
