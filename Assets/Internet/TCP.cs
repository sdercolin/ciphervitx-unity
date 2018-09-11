using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class TCP : MonoBehaviour
{
    private Socket tcpSocket;
    public void Start()
    {
        //创建socket
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //连接服务器
        tcpSocket.Connect(IPAddress.Parse("127.0.0.1"), 6000);
        Debug.Log("连接服务器");
        //接受消息
        byte[] bt = new byte[1024];
        int messageLength = tcpSocket.Receive(bt);
        Debug.Log(ASCIIEncoding.UTF8.GetString(bt));
        //发送消息
        tcpSocket.Send(ASCIIEncoding.UTF8.GetBytes("我有个问题"));
    }
    //// Use this for initialization
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {

    }
}
