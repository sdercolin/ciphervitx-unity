using System;
using System.Threading.Tasks;

public interface IService
{
    bool Connected { get; }
    Task<bool> Connect(string url);
    Task<bool> Join(string roomId);
    Task Send(string data);
    Task<string> Receive();
    void Disconnect();
}
