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
        }
    }
}
