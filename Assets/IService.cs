using System;
using System.Threading.Tasks;

public interface IService
{
    Task<bool> Connect(string url);
    Task<bool> Join(string roomId);
    Task<bool> Send(Message message);
    Task<T> Request<T>(Message message);
}
