using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void CloneTest()
        {
            EmptyMessage message1 = new EmptyMessage();
            Message clone1 = message1.Clone();
            Assert.IsTrue(clone1 is EmptyMessage);

            var card = new Card00001(1, new Game().Player);
            var targets = new List<Card>() { card };
            MoveMessage message2 = new MoveMessage()
            {
                ReasonCard = card,
                Targets = targets
            };
            Message clone2 = message2.Clone();
            Assert.IsTrue(clone2 is MoveMessage);
            Assert.IsTrue(clone2.ReasonCard == card);
            Assert.IsFalse(clone2.Targets == targets);
            Assert.IsTrue(clone2.Targets.SequenceEqual(targets));

            DeployMessage message3 = new DeployMessage()
            {
                ReasonCard = card,
                Targets = targets,
            };
            message3.MetaDict.Add(card,
                new DeployMessage.MetaData()
                {
                    ToFrontField = false,
                    Actioned = true
                });
            DeployMessage clone3 = message3.Clone() as DeployMessage;
            Assert.IsTrue(clone3 is DeployMessage);
            Assert.IsTrue(clone3.ReasonCard == card);
            Assert.IsFalse(clone3.Targets == targets);
            Assert.IsTrue(clone3.Targets.SequenceEqual(targets));
            Assert.IsFalse(clone3.MetaDict == message3.MetaDict);
            Assert.IsTrue(clone3.MetaDict.Keys.SequenceEqual(message3.MetaDict.Keys));
            Assert.IsTrue(clone3.MetaDict[clone3.Targets[0]].ToFrontField == false);
            Assert.IsTrue(clone3.MetaDict[clone3.Targets[0]].Actioned == true);
            clone3.MetaDict[clone3.Targets[0]] = new DeployMessage.MetaData()
            {
                ToFrontField = false,
                Actioned = false
            };
            Assert.IsTrue(clone3.MetaDict[clone3.Targets[0]].Actioned == false);
            Assert.IsTrue(message3.MetaDict[message3.Targets[0]].Actioned == true);
        }
    }
}
