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
        //Debug.Log(message1Json);

        Game.Initialize();
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
        //Debug.Log(message2Json);

        DeployMessage message3 = new DeployMessage()
        {
            Targets = targets,
            TargetsActioned = new List<bool>() { true },
            TargetsToFrontField = new List<bool>() { false },
            Reason = null
        };
        DeployMessage clone3 = message3.Clone() as DeployMessage;
        Assert.IsTrue(clone3 is DeployMessage);
        Assert.IsTrue(clone3.Reason == null);
        Assert.IsFalse(clone3.Targets == targets);
        Assert.IsTrue(clone3.Targets.SequenceEqual(targets));
        Assert.IsTrue(clone3.TargetsActioned[0]);
        Assert.IsFalse(clone3.TargetsToFrontField[0]);
        clone3.TargetsActioned[0] = false;
        Assert.IsFalse(clone3.TargetsActioned[0]);
        Assert.IsTrue(message3.TargetsActioned[0]);
        //string message3Json = message3.ToString();
        //Debug.Log(message2Json);
    }
}
