// SetType2.cpp : 实现文件
//

#include "stdafx.h"
#include "SetID.h"
#include "SetType2.h"


BYTE CBD_GETDEVICE[] = {0xFF, 0xFE, 0x00, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETVERSION[] = {0xFF, 0xFE, 0x01, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETDHCP[] = {0xFF, 0xFE, 0x02, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETLOCALIP[] = {0xFF, 0xFE, 0x04, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETLOCALPORT[] = {0xFF, 0xFE, 0x06, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETSERVERIP[] = {0xFF, 0xFE, 0x08, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETSERVERPORT[] = {0xFF, 0xFE, 0x0A, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETLOCALMAC[] = {0xFF, 0xFE, 0x0C, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETLOCALID[] = {0xFF, 0xFE, 0x0D, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETSERVERIPMODE[] = {0xFF, 0xFE, 0x0F, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETSUBMASK[] = {0xFF, 0xFE, 0x11, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETGATEWAY[] = {0xFF, 0xFE, 0x13, 0x00, 0x00, 0xFD, 0xFC};
BYTE CBD_GETSENDNAMETIME[] = {0xFF, 0xFE, 0x15, 0x00, 0x00, 0xFD, 0xFC};

float Txpower[] = {4.5,2.5,1,-0.5,-1.5,-3,-4,-6,-8,-10,-12,-14,-16,-18,-20,-22};

// SetType2 对话框

IMPLEMENT_DYNAMIC(SetType2, CDialog)

SetType2::SetType2(CWnd* pParent /*=NULL*/)
	: CDialog(SetType2::IDD, pParent)
	, m_edit_tag_setsleeptime(5)
	, m_edit_local_setport(50000)
	, m_edit_local_settime(30)
	, m_edit_server_setport(51234)
	, m_edit_tag_setresponse(15)
{

}

SetType2::~SetType2()
{
}

void SetType2::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_CONNECT, m_button_connect);
	DDX_Control(pDX, IDC_COMBO_TWO, m_combo_two);
	DDX_Control(pDX, IDC_BUTTON_TAG_GETID, m_button_tag_getid);
	DDX_Control(pDX, IDC_BUTTON_TAG_SETID, m_button_tag_setid);
	DDX_Control(pDX, IDC_BUTTON_TAG_GETSLEEPTIME, m_button_tag_getsleeptime);
	DDX_Control(pDX, IDC_BUTTON_TAG_SETSLEEPTIME, m_button_tag_setsleeptime);
	DDX_Control(pDX, IDC_EDIT_TAG_GETID1, m_edit_tag_getid1);
	DDX_Control(pDX, IDC_EDIT_TAG_GETID2, m_edit_tag_getid2);
	DDX_Control(pDX, IDC_EDIT_TAG_SETID1, m_edit_tag_setid1);
	DDX_Control(pDX, IDC_EDIT_TAG_SETID2, m_edit_tag_setid2);
	DDX_Control(pDX, IDC_EDIT_TAG_GETSLEEPTIME, m_edit_tag_getsleeptime);
	DDX_Text(pDX, IDC_EDIT_TAG_SETSLEEPTIME, m_edit_tag_setsleeptime);
	DDX_Control(pDX, IDC_EDIT_LOCAL_DHCP, m_edit_local_dhcp);
	DDX_Text(pDX, IDC_EDIT_LOCAL_SETPORT, m_edit_local_setport);
	DDX_Text(pDX, IDC_EDIT_LOCAL_SETTIME, m_edit_local_settime);
	DDX_Text(pDX, IDC_EDIT_SERVER_SETPORT, m_edit_server_setport);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_IP, m_ipaddress_local_ip);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_SETIP, m_ipaddress_local_setip);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_DHCP, m_button_local_dhcp);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_STATICIP, m_button_local_staticip);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_SETIP, m_button_local_setip);
	DDX_Control(pDX, IDC_EDIT_LOCAL_PORT, m_edit_local_port);
	DDX_Control(pDX, IDC_EDIT_LOCAL_MAC, m_edit_local_mac);
	DDX_Control(pDX, IDC_EDIT_LOCAL_TIME, m_edit_local_time);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_SETTIME, m_button_local_settime);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_SUBMASK, m_ipaddress_local_submask);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_SUBMASK, m_button_local_submask);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_SETSUBMASK, m_ipaddress_local_setsubmask);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_GATEWAY, m_ipaddress_local_gateway);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_GATEWAY, m_button_local_gateway);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_SETGATEWAY, m_ipaddress_local_setgateway);
	DDX_Control(pDX, IDC_EDIT_SERVER_DHCP, m_edit_server_dhcp);
	DDX_Control(pDX, IDC_BUTTON_SERVER_DHCP, m_button_server_dhcp);
	DDX_Control(pDX, IDC_BUTTON_SERVER_STATICIP, m_button_server_staticip);
	DDX_Control(pDX, IDC_IPADDRESS_SERVER_IP, m_ipaddress_server_ip);
	DDX_Control(pDX, IDC_BUTTON_SERVER_SETIP, m_button_server_setip);
	DDX_Control(pDX, IDC_IPADDRESS_SERVER_SETIP, m_ipaddress_server_setip);
	DDX_Control(pDX, IDC_EDIT_SERVER_PORT, m_edit_server_port);
	DDX_Control(pDX, IDC_BUTTON_SERVER_SETPORT, m_button_server_setport);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_SETPORT, m_button_local_setport);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_SETID, m_button_local_setid);
	DDX_Control(pDX, IDC_EDIT_LOCAL_SETID1, m_edit_local_setid1);
	DDX_Control(pDX, IDC_EDIT_LOCAL_SETID2, m_edit_local_setid2);
	DDX_Control(pDX, IDC_EDIT_LOCAL_ID2, m_edit_local_id2);
	DDX_Control(pDX, IDC_EDIT_LOCAL_ID1, m_edit_local_id1);
	DDX_Text(pDX, IDC_EDIT_TAG_SETRESPONSE, m_edit_tag_setresponse);
	DDX_Control(pDX, IDC_BUTTON_TAG_GETRESPONSE, m_button_tag_getresponse);
	DDX_Control(pDX, IDC_EDIT_TAG_GETRESPONSE, m_edit_tag_getresponse);
	DDX_Control(pDX, IDC_BUTTON_TAG_SETRESPONSE, m_button_tag_setresponse);
	DDX_Control(pDX, IDC_BUTTON_TAG_GETPOWER, m_button_tag_getpower);
	DDX_Control(pDX, IDC_EDIT_TAG_GETPOWER, m_edit_tag_getpower);
	DDX_Control(pDX, IDC_COMBO_TAG_SETPOWER, m_combo_tag_setpower);
	DDX_Control(pDX, IDC_BUTTON_TAG_SETPOWER, m_button_tag_setpower);
}


BEGIN_MESSAGE_MAP(SetType2, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_CONNECT, &SetType2::OnBnClickedButtonConnect)
	ON_BN_CLICKED(IDC_BUTTON_TAG_GETID, &SetType2::OnBnClickedButtonTagGetid)
	ON_BN_CLICKED(IDC_BUTTON_TAG_SETID, &SetType2::OnBnClickedButtonTagSetid)
	ON_BN_CLICKED(IDC_BUTTON_TAG_GETSLEEPTIME, &SetType2::OnBnClickedButtonTagGetsleeptime)
	ON_BN_CLICKED(IDC_BUTTON_TAG_SETSLEEPTIME, &SetType2::OnBnClickedButtonTagSetsleeptime)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_DHCP, &SetType2::OnBnClickedButtonLocalDhcp)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_STATICIP, &SetType2::OnBnClickedButtonLocalStaticip)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETIP, &SetType2::OnBnClickedButtonLocalSetip)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETPORT, &SetType2::OnBnClickedButtonLocalSetport)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETTIME, &SetType2::OnBnClickedButtonLocalSettime)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SUBMASK, &SetType2::OnBnClickedButtonLocalSubmask)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_GATEWAY, &SetType2::OnBnClickedButtonLocalGateway)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_DHCP, &SetType2::OnBnClickedButtonServerDhcp)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_STATICIP, &SetType2::OnBnClickedButtonServerStaticip)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_SETIP, &SetType2::OnBnClickedButtonServerSetip)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_SETPORT, &SetType2::OnBnClickedButtonServerSetport)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETID, &SetType2::OnBnClickedButtonLocalSetid)
	ON_BN_CLICKED(IDC_BUTTON_TAG_GETRESPONSE, &SetType2::OnBnClickedButtonTagGetresponse)
	ON_BN_CLICKED(IDC_BUTTON_TAG_SETRESPONSE, &SetType2::OnBnClickedButtonTagSetresponse)
	ON_BN_CLICKED(IDC_BUTTON_TAG_GETPOWER, &SetType2::OnBnClickedButtonTagGetpower)
	ON_BN_CLICKED(IDC_BUTTON_TAG_SETPOWER, &SetType2::OnBnClickedButtonTagSetpower)
END_MESSAGE_MAP()

DWORD ComReadThread_Two(LPVOID lparam)
{	
	DWORD	actualReadLen=0;	//实际读取的字节数
	DWORD	willReadLen;	
	
	DWORD dwReadErrors;
	COMSTAT	cmState;
	
	SetType2 *pdlg;
	pdlg = (SetType2 *)lparam;

	pdlg->m_Receive_Data_Len = 0;
		
	// 清空缓冲，并检查串口是否打开。
	ASSERT(pdlg->m_hcom != INVALID_HANDLE_VALUE); 	
	//清空串口
	//PurgeComm(pdlg->m_hcom, PURGE_RXCLEAR | PURGE_TXCLEAR );	
	SetCommMask (pdlg->m_hcom, EV_RXCHAR | EV_CTS | EV_DSR);
	while(pdlg->m_hcom != NULL && pdlg->m_hcom != INVALID_HANDLE_VALUE)
	{ 			
		ClearCommError(pdlg->m_hcom,&dwReadErrors,&cmState);
		willReadLen = cmState.cbInQue ;
		if (willReadLen <= 0)
		{
			Sleep(10);
			continue;
		}			
		if(willReadLen + pdlg->m_Receive_Data_Len > MAX_RECEIVE_DATA_LEN)
			willReadLen = MAX_RECEIVE_DATA_LEN - pdlg->m_Receive_Data_Len;

		ReadFile(pdlg->m_hcom, pdlg->m_Receive_Data_Char + pdlg->m_Receive_Data_Len, willReadLen, &actualReadLen, 0);
		if (actualReadLen <= 0 && pdlg->m_Receive_Data_Len == 0)
		{
			Sleep(10);
			continue;
		}
		pdlg->m_Receive_Data_Len += actualReadLen;

		
		pdlg->ParseData();

	}
	return 0;
}

void SetType2::ParseData(void)
{
	int start;
	while(m_Receive_Data_Len >= 7)
	{
		//查找开始标志FF FF
		start = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, 0xFB, 0xFA);
		if(start >= 0)
		{
			//找到开始标志
			//判断从开始标志开始，是否有接收一个最小包的长度
			if(start + 7 <= m_Receive_Data_Len)
			{				
				//把数据抓取出来
				switch(m_Receive_Data_Char[start+2])
				{
				case 0x80:	//模块类型返回 7Byte
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						if(m_Receive_Data_Char[start+3] == 0x02 && m_Receive_Data_Char[start+4] == 0x00)
							m_bGetTag = TRUE;
						else if(m_Receive_Data_Char[start+3] == 0x04 && m_Receive_Data_Char[start+4] == 0x00)
							m_bGetServer = TRUE;
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					
					break;
				case 0x81:	
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_Receive_Data_Char[start+3] == m_Tag_ID[0] && m_Receive_Data_Char[start+4] == m_Tag_ID[1])
						{		
								m_bSetTagID = TRUE;
						}
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					if(start + 10 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+8] == 0xF9 && m_Receive_Data_Char[start+9] == 0xF8)
						{
							if(m_bGetServer == TRUE)
							{
								m_bGetVersion = TRUE;
								m_StrVersion.Format(TEXT("%d%d-%d-%d-%d"), m_Receive_Data_Char[start+3], m_Receive_Data_Char[start+4], m_Receive_Data_Char[start+5], m_Receive_Data_Char[start+6], m_Receive_Data_Char[start+7]);
							}
							//处理完成，丢掉处理过的数据
							m_Receive_Data_Len = m_Receive_Data_Len - start - 10;
							memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+10, m_Receive_Data_Len);
						}
					}
					else
					{
						//數據為收完
						return;
					}					
					break;
				case 0x82:	
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_bGetTag == TRUE)
						{
							m_bGetTagID = TRUE;
							m_Tag_ID[0] = m_Receive_Data_Char[start+3];
							m_Tag_ID[1] = m_Receive_Data_Char[start+4];
						}
						if(m_bGetServer == TRUE)
						{
							m_bDHCP = TRUE;
							m_dhcpvalue = m_Receive_Data_Char[start+3];
						}
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x83:	
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_bGetTag == TRUE)
						{
							if(((m_Receive_Data_Char[start+3] <<8) | m_Receive_Data_Char[start+4])== m_edit_tag_setsleeptime)
							m_bSleepTime = TRUE;
						}
						if(m_bGetServer == TRUE)
						{
							if(m_dhcpvalue == m_Receive_Data_Char[start+3])
								m_bDHCP = TRUE;	
						}
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x84:	
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_bGetTag == TRUE)
						{
							m_bSleepTime = TRUE;
							m_SleepTime = (m_Receive_Data_Char[start+3]<<8) | m_Receive_Data_Char[start+4];
						}
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bGetLocalIp = TRUE;
							memcpy(m_LocalIp, m_Receive_Data_Char+start+3, 4);
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0xC0:   //设置发射功率
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_Receive_Data_Char[start+4] == number)
								m_bSetPower = TRUE;
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0xC1:  //获取发射功率
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bGetPower = TRUE;
						m_SendPower = m_Receive_Data_Char[start+4];
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0xC2:  //设置Sensor灵敏度
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_Receive_Data_Char[start+4] == m_edit_tag_setresponse)
								m_bSetResponse = TRUE;
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0xC3:  //获取Sensor灵敏度
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bGetResponse = TRUE;
						m_Response = m_Receive_Data_Char[start+4];
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x85:	//設置自身IP的返回，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_LocalIp, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGetLocalIp = TRUE;
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}
					break;
				case 0x86:	//查詢模組端口號的返回
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bGetLocalPort = TRUE;
						m_LocalPort = (m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4];
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x87:	//设置模組端口號的返回
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_LocalPort == ((m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4]))
							m_bGetLocalPort = TRUE;						
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x88:	//讀取ServerIP的返回，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bGetServerIp = TRUE;
							memcpy(m_ServerIp, m_Receive_Data_Char+start+3, 4);
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x89:	//設置Server IP的返回，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_ServerIp, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGetServerIp = TRUE;
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}
					break;
				case 0x8A:	//查詢Server端口號的返回
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bGetServerPort = TRUE;
						m_ServerPort = (m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4];
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x8B:	//设置Server端口號的返回
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_ServerPort == ((m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4]))
							m_bGetServerPort = TRUE;						
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x8C:	//讀取本地MAC地址返回，11Byte
					if(start + 11 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+9] == 0xF9 && m_Receive_Data_Char[start+10] == 0xF8)
						{
							m_bGetLocalMac = TRUE;
							memcpy(m_LocalMac, m_Receive_Data_Char+start+3, 6);
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 11;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+11, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x8D:	//读取本地ID返回，7Byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetLocalID = TRUE;
							m_LocalID[0] = m_Receive_Data_Char[start+3];
							m_LocalID[1] = m_Receive_Data_Char[start+4];
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x8E:	//设置本地ID返回，7Byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(m_Receive_Data_Char[start+3] == m_LocalID[0] && m_Receive_Data_Char[start+4] == m_LocalID[1])
								m_bGetLocalID = TRUE;							
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x8F:	//读取Server IP 模式，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetServerIpMode = TRUE;		
							m_ServerIpMode = m_Receive_Data_Char[start+3];
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x90:	//设置Server IP 模式，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(m_Receive_Data_Char[start+3] == m_ServerIpMode)
								m_bGetServerIpMode = TRUE;							
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x91:	//讀取子网掩码，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bSubMask = TRUE;
							memcpy(m_SubMask, m_Receive_Data_Char+start+3, 4);
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x92:	//設置子网掩码，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_SubMask, m_Receive_Data_Char+start+3, 4) == 0)
								m_bSubMask = TRUE;
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}
					break;
				case 0x93:	//讀取默认网关，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bGateWay = TRUE;
							memcpy(m_GateWay, m_Receive_Data_Char+start+3, 4);
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x94:	//設置默认网关，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_GateWay, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGateWay = TRUE;
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}
					break;
				case 0x95:	//读取自动上报名称的时间，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetSendNameTime = TRUE;		
							m_SendNameTime = (m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4];
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x96:	//设置自动上报名称的时间，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(((m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4]) == m_SendNameTime)
								m_bGetSendNameTime = TRUE;							
						}
						//处理完成，丢掉处理过的数据
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;

				default:	//未知的指令
					//丢掉头
					m_Receive_Data_Len = m_Receive_Data_Len - start - 2;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+2, m_Receive_Data_Len);					
					break;
				}
								
			}
			else
			{
				//接收到的不满一个包，等待收满再处理
				if(start > 0)
				{
					//在数据包头前，还有数据，把这些数据清除掉
					m_Receive_Data_Len = m_Receive_Data_Len - start;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start, m_Receive_Data_Len);
				}
				break;
			}
		}
		else
		{
			//没有找到开始标志
			if(m_Receive_Data_Char[m_Receive_Data_Len-1] == 0xFB)
			{
				//最后一个字符是0xFB，那么可能是下一个数据包的开头，保留最后一个字符，其它丢掉
				m_Receive_Data_Char[0] = 0xFB;
				m_Receive_Data_Len = 1;
			}
			else
			{
				//最后一个字符不是0xFB，那么全部丢掉
				m_Receive_Data_Len = 0;
			}
			break;
		}
	}
}


int SetType2::FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2)
{
	for(int i=start; i<end-1; i++)
	{
		if(str[i] == c1 && str[i+1] == c2)
			return i;
	}
	return -1;
}
int SetType2::FindChar(BYTE *str, int start, int end, BYTE c1)
{
	for(int i=start; i<end; i++)
	{
		if(str[i] == c1)
			return i;
	}
	return -1;
}


BOOL SetType2::OpenCom(CString str_com)
{	
	str_com = TEXT("\\\\.\\") + str_com;
	m_hcom = CreateFile(str_com, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
	///配置串口
	if(m_hcom != INVALID_HANDLE_VALUE && m_hcom != NULL)
	{		
		DCB  dcb;    
		dcb.DCBlength = sizeof(DCB); 
		// 默认串口参数
		GetCommState(m_hcom, &dcb);

		dcb.BaudRate = 115200;					// 设置波特率 
		dcb.fBinary = TRUE;						// 设置二进制模式，此处必须设置TRUE
		dcb.fParity = TRUE;						// 支持奇偶校验 
		dcb.ByteSize = 8;						// 数据位,范围:4-8 
		dcb.Parity = NOPARITY;					// 校验模式
		dcb.StopBits = ONESTOPBIT;				// 停止位 0,1,2 = 1, 1.5, 2

		dcb.fOutxCtsFlow = FALSE;				// No CTS output flow control 
		dcb.fOutxDsrFlow = FALSE;				// No DSR output flow control 
		dcb.fDtrControl = DTR_CONTROL_ENABLE; 
		// DTR flow control type 
		dcb.fDsrSensitivity = FALSE;			// DSR sensitivity 
		dcb.fTXContinueOnXoff = TRUE;			// XOFF continues Tx 
		dcb.fOutX = FALSE;					// No XON/XOFF out flow control 
		dcb.fInX = FALSE;						// No XON/XOFF in flow control 
		dcb.fErrorChar = FALSE;				// Disable error replacement 
		dcb.fNull = FALSE;					// Disable null stripping 
		dcb.fRtsControl = RTS_CONTROL_ENABLE; 
		// RTS flow control 
		dcb.fAbortOnError = FALSE;			// 当串口发生错误，并不终止串口读写


		if (!SetCommState(m_hcom, &dcb))
		{
			///L"配置串口失败";			
			return FALSE;
		}
		////配置超时值
		COMMTIMEOUTS  cto;
		GetCommTimeouts(m_hcom, &cto);
		cto.ReadIntervalTimeout = MAXDWORD;  
		cto.ReadTotalTimeoutMultiplier = 10;  
		cto.ReadTotalTimeoutConstant = 10;    
		cto.WriteTotalTimeoutMultiplier = 50;  
		cto.WriteTotalTimeoutConstant = 100;    
		if (!SetCommTimeouts(m_hcom, &cto))
		{
			///L"不能设置超时参数";		
			return FALSE;
		}	

		//指定端口监测的事件集
		SetCommMask (m_hcom, EV_RXCHAR);
		
		//分配设备缓冲区
	//	SetupComm(m_hcom,8192,8192);

		//初始化缓冲区中的信息
		PurgeComm(m_hcom,PURGE_TXCLEAR|PURGE_RXCLEAR);
	}
	else
	{		
		return FALSE;
	}

	HANDLE m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ComReadThread_Two, this, 0, NULL);
	CloseHandle(m_hThread);

	return TRUE;
}

BOOL SetType2::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	
	m_button_connect.SetWindowTextW(TEXT("自動連接設備"));
	m_combo_two.EnableWindow(FALSE);
	m_combo_two.SetWindowTextW(TEXT(""));

	m_button_tag_getid.EnableWindow(FALSE);
	m_button_tag_setid.EnableWindow(FALSE);
	m_button_tag_getsleeptime.EnableWindow(FALSE);
	m_button_tag_setsleeptime.EnableWindow(FALSE);
	m_button_tag_getpower.EnableWindow(FALSE);
	m_button_tag_setpower.EnableWindow(FALSE);
	m_button_tag_getresponse.EnableWindow(FALSE);
	m_button_tag_setresponse.EnableWindow(FALSE);

	m_button_local_setid.EnableWindow(FALSE);
	m_button_local_dhcp.EnableWindow(FALSE);
	m_button_local_staticip.EnableWindow(FALSE);
	m_button_local_setip.EnableWindow(FALSE);
	m_button_local_settime.EnableWindow(FALSE);
	m_button_local_submask.EnableWindow(FALSE);
	m_button_local_gateway.EnableWindow(FALSE);
	m_button_local_setport.EnableWindow(FALSE);
	m_button_server_dhcp.EnableWindow(FALSE);
	m_button_server_staticip.EnableWindow(FALSE);
	m_button_server_setip.EnableWindow(FALSE);
	m_button_server_setport.EnableWindow(FALSE);

	m_combo_tag_setpower.SetCurSel(0);

	m_bConnect = FALSE;

	//m_LeftButton_down = FALSE;

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void SetType2::GetInfo()
{
	DWORD write;

	m_bDHCP = FALSE;
	WriteFile(m_hcom, CBD_GETDHCP, sizeof(CBD_GETDHCP), &write, NULL);
	Sleep(100);
	if(m_bDHCP)
	{
		if(m_dhcpvalue == 1)
			m_edit_local_dhcp.SetWindowTextW(TEXT("使用靜態IP功能"));
		else
			m_edit_local_dhcp.SetWindowTextW(TEXT("使用DHCP功能"));
	}
	else
		MessageBox(TEXT("獲取DHCP狀態失敗！"));

	m_bGetLocalIp = FALSE;
	WriteFile(m_hcom, CBD_GETLOCALIP, sizeof(CBD_GETLOCALIP), &write, NULL);
	Sleep(100);
	if(m_bGetLocalIp)
	{
		m_ipaddress_local_ip.SetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
	}
	else
		MessageBox(TEXT("獲取本機IP地址失敗！"));

	m_bGetLocalPort = FALSE;
	WriteFile(m_hcom, CBD_GETLOCALPORT, sizeof(CBD_GETLOCALPORT), &write, NULL);
	Sleep(100);
	if(m_bGetLocalPort)
	{
		CString str;
		str.Format(TEXT("%d"), m_LocalPort);
		m_edit_local_port.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取本機端口號失敗！"));

	m_bGetServerIp = FALSE;
	WriteFile(m_hcom, CBD_GETSERVERIP, sizeof(CBD_GETSERVERIP), &write, NULL);
	Sleep(100);
	if(m_bGetServerIp)
	{
		m_ipaddress_server_ip.SetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
	}
	else
		MessageBox(TEXT("獲取Server IP地址失敗！"));

	m_bGetServerPort = FALSE;
	WriteFile(m_hcom, CBD_GETSERVERPORT, sizeof(CBD_GETSERVERPORT), &write, NULL);
	Sleep(100);
	if(m_bGetServerPort)
	{
		CString str;
		str.Format(TEXT("%d"), m_ServerPort);
		m_edit_server_port.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取Server端口號失敗！"));

	m_bGetLocalMac = FALSE;
	WriteFile(m_hcom, CBD_GETLOCALMAC, sizeof(CBD_GETLOCALMAC), &write, NULL);
	Sleep(100);
	if(m_bGetLocalMac)
	{
		CString str;
		str.Format(TEXT("%02X:%02X:%02X:%02X:%02X:%02X"), m_LocalMac[0], m_LocalMac[1], m_LocalMac[2], m_LocalMac[3], m_LocalMac[4], m_LocalMac[5]);
		m_edit_local_mac.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取Server端口號失敗！"));

	m_bGetLocalID = FALSE;
	WriteFile(m_hcom, CBD_GETLOCALID, sizeof(CBD_GETLOCALID), &write, NULL);
	Sleep(100);
	if(m_bGetLocalID)
	{
		CString str;
		str.Format(TEXT("%02X"), m_LocalID[0]);
		m_edit_local_id1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_LocalID[1]);
		m_edit_local_id2.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取本機ID失敗！"));

	m_bGetServerIpMode = FALSE;
	WriteFile(m_hcom, CBD_GETSERVERIPMODE, sizeof(CBD_GETSERVERIPMODE), &write, NULL);
	Sleep(100);
	if(m_bGetServerIpMode)
	{
		CString str;
		if(m_ServerIpMode == 1)
			str = TEXT("靜態IP模式");
		else if(m_ServerIpMode == 2)
			str = TEXT("動態IP模式");
		else
			str = TEXT("未知的IP模式");

		m_edit_server_dhcp.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取Server IP模式失敗！"));

	m_bSubMask = FALSE;
	WriteFile(m_hcom, CBD_GETSUBMASK, sizeof(CBD_GETSUBMASK), &write, NULL);
	Sleep(100);
	if(m_bSubMask)
	{
		m_ipaddress_local_submask.SetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
	}
	else
		MessageBox(TEXT("獲取子網掩碼失敗！"));

	m_bGateWay = FALSE;
	WriteFile(m_hcom, CBD_GETGATEWAY, sizeof(CBD_GETGATEWAY), &write, NULL);
	Sleep(100);
	if(m_bGateWay)
	{
		m_ipaddress_local_gateway.SetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
	}
	else
		MessageBox(TEXT("獲取默認網関失敗！"));

	m_bGetSendNameTime = FALSE;
	WriteFile(m_hcom, CBD_GETSENDNAMETIME, sizeof(CBD_GETSENDNAMETIME), &write, NULL);
	Sleep(100);
	if(m_bGetSendNameTime)
	{
		CString str;
		if(m_SendNameTime == 0)
			str.Format(TEXT("不上報名稱"));
		else
			str.Format(TEXT("%d 秒"), m_SendNameTime);
		m_edit_local_time.SetWindowTextW(str);	
	}
	else
		MessageBox(TEXT("獲取上報名稱時間失敗！"));

}

bool SetType2::StringToChar(CString str, BYTE* data)
{
	int strlen = str.GetLength();
	int datalen = 0;
	int value;
	for(int i=0; i<strlen; i++)
	{
		if(str.GetAt(i) == ' ')
			continue;
		switch(str.GetAt(i))
		{
		case '0': value = 0x0; break;
		case '1': value = 0x1; break;
		case '2': value = 0x2; break;
		case '3': value = 0x3; break;
		case '4': value = 0x4; break;
		case '5': value = 0x5; break;
		case '6': value = 0x6; break;
		case '7': value = 0x7; break;
		case '8': value = 0x8; break;
		case '9': value = 0x9; break;
		case 'a':
		case 'A': value = 0xA; break;
		case 'b':
		case 'B': value = 0xB; break;
		case 'c':
		case 'C': value = 0xC; break;
		case 'd':
		case 'D': value = 0xD; break;
		case 'e':
		case 'E': value = 0xE; break;
		case 'f':
		case 'F': value = 0xF; break;	
		default: return false; 
		}
		data[datalen] = (char) (value<<4);
		if(i+1 >= strlen)
			return false;
		switch(str.GetAt(i+1))
		{
		case '0': value = 0x0; break;
		case '1': value = 0x1; break;
		case '2': value = 0x2; break;
		case '3': value = 0x3; break;
		case '4': value = 0x4; break;
		case '5': value = 0x5; break;
		case '6': value = 0x6; break;
		case '7': value = 0x7; break;
		case '8': value = 0x8; break;
		case '9': value = 0x9; break;
		case 'a':
		case 'A': value = 0xA; break;
		case 'b':
		case 'B': value = 0xB; break;
		case 'c':
		case 'C': value = 0xC; break;
		case 'd':
		case 'D': value = 0xD; break;
		case 'e':
		case 'E': value = 0xE; break;
		case 'f':
		case 'F': value = 0xF; break;	
		default: return false; 
		}
		data[datalen] = (char) (data[datalen] + value);
		return true;			
	}		
	return false;
}

// SetType2 消息处理程序

void SetType2::OnBnClickedButtonConnect()
{
	// TODO: 在此添加控件通知处理程序代码
	if(m_bConnect)
	{
		m_bConnect = FALSE;
		m_button_connect.SetWindowTextW(TEXT("自動連接設備"));
		m_combo_two.SetWindowTextW(TEXT(""));

		m_button_tag_getid.EnableWindow(FALSE);
		m_button_tag_setid.EnableWindow(FALSE);
		m_button_tag_getsleeptime.EnableWindow(FALSE);
		m_button_tag_setsleeptime.EnableWindow(FALSE);
		m_button_tag_getpower.EnableWindow(FALSE);
		m_button_tag_setpower.EnableWindow(FALSE);
		m_button_tag_getresponse.EnableWindow(FALSE);
		m_button_tag_setresponse.EnableWindow(FALSE);

		m_button_local_setid.EnableWindow(FALSE);
		m_button_local_dhcp.EnableWindow(FALSE);
		m_button_local_staticip.EnableWindow(FALSE);
		m_button_local_setip.EnableWindow(FALSE);
		m_button_local_settime.EnableWindow(FALSE);
		m_button_local_submask.EnableWindow(FALSE);
		m_button_local_gateway.EnableWindow(FALSE);
		m_button_local_setport.EnableWindow(FALSE);
		m_button_server_dhcp.EnableWindow(FALSE);
		m_button_server_staticip.EnableWindow(FALSE);
		m_button_server_setip.EnableWindow(FALSE);
		m_button_server_setport.EnableWindow(FALSE);

		m_edit_tag_getid1.SetWindowTextW(TEXT(""));
		m_edit_tag_getid2.SetWindowTextW(TEXT(""));
		m_edit_tag_getsleeptime.SetWindowTextW(TEXT(""));
		m_edit_tag_getpower.SetWindowTextW(TEXT(""));
		m_edit_tag_getresponse.SetWindowTextW(TEXT(""));

		m_edit_local_id1.SetWindowTextW(TEXT(""));
		m_edit_local_id2.SetWindowTextW(TEXT(""));
		m_edit_local_dhcp.SetWindowTextW(TEXT(""));
		m_edit_local_time.SetWindowTextW(TEXT(""));
		m_edit_local_port.SetWindowTextW(TEXT(""));
		m_edit_local_mac.SetWindowTextW(TEXT(""));
		m_edit_server_dhcp.SetWindowTextW(TEXT(""));
		m_edit_server_port.SetWindowTextW(TEXT(""));
		m_ipaddress_local_ip.SetWindowTextW(TEXT(""));
		m_ipaddress_local_submask.SetWindowTextW(TEXT(""));
		m_ipaddress_local_gateway.SetWindowTextW(TEXT(""));
		m_ipaddress_server_ip.SetWindowTextW(TEXT(""));

		CloseHandle(m_hcom);
		m_hcom = NULL;
	}
	else
	{
		CString str;
		for(int i=0; i<100; i++)
		{
			str.Format(TEXT("COM%d"), i);
			if(OpenCom(str))
			{
				m_bGetTag = FALSE;
				m_bGetServer = FALSE;
				DWORD write;
				BYTE CBD[] = {0xFF, 0xFE, 0x00, 0x00, 0x00, 0xFD, 0xFC};
				WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
				Sleep(200);
				if(m_bGetTag)
				{
					//连接到设备
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("斷開連接"));

					m_button_tag_getid.EnableWindow(TRUE);
					m_button_tag_setid.EnableWindow(TRUE);
					m_button_tag_getsleeptime.EnableWindow(TRUE);
					m_button_tag_setsleeptime.EnableWindow(TRUE);
					m_button_tag_getpower.EnableWindow(TRUE);
					m_button_tag_setpower.EnableWindow(TRUE);
					m_button_tag_getresponse.EnableWindow(TRUE);
					m_button_tag_setresponse.EnableWindow(TRUE);

					m_button_local_setid.EnableWindow(FALSE);
					m_button_local_dhcp.EnableWindow(FALSE);
					m_button_local_staticip.EnableWindow(FALSE);
					m_button_local_setip.EnableWindow(FALSE);
					m_button_local_settime.EnableWindow(FALSE);
					m_button_local_submask.EnableWindow(FALSE);
					m_button_local_gateway.EnableWindow(FALSE);
					m_button_local_setport.EnableWindow(FALSE);
					m_button_server_dhcp.EnableWindow(FALSE);
					m_button_server_staticip.EnableWindow(FALSE);
					m_button_server_setip.EnableWindow(FALSE);
					m_button_server_setport.EnableWindow(FALSE);

					m_combo_two.SetWindowTextW(str);

					OnBnClickedButtonTagGetid();
					OnBnClickedButtonTagGetsleeptime();
					OnBnClickedButtonTagGetpower();
					OnBnClickedButtonTagGetresponse();
					
					return;
				}
				if(m_bGetServer)
				{
					//连接到设备
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("斷開連接"));

					m_button_tag_getid.EnableWindow(FALSE);
					m_button_tag_setid.EnableWindow(FALSE);
					m_button_tag_getsleeptime.EnableWindow(FALSE);
					m_button_tag_setsleeptime.EnableWindow(FALSE);
					m_button_tag_getpower.EnableWindow(FALSE);
					m_button_tag_setpower.EnableWindow(FALSE);
					m_button_tag_getresponse.EnableWindow(FALSE);
					m_button_tag_setresponse.EnableWindow(FALSE);

					m_button_local_setid.EnableWindow(TRUE);
					m_button_local_dhcp.EnableWindow(TRUE);
					m_button_local_staticip.EnableWindow(TRUE);
					m_button_local_setip.EnableWindow(TRUE);
					m_button_local_settime.EnableWindow(TRUE);
					m_button_local_submask.EnableWindow(TRUE);
					m_button_local_gateway.EnableWindow(TRUE);
					m_button_local_setport.EnableWindow(TRUE);
					m_button_server_dhcp.EnableWindow(TRUE);
					m_button_server_staticip.EnableWindow(TRUE);
					m_button_server_setip.EnableWindow(TRUE);
					m_button_server_setport.EnableWindow(TRUE);

					m_combo_two.SetWindowTextW(str);
					GetInfo();

					return;
				}
				CloseHandle(m_hcom);
				m_hcom = NULL;
				Sleep(50);
			}
		}
		MessageBox(TEXT("連接設備失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagGetid()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bGetTagID = FALSE;
	BYTE CBD[] = {0xFF, 0xFE, 0x02, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(m_bGetTagID)
	{
		CString str;
		str.Format(TEXT("%02X"), m_Tag_ID[0]);
		m_edit_tag_getid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Tag_ID[1]);
		m_edit_tag_getid2.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取ID失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagSetid()
{
	// TODO: 在此添加控件通知处理程序代码
	CString panid1, panid2;
	m_edit_tag_setid1.GetWindowTextW(panid1);
	m_edit_tag_setid2.GetWindowTextW(panid2);
	if(panid1.GetLength() != 2 || panid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid1, m_Tag_ID))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid2, m_Tag_ID+1))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(m_Tag_ID[0] == 0 && m_Tag_ID[1] == 0)
	{
		CString str;
		str = TEXT("不能設置定位卡片的ID號為：00 00");
		MessageBox(str);
		return;
	}
	m_bSetTagID = FALSE;
	BYTE CBD[] = {0xFF, 0xFE, 0x01, 0x00, 0x00, 0xFD, 0xFC};
	CBD[3] = m_Tag_ID[0];
	CBD[4] = m_Tag_ID[1];
	DWORD write;					
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(200);
	if(m_bSetTagID)
	{
		//设置成功，ID号自动加1
		if(m_Tag_ID[1] == 0xFF)
		{
			m_Tag_ID[1] = 0x00;
			if(m_Tag_ID[0] == 0xFF)
				m_Tag_ID[0] = 0x00;
			else
				m_Tag_ID[0]++;
		}
		else
			m_Tag_ID[1]++;
		CString str;
		str.Format(TEXT("%02X"), m_Tag_ID[0]);
		m_edit_tag_setid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Tag_ID[1]);
		m_edit_tag_setid2.SetWindowTextW(str);

		OnBnClickedButtonTagGetid();
	}
	else
	{
		MessageBox(TEXT("設置ID失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagGetsleeptime()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bSleepTime = FALSE;
	BYTE CBD[] = {0xFF, 0xFE, 0x04, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(200);
	if(m_bSleepTime)
	{
		CString str;
		str.Format(TEXT("%d"), m_SleepTime);
		m_edit_tag_getsleeptime.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取時間失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagSetsleeptime()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	if(m_edit_tag_setsleeptime == 0)
	{
		MessageBox(TEXT("發送間隔不能設置為0"));
		return;
	}
	m_bSleepTime = FALSE;
	BYTE CBD[] = {0xFF, 0xFE, 0x03, 0x00, 0x00, 0xFD, 0xFC};
	CBD[3] = (m_edit_tag_setsleeptime >> 8) & 0xFF;
	CBD[4] = m_edit_tag_setsleeptime & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(200);
	if(m_bSleepTime)
	{
		OnBnClickedButtonTagGetsleeptime();
	}
	else
	{
		MessageBox(TEXT("設置時間失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagGetresponse()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bGetResponse = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x43, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetResponse)
	{
		CString str;
		str.Format(TEXT("%d"), m_Response);
		m_edit_tag_getresponse.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取靈敏度值失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagSetresponse()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	if(m_edit_tag_setresponse == 0)
	{
		MessageBox(TEXT("靈敏度值不能設置為0"));
		return;
	}
	m_bSetResponse = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x42, 0x00, 0x00, 0xFD, 0xFC};
	cmd[4] = m_edit_tag_setresponse & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetResponse)
	{
		OnBnClickedButtonTagGetresponse();
	}
	else
	{
		MessageBox(TEXT("設置靈敏度值失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagGetpower()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bGetPower = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x41, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetPower)
	{
		CString str;
		str.Format(TEXT("%.1f"), Txpower[m_SendPower]);
		m_edit_tag_getpower.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取發射功率失敗！"));
	}
}

void SetType2::OnBnClickedButtonTagSetpower()
{
	// TODO: 在此添加控件通知处理程序代码
	int num;
	num = m_combo_tag_setpower.GetCurSel();
	number = num;
	m_bSetPower = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x40, 0x00, 0x00, 0xFD, 0xFC};
	cmd[4] = number;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetPower)
	{
		OnBnClickedButtonTagGetpower();
	}
	else
	{
		MessageBox(TEXT("設置發射功率失敗！"));
	}
}


void SetType2::OnBnClickedButtonLocalDhcp()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bDHCP = FALSE;
	m_dhcpvalue = 2;
	BYTE CBD[7] = {0xFF, 0xFE, 0x03, m_dhcpvalue, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bDHCP)
	{
		MessageBox(TEXT("設定DHCP失敗！"));
		return;
	}
	m_edit_local_dhcp.SetWindowTextW(TEXT("使用DHCP功能"));
}

void SetType2::OnBnClickedButtonLocalStaticip()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bDHCP = FALSE;
	m_dhcpvalue = 1;
	BYTE CBD[7] = {0xFF, 0xFE, 0x03, m_dhcpvalue, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bDHCP)
	{
		MessageBox(TEXT("設定靜態IP失敗！"));
		return;
	}
	m_edit_local_dhcp.SetWindowTextW(TEXT("使用靜態IP功能"));
}

void SetType2::OnBnClickedButtonLocalSetip()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGetLocalIp = FALSE;
	m_ipaddress_local_setip.GetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
	BYTE CBD[9] = {0xFF, 0xFE, 0x05, m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3], 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGetLocalIp)
	{
		MessageBox(TEXT("設定本機IP地址失敗！"));
		return;
	}
	m_ipaddress_local_ip.SetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
}

void SetType2::OnBnClickedButtonLocalSetport()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGetLocalPort = FALSE;
	UpdateData(TRUE);
	if(m_edit_local_setport <= 1024 || m_edit_local_setport > 65535)
	{
		MessageBox(TEXT("請輸入1025 - 65535之間的數字！"));
		return;
	}
	m_LocalPort = m_edit_local_setport;
	BYTE CBD[7] = {0xFF, 0xFE, 0x07, m_LocalPort>>8, m_LocalPort&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGetLocalPort)
	{
		MessageBox(TEXT("設定本機端口號失敗！"));
		return;
	}
	CString str;
	str.Format(TEXT("%d"), m_LocalPort);
	m_edit_local_port.SetWindowTextW(str);
}

void SetType2::OnBnClickedButtonLocalSettime()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGetSendNameTime = FALSE;
	UpdateData(TRUE);
	if(m_edit_local_settime > 65535)
	{
		MessageBox(TEXT("請輸入0 - 65535之間的數字！"));
		return;
	}
	m_SendNameTime = m_edit_local_settime;
	BYTE CBD[7] = {0xFF, 0xFE, 0x16, m_SendNameTime>>8, m_SendNameTime&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGetSendNameTime)
	{
		MessageBox(TEXT("設定自動上報名稱時間失敗！"));
		return;
	}
	CString str;
	if(m_SendNameTime == 0)
		str = TEXT("不上報名稱");
	else
		str.Format(TEXT("%d 秒"), m_SendNameTime);
	m_edit_local_time.SetWindowTextW(str);
}

void SetType2::OnBnClickedButtonLocalSubmask()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bSubMask = FALSE;
	m_ipaddress_local_setsubmask.GetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
	BYTE CBD[9] = {0xFF, 0xFE, 0x12, m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3], 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bSubMask)
	{
		MessageBox(TEXT("設定子網掩碼失敗！"));
		return;
	}
	m_ipaddress_local_submask.SetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
}

void SetType2::OnBnClickedButtonLocalGateway()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGateWay = FALSE;
	m_ipaddress_local_setgateway.GetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
	BYTE CBD[9] = {0xFF, 0xFE, 0x14, m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3], 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGateWay)
	{
		MessageBox(TEXT("設定默認網関失敗！"));
		return;
	}
	m_ipaddress_local_gateway.SetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
}

void SetType2::OnBnClickedButtonServerDhcp()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGetServerIpMode = FALSE;
	m_ServerIpMode = 2;
	BYTE CBD[7] = {0xFF, 0xFE, 0x10, m_ServerIpMode, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIpMode)
	{
		MessageBox(TEXT("設定動態Server IP失敗！"));
		return;
	}
	m_edit_server_dhcp.SetWindowTextW(TEXT("動態IP模式"));
}

void SetType2::OnBnClickedButtonServerStaticip()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGetServerIpMode = FALSE;
	m_ServerIpMode = 1;
	BYTE CBD[7] = {0xFF, 0xFE, 0x10, m_ServerIpMode, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIpMode)
	{
		MessageBox(TEXT("設定固定Server IP失敗！"));
		return;
	}
	m_edit_server_dhcp.SetWindowTextW(TEXT("靜態IP模式"));
}

void SetType2::OnBnClickedButtonServerSetip()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGetServerIp = FALSE;
	m_ipaddress_server_setip.GetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
	BYTE CBD[9] = {0xFF, 0xFE, 0x09, m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3], 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIp)
	{
		MessageBox(TEXT("設定Server IP地址失敗！"));
		return;
	}
	m_ipaddress_server_ip.SetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
}

void SetType2::OnBnClickedButtonServerSetport()
{
	// TODO: 在此添加控件通知处理程序代码
	DWORD write;
	m_bGetServerPort = FALSE;
	UpdateData(TRUE);
	if(m_edit_server_setport <= 1024 || m_edit_server_setport > 65535)
	{
		MessageBox(TEXT("請輸入1025 - 65535之間的數字！"));
		return;
	}
	m_ServerPort = m_edit_server_setport;
	BYTE CBD[7] = {0xFF, 0xFE, 0x0B, m_ServerPort>>8, m_ServerPort&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, CBD, sizeof(CBD), &write, NULL);
	Sleep(100);
	if(!m_bGetServerPort)
	{
		MessageBox(TEXT("設定Server端口號失敗！"));
		return;
	}
	CString str;
	str.Format(TEXT("%d"), m_ServerPort);
	m_edit_server_port.SetWindowTextW(str);
}

void SetType2::OnBnClickedButtonLocalSetid()
{
	// TODO: 在此添加控件通知处理程序代码
	CString locid1,locid2;
	m_edit_local_setid1.GetWindowTextW(locid1);
	m_edit_local_setid2.GetWindowTextW(locid2);
	
	if(locid1.GetLength() != 2 || locid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：10 11");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(locid1, m_LocalID))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：10 11");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(locid2, m_LocalID+1))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：10 11");
		MessageBox(str);
		return;
	}
	if(m_LocalID[0] == 0 && m_LocalID[1] == 0)
	{
		CString str;
		str = TEXT("不能設置定位卡片的ID號為：00 00");
		MessageBox(str);
		return;
	}

	DWORD write;
	m_bGetLocalID = FALSE;	
	BYTE cmd[7];
	cmd[0] = 0xFF; cmd[1] = 0xFE;
	cmd[2] = 0x0E;
	cmd[3] = m_LocalID[0];
	cmd[4] = m_LocalID[1];
	cmd[5] = 0xFD; cmd[6] = 0xFC;
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetLocalID)
	{
		//设置成功，ID号自动加1
		if(m_LocalID[1] == 0xFF)
		{
			m_LocalID[1] = 0x00;
			if(m_LocalID[0] == 0xFF)
				m_LocalID[0] = 0x00;
			else
				m_LocalID[0]++;
		}
		else
			m_LocalID[1]++;
		CString str;
		str.Format(TEXT("%02X"), m_LocalID[0]);
		m_edit_local_setid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_LocalID[1]);
		m_edit_local_setid2.SetWindowTextW(str);

		m_edit_local_id1.SetWindowTextW(locid1);
		m_edit_local_id2.SetWindowTextW(locid2);
		
	}
	else
	{
		MessageBox(TEXT("設定本機ID失敗！"));
		return;
	}
}
