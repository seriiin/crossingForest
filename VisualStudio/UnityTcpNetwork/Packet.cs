using System;

namespace Unity.Network
{
    public enum PacketType
    {
        None,
        Connect, Reconnect, Disconnect,
        Message,
    }

    [Serializable]
    public class Packet
    {
        public PacketType Type;

        public Packet()
        {
            
        }

        public Packet(PacketType type)
        {
            Type = type;
        }
    }

    [Serializable]
    public class Connect : Packet
    {
        public string UserName; //Connect 시 유저네임

        public Connect() : base(PacketType.Connect)
        {

        }
    }

    [Serializable]
    public class Message : Packet
    {
        public string Content; //메시지 저장할 변수

        public Message() : base(PacketType.Message)
        {

        }
    }
}
