// SetServer.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "SetServer.h"

BYTE CMD_GETDEVICE[] = {0xFF, 0xFE, 0x00, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETVERSION[] = {0xFF, 0xFE, 0x01, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETDHCP[] = {0xFF, 0xFE, 0x02, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALIP[] = {0xFF, 0xFE, 0x04, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALPORT[] = {0xFF, 0xFE, 0x06, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSERVERIP[] = {0xFF, 0xFE, 0x08, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSERVERPORT[] = {0xFF, 0xFE, 0x0A, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALMAC[] = {0xFF, 0xFE, 0x0C, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETLOCALNAME[] = {0xFF, 0xFE, 0x0D, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSERVERIPMODE[] = {0xFF, 0xFE, 0x0F, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSUBMASK[] = {0xFF, 0xFE, 0x11, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETGATEWAY[] = {0xFF, 0xFE, 0x13, 0x00, 0x00, 0xFD, 0xFC};
BYTE CMD_GETSENDNAMETIME[] = {0xFF, 0xFE, 0x15, 0x00, 0x00, 0xFD, 0xFC};

//串口讀線程函數
DWORD ComReadThread_SetServer(LPVOID lparam)
{	
	DWORD	actualReadLen=0;	//實際讀取的字節數
	DWORD	willReadLen;	
	
	DWORD dwReadErrors;
	COMSTAT	cmState;
	
	SetServer *pdlg;
	pdlg = (SetServer *)lparam;

	pdlg->m_Receive_Data_Len = 0;
		
	// 清空緩沖，并檢查串口是否打開。
	ASSERT(pdlg->m_hcom != INVALID_HANDLE_VALUE); 	
	//清空串口
	PurgeComm(pdlg->m_hcom, PURGE_RXCLEAR | PURGE_TXCLEAR );	
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

void SetServer::ParseData(void)
{
	int start;
	while(m_Receive_Data_Len >= 7)
	{
		//查找開始標志FF FF
		start = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, 0xFB, 0xFA);
		if(start >= 0)
		{
			//找到開始標志
			//判斷從開始標志開始，是否有接收一個最小包的長度
			if(start + 7 <= m_Receive_Data_Len)
			{				
				//把數據抓取出來
				switch(m_Receive_Data_Char[start+2])
				{
				case 0x80:	//模塊類型返回 7Byte
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						if(m_Receive_Data_Char[start+3] == 0x04 && m_Receive_Data_Char[start+4] == 0x00)
							m_bGetDevice = TRUE;
					//處理完成，丟掉處理過的數據
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					
					break;
				case 0x81:	//獲取固件版本號的返回，10byte
					if(start + 10 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+8] == 0xF9 && m_Receive_Data_Char[start+9] == 0xF8)
						{
							m_bGetVersion = TRUE;
							m_StrVersion.Format(TEXT("%d%d-%d-%d-%d"), m_Receive_Data_Char[start+3], m_Receive_Data_Char[start+4], m_Receive_Data_Char[start+5], m_Receive_Data_Char[start+6], m_Receive_Data_Char[start+7]);
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 10;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+10, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}					
					break;
				case 0x82:	//讀取DHCP的返回, 7byte
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						m_bDHCP = TRUE;
						m_dhcpvalue = m_Receive_Data_Char[start+3];
					}
					//處理完成，丟掉處理過的數據
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x83:	//設置DHCP的返回, 7byte
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_dhcpvalue == m_Receive_Data_Char[start+3])
							m_bDHCP = TRUE;						
					}
					//處理完成，丟掉處理過的數據
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					break;
				case 0x84:	//讀取自身IP的返回，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bGetLocalIp = TRUE;
							memcpy(m_LocalIp, m_Receive_Data_Char+start+3, 4);
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x85:	//設置自身IP的返回，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_LocalIp, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGetLocalIp = TRUE;
						}
						//處理完成，丟掉處理過的數據
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
					//處理完成，丟掉處理過的數據
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x87:	//設置模組端口號的返回
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_LocalPort == ((m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4]))
							m_bGetLocalPort = TRUE;						
					}
					//處理完成，丟掉處理過的數據
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
						//處理完成，丟掉處理過的數據
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
						//處理完成，丟掉處理過的數據
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
					//處理完成，丟掉處理過的數據
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
										
					break;
				case 0x8B:	//設置Server端口號的返回
					if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
					{
						if(m_ServerPort == ((m_Receive_Data_Char[start+3]<< 8) | m_Receive_Data_Char[start+4]))
							m_bGetServerPort = TRUE;						
					}
					//處理完成，丟掉處理過的數據
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
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 11;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+11, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x8D:	//讀取本地名稱返回，19Byte
					if(start + 19 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+17] == 0xF9 && m_Receive_Data_Char[start+18] == 0xF8)
						{
							m_bGetLocalName = TRUE;
							memset(m_LocalName, 0, sizeof(m_LocalName));
							memcpy(m_LocalName, m_Receive_Data_Char+start+3, 14);
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 19;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+19, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x8E:	//設置本地名稱返回，19Byte
					if(start + 19 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+17] == 0xF9 && m_Receive_Data_Char[start+18] == 0xF8)
						{
							if(memcmp(m_LocalName, m_Receive_Data_Char+start+3, 14) == 0)
								m_bGetLocalName = TRUE;							
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 19;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+19, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x8F:	//讀取Server IP 模式，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetServerIpMode = TRUE;		
							m_ServerIpMode = m_Receive_Data_Char[start+3];
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x90:	//設置Server IP 模式，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(m_Receive_Data_Char[start+3] == m_ServerIpMode)
								m_bGetServerIpMode = TRUE;							
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x91:	//讀取子網掩碼，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bSubMask = TRUE;
							memcpy(m_SubMask, m_Receive_Data_Char+start+3, 4);
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x92:	//設置子網掩碼，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_SubMask, m_Receive_Data_Char+start+3, 4) == 0)
								m_bSubMask = TRUE;
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}
					break;
				case 0x93:	//讀取默認網關，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							m_bGateWay = TRUE;
							memcpy(m_GateWay, m_Receive_Data_Char+start+3, 4);
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x94:	//設置默認網關，9Byte
					if(start + 9 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+7] == 0xF9 && m_Receive_Data_Char[start+8] == 0xF8)
						{
							if(memcmp(m_GateWay, m_Receive_Data_Char+start+3, 4) == 0)
								m_bGateWay = TRUE;
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 9;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+9, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}
					break;
				case 0x95:	//讀取自動上報名稱的時間，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							m_bGetSendNameTime = TRUE;		
							m_SendNameTime = (m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4];
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
					}
					else
					{
						//數據為收完
						return;
					}	
					break;
				case 0x96:	//設置自動上報名稱的時間，7byte
					if(start + 7 <= m_Receive_Data_Len)
					{
						if(m_Receive_Data_Char[start+5] == 0xF9 && m_Receive_Data_Char[start+6] == 0xF8)
						{
							if(((m_Receive_Data_Char[start+3] << 8) | m_Receive_Data_Char[start+4]) == m_SendNameTime)
								m_bGetSendNameTime = TRUE;							
						}
						//處理完成，丟掉處理過的數據
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
					//丟掉頭
					m_Receive_Data_Len = m_Receive_Data_Len - start - 2;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+2, m_Receive_Data_Len);					
					break;
				}
								
			}
			else
			{
				//接收到的不滿一個包，等待收滿再處理
				if(start > 0)
				{
					//在數據包頭前，還有數據，把這些數據清除掉
					m_Receive_Data_Len = m_Receive_Data_Len - start;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start, m_Receive_Data_Len);
				}
				break;
			}
		}
		else
		{
			//沒有找到開始標志
			if(m_Receive_Data_Char[m_Receive_Data_Len-1] == 0xFB)
			{
				//最后一個字符是0xFB，那么可能是下一個數據包的開頭，保留最后一個字符，其它丟掉
				m_Receive_Data_Char[0] = 0xFB;
				m_Receive_Data_Len = 1;
			}
			else
			{
				//最后一個字符不是0xFB，那么全部丟掉
				m_Receive_Data_Len = 0;
			}
			break;
		}
	}
}

int SetServer::FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2)
{
	for(int i=start; i<end-1; i++)
	{
		if(str[i] == c1 && str[i+1] == c2)
			return i;
	}
	return -1;
}


// SetServer dialog

IMPLEMENT_DYNAMIC(SetServer, CDialog)

SetServer::SetServer(CWnd* pParent /*=NULL*/)
	: CDialog(SetServer::IDD, pParent)
	, m_port_local_set(50000)
	, m_port_server_set(51234)
	, m_sendname_time_set(600)
{

}

SetServer::~SetServer()
{
}

void SetServer::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_CONNECT, m_button_connect);
	DDX_Control(pDX, IDC_COMBO_COMPORT, m_combo_comport);
	DDX_Control(pDX, IDC_CHECK, m_check_manual);
	DDX_Control(pDX, IDC_EDIT_VERSION, m_edit_version);
	DDX_Control(pDX, IDC_EDIT_DHCP, m_edit_dhcp);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_SHOW, m_ip_local_show);
	DDX_Control(pDX, IDC_EDIT_LOCAL_PORT_SHOW, m_edit_local_port_show);
	DDX_Control(pDX, IDC_EDIT_LOCAL_MAC, m_edit_local_mac_show);
	DDX_Control(pDX, IDC_IPADDRESS_SERVER_IP_SHOW, m_ip_server_show);
	DDX_Control(pDX, IDC_EDIT_SERVER_PORT_SHOW, m_edit_server_port_show);
	DDX_Control(pDX, IDC_BUTTON_DHCP, m_btn_set_dhcp);
	DDX_Control(pDX, IDC_BUTTON_STATIC_IP, m_btn_set_static_ip);
	DDX_Control(pDX, IDC_BUTTON_SET_LOCAL_IP, m_btn_set_local_ip);
	DDX_Control(pDX, IDC_BUTTON_SET_LOCAL_PORT, m_btn_set_local_port);
	DDX_Control(pDX, IDC_BUTTON_SERVER_SET_IP, m_btn_set_server_ip);
	DDX_Control(pDX, IDC_BUTTON_SET_SERVER_PORT, m_btn_set_server_port);
	DDX_Control(pDX, IDC_IPADDRESS_LOCAL_SET, m_ip_local_set);
	DDX_Text(pDX, IDC_EDIT_LOCAL_PORT_SET, m_port_local_set);
	DDX_Control(pDX, IDC_IPADDRESS_SERVER_IP_SET, m_ip_server_set);
	DDX_Text(pDX, IDC_EDIT_SERVER_PORT_SET, m_port_server_set);	
	DDX_Control(pDX, IDC_EDIT_LOCAL_NAME, m_edit_local_name);
	DDX_Control(pDX, IDC_BUTTON_SET_LOCAL_NAME, m_btn_set_local_name);
	DDX_Control(pDX, IDC_EDIT_LOCAL_NAME_SET, m_edit_local_name_set);
	DDX_Control(pDX, IDC_EDIT_LOCAL_SENDNAME_TIME_SHOW, m_edit_sendname_time_show);
	DDX_Control(pDX, IDC_BUTTON_SET_SENDNAME_TIME, m_btn_set_sendname);
	DDX_Control(pDX, IDC_EDIT_SENDNAME_TIME_SET, m_edit_sendname_time_set);
	DDX_Control(pDX, IDC_IPADDRESS_SUBMASK_SHOW, m_ipaddr_submask_show);
	DDX_Control(pDX, IDC_BUTTON_SET_SUBMUSK, m_btn_set_submask);
	DDX_Control(pDX, IDC_IPADDRESS_SUBMASK_SET, m_ipaddr_submask_set);
	DDX_Control(pDX, IDC_IPADDRESS_GATEWAY_SHOW, m_ipaddr_gateway_show);
	DDX_Control(pDX, IDC_BUTTON_SET_GATEWAY, m_btn_set_gateway);
	DDX_Control(pDX, IDC_IPADDRESS_GATEWAY_SET, m_ipaddr_gateway_set);
	DDX_Control(pDX, IDC_EDIT_SERVER_IPMODE_SHOW, m_edit_server_ipmode_show);
	DDX_Control(pDX, IDC_BUTTON_SET_SERVER_IP_DHCP, m_btn_set_serverip_dhcp);
	DDX_Control(pDX, IDC_BUTTON_SERVER_IP_STATIC, m_btn_server_ip_static);
	DDX_Text(pDX, IDC_EDIT_SENDNAME_TIME_SET, m_sendname_time_set);
}


BEGIN_MESSAGE_MAP(SetServer, CDialog)
	ON_BN_CLICKED(IDC_CHECK, &SetServer::OnBnClickedCheck)
	ON_BN_CLICKED(IDC_BUTTON_CONNECT, &SetServer::OnBnClickedButtonConnect)
	ON_BN_CLICKED(IDC_BUTTON_DHCP, &SetServer::OnBnClickedButtonDhcp)
	ON_BN_CLICKED(IDC_BUTTON_STATIC_IP, &SetServer::OnBnClickedButtonStaticIp)
	ON_BN_CLICKED(IDC_BUTTON_SET_LOCAL_IP, &SetServer::OnBnClickedButtonSetLocalIp)
	ON_BN_CLICKED(IDC_BUTTON_SET_LOCAL_PORT, &SetServer::OnBnClickedButtonSetLocalPort)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_SET_IP, &SetServer::OnBnClickedButtonServerSetIp)
	ON_BN_CLICKED(IDC_BUTTON_SET_SERVER_PORT, &SetServer::OnBnClickedButtonSetServerPort)
	ON_BN_CLICKED(IDC_BUTTON_SET_LOCAL_NAME, &SetServer::OnBnClickedButtonSetLocalName)
	ON_BN_CLICKED(IDC_BUTTON_SET_SENDNAME_TIME, &SetServer::OnBnClickedButtonSetSendnameTime)
	ON_BN_CLICKED(IDC_BUTTON_SET_SUBMUSK, &SetServer::OnBnClickedButtonSetSubmusk)
	ON_BN_CLICKED(IDC_BUTTON_SET_GATEWAY, &SetServer::OnBnClickedButtonSetGateway)
	ON_BN_CLICKED(IDC_BUTTON_SET_SERVER_IP_DHCP, &SetServer::OnBnClickedButtonSetServerIpDhcp)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_IP_STATIC, &SetServer::OnBnClickedButtonServerIpStatic)
END_MESSAGE_MAP()


// SetServer message handlers

BOOL SetServer::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	m_button_connect.SetWindowTextW(TEXT("自動連接設備"));
	m_check_manual.SetWindowTextW(TEXT("手動選擇串列埠"));
	m_check_manual.SetCheck(0);

	m_ip_local_set.SetAddress(192, 168, 2, 101);
	m_ip_server_set.SetAddress(192, 168, 2, 250);
	m_ipaddr_submask_set.SetAddress(255, 255, 255, 0);
	m_ipaddr_gateway_set.SetAddress(192, 168, 2, 1);

	m_bConnect = FALSE;
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void SetServer::OnBnClickedCheck()
{
	// TODO: Add your control notification handler code here
	if(m_check_manual.GetCheck() == 1)
	{
		m_combo_comport.EnableWindow(TRUE);
		m_button_connect.SetWindowTextW(TEXT("手動連接設備"));
	}
	else
	{
		m_combo_comport.EnableWindow(FALSE);
		m_combo_comport.SetWindowTextW(TEXT(""));
		m_button_connect.SetWindowTextW(TEXT("自動連接設備"));
	}
}

BOOL SetServer::OpenCom(CString str_com)
{	
	str_com = TEXT("\\\\.\\") + str_com;
	m_hcom = CreateFile(str_com, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
	///配置串口
	if(m_hcom != INVALID_HANDLE_VALUE && m_hcom != NULL)
	{		
		DCB  dcb;    
		dcb.DCBlength = sizeof(DCB); 
		// 默認串口參數
		GetCommState(m_hcom, &dcb);

		dcb.BaudRate = 115200;					// 設置波特率 
		dcb.fBinary = TRUE;						// 設置二進制模式，此處必須設置TRUE
		dcb.fParity = TRUE;						// 支持奇偶校驗 
		dcb.ByteSize = 8;						// 數據位,范圍:4-8 
		dcb.Parity = NOPARITY;					// 校驗模式
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
		dcb.fAbortOnError = FALSE;			// 當串口發生錯誤，并不終止串口讀寫


		if (!SetCommState(m_hcom, &dcb))
		{
			///L"配置串口失敗";			
			return FALSE;
		}
		////配置超時值
		COMMTIMEOUTS  cto;
		GetCommTimeouts(m_hcom, &cto);
		cto.ReadIntervalTimeout = MAXDWORD;  
		cto.ReadTotalTimeoutMultiplier = 10;  
		cto.ReadTotalTimeoutConstant = 10;    
		cto.WriteTotalTimeoutMultiplier = 50;  
		cto.WriteTotalTimeoutConstant = 100;    
		if (!SetCommTimeouts(m_hcom, &cto))
		{
			///L"不能設置超時參數";		
			return FALSE;
		}	

		//指定端口監測的事件集
		SetCommMask (m_hcom, EV_RXCHAR);
		
		//分配設備緩沖區
	//	SetupComm(m_hcom,8192,8192);

		//初始化緩沖區中的信息
		PurgeComm(m_hcom,PURGE_TXCLEAR|PURGE_RXCLEAR);
	}
	else
	{		
		return FALSE;
	}

	HANDLE m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ComReadThread_SetServer, this, 0, NULL);
	CloseHandle(m_hThread);

	return TRUE;
}

void SetServer::GetInfo()
{
	DWORD write;

	m_bGetVersion = FALSE;
	WriteFile(m_hcom, CMD_GETVERSION, sizeof(CMD_GETVERSION), &write, NULL);
	Sleep(100);
	if(m_bGetVersion)
	{
		m_edit_version.SetWindowTextW(m_StrVersion);
	}
	else
		MessageBox(TEXT("獲取固件版本號失敗！"));

	m_bDHCP = FALSE;
	WriteFile(m_hcom, CMD_GETDHCP, sizeof(CMD_GETDHCP), &write, NULL);
	Sleep(100);
	if(m_bDHCP)
	{
		if(m_dhcpvalue == 1)
			m_edit_dhcp.SetWindowTextW(TEXT("使用靜態IP功能"));
		else
			m_edit_dhcp.SetWindowTextW(TEXT("使用DHCP功能"));
	}
	else
		MessageBox(TEXT("獲取DHCP狀態失敗！"));

	m_bGetLocalIp = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALIP, sizeof(CMD_GETLOCALIP), &write, NULL);
	Sleep(100);
	if(m_bGetLocalIp)
	{
		m_ip_local_show.SetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
	}
	else
		MessageBox(TEXT("獲取本機IP地址失敗！"));

	m_bGetLocalPort = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALPORT, sizeof(CMD_GETLOCALPORT), &write, NULL);
	Sleep(100);
	if(m_bGetLocalPort)
	{
		CString str;
		str.Format(TEXT("%d"), m_LocalPort);
		m_edit_local_port_show.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取本機端口號失敗！"));

	m_bGetServerIp = FALSE;
	WriteFile(m_hcom, CMD_GETSERVERIP, sizeof(CMD_GETSERVERIP), &write, NULL);
	Sleep(100);
	if(m_bGetServerIp)
	{
		m_ip_server_show.SetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
	}
	else
		MessageBox(TEXT("獲取Server IP地址失敗！"));

	m_bGetServerPort = FALSE;
	WriteFile(m_hcom, CMD_GETSERVERPORT, sizeof(CMD_GETSERVERPORT), &write, NULL);
	Sleep(100);
	if(m_bGetServerPort)
	{
		CString str;
		str.Format(TEXT("%d"), m_ServerPort);
		m_edit_server_port_show.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取Server端口號失敗！"));

	m_bGetLocalMac = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALMAC, sizeof(CMD_GETLOCALMAC), &write, NULL);
	Sleep(100);
	if(m_bGetLocalMac)
	{
		CString str;
		str.Format(TEXT("%02X:%02X:%02X:%02X:%02X:%02X"), m_LocalMac[0], m_LocalMac[1], m_LocalMac[2], m_LocalMac[3], m_LocalMac[4], m_LocalMac[5]);
		m_edit_local_mac_show.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取Server端口號失敗！"));

	m_bGetLocalName = FALSE;
	WriteFile(m_hcom, CMD_GETLOCALNAME, sizeof(CMD_GETLOCALNAME), &write, NULL);
	Sleep(100);
	if(m_bGetLocalName)
	{
		CString str;
		wchar_t wt[14];
		MultiByteToWideChar(936, 0, (char *)m_LocalName, 14, wt, 14);
		str = wt;
		m_edit_local_name.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取本機名稱失敗！"));

	m_bGetServerIpMode = FALSE;
	WriteFile(m_hcom, CMD_GETSERVERIPMODE, sizeof(CMD_GETSERVERIPMODE), &write, NULL);
	Sleep(100);
	if(m_bGetServerIpMode)
	{
		CString str;
		if(m_ServerIpMode == 1)
			str = TEXT("固定IP模式");
		else if(m_ServerIpMode == 2)
			str = TEXT("動態IP模式");
		else
			str = TEXT("未知的IP模式");

		m_edit_server_ipmode_show.SetWindowTextW(str);
	}
	else
		MessageBox(TEXT("獲取Server IP模式失敗！"));

	m_bSubMask = FALSE;
	WriteFile(m_hcom, CMD_GETSUBMASK, sizeof(CMD_GETSUBMASK), &write, NULL);
	Sleep(100);
	if(m_bSubMask)
	{
		m_ipaddr_submask_show.SetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
	}
	else
		MessageBox(TEXT("獲取子網掩碼失敗！"));

	m_bGateWay = FALSE;
	WriteFile(m_hcom, CMD_GETGATEWAY, sizeof(CMD_GETGATEWAY), &write, NULL);
	Sleep(100);
	if(m_bGateWay)
	{
		m_ipaddr_gateway_show.SetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
	}
	else
		MessageBox(TEXT("獲取默認網関失敗！"));

	m_bGetSendNameTime = FALSE;
	WriteFile(m_hcom, CMD_GETSENDNAMETIME, sizeof(CMD_GETSENDNAMETIME), &write, NULL);
	Sleep(100);
	if(m_bGetSendNameTime)
	{
		CString str;
		if(m_SendNameTime == 0)
			str.Format(TEXT("不上報名稱"));
		else
			str.Format(TEXT("%d 秒"), m_SendNameTime);
		m_edit_sendname_time_show.SetWindowTextW(str);	
	}
	else
		MessageBox(TEXT("獲取上報名稱時間失敗！"));

}

void SetServer::OnBnClickedButtonConnect()
{
	// TODO: Add your control notification handler code here
	if(m_bConnect)
	{
		m_bConnect = FALSE;
		OnBnClickedCheck();
		m_check_manual.EnableWindow(TRUE);
		m_btn_set_dhcp.EnableWindow(FALSE);
		m_btn_set_static_ip.EnableWindow(FALSE);
		m_btn_set_local_ip.EnableWindow(FALSE);
		m_btn_set_local_port.EnableWindow(FALSE);
		m_btn_set_server_ip.EnableWindow(FALSE);
		m_btn_set_server_port.EnableWindow(FALSE);
		m_btn_set_local_name.EnableWindow(FALSE);
		m_btn_set_sendname.EnableWindow(FALSE);
		m_btn_set_submask.EnableWindow(FALSE);
		m_btn_set_gateway.EnableWindow(FALSE);
		m_btn_set_serverip_dhcp.EnableWindow(FALSE);
		m_btn_server_ip_static.EnableWindow(FALSE);

		m_edit_version.SetWindowTextW(TEXT(""));
		m_edit_dhcp.SetWindowTextW(TEXT(""));
		m_ip_local_show.SetWindowTextW(TEXT(""));
		m_edit_local_port_show.SetWindowTextW(TEXT(""));
		m_edit_local_mac_show.SetWindowTextW(TEXT(""));
		m_ip_server_show.SetWindowTextW(TEXT(""));
		m_edit_server_port_show.SetWindowTextW(TEXT(""));
		m_edit_local_name.SetWindowTextW(TEXT(""));
		m_edit_sendname_time_show.SetWindowTextW(TEXT(""));
		m_ipaddr_submask_show.SetWindowTextW(TEXT(""));
		m_ipaddr_gateway_show.SetWindowTextW(TEXT(""));
		m_edit_server_ipmode_show.SetWindowTextW(TEXT(""));
		
		CloseHandle(m_hcom);
		m_hcom = NULL;
	}
	else
	{
		CString str;
		if(m_check_manual.GetCheck() == 1)
		{
			m_combo_comport.GetWindowTextW(str);
			if(!OpenCom(str))
			{
				MessageBox(TEXT("Open ")+str + TEXT(" Fail!"));
				return;
			}
			m_bGetDevice = FALSE;
			DWORD write;
			
			WriteFile(m_hcom, CMD_GETDEVICE, sizeof(CMD_GETDEVICE), &write, NULL);
			Sleep(200);
			if(m_bGetDevice)
			{
				//連接到設備
				m_bConnect = TRUE;
				m_button_connect.SetWindowTextW(TEXT("斷開連接"));
				m_check_manual.EnableWindow(FALSE);

				m_btn_set_dhcp.EnableWindow(TRUE);
				m_btn_set_static_ip.EnableWindow(TRUE);
				m_btn_set_local_ip.EnableWindow(TRUE);
				m_btn_set_local_port.EnableWindow(TRUE);
				m_btn_set_server_ip.EnableWindow(TRUE);
				m_btn_set_server_port.EnableWindow(TRUE);
				m_btn_set_local_name.EnableWindow(TRUE);
				m_btn_set_sendname.EnableWindow(TRUE);
				m_btn_set_submask.EnableWindow(TRUE);
				m_btn_set_gateway.EnableWindow(TRUE);
				m_btn_set_serverip_dhcp.EnableWindow(TRUE);
				m_btn_server_ip_static.EnableWindow(TRUE);
				m_combo_comport.SetWindowTextW(str);

				GetInfo();
				return;
			}
			CloseHandle(m_hcom);
			m_hcom = NULL;
			Sleep(50);

		}
		else
		{
			for(int i=0; i<100; i++)
			{
				str.Format(TEXT("COM%d"), i);
				if(OpenCom(str))
				{
					m_bGetDevice = FALSE;
					DWORD write;					
					WriteFile(m_hcom, CMD_GETDEVICE, sizeof(CMD_GETDEVICE), &write, NULL);
					Sleep(200);
					if(m_bGetDevice)
					{
						//連接到設備
						m_bConnect = TRUE;
						m_button_connect.SetWindowTextW(TEXT("斷開連接"));
						m_check_manual.EnableWindow(FALSE);
						m_btn_set_dhcp.EnableWindow(TRUE);
						m_btn_set_static_ip.EnableWindow(TRUE);
						m_btn_set_local_ip.EnableWindow(TRUE);
						m_btn_set_local_port.EnableWindow(TRUE);
						m_btn_set_server_ip.EnableWindow(TRUE);
						m_btn_set_server_port.EnableWindow(TRUE);
						m_btn_set_local_name.EnableWindow(TRUE);
						m_btn_set_sendname.EnableWindow(TRUE);
						m_btn_set_submask.EnableWindow(TRUE);
						m_btn_set_gateway.EnableWindow(TRUE);
						m_btn_set_serverip_dhcp.EnableWindow(TRUE);
						m_btn_server_ip_static.EnableWindow(TRUE);
						m_combo_comport.SetWindowTextW(str);

						GetInfo();
						return;
					}
					CloseHandle(m_hcom);
					m_hcom = NULL;
					Sleep(50);
				}
			}
		}
		MessageBox(TEXT("連接設備失敗！"));
	}
}

void SetServer::OnBnClickedButtonDhcp()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bDHCP = FALSE;
	m_dhcpvalue = 2;
	BYTE cmd[7] = {0xFF, 0xFE, 0x03, m_dhcpvalue, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bDHCP)
	{
		MessageBox(TEXT("設定DHCP失敗！"));
		return;
	}
	m_edit_dhcp.SetWindowTextW(TEXT("使用DHCP功能"));
}

void SetServer::OnBnClickedButtonStaticIp()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bDHCP = FALSE;
	m_dhcpvalue = 1;
	BYTE cmd[7] = {0xFF, 0xFE, 0x03, m_dhcpvalue, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bDHCP)
	{
		MessageBox(TEXT("設定靜態IP失敗！"));
		return;
	}
	m_edit_dhcp.SetWindowTextW(TEXT("使用靜態IP功能"));
}

void SetServer::OnBnClickedButtonSetLocalIp()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGetLocalIp = FALSE;
	m_ip_local_set.GetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x05, m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetLocalIp)
	{
		MessageBox(TEXT("設定本機IP地址失敗！"));
		return;
	}
	m_ip_local_show.SetAddress(m_LocalIp[0], m_LocalIp[1], m_LocalIp[2], m_LocalIp[3]);
}

void SetServer::OnBnClickedButtonSetLocalPort()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGetLocalPort = FALSE;
	UpdateData(TRUE);
	if(m_port_local_set <= 1024 || m_port_local_set > 65535)
	{
		MessageBox(TEXT("請輸入1025 - 65535之間的數字！"));
		return;
	}
	m_LocalPort = m_port_local_set;
	BYTE cmd[7] = {0xFF, 0xFE, 0x07, m_LocalPort>>8, m_LocalPort&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetLocalPort)
	{
		MessageBox(TEXT("設定本機端口號失敗！"));
		return;
	}
	CString str;
	str.Format(TEXT("%d"), m_LocalPort);
	m_edit_local_port_show.SetWindowTextW(str);
}

void SetServer::OnBnClickedButtonServerSetIp()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGetServerIp = FALSE;
	m_ip_server_set.GetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x09, m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIp)
	{
		MessageBox(TEXT("設定Server IP地址失敗！"));
		return;
	}
	m_ip_server_show.SetAddress(m_ServerIp[0], m_ServerIp[1], m_ServerIp[2], m_ServerIp[3]);
}

void SetServer::OnBnClickedButtonSetServerPort()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGetServerPort = FALSE;
	UpdateData(TRUE);
	if(m_port_server_set <= 1024 || m_port_server_set > 65535)
	{
		MessageBox(TEXT("請輸入1025 - 65535之間的數字！"));
		return;
	}
	m_ServerPort = m_port_server_set;
	BYTE cmd[7] = {0xFF, 0xFE, 0x0B, m_ServerPort>>8, m_ServerPort&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerPort)
	{
		MessageBox(TEXT("設定Server端口號失敗！"));
		return;
	}
	CString str;
	str.Format(TEXT("%d"), m_ServerPort);
	m_edit_server_port_show.SetWindowTextW(str);
}

void SetServer::OnBnClickedButtonSetLocalName()
{
	// TODO: Add your control notification handler code here
	CString str;
	m_edit_local_name_set.GetWindowTextW(str);
	if(str.IsEmpty())
		return;
	int len = WideCharToMultiByte(936, 0, str, str.GetLength(), NULL, 0, NULL, NULL);
	if(len > 14)
	{
		MessageBox(TEXT("名稱長度太長！"));
		return;
	}
	memset(m_LocalName, 0, sizeof(m_LocalName));
	WideCharToMultiByte(936, 0, str, str.GetLength(), (char*)m_LocalName, len, NULL, NULL);

	DWORD write;
	m_bGetLocalName = FALSE;	
	BYTE cmd[19];
	cmd[0] = 0xFF; cmd[1] = 0xFE;
	cmd[2] = 0x0E;
	cmd[17] = 0xFD; cmd[18] = 0xFC;
	memcpy(cmd+3, m_LocalName, 14);
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetLocalName)
	{
		MessageBox(TEXT("設定本機名稱失敗！"));
		return;
	}	
	m_edit_local_name.SetWindowTextW(str);
}

void SetServer::OnBnClickedButtonSetSendnameTime()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGetSendNameTime = FALSE;
	UpdateData(TRUE);
	if(m_sendname_time_set > 65535)
	{
		MessageBox(TEXT("請輸入0 - 65535之間的數字！"));
		return;
	}
	m_SendNameTime = m_sendname_time_set;
	BYTE cmd[7] = {0xFF, 0xFE, 0x16, m_SendNameTime>>8, m_SendNameTime&0xFF, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
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
	m_edit_sendname_time_show.SetWindowTextW(str);
}

void SetServer::OnBnClickedButtonSetSubmusk()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bSubMask = FALSE;
	m_ipaddr_submask_set.GetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x12, m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bSubMask)
	{
		MessageBox(TEXT("設定子網掩碼失敗！"));
		return;
	}
	m_ipaddr_submask_show.SetAddress(m_SubMask[0], m_SubMask[1], m_SubMask[2], m_SubMask[3]);
}

void SetServer::OnBnClickedButtonSetGateway()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGateWay = FALSE;
	m_ipaddr_gateway_set.GetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
	BYTE cmd[9] = {0xFF, 0xFE, 0x14, m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3], 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGateWay)
	{
		MessageBox(TEXT("設定默認網関失敗！"));
		return;
	}
	m_ipaddr_gateway_show.SetAddress(m_GateWay[0], m_GateWay[1], m_GateWay[2], m_GateWay[3]);
}

void SetServer::OnBnClickedButtonSetServerIpDhcp()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGetServerIpMode = FALSE;
	m_ServerIpMode = 2;
	BYTE cmd[7] = {0xFF, 0xFE, 0x10, m_ServerIpMode, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIpMode)
	{
		MessageBox(TEXT("設定動態Server IP失敗！"));
		return;
	}
	m_edit_server_ipmode_show.SetWindowTextW(TEXT("動態IP模式"));
}

void SetServer::OnBnClickedButtonServerIpStatic()
{
	// TODO: Add your control notification handler code here
	DWORD write;
	m_bGetServerIpMode = FALSE;
	m_ServerIpMode = 1;
	BYTE cmd[7] = {0xFF, 0xFE, 0x10, m_ServerIpMode, 0x00, 0xFD, 0xFC};
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(100);
	if(!m_bGetServerIpMode)
	{
		MessageBox(TEXT("設定固定Server IP失敗！"));
		return;
	}
	m_edit_server_ipmode_show.SetWindowTextW(TEXT("靜態IP模式"));
}
