static class PayCostUtils
{
    public static void UseBond(Skill reason, int number)
    {
        var choices = reason.Controller.Bond.UnusedBonds;
        var targets = Request<Card>.Choose(choices, number);
        reason.Controller.UseBond(targets, reason);
    }
}

