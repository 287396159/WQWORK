// SetType1.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "SetID.h"
#include "SetType1.h"


BYTE CMD_GETDEVICE[] = {0xFF, 0xFE, 0x00, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETVERSION[] = {0xFF, 0xFE, 0x01, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETDHCP[] = {0xFF, 0xFE, 0x02, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALIP[] = {0xFF, 0xFE, 0x04, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALPORT[] = {0xFF, 0xFE, 0x06, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSERVERIP[] = {0xFF, 0xFE, 0x08, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSERVERPORT[] = {0xFF, 0xFE, 0x0A, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALMAC[] = {0xFF, 0xFE, 0x0C, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALID[] = {0xFF, 0xFE, 0x0D, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSERVERIPMODE[] = {0xFF, 0xFE, 0x0F, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSUBMASK[] = {0xFF, 0xFE, 0x11, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETGATEWAY[] = {0xFF, 0xFE, 0x13, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSENDNAMETIME[] = {0xFF, 0xFE, 0x15, 0x00, 0x00, 0xFD, 0xFC};

float power[] = {4.5,2.5,1,-0.5,-1.5,-3,-4,-6,-8,-10,-12,-14,-16,-18,-20,-22};

// SetType1 �Ի���

IMPLEMENT_DYNAMIC(SetType1, CDialog)

SetType1::SetType1(CWnd* pParent /*=NULL*/)
	: CDialog(SetType1::IDD, pParent)
	, m_edit_tag_seteworktime(50)
	, m_edit_tag_setsleeptime(5)
	, m_edit_local_setport(50000)
	, m_edit_local_settime(600)
	, m_edit_server_setport(51234)
	, m_edit_setresponse(15)
{
}

SetType1::~SetType1()
{
}

void SetType1::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_CONNECT, m_button_connect);
	DDX_Control(pDX, IDC_COMBO_ONE, m_combo_one);
	DDX_Control(pDX, IDC_BUTTON_PORT_GETID, m_button_port_getid);
	DDX_Control(pDX, IDC_BUTTON_PORT_SETID, m_button_port_setid);
	DDX_Control(pDX, IDC_EDIT_PORT_GETID1, m_edit_port_getid1);
	DDX_Control(pDX, IDC_EDIT_PORT_GETID2, m_edit_port_getid2);
	DDX_Control(pDX, IDC_EDIT_PORT_SETID1, m_edit_port_setid1);
	DDX_Control(pDX, IDC_EDIT_PORT_SETID2, m_edit_port_setid2);
	DDX_Control(pDX, IDC_BUTTON_TAG_GETID, m_button_tag_getid);
	DDX_Control(pDX, IDC_BUTTON_TAG_SETID, m_button_tag_setid);
	DDX_Control(pDX, IDC_BUTTON_TAG_GETWORKTIME, m_button_tag_getworktime);
	DDX_Control(pDX, IDC_BUTTON_TAG_SETWORKTIME, m_button_tag_setworktime);
	DDX_Control(pDX, IDC_BUTTON_TAG_GETSLEEPTIME, m_button_tag_getsleeptime);
	DDX_Control(pDX, IDC_BUTTON_TAG_SETSLEEPTIME, m_button_tag_setsleeptime);
	DDX_Control(pDX, IDC_EDIT_TAG_GETID1, m_edit_tag_getid1);
	DDX_Control(pDX, IDC_EDIT_TAG_GETID2, m_edit_tag_getid2);
	DDX_Control(pDX, IDC_EDIT_TAG_SETID1, m_edit_tag_setid1);
	DDX_Control(pDX, IDC_EDIT_TAG_SETID2, m_edit_tag_setid2);
	DDX_Control(pDX, IDC_EDIT_TAG_GETWORKTIME, m_edit_tag_getworktime);
	DDX_Text(pDX, IDC_EDIT_TAG_SETWORKTIME, m_edit_tag_seteworktime);
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
	DDX_Control(pDX, IDC_BUTTON_GETPOWER, m_button_getpower);
	DDX_Control(pDX, IDC_BUTTON_SETPOWER, m_button_setpower);
	DDX_Control(pDX, IDC_EDIT_GETPOWER, m_edit_getpower);
	DDX_Control(pDX, IDC_COMBO_SETPOWER, m_combo_setpower);
	DDX_Control(pDX, IDC_EDIT_GETRESPONSE, m_edit_getresponse);
	DDX_Text(pDX, IDC_EDIT_SETRESPONSE, m_edit_setresponse);
	DDX_Control(pDX, IDC_BUTTON_GETRESPONSE, m_button_getresponse);
	DDX_Control(pDX, IDC_BUTTON_SETRESPONSE, m_button_setresponse);
	DDX_Control(pDX, IDC_BUTTON_LOCAL_SETID, m_button_local_setid);
	DDX_Control(pDX, IDC_EDIT_LOCAL2_SETID1, m_edit_local_setid1);
	DDX_Control(pDX, IDC_EDIT_LOCAL2_GETID1, m_edit_local_id1);
	DDX_Control(pDX, IDC_EDIT_LOCAL2_GETID2, m_edit_local_id2);
	DDX_Control(pDX, IDC_EDIT_LOCAL2_SETID2, m_edit_local_setid2);
}


BEGIN_MESSAGE_MAP(SetType1, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_PORT_GETID, &SetType1::OnBnClickedButtonPortGetid)
	ON_BN_CLICKED(IDC_BUTTON_PORT_SETID, &SetType1::OnBnClickedButtonPortSetid)
	ON_BN_CLICKED(IDC_BUTTON_CONNECT, &SetType1::OnBnClickedButtonConnect)
	ON_BN_CLICKED(IDC_BUTTON_TAG_GETID, &SetType1::OnBnClickedButtonTagGetid)
	ON_BN_CLICKED(IDC_BUTTON_TAG_GETWORKTIME, &SetType1::OnBnClickedButtonTagGetworktime)
	ON_BN_CLICKED(IDC_BUTTON_TAG_GETSLEEPTIME, &SetType1::OnBnClickedButtonTagGetsleeptime)
	ON_BN_CLICKED(IDC_BUTTON_TAG_SETID, &SetType1::OnBnClickedButtonTagSetid)
	ON_BN_CLICKED(IDC_BUTTON_TAG_SETWORKTIME, &SetType1::OnBnClickedButtonTagSetworktime)
	ON_BN_CLICKED(IDC_BUTTON_TAG_SETSLEEPTIME, &SetType1::OnBnClickedButtonTagSetsleeptime)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_DHCP, &SetType1::OnBnClickedButtonLocalDhcp)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_STATICIP, &SetType1::OnBnClickedButtonLocalStaticip)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETIP, &SetType1::OnBnClickedButtonLocalSetip)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETPORT, &SetType1::OnBnClickedButtonLocalSetport)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETTIME, &SetType1::OnBnClickedButtonLocalSettime)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SUBMASK, &SetType1::OnBnClickedButtonLocalSubmask)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_GATEWAY, &SetType1::OnBnClickedButtonLocalGateway)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_DHCP, &SetType1::OnBnClickedButtonServerDhcp)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_STATICIP, &SetType1::OnBnClickedButtonServerStaticip)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_SETIP, &SetType1::OnBnClickedButtonServerSetip)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_SETPORT, &SetType1::OnBnClickedButtonServerSetport)
	ON_BN_CLICKED(IDC_BUTTON_GETPOWER, &SetType1::OnBnClickedButtonGetpower)
	ON_BN_CLICKED(IDC_BUTTON_SETPOWER, &SetType1::OnBnClickedButtonSetpower)
	ON_BN_CLICKED(IDC_BUTTON_GETRESPONSE, &SetType1::OnBnClickedButtonGetresponse)
	ON_BN_CLICKED(IDC_BUTTON_SETRESPONSE, &SetType1::OnBnClickedButtonSetresponse)
	ON_BN_CLICKED(IDC_BUTTON_LOCAL_SETID, &SetType1::OnBnClickedButtonLocalSetid)
END_MESSAGE_MAP()

DWORD ComReadThread(LPVOID lparam)
{	
	DWORD	actualReadLen=0;	//ʵ�ʶ�ȡ���ֽ���
	DWORD	willReadLen;	
	
	DWORD dwReadErrors;
	COMSTAT	cmState;
	
	SetType1 *pdlg;
	pdlg = (SetType1 *)lparam;

	pdlg->m_Receive_Data_Len = 0;
		
	// ��ջ��壬����鴮���Ƿ�򿪡�
	ASSERT(pdlg->m_hcom != INVALID_HANDLE_VALUE); 	
	//��մ���
//	PurgeComm(pdlg->m_hcom, PURGE_RXCLEAR | PURGE_TXCLEAR );	
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


void SetType1::ParseData(void)
{
	int start;
	while(m_Receive_Data_Len >= 7)
	{
		//���ҿ�ʼ��־FF FF
		start = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, 0xFB, 0xFA);
		if(start >= 0)
		{
			//�ҵ���ʼ��־
			//�жϴӿ�ʼ��־��ʼ���Ƿ��н���һ����С���ĳ���
			if(start + 7 <= m_Receive_Data_Len)
			{				   
				//������ץȡ����
				switch(m_Receive_Data_Char[start+2])
				{
				case 0x80:	//ģ�����ͷ��� 7Byte
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						if(m_Receive_Data_Char[start+3] == 0x01 && m_Receive_Data_Char[start+4] == 0x00)
							m_bGetPort = TRUE;
						else if(m_Receive_Data_Char[start+3] == 0x02 && m_Receive_Data_Char[start+4] == 0x00)
							m_bGetTag = TRUE;
						else if(m_Receive_Data_Char[start+3] == 0x04 && m_Receive_Data_Char[start+4] == 0x00)
							m_bGetServer = TRUE;
					//������ɣ����������������
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					
					break;
				case 0x81:	
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_Receive_Data_Char[start+3] == m_Port_ID[0] && m_Receive_Data_Char[start+4] == m_Port_ID[1])
						{		
								m_bSetPortID = TRUE;
						}
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
							//������ɣ����������������
							m_Receive_Data_Len = m_Receive_Data_Len - start - 10;
							memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+10, m_Receive_Data_Len);
						}
					}
					else
					{
						//����������
						return;
					}					
					break;
				case 0x82:	
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_bGetPort == TRUE)
						{
							m_bGetPortID = TRUE;
							m_Port_ID[0] = m_Receive_Data_Char[start+3];
							m_Port_ID[1] = m_Receive_Data_Char[start+4];
						}
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
					//������ɣ����������������
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x83:	//��λ��Ƭ������˯��ʱ��      //λ�òο��㣺���÷��书��
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_bGetPort == TRUE)
						{
							if(m_Receive_Data_Char[start+4] == number)
								m_bSetPower = TRUE;
						}
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
					//������ɣ����������������
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x84:	//��λ��Ƭ����ȡ˯��ʱ��     //λ�òο��㣺��ȡ���书��
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_bGetPort == TRUE)
						{
							m_bGetPower = TRUE;
							m_SendPower = m_Receive_Data_Char[start+4];
						}
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
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0xC1:  //��ȡ��λ��Ƭ����ʱ��
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bWorkTime = TRUE;
						m_WorkTime = (m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4];
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0xC0:  //���ö�λ��Ƭ����ʱ��
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(((m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4]) == m_edit_tag_seteworktime)
						{
								m_bWorkTime = TRUE;
						}
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0xC2:  //����Sensor������
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_Receive_Data_Char[start+4] == m_edit_setresponse)
								m_bSetResponse = TRUE;
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0xC3:   //��ȡSensor������
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bGetResponse = TRUE;
						m_Response = m_Receive_Data_Char[start+4];
					}
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x85:	//�O������IP�ķ��أ�9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_LocalIp, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGetLocalIp = TRUE;
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}
					break;
				case 0x86:	//��ԃģ�M�˿�̖�ķ���
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bGetLocalPort = TRUE;
						m_LocalPort = (m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4];
					}
					//������ɣ����������������
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x87:	//����ģ�M�˿�̖�ķ���
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_LocalPort == ((m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4]))
							m_bGetLocalPort = TRUE;						
					}
					//������ɣ����������������
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x88:	//�xȡServerIP�ķ��أ�9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bGetServerIp = TRUE;
							memcpy(m_ServerIp, m_Receive_Data_Char+start+3, 4);
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x89:	//�O��Server IP�ķ��أ�9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_ServerIp, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGetServerIp = TRUE;
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}
					break;
				case 0x8A:	//��ԃServer�˿�̖�ķ���
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bGetServerPort = TRUE;
						m_ServerPort = (m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4];
					}
					//������ɣ����������������
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x8B:	//����Server�˿�̖�ķ���
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_ServerPort == ((m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4]))
							m_bGetServerPort = TRUE;						
					}
					//������ɣ����������������
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x8C:	//�xȡ����MAC��ַ���أ�11Byte
					if(start + 11 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+9] == 0xF9 && m_Receive_Data_Char[start+10] == 0xF8)
						{
							m_bGetLocalMac = TRUE;
							memcpy(m_LocalMac, m_Receive_Data_Char+start+3, 6);
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 11;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+11, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x8D:	//��ȡ����ID���أ�7Byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetLocalID = TRUE;
							m_LocalID[0] = m_Receive_Data_Char[start+3];
							m_LocalID[1] = m_Receive_Data_Char[start+4];
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x8E:	//���ñ���ID���أ�7Byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(m_Receive_Data_Char[start+3] == m_LocalID[0] && m_Receive_Data_Char[start+4] == m_LocalID[1])
								m_bGetLocalID = TRUE;							
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x8F:	//��ȡServer IP ģʽ��7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetServerIpMode = TRUE;		
							m_ServerIpMode = m_Receive_Data_Char[start+3];
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x90:	//����Server IP ģʽ��7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(m_Receive_Data_Char[start+3] == m_ServerIpMode)
								m_bGetServerIpMode = TRUE;							
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x91:	//�xȡ�������룬9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bSubMask = TRUE;
							memcpy(m_SubMask, m_Receive_Data_Char+start+3, 4);
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x92:	//�O���������룬9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_SubMask, m_Receive_Data_Char+start+3, 4) == 0)
								m_bSubMask = TRUE;
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}
					break;
				case 0x93:	//�xȡĬ�����أ�9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bGateWay = TRUE;
							memcpy(m_GateWay, m_Receive_Data_Char+start+3, 4);
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x94:	//�O��Ĭ�����أ�9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_GateWay, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGateWay = TRUE;
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}
					break;
				case 0x95:	//��ȡ�Զ��ϱ����Ƶ�ʱ�䣬7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetSendNameTime = TRUE;		
							m_SendNameTime = (m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4];
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;
				case 0x96:	//�����Զ��ϱ����Ƶ�ʱ�䣬7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(((m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4]) == m_SendNameTime)
								m_bGetSendNameTime = TRUE;							
						}
						//������ɣ����������������
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//����������
						return;
					}	
					break;

				default:	//δ֪��ָ��
					//����ͷ
					m_Receive_Data_Len = m_Receive_Data_Len - start - 2;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+2, m_Receive_Data_Len);					
					break;
				}
								
			}
			else
			{
				//���յ��Ĳ���һ�������ȴ������ٴ���
				if(start > 0)
				{
					//�����ݰ�ͷǰ���������ݣ�����Щ���������
					m_Receive_Data_Len = m_Receive_Data_Len - start;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start, m_Receive_Data_Len);
				}
				break;
			}
		}
		else
		{
			//û���ҵ���ʼ��־
			if(m_Receive_Data_Char[m_Receive_Data_Len-1] == 0xFB)
			{
				//���һ���ַ���0xFB����ô��������һ�����ݰ��Ŀ�ͷ���������һ���ַ�����������
				m_Receive_Data_Char[0] = 0xFB;
				m_Receive_Data_Len = 1;
			}
			else
			{
				//���һ���ַ�����0xFB����ôȫ������
				m_Receive_Data_Len = 0;
			}
			break;
		}
	}
}


int SetType1::FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2)
{
	for(int i=start; i<end-1; i++)
	{
		if(str[i] == c1 && str[i+1] == c2)
			return i;
	}
	return -1;
}


BOOL SetType1::OpenCom(CString str_com)
{	
	str_com = TEXT("\\\\.\\") + str_com;
	m_hcom = CreateFile(str_com, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
	///���ô���
	if(m_hcom != INVALID_HANDLE_VALUE && m_hcom != NULL)
	{		
		DCB  dcb;    
		dcb.DCBlength = sizeof(DCB); 
		// Ĭ�ϴ��ڲ���
		GetCommState(m_hcom, &dcb);

		dcb.BaudRate = 115200;					// ���ò����� 
		dcb.fBinary = TRUE;						// ���ö�����ģʽ���˴���������TRUE
		dcb.fParity = TRUE;						// ֧����żУ�� 
		dcb.ByteSize = 8;						// ����λ,��Χ:4-8 
		dcb.Parity = NOPARITY;					// У��ģʽ
		dcb.StopBits = ONESTOPBIT;				// ֹͣλ 0,1,2 = 1, 1.5, 2

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
		dcb.fAbortOnError = FALSE;			// �����ڷ������󣬲�����ֹ���ڶ�д


		if (!SetCommState(m_hcom, &dcb))
		{
			///L"���ô���ʧ��";			
			return FALSE;
		}
		////���ó�ʱֵ
		COMMTIMEOUTS  cto;
		GetCommTimeouts(m_hcom, &cto);
		cto.ReadIntervalTimeout = MAXDWORD;  
		cto.ReadTotalTimeoutMultiplier = 10;  
		cto.ReadTotalTimeoutConstant = 10;    
		cto.WriteTotalTimeoutMultiplier = 50;  
		cto.WriteTotalTimeoutConstant = 100;    
		if (!SetCommTimeouts(m_hcom, &cto))
		{
			///L"�������ó�ʱ����";		
			return FALSE;
		}	

		//ָ���˿ڼ����¼���
		SetCommMask (m_hcom, EV_RXCHAR);
		
		//�����豸������
	//	SetupComm(m_hcom,8192,8192);

		//��ʼ���������е���Ϣ
		PurgeComm(m_hcom,PURGE_TXCLEAR|PURGE_RXCLEAR);
	}
	else
	{		
		return FALSE;
	}

	HANDLE m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ComReadThread, this, 0, NULL);
	CloseHandle(m_hThread);

	return TRUE;
}

BOOL SetType1::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	
	m_button_connect.SetWindowTextW(TEXT("�Ԅ��B���O��"));
	m_combo_one.EnableWindow(FALSE);
	m_combo_one.SetWindowTextW(TEXT(""));

	m_button_port_getid.EnableWindow(FALSE);
	m_button_port_setid.EnableWindow(FALSE);
	m_button_getpower.EnableWindow(FALSE);
	m_button_setpower.EnableWindow(FALSE);
	m_combo_setpower.SetCurSel(0);

	m_button_tag_getid.EnableWindow(FALSE);
	m_button_tag_setid.EnableWindow(FALSE);
	m_button_tag_getworktime.EnableWindow(FALSE);
	m_button_tag_setworktime.EnableWindow(FALSE);
	m_button_tag_getsleeptime.EnableWindow(FALSE);
	m_button_tag_setsleeptime.EnableWindow(FALSE);
	m_button_getresponse.EnableWindow(FALSE);
	m_button_setresponse.EnableWindow(FALSE);

	m_button_local_dhcp.EnableWindow(FALSE);
	m_button_local_staticip.EnableWindow(FALSE);
	m_button_local_setip.EnableWindow(FALSE);
	m_button_local_settime.EnableWindow(FALSE);
	m_button_local_setid.EnableWindow(FALSE);
	m_button_local_submask.EnableWindow(FALSE);
	m_button_local_gateway.EnableWindow(FALSE);
	m_button_local_setport.EnableWindow(FALSE);
	m_button_server_dhcp.EnableWindow(FALSE);
	m_button_server_staticip.EnableWindow(FALSE);
	m_button_server_setip.EnableWindow(FALSE);
	m_button_server_setport.EnableWindow(FALSE);

	m_bConnect = FALSE;

	//m_LeftButton_down = FALSE;

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void SetType1::GetInfo()
{
	DWORD write;

	m_bDHCP = FALSE;
	WriteFile(m_hcom, CMD_GETDHCP, sizeof(CMD_GETDHCP), &write, NULL);
	Sleep(100);
	if(m_bDHCP)
	{
		if(m_dhcpvalue == 1)
			m_edit_local_dhcp.SetWindowTextW(TEXT("ʹ���o�BIP����"));
		else
			m_edit_local_dhcp.SetWindowTextW(TEXT("ʹ��DHCP����"));
	}
	else
		MessageBox(TEXT("�@ȡDHCP��Bʧ����"));

	m_bGetLocalIp = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALIP, sizeof(CMD_GETLOCALIP), &write, NULL);
	Sleep(100);
	if(m_bGetLocalIp)
	{
		m_ipaddress_local_ip.SetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
	}
	else
		MessageBox(TEXT("�@ȡ���CIP��ַʧ����"));

	m_bGetLocalPort = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALPORT, sizeof(CMD_GETLOCALPORT), &write, NULL);
	Sleep(100);
	if(m_bGetLocalPort)
	{
		CString str;
		str.Format(TEXT("%d"), m_LocalPort);
		m_edit_local_port.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("�@ȡ���C�˿�̖ʧ����"));

	m_bGetServerIp = FALSE;
	WriteFile(m_hcom, CMD_GETSERVERIP, sizeof(CMD_GETSERVERIP), &write, NULL);
	Sleep(100);
	if(m_bGetServerIp)
	{
		m_ipaddress_server_ip.SetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
	}
	else
		MessageBox(TEXT("�@ȡServer IP��ַʧ����"));

	m_bGetServerPort = FALSE;
	WriteFile(m_hcom, CMD_GETSERVERPORT, sizeof(CMD_GETSERVERPORT), &write, NULL);
	Sleep(100);
	if(m_bGetServerPort)
	{
		CString str;
		str.Format(TEXT("%d"), m_ServerPort);
		m_edit_server_port.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("�@ȡServer�˿�̖ʧ����"));

	m_bGetLocalMac = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALMAC, sizeof(CMD_GETLOCALMAC), &write, NULL);
	Sleep(100);
	if(m_bGetLocalMac)
	{
		CString str;
		str.Format(TEXT("%02X:%02X:%02X:%02X:%02X:%02X"), m_LocalMac[0], m_LocalMac[1], m_LocalMac[2], m_LocalMac[3], m_LocalMac[4], m_LocalMac[5]);
		m_edit_local_mac.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("�@ȡServer�˿�̖ʧ����"));

	m_bGetLocalID = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALID, sizeof(CMD_GETLOCALID), &write, NULL);
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
		MessageBox(TEXT("�@ȡ���CIDʧ����"));

	m_bGetServerIpMode = FALSE;
	WriteFile(m_hcom, CMD_GETSERVERIPMODE, sizeof(CMD_GETSERVERIPMODE), &write, NULL);
	Sleep(100);
	if(m_bGetServerIpMode)
	{
		CString str;
		if(m_ServerIpMode == 1)
			str = TEXT("�o�BIPģʽ");
		else if(m_ServerIpMode == 2)
			str = TEXT("�ӑBIPģʽ");
		else
			str = TEXT("δ֪��IPģʽ");

		m_edit_server_dhcp.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("�@ȡServer IPģʽʧ����"));

	m_bSubMask = FALSE;
	WriteFile(m_hcom, CMD_GETSUBMASK, sizeof(CMD_GETSUBMASK), &write, NULL);
	Sleep(100);
	if(m_bSubMask)
	{
		m_ipaddress_local_submask.SetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
	}
	else
		MessageBox(TEXT("�@ȡ�ӾW�ڴaʧ����"));

	m_bGateWay = FALSE;
	WriteFile(m_hcom, CMD_GETGATEWAY, sizeof(CMD_GETGATEWAY), &write, NULL);
	Sleep(100);
	if(m_bGateWay)
	{
		m_ipaddress_local_gateway.SetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
	}
	else
		MessageBox(TEXT("�@ȡĬ�J�W�vʧ����"));

	m_bGetSendNameTime = FALSE;
	WriteFile(m_hcom, CMD_GETSENDNAMETIME, sizeof(CMD_GETSENDNAMETIME), &write, NULL);
	Sleep(100);
	if(m_bGetSendNameTime)
	{
		CString str;
		if(m_SendNameTime == 0)
			str.Format(TEXT("���ψ����Q"));
		else
			str.Format(TEXT("%d ��"), m_SendNameTime);
		m_edit_local_time.SetWindowTextW(str);	
	}
	else
		MessageBox(TEXT("�@ȡ�ψ����Q�r�gʧ����"));
}


// SetType1 ��Ϣ�������
void SetType1::OnBnClickedButtonConnect(){
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	if(m_bConnect){
		m_bConnect = FALSE;
		m_button_connect.SetWindowTextW(TEXT("�Ԅ��B���O��"));
		m_combo_one.SetWindowTextW(TEXT(""));
		m_button_port_getid.EnableWindow(FALSE);
		m_button_port_setid.EnableWindow(FALSE);
		m_button_getpower.EnableWindow(FALSE);
		m_button_setpower.EnableWindow(FALSE);

		m_button_tag_getid.EnableWindow(FALSE);
		m_button_tag_setid.EnableWindow(FALSE);
		m_button_tag_getworktime.EnableWindow(FALSE);
		m_button_tag_setworktime.EnableWindow(FALSE);
		m_button_tag_getsleeptime.EnableWindow(FALSE);
		m_button_tag_setsleeptime.EnableWindow(FALSE);
		m_button_getresponse.EnableWindow(FALSE);
		m_button_setresponse.EnableWindow(FALSE);

		m_button_local_dhcp.EnableWindow(FALSE);
		m_button_local_staticip.EnableWindow(FALSE);
		m_button_local_setip.EnableWindow(FALSE);
		m_button_local_settime.EnableWindow(FALSE);
		m_button_local_setid.EnableWindow(FALSE);
		m_button_local_submask.EnableWindow(FALSE);
		m_button_local_gateway.EnableWindow(FALSE);
		m_button_local_setport.EnableWindow(FALSE);
		m_button_server_dhcp.EnableWindow(FALSE);
		m_button_server_staticip.EnableWindow(FALSE);
		m_button_server_setip.EnableWindow(FALSE);
		m_button_server_setport.EnableWindow(FALSE);

		m_edit_port_getid1.SetWindowTextW(TEXT(""));
		m_edit_port_getid2.SetWindowTextW(TEXT(""));
		m_edit_getpower.SetWindowTextW(TEXT(""));
		m_edit_tag_getid1.SetWindowTextW(TEXT(""));
		m_edit_tag_getid2.SetWindowTextW(TEXT(""));
		m_edit_tag_getworktime.SetWindowTextW(TEXT(""));
		m_edit_tag_getsleeptime.SetWindowTextW(TEXT(""));
		m_edit_getresponse.SetWindowTextW(TEXT(""));

		m_edit_local_dhcp.SetWindowTextW(TEXT(""));
		m_edit_local_time.SetWindowTextW(TEXT(""));
		m_edit_local_port.SetWindowTextW(TEXT(""));
		m_edit_local_mac.SetWindowTextW(TEXT(""));
		m_edit_local_id1.SetWindowTextW(TEXT(""));
		m_edit_local_id2.SetWindowTextW(TEXT(""));
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
			if(OpenCom(str)){
				m_bGetTag = FALSE;
				m_bGetPort = FALSE;
				m_bGetServer = FALSE;
				DWORD write;
				BYTE cmd[] = {0xFF, 0xFE, 0x00, 0x00, 0x00, 0xFD, 0xFC};
				WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
				Sleep(200);
				if(m_bGetPort)
				{
					//���ӵ��豸
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("���_�B��"));

					m_button_port_getid.EnableWindow(TRUE);
					m_button_port_setid.EnableWindow(TRUE);
					m_button_getpower.EnableWindow(TRUE);
					m_button_setpower.EnableWindow(TRUE);

					m_button_tag_getid.EnableWindow(FALSE);
					m_button_tag_setid.EnableWindow(FALSE);
					m_button_tag_getworktime.EnableWindow(FALSE);
					m_button_tag_setworktime.EnableWindow(FALSE);
					m_button_tag_getsleeptime.EnableWindow(FALSE);
					m_button_tag_setsleeptime.EnableWindow(FALSE);
					m_button_getresponse.EnableWindow(FALSE);
					m_button_setresponse.EnableWindow(FALSE);

					m_button_local_dhcp.EnableWindow(FALSE);
					m_button_local_staticip.EnableWindow(FALSE);
					m_button_local_setip.EnableWindow(FALSE);
					m_button_local_settime.EnableWindow(FALSE);
					m_button_local_setid.EnableWindow(FALSE);
					m_button_local_submask.EnableWindow(FALSE);
					m_button_local_gateway.EnableWindow(FALSE);
					m_button_local_setport.EnableWindow(FALSE);
					m_button_server_dhcp.EnableWindow(FALSE);
					m_button_server_staticip.EnableWindow(FALSE);
					m_button_server_setip.EnableWindow(FALSE);
					m_button_server_setport.EnableWindow(FALSE);

					m_combo_one.SetWindowTextW(str);

					OnBnClickedButtonPortGetid();
					OnBnClickedButtonGetpower();

					return;
				}
				if(m_bGetTag)
				{
					//���ӵ��豸
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("���_�B��"));

					m_button_port_getid.EnableWindow(FALSE);
					m_button_port_setid.EnableWindow(FALSE);
					m_button_getpower.EnableWindow(FALSE);
					m_button_setpower.EnableWindow(FALSE);

					m_button_tag_getid.EnableWindow(TRUE);
					m_button_tag_setid.EnableWindow(TRUE);
					m_button_tag_getworktime.EnableWindow(TRUE);
					m_button_tag_setworktime.EnableWindow(TRUE);
					m_button_tag_getsleeptime.EnableWindow(TRUE);
					m_button_tag_setsleeptime.EnableWindow(TRUE);
					m_button_getresponse.EnableWindow(TRUE);
					m_button_setresponse.EnableWindow(TRUE);

					m_button_local_dhcp.EnableWindow(FALSE);
					m_button_local_staticip.EnableWindow(FALSE);
					m_button_local_setip.EnableWindow(FALSE);
					m_button_local_settime.EnableWindow(FALSE);
					m_button_local_setid.EnableWindow(FALSE);
					m_button_local_submask.EnableWindow(FALSE);
					m_button_local_gateway.EnableWindow(FALSE);
					m_button_local_setport.EnableWindow(FALSE);
					m_button_server_dhcp.EnableWindow(FALSE);
					m_button_server_staticip.EnableWindow(FALSE);
					m_button_server_setip.EnableWindow(FALSE);
					m_button_server_setport.EnableWindow(FALSE);

					m_combo_one.SetWindowTextW(str);

					OnBnClickedButtonTagGetid();
					OnBnClickedButtonTagGetsleeptime();
					OnBnClickedButtonTagGetworktime();
					OnBnClickedButtonGetresponse();
					
					return;
				}
				if(m_bGetServer)
				{
					//���ӵ��豸
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("���_�B��"));
					m_button_port_getid.EnableWindow(FALSE);
					m_button_port_setid.EnableWindow(FALSE);
					m_button_getpower.EnableWindow(FALSE);
					m_button_setpower.EnableWindow(FALSE);

					m_button_tag_getid.EnableWindow(FALSE);
					m_button_tag_setid.EnableWindow(FALSE);
					m_button_tag_getworktime.EnableWindow(FALSE);
					m_button_tag_setworktime.EnableWindow(FALSE);
					m_button_tag_getsleeptime.EnableWindow(FALSE);
					m_button_tag_setsleeptime.EnableWindow(FALSE);
					m_button_getresponse.EnableWindow(FALSE);
					m_button_setresponse.EnableWindow(FALSE);


					m_button_local_dhcp.EnableWindow(TRUE);
					m_button_local_staticip.EnableWindow(TRUE);
					m_button_local_setip.EnableWindow(TRUE);
					m_button_local_settime.EnableWindow(TRUE);
					m_button_local_setid.EnableWindow(TRUE);
					m_button_local_submask.EnableWindow(TRUE);
					m_button_local_gateway.EnableWindow(TRUE);
					m_button_local_setport.EnableWindow(TRUE);
					m_button_server_dhcp.EnableWindow(TRUE);
					m_button_server_staticip.EnableWindow(TRUE);
					m_button_server_setip.EnableWindow(TRUE);
					m_button_server_setport.EnableWindow(TRUE);

					m_combo_one.SetWindowTextW(str);
					GetInfo();

					return;
				}
				CloseHandle(m_hcom);
				m_hcom = NULL;
				Sleep(50);
			}
		}
		MessageBox(TEXT("�B���O��ʧ����"));
	}
}

bool SetType1::StringToChar(CString str, BYTE* data)
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


void SetType1::OnBnClickedButtonPortGetid()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	m_bGetPortID = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x02, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetPortID)
	{
		CString str;
		str.Format(TEXT("%02X"), m_Port_ID[0]);
		m_edit_port_getid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Port_ID[1]);
		m_edit_port_getid2.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("�@ȡIDʧ����"));
	}
}

void SetType1::OnBnClickedButtonPortSetid()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	CString panid1, panid2;
	m_edit_port_setid1.GetWindowTextW(panid1);
	m_edit_port_setid2.GetWindowTextW(panid2);
	if(panid1.GetLength() != 2 || panid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺01 02");
		MessageBox(str);
		return;
	}
	if(!StringToChar(panid1, m_Port_ID))
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺01 02");
		MessageBox(str);
		return;
	}
	if(!StringToChar(panid2, m_Port_ID+1))
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺19 9B");
		MessageBox(str);
		return;
	}
	if(m_Port_ID[0] == 0 && m_Port_ID[1] == 0)
	{
		CString str;
		str = TEXT("�����O�Å���λ�õ�ID̖�飺00 00");
		MessageBox(str);
		return;
	}
	m_bSetPortID = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x01, 0x00, 0x00, 0xFD, 0xFC};
	cmd[3] = m_Port_ID[0];
	cmd[4] = m_Port_ID[1];
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetPortID)
	{
		//���óɹ���ID���Զ���1
		if(m_Port_ID[1] == 0xFF)
		{
			m_Port_ID[1] = 0x00;
			if(m_Port_ID[0] == 0xFF)
				m_Port_ID[0] = 0x00;
			else
				m_Port_ID[0]++;
		}
		else
			m_Port_ID[1]++;
		CString str;
		str.Format(TEXT("%02X"), m_Port_ID[0]);
		m_edit_port_setid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Port_ID[1]);
		m_edit_port_setid2.SetWindowTextW(str);

		OnBnClickedButtonPortGetid();
	}
	else
	{
		MessageBox(TEXT("�O��IDʧ����"));
	}
}

void SetType1::OnBnClickedButtonGetpower()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	m_bGetPower = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x04, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetPower)
	{
		CString str;
		str.Format(TEXT("%.1f"), power[m_SendPower]);
		m_edit_getpower.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("�@ȡ�l�书��ʧ����"));
	}

}

void SetType1::OnBnClickedButtonSetpower()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	int num;
	num = m_combo_setpower.GetCurSel();
	number = num;
	m_bSetPower = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x03, 0x00, 0x00, 0xFD, 0xFC};
	cmd[4] = number;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetPower)
	{
		OnBnClickedButtonGetpower();
	}
	else
	{
		MessageBox(TEXT("�O�ðl�书��ʧ����"));
	}
}


void SetType1::OnBnClickedButtonTagGetid()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	m_bGetTagID = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x02, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
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
		MessageBox(TEXT("�@ȡIDʧ����"));
	}
}

void SetType1::OnBnClickedButtonTagSetid()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	CString panid1, panid2;
	m_edit_tag_setid1.GetWindowTextW(panid1);
	m_edit_tag_setid2.GetWindowTextW(panid2);
	if(panid1.GetLength() != 2 || panid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid1, m_Tag_ID))
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid2, m_Tag_ID+1))
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺19 9B");
		MessageBox(str);
		return;
	}
	if(m_Tag_ID[0] == 0 && m_Tag_ID[1] == 0)
	{
		CString str;
		str = TEXT("�����O�ö�λ��Ƭ��ID̖�飺00 00");
		MessageBox(str);
		return;
	}
	m_bSetTagID = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x01, 0x00, 0x00, 0xFD, 0xFC};
	cmd[3] = m_Tag_ID[0];
	cmd[4] = m_Tag_ID[1];
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetTagID)
	{
		//���óɹ���ID���Զ���1
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
		MessageBox(TEXT("�O��IDʧ����"));
	}
}

void SetType1::OnBnClickedButtonGetresponse()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	m_bGetResponse = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x43, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetResponse)
	{
		CString str;
		str.Format(TEXT("%d"), m_Response);
		m_edit_getresponse.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("�@ȡ�`����ֵʧ����"));
	}
}

void SetType1::OnBnClickedButtonSetresponse()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	UpdateData(TRUE);
	if(m_edit_setresponse == 0)
	{
		MessageBox(TEXT("�`����ֵ�����O�Þ�0"));
		return;
	}
	m_bSetResponse = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x42, 0x00, 0x00, 0xFD, 0xFC};
	cmd[4] = m_edit_setresponse & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetResponse)
	{
		OnBnClickedButtonGetresponse();
	}
	else
	{
		MessageBox(TEXT("�O���`����ֵʧ����"));
	}
}


void SetType1::OnBnClickedButtonTagGetworktime()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	m_bWorkTime = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x41, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bWorkTime)
	{
		CString str;
		str.Format(TEXT("%d"), m_WorkTime);
		m_edit_tag_getworktime.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("�@ȡ�����r�gʧ����"));
	}
}

void SetType1::OnBnClickedButtonTagSetworktime()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	UpdateData(TRUE);
	if(m_edit_tag_seteworktime == 0)
	{
		MessageBox(TEXT("�����g�������O�Þ�0"));
		return;
	}
	m_bWorkTime = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x40, 0x00, 0x00, 0xFD, 0xFC};
	cmd[3] = (m_edit_tag_seteworktime >> 8) & 0xFF;
	cmd[4] = m_edit_tag_seteworktime & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bWorkTime)
	{
		OnBnClickedButtonTagGetworktime();
	}
	else
	{
		MessageBox(TEXT("�O�ù����r�gʧ����"));
	}
}


void SetType1::OnBnClickedButtonTagGetsleeptime()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	m_bSleepTime = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x04, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSleepTime)
	{
		CString str;
		str.Format(TEXT("%d"), m_SleepTime);
		m_edit_tag_getsleeptime.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("�@ȡ�r�gʧ����"));
	}
}

void SetType1::OnBnClickedButtonTagSetsleeptime()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	UpdateData(TRUE);
	if(m_edit_tag_setsleeptime == 0)
	{
		MessageBox(TEXT("�l���g�������O�Þ�0"));
		return;
	}
	m_bSleepTime = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x03, 0x00, 0x00, 0xFD, 0xFC};
	cmd[3] = (m_edit_tag_setsleeptime >> 8) & 0xFF;
	cmd[4] = m_edit_tag_setsleeptime & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSleepTime)
	{
		OnBnClickedButtonTagGetsleeptime();
	}
	else
	{
		MessageBox(TEXT("�O�Õr�gʧ����"));
	}
}

void SetType1::OnBnClickedButtonLocalDhcp()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bDHCP = FALSE;
	m_dhcpvalue = 2;
	BYTE cmd[7] = {0xFF, 0xFE, 0x03, m_dhcpvalue, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bDHCP)
	{
		MessageBox(TEXT("�O��DHCPʧ����"));
		return;
	}
	m_edit_local_dhcp.SetWindowTextW(TEXT("ʹ��DHCP����"));
}

void SetType1::OnBnClickedButtonLocalStaticip()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bDHCP = FALSE;
	m_dhcpvalue = 1;
	BYTE cmd[7] = {0xFF, 0xFE, 0x03, m_dhcpvalue, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bDHCP)
	{
		MessageBox(TEXT("�O���o�BIPʧ����"));
		return;
	}
	m_edit_local_dhcp.SetWindowTextW(TEXT("ʹ���o�BIP����"));
}

void SetType1::OnBnClickedButtonLocalSetip()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGetLocalIp = FALSE;
	m_ipaddress_local_setip.GetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x05, m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetLocalIp)
	{
		MessageBox(TEXT("�O�����CIP��ַʧ����"));
		return;
	}
	m_ipaddress_local_ip.SetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
}

void SetType1::OnBnClickedButtonLocalSetport()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGetLocalPort = FALSE;
	UpdateData(TRUE);
	if(m_edit_local_setport <= 1024 || m_edit_local_setport > 65535)
	{
		MessageBox(TEXT("Ոݔ��1025 - 65535֮�g�Ĕ��֣�"));
		return;
	}
	m_LocalPort = m_edit_local_setport;
	BYTE cmd[7] = {0xFF, 0xFE, 0x07, m_LocalPort>>8, m_LocalPort&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetLocalPort)
	{
		MessageBox(TEXT("�O�����C�˿�̖ʧ����"));
		return;
	}
	CString str;
	str.Format(TEXT("%d"), m_LocalPort);
	m_edit_local_port.SetWindowTextW(str);
}

void SetType1::OnBnClickedButtonLocalSetid()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	CString locid1,locid2;
	m_edit_local_setid1.GetWindowTextW(locid1);
	m_edit_local_setid2.GetWindowTextW(locid2);
	
	if(locid1.GetLength() != 2 || locid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺10 11");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(locid1, m_LocalID))
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺10 11");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(locid2, m_LocalID+1))
	{
		CString str;
		str = TEXT("Ոݔ��2λʮ���M�ƵĔ��֣����磺10 11");
		MessageBox(str);
		return;
	}
	if(m_LocalID[0] == 0 && m_LocalID[1] == 0)
	{
		CString str;
		str = TEXT("�����O�ö�λ��Ƭ��ID̖�飺00 00");
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
		//���óɹ���ID���Զ���1
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
		MessageBox(TEXT("�O�����CIDʧ����"));
		return;
	}
}


void SetType1::OnBnClickedButtonLocalSettime()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGetSendNameTime = FALSE;
	UpdateData(TRUE);
	if(m_edit_local_settime > 65535)
	{
		MessageBox(TEXT("Ոݔ��0 - 65535֮�g�Ĕ��֣�"));
		return;
	}
	m_SendNameTime = m_edit_local_settime;
	BYTE cmd[7] = {0xFF, 0xFE, 0x16, m_SendNameTime>>8, m_SendNameTime&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetSendNameTime)
	{
		MessageBox(TEXT("�O���Ԅ��ψ����Q�r�gʧ����"));
		return;
	}
	CString str;
	if(m_SendNameTime == 0)
		str = TEXT("���ψ����Q");
	else
		str.Format(TEXT("%d ��"), m_SendNameTime);
	m_edit_local_time.SetWindowTextW(str);
}

void SetType1::OnBnClickedButtonLocalSubmask()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bSubMask = FALSE;
	m_ipaddress_local_setsubmask.GetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x12, m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bSubMask)
	{
		MessageBox(TEXT("�O���ӾW�ڴaʧ����"));
		return;
	}
	m_ipaddress_local_submask.SetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
}

void SetType1::OnBnClickedButtonLocalGateway()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGateWay = FALSE;
	m_ipaddress_local_setgateway.GetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x14, m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGateWay)
	{
		MessageBox(TEXT("�O��Ĭ�J�W�vʧ����"));
		return;
	}
	m_ipaddress_local_gateway.SetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
}

void SetType1::OnBnClickedButtonServerDhcp()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGetServerIpMode = FALSE;
	m_ServerIpMode = 2;
	BYTE cmd[7] = {0xFF, 0xFE, 0x10, m_ServerIpMode, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIpMode)
	{
		MessageBox(TEXT("�O���ӑBServer IPʧ����"));
		return;
	}
	m_edit_server_dhcp.SetWindowTextW(TEXT("�ӑBIPģʽ"));
}

void SetType1::OnBnClickedButtonServerStaticip()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGetServerIpMode = FALSE;
	m_ServerIpMode = 1;
	BYTE cmd[7] = {0xFF, 0xFE, 0x10, m_ServerIpMode, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIpMode)
	{
		MessageBox(TEXT("�O���̶�Server IPʧ����"));
		return;
	}
	m_edit_server_dhcp.SetWindowTextW(TEXT("�o�BIPģʽ"));
}

void SetType1::OnBnClickedButtonServerSetip()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGetServerIp = FALSE;
	m_ipaddress_server_setip.GetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x09, m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIp)
	{
		MessageBox(TEXT("�O��Server IP��ַʧ����"));
		return;
	}
	m_ipaddress_server_ip.SetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
}

void SetType1::OnBnClickedButtonServerSetport()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	DWORD write;
	m_bGetServerPort = FALSE;
	UpdateData(TRUE);
	if(m_edit_server_setport <= 1024 || m_edit_server_setport > 65535)
	{
		MessageBox(TEXT("Ոݔ��1025 - 65535֮�g�Ĕ��֣�"));
		return;
	}
	m_ServerPort = m_edit_server_setport;
	BYTE cmd[7] = {0xFF, 0xFE, 0x0B, m_ServerPort>>8, m_ServerPort&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerPort)
	{
		MessageBox(TEXT("�O��Server�˿�̖ʧ����"));
		return;
	}
	CString str;
	str.Format(TEXT("%d"), m_ServerPort);
	m_edit_server_port.SetWindowTextW(str);
}
