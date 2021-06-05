using System;
using System.Windows.Forms;
using Unity.Network;

namespace CsConnector
{
    public partial class FrmMain : Form
    {
        private readonly NetServer _netServer;

        public FrmMain()
        {
            InitializeComponent();

            _netServer = new NetServer();//유니티 서버 생성    
            _netServer.OnReceiveMessage += ReceiveMessage;//클라이언트에서 받는 값 listview에 더해주기
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateStatus("유저와 연결되었습니다");

            _netServer.Start(); //서버 시작
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _netServer.Close(); //서버 닫기
        }

        private void UpdateStatus(string message)
        {
            LstStatus.Items.Add("받은 메시지 : " + message); // 받은 메시지 업데이트
        }

        private void ReceiveMessage(string message) //대리자 전달용
        {
            UpdateStatus(message);
        }

        private void BtSend_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(TbSend.Text) == false) //false시에는 문자열이 존재
            {
                LstStatus.Items.Add("보낸 메시지 : " + TbSend.Text); //LstState에 업데이트
                _netServer.SendMessage(TbSend.Text); //메시지 전송
            }
        }
    }
}
