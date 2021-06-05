using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Unity.Network
{
    public class LocalClient
    {
        private const string LocalIp = "127.0.0.1";
        public const int PortNumber = 10001;
        private const int BufferSize = 2048;

        private NetworkStream _stream;
        public NetworkStream Stream
        {
            get { return _stream; }
        }

        private readonly BinaryFormatter _formatter;

        private TcpClient _client;
        private byte[] _buffer;

        public string UserName { get; set; }

        public event Connected OnConnected;//server와 연결됨을 알려주는 메소드 
        public event Disconnected OnDisconnected;
        public event ConnectError OnConnectError;
        public event ReceiveObject OnReceiveObject;

        /// <summary>
        /// 클라이언트용 생성자
        /// </summary>
        public LocalClient() : this(null)
        {

        }

        /// <summary>
        /// 서버용 생성자
        /// </summary>
        /// <param name="client">이미 생성된 TcpClient</param>
        /// 
        //클라이언트 클래스 생성
        public LocalClient(TcpClient client)
        {
            _client = client;
            //패킷 형식 압축을 위한 포맷변수, 버퍼사이즈 생성
            _formatter = new BinaryFormatter();
            _formatter.Binder = new PacketBinder();
            _buffer = new byte[BufferSize];
        }
        //클라이언트 켜기
        public void Start()
        {
            if (_client != null)
            {
                //netstream으로 받은 값 저장
                _stream = _client.GetStream();
                //비동기형태로 서버에게 받은 값 읽기
                //서버 -> 클라이언트
                BeginRead();
            }
            else
            {
                //클라이언트 값 존재하지 않는 경우 => 클라이언트 생성 상태
                //클라이언트 -> 서버
                StartClient();
            }
        }

        private void StartClient()
        {
            //클라이언트 -> 서버
            _client = new TcpClient();

            try
            {
                //서버와 동일한 ip,port로 비동기형태로 연결요청하기
                _client.BeginConnect(LocalIp, PortNumber, EndConnect, null);
            }
            catch (SocketException ex)
            {
                if (OnConnectError != null)
                    OnConnectError(ex);
            }
        }
        //Connect작업이 완료되었을 때 호출할 메서드
        private void EndConnect(IAsyncResult result)
        {
            try
            {
                _client.EndConnect(result);
                _stream = _client.GetStream();

                //연결되지 않을 경우, 예외처리
                if (OnConnected != null)
                    OnConnected();

                //연결되었을경우 NetworkStream에서 비동기 읽기를 시작
                BeginRead();
            }
            catch (SocketException ex)
            {
                if (OnConnectError != null)
                    OnConnectError(ex);
            }
        }

        private void BeginRead()
        {
            //getstream
            _stream.BeginRead(_buffer, 0, BufferSize, new AsyncCallback(ReadObject), null); //비동기 콜백함수로 ReadObject
        }

        public void Close()
        {
            _stream.Close();
            _client.Close();
        }

        private void ReadObject(IAsyncResult result)
        {
            int readSize = 0;

            try
            {
                lock (_stream)//한번에 하나의 localclient 스레드만 실행할 수 있도록한다.
                {
                    readSize = _stream.EndRead(result);
                    //비동기 읽기 작업을 완료한다
                }

                //비동기 읽기 작업이 수행되지 않을 경우, 예외처리
                if (readSize < 1)
                    throw new Exception("Disconnect");


                Packet packet = null;
                //읽은 buffer값 패킷형태로 변환한다
                using (MemoryStream stream = new MemoryStream(_buffer))
                {
                    packet = (Packet)_formatter.Deserialize(stream);//stream을 패킷형태로 받는다
                }
                //받은 값이 null이 아니라면 패킷을 받는다.
                if (OnReceiveObject != null)
                    OnReceiveObject(packet);

                //한번에 하나의 localclient 스레드만 실행할 수 있도록한다.
                lock (_stream)
                {
                    BeginRead();//비동기적으로 데이터를 읽기 시작
                }
            }
            catch (Exception ex)
            {
                if (OnDisconnected != null)
                    OnDisconnected(ex);
            }
        }

        public void SendPacket(Packet packet)
        {
            byte[] data = null;
            try
            {
                //메모리스트림에 메모리
                using (MemoryStream stream = new MemoryStream())
                {
                    _formatter.Serialize(stream, packet);//패킷으로 묶음

                    data = stream.ToArray();//stream을 배열로 바꾼 값을 datad에 저장
                }

                if (data == null || data.Length < 1)
                    return;//데이터 예외처리

                _stream.Write(data, 0, data.Length);//data write
                _stream.Flush();//data 보내기
            }
            catch
            {

            }//try
        }
    }

    public delegate void Connected();
    public delegate void Disconnected(Exception ex);
    public delegate void ConnectError(SocketException ex);
    public delegate void ReceiveObject(Packet packet);
}
