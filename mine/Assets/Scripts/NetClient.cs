using System;
using Unity.Network;
using UnityEngine;

namespace Unity.Network
{
    public class NetClient
    {
        private const string UserName = "UnityClient";

        private LocalClient _client;

        public event ReceiveMessage OnReceiveMessage;

        public NetClient()
        {

        }

        public void Start()
        {
            _client = new LocalClient();
            _client.OnConnected += Connected;
            _client.OnReceiveObject += ReceiveObject;
            _client.Start();
        }

        public void Close()
        {
            _client.Close();
        }

        private void Connected() //커넥트
        {
            Connect connect = new Connect()
            {
                Type = PacketType.Connect,
                UserName = UserName,
            };

            _client.UserName = UserName;
            _client.SendPacket(connect); //패킷 서버에 전송
        }

        private void ConnectError(Exception ex)
        {
            Debug.Log("접속 에러\n" + ex.ToString());
        }

        private void ReceiveObject(Packet packet) //메시지 패킷 수신
        {
            if (packet == null)
                return;

            switch (packet.Type)
            {
                case PacketType.Message:
                    ReceiveMessage(packet);
                    break;
            }
        }

        private void ReceiveMessage(Packet packet)
        {
            Message message = packet as Message;
            if (message == null)
                return;

            if (OnReceiveMessage != null)
                OnReceiveMessage(message.Content);
        }

        public void SendMessage(string message)
        {
            Message packet = new Message()
            {
                Content = message,
            };
            SendPacket(packet);
        }

        private void SendPacket(Packet packet)
        {
            _client.SendPacket(packet);
        }
    }

    public delegate void ReceiveMessage(string message);

}