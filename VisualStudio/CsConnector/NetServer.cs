using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Unity.Network;

namespace Unity.Network
{
    public class NetServer
    {
        private readonly TcpListener _listener;
        private LocalClient _localClient;//유니티의 client 메소드
        public event ReceiveMessage OnReceiveMessage;//유니티의 network get 메소드


        //Winform서버 클래스 생성 
        public NetServer()
        {
            _listener = new TcpListener(System.Net.IPAddress.Any, LocalClient.PortNumber);//모든 유니티서버ip주소와 클라이언트와 동일한 portnum(10001)으로 서버 생성
        }
        //Winform서버 Start 함수 생성
        public void Start()
        {
            try
            {
                _listener.Start();//서버생성
                TcpClient tcpClient = _listener.AcceptTcpClient();//서버 받는 winform 클라이언트 대기
                _localClient = new LocalClient(tcpClient);//winform 클라이언트로 클라이언트클래스 생성
                _localClient.OnReceiveObject += ReceiveObject;//받는 값 더해주기
                _localClient.Start();//클라이언트 시작
            }
            catch (Exception)
            {

            }//try   
        }

        public void Close()
        {
            _localClient.Close();
            _listener.Stop();
        }

        private void ReceiveObject(Packet packet)
        {
            if (packet == null)
                return;

            switch (packet.Type)
            {
                case PacketType.Connect: //패킷형식이 연결
                    Connected(packet); //Connected 실행
                    break;
                case PacketType.Message: //패킷형식이 메시지
                    ReceiveMessage(packet); //ReceiveMessage 실행
                    break;
            }
        }

        private void Connected(Packet packet)
        {
            Connect connect = packet as Connect;
            if (connect == null)
                return;

            _localClient.UserName = connect.UserName;
        }

        private void ReceiveMessage(Packet packet)
        {
            Message message = packet as Message;
            if (message == null)
                return;
            if (OnReceiveMessage != null)
                OnReceiveMessage(message.Content);//메세지 값을 더해준다. 출력
        }
        public void SendMessage(string message)//받은 메세지를 패킷으로 변환해서 보낸다
        {
            Message packet = new Message()
            {
                Content = message,//메세지를 패킷형태로 보낸다.
            };
            SendPacket(packet);
        }

        private void SendPacket(Packet packet)
        {
            _localClient.SendPacket(packet);//클라이언트에게 보낸다
        }
    }

    public delegate void ReceiveMessage(string message);
}
