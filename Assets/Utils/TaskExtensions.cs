using System;
using System.Threading;
using System.Threading.Tasks;

public static class TaskExtensions
{
    public static void Forget(this Task task) { }
}