using System;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable RECS0154 // Parameter is never used
public static class TaskExtensions
{
    public static void Forget(this Task task) { }
}
#pragma warning restore RECS0154 // Parameter is never used