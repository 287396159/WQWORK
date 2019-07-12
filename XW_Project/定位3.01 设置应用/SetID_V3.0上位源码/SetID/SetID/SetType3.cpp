// SetType3.cpp : 实现文件
//

#include "stdafx.h"
#include "SetID.h"
#include "SetType3.h"


// SetType3 对话框

IMPLEMENT_DYNAMIC(SetType3, CDialog)

SetType3::SetType3(CWnd* pParent /*=NULL*/)
	: CDialog(SetType3::IDD, pParent)
	, m_edit_settime_value(5)
	, m_edit_server_settime(10)
	, m_edit_router_settime(10)
{

}

SetType3::~SetType3()
{
}

void SetType3::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_CONNECT, m_button_connect);
	DDX_Control(pDX, IDC_TAG_GETID, m_tag_getid);
	DDX_Control(pDX, IDC_TAG_SETID, m_tag_setid);
	DDX_Control(pDX, IDC_BUTTON_GETTIME, m_tag_gettime);
	DDX_Control(pDX, IDC_BUTTON_SETTIME, m_tag_settime);
	DDX_Control(pDX, IDC_SERVER_GETID, m_server_getid);
	DDX_Control(pDX, IDC_SERVER_SETID, m_server_setid);
	DDX_Control(pDX, IDC_ROUTER_GETID, m_router_getid);
	DDX_Control(pDX, IDC_ROUTER_SETID, m_router_setid);
	DDX_Control(pDX, IDC_EDIT_TAG_GETID1, m_edit_tag_getid1);
	DDX_Control(pDX, IDC_EDIT_TAG_GETID2, m_edit_tag_getid2);
	DDX_Control(pDX, IDC_EDIT_TAG_SETID1, m_edit_tag_setid1);
	DDX_Control(pDX, IDC_EDIT_TAG_SETID2, m_edit_tag_setid2);
	DDX_Control(pDX, IDC_EDIT_TAG_GETTIME, m_edit_tag_gettime);
	DDX_Control(pDX, IDC_EDIT_SERVER_GETID1, m_edit_server_getid1);
	DDX_Control(pDX, IDC_EDIT_SERVER_GETID2, m_edit_server_getid2);
	DDX_Control(pDX, IDC_EDIT_SERVER_SETID1, m_edit_server_setid1);
	DDX_Control(pDX, IDC_EDIT_SERVER_SETID2, m_edit_server_setid2);
	DDX_Control(pDX, IDC_EDIT_ROUTER_GETID1, m_edit_router_getid1);
	DDX_Control(pDX, IDC_EDIT_ROUTER_GETID2, m_edit_router_getid2);
	DDX_Control(pDX, IDC_EDIT_ROUTER_SETID1, m_edit_router_setid1);
	DDX_Control(pDX, IDC_EDIT_ROUTER_SETID2, m_edit_router_setid2);
	DDX_Control(pDX, IDC_COMBO_THREE, m_combo_three);
	DDX_Text(pDX, IDC_EDIT_TAG_SETTIME, m_edit_settime_value);
	DDX_Control(pDX, IDC_BUTTON_SERVER_GETTIME, m_button_server_gettime);
	DDX_Control(pDX, IDC_BUTTON_SERVER_SETTIME, m_button_server_settime);
	DDX_Control(pDX, IDC_BUTTON_ROUTER_GETTIME, m_button_router_gettime);
	DDX_Control(pDX, IDC_BUTTON_ROUTER_SETTIME, m_button_router_settime);
	DDX_Control(pDX, IDC_EDIT_ROUTER_GETTIME, m_edit_router_gettime);
	DDX_Control(pDX, IDC_EDIT_SERVER_GETTIME, m_edit_server_gettime);
	DDX_Text(pDX, IDC_EDIT_SERVER_SETTIME, m_edit_server_settime);
	DDX_Text(pDX, IDC_EDIT_ROUTER_SETTIME, m_edit_router_settime);
}


BEGIN_MESSAGE_MAP(SetType3, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_CONNECT, &SetType3::OnBnClickedButtonConnect)
	ON_BN_CLICKED(IDC_TAG_GETID, &SetType3::OnBnClickedTagGetid)
	ON_BN_CLICKED(IDC_TAG_SETID, &SetType3::OnBnClickedTagSetid)
	ON_BN_CLICKED(IDC_BUTTON_GETTIME, &SetType3::OnBnClickedButtonGettime)
	ON_BN_CLICKED(IDC_BUTTON_SETTIME, &SetType3::OnBnClickedButtonSettime)
	ON_BN_CLICKED(IDC_SERVER_GETID, &SetType3::OnBnClickedServerGetid)
	ON_BN_CLICKED(IDC_SERVER_SETID, &SetType3::OnBnClickedServerSetid)
	ON_BN_CLICKED(IDC_ROUTER_GETID, &SetType3::OnBnClickedRouterGetid)
	ON_BN_CLICKED(IDC_ROUTER_SETID, &SetType3::OnBnClickedRouterSetid)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_GETTIME, &SetType3::OnBnClickedButtonServerGettime)
	ON_BN_CLICKED(IDC_BUTTON_SERVER_SETTIME, &SetType3::OnBnClickedButtonServerSettime)
	ON_BN_CLICKED(IDC_BUTTON_ROUTER_GETTIME, &SetType3::OnBnClickedButtonRouterGettime)
	ON_BN_CLICKED(IDC_BUTTON_ROUTER_SETTIME, &SetType3::OnBnClickedButtonRouterSettime)
END_MESSAGE_MAP()


//串口读线程函数
DWORD ComReadThread_TAG(LPVOID lparam)
{	
	DWORD	actualReadLen=0;	//实际读取的字节数
	DWORD	willReadLen;	
	
	DWORD dwReadErrors;
	COMSTAT	cmState;
	
	SetType3 *pdlg;
	pdlg = (SetType3 *)lparam;

	pdlg->m_Receive_Data_Len = 0;
		
	// 清空缓冲，并检查串口是否打开。
	ASSERT(pdlg->m_hcom != INVALID_HANDLE_VALUE); 	
	//清空串口
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

void SetType3::ParseData(void)
{
	int start,start1;
	while(m_Receive_Data_Len >= 5)
	{
		//查找开始标志FF FF
		start = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, 0xFB, 0xFA);
		start1 = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, 0xFA);
		if((start >= 0)|(start1 >= 0))
		{
			//找到开始标志
			//判断从开始标志开始，是否有接收一个包的长度
			if(start + 7 <= m_Receive_Data_Len)
			{
				//有接收到一个包的长度，判断包尾格式		
				if(0xF9 == m_Receive_Data_Char[start+5] && 0xF8 == m_Receive_Data_Char[start+6])
				{
					//包尾格式匹配，认为是正确的包
					//把数据抓取出来
					switch(m_Receive_Data_Char[start+2])
					{
					case 0x80:	//模块类型返回
						if(m_Receive_Data_Char[start+3] == 0x02 && m_Receive_Data_Char[start+4] == 0x00)
							m_bGetTag = TRUE;
						break;
					case 0x81:	//设置ID的返回
						if(m_Receive_Data_Char[start+3] == m_Tag_ID[0] && m_Receive_Data_Char[start+4] == m_Tag_ID[1])
							m_bSetTagID = TRUE;
						break;
					case 0x82:	//读取ID的返回
						m_bGetTagID = TRUE;
						m_Tag_ID[0] = m_Receive_Data_Char[start+3];
						m_Tag_ID[1] = m_Receive_Data_Char[start+4];
						break;
					case 0x84:	//读取睡眠时间的返回
						m_bSleepTime = TRUE;
						m_SleepTime = (m_Receive_Data_Char[start+3]<<8) | m_Receive_Data_Char[start+4];
						break;
					case 0x83:	//设置睡眠时间的返回
						if((m_Receive_Data_Char[start+3] <<8) | m_Receive_Data_Char[start+4] == m_edit_settime_value)
							m_bSleepTime = TRUE;
						break;
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start - 7;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+7, m_Receive_Data_Len);
				}
				else
				{
					//包不正确
					//查找下一个开始标志头，
					int start2 = FindChar(m_Receive_Data_Char, start+2, m_Receive_Data_Len, 0xFB, 0xFA);
					if(start2 >= 0)
					{
						//有下一个开始标志头，把这个头前的所有数据丢掉
						m_Receive_Data_Len = m_Receive_Data_Len - start2;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start2, m_Receive_Data_Len);
					}
					else
					{
						//没有找到下一个开始标志
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
			if(start1 + 5 <= m_Receive_Data_Len)
			{
				//有接收到一个包的长度，判断包尾格式		
				if(0xF8 == m_Receive_Data_Char[start1+4])
				{
					//包尾格式匹配，认为是正确的包
					//把数据抓取出来
					switch(m_Receive_Data_Char[start1+1])
					{
					case 0x41:	//模块类型返回
						{
							if(m_Receive_Data_Char[start1+2] == 0x01 && m_Receive_Data_Char[start1+3] == 0x00)
								m_bGetServer = TRUE;
							else if(m_Receive_Data_Char[start1+2] == 0x02 && m_Receive_Data_Char[start1+3] == 0x00)
								m_bGetRouter = TRUE;
						}
						break;
					case 0x81:	//设置ID的返回
						if(m_bGetServer == TRUE)
						{
							if(m_Receive_Data_Char[start1+2] == m_Server_ID[0] && m_Receive_Data_Char[start1+3] == m_Server_ID[1])
							{
								m_bSetServerID = TRUE;
							}
						}
						if(m_bGetRouter == TRUE)
						{
							if(m_Receive_Data_Char[start1+2] == m_Router_ID[0] && m_Receive_Data_Char[start1+3] == m_Router_ID[1])
							{
								m_bSetRouterID = TRUE;
							}
						}
						break;
					case 0x82:	//读取ID的返回
						if(m_bGetServer == TRUE)
						{
							m_bGetServerID = TRUE;
							m_Server_ID[0] = m_Receive_Data_Char[start1+2];
							m_Server_ID[1] = m_Receive_Data_Char[start1+3];
						}		
						if(m_bGetRouter == TRUE)
						{
							m_bGetRouterID = TRUE;
							m_Router_ID[0] = m_Receive_Data_Char[start1+2];
							m_Router_ID[1] = m_Receive_Data_Char[start1+3];
						}
						break;
					case 0x83:	//设置上报时间的返回
						if(m_bGetServer == TRUE)
						{
							if((m_Receive_Data_Char[start1+2] <<8) | m_Receive_Data_Char[start1+3] == m_edit_server_settime)
								m_bInformTime = TRUE;
						}		
						if(m_bGetRouter == TRUE)
						{
							if((m_Receive_Data_Char[start1+2] <<8) | m_Receive_Data_Char[start1+3] == m_edit_router_settime)
								m_bInformTime = TRUE;
						}
						break;
					case 0x84:	//读取上报时间的返回
						if(m_bGetServer == TRUE)
						{
							m_bInformTime = TRUE;
							m_InformTime = (m_Receive_Data_Char[start1+2]<<8) | m_Receive_Data_Char[start1+3];
						}		
						if(m_bGetRouter == TRUE)
						{
							m_bInformTime = TRUE;
							m_InformTime = (m_Receive_Data_Char[start1+2]<<8) | m_Receive_Data_Char[start1+3];
						}
						break;
					}
					//处理完成，丢掉处理过的数据
					m_Receive_Data_Len = m_Receive_Data_Len - start1 - 5;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start1+5, m_Receive_Data_Len);
				}
				else
				{
					//包不正确
					//查找下一个开始标志头，
					int start3 = FindChar(m_Receive_Data_Char, start+1, m_Receive_Data_Len, 0xFA);
					if(start3 >= 0)
					{
						//有下一个开始标志头，把这个头前的所有数据丢掉
						m_Receive_Data_Len = m_Receive_Data_Len - start3;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start3, m_Receive_Data_Len);
					}
					else
					{
						//没有找到下一个开始标志
						if(m_Receive_Data_Char[m_Receive_Data_Len-1] == 0xFA)
						{
							//最后一个字符是0xF8，那么可能是下一个数据包的开头，保留最后一个字符，其它丢掉
							m_Receive_Data_Char[0] = 0xFA;
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
			else
			{
				//接收到的不满一个包，等待收满再处理
				if(start1 > 0)
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
			else if(m_Receive_Data_Char[m_Receive_Data_Len-1] == 0xF8)
			{
				//最后一个字符是0xF8，那么可能是下一个数据包的开头，保留最后一个字符，其它丢掉
				m_Receive_Data_Char[0] = 0xF8;
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

int SetType3::FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2)
{
	for(int i=start; i<end-1; i++)
	{
		if(str[i] == c1 && str[i+1] == c2)
			return i;
	}
	return -1;
}
int SetType3::FindChar(BYTE *str, int start, int end, BYTE c1)
{
	for(int i=start; i<end; i++)
	{
		if(str[i] == c1)
			return i;
	}
	return -1;
}

BOOL SetType3::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	
	m_button_connect.SetWindowTextW(TEXT("自動連接設備"));
	m_combo_three.EnableWindow(FALSE);
	m_combo_three.SetWindowTextW(TEXT(""));
	//m_button_getid.SetWindowTextW(TEXT("獲取ID號"));
	//m_button_setid.SetWindowTextW(TEXT("設置ID號"));
	//m_button_gettime.SetWindowTextW(TEXT("獲取發送間隔時間"));
	//m_button_settime.SetWindowTextW(TEXT("設置發送間隔時間"));
	m_tag_getid.EnableWindow(FALSE);
	m_tag_setid.EnableWindow(FALSE);
	m_tag_gettime.EnableWindow(FALSE);
	m_tag_settime.EnableWindow(FALSE);
	m_server_getid.EnableWindow(FALSE);
	m_server_setid.EnableWindow(FALSE);
	m_router_getid.EnableWindow(FALSE);
	m_router_setid.EnableWindow(FALSE);
	m_button_server_gettime.EnableWindow(FALSE);
	m_button_server_settime.EnableWindow(FALSE);
	m_button_router_gettime.EnableWindow(FALSE);
	m_button_router_settime.EnableWindow(FALSE);

	m_bConnect = FALSE;

	//m_LeftButton_down = FALSE;

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

BOOL SetType3::OpenCom(CString str_com)
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

	HANDLE m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ComReadThread_TAG, this, 0, NULL);
	CloseHandle(m_hThread);

	return TRUE;
}

// SetType3 消息处理程序
void SetType3::OnBnClickedButtonConnect()
{
	// TODO: 在此添加控件通知处理程序代码
	if(m_bConnect)
	{
		m_bConnect = FALSE;
		m_button_connect.SetWindowTextW(TEXT("自動連接設備"));
		m_combo_three.SetWindowTextW(TEXT(""));
		m_tag_getid.EnableWindow(FALSE);
		m_tag_setid.EnableWindow(FALSE);
		m_tag_gettime.EnableWindow(FALSE);
		m_tag_settime.EnableWindow(FALSE);
		m_server_getid.EnableWindow(FALSE);
		m_server_setid.EnableWindow(FALSE);
		m_router_getid.EnableWindow(FALSE);
		m_router_setid.EnableWindow(FALSE);
		m_button_server_gettime.EnableWindow(FALSE);
		m_button_server_settime.EnableWindow(FALSE);
		m_button_router_gettime.EnableWindow(FALSE);
		m_button_router_settime.EnableWindow(FALSE);
		
		m_edit_tag_getid1.SetWindowTextW(TEXT(""));
		m_edit_tag_getid2.SetWindowTextW(TEXT(""));
		m_edit_tag_gettime.SetWindowTextW(TEXT(""));
		m_edit_server_getid1.SetWindowTextW(TEXT(""));
		m_edit_server_getid2.SetWindowTextW(TEXT(""));
		m_edit_router_getid1.SetWindowTextW(TEXT(""));
		m_edit_router_getid2.SetWindowTextW(TEXT(""));
		m_edit_router_gettime.SetWindowTextW(TEXT(""));
		m_edit_server_gettime.SetWindowTextW(TEXT(""));

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
				m_bGetRouter = FALSE;
				DWORD write;
				BYTE cmd[] = {0xFF, 0xFE, 0x00, 0x00, 0x00, 0xFD, 0xFC};
				BYTE cmd1[] = {0xFC, 0x40, 0x00, 0x00, 0xFB};
				for(int i=0; i<3; i++)
				{
					cmd1[3] += cmd1[i];  
				}
				WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
				Sleep(10);
				WriteFile(m_hcom, cmd1, sizeof(cmd1), &write, NULL);
				Sleep(200);
				if(m_bGetTag)
				{
					//连接到设备
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("斷開連接"));
					m_tag_getid.EnableWindow(TRUE);
					m_tag_setid.EnableWindow(TRUE);
					m_tag_gettime.EnableWindow(TRUE);
					m_tag_settime.EnableWindow(TRUE);

					m_server_getid.EnableWindow(FALSE);
					m_server_setid.EnableWindow(FALSE);
					m_button_server_gettime.EnableWindow(FALSE);
					m_button_server_settime.EnableWindow(FALSE);

					m_router_getid.EnableWindow(FALSE);
					m_router_setid.EnableWindow(FALSE);
					m_button_router_gettime.EnableWindow(FALSE);
					m_button_router_settime.EnableWindow(FALSE);
					m_combo_three.SetWindowTextW(str);

					OnBnClickedTagGetid();
					OnBnClickedButtonGettime();

					return;
				}
				if(m_bGetServer)
				{
					//连接到设备
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("斷開連接"));
					m_tag_getid.EnableWindow(FALSE);
					m_tag_setid.EnableWindow(FALSE);
					m_tag_gettime.EnableWindow(FALSE);
					m_tag_settime.EnableWindow(FALSE);

					m_server_getid.EnableWindow(TRUE);
					m_server_setid.EnableWindow(TRUE);
					m_button_server_gettime.EnableWindow(TRUE);
					m_button_server_settime.EnableWindow(TRUE);

					m_router_getid.EnableWindow(FALSE);
					m_router_setid.EnableWindow(FALSE);
					m_button_router_gettime.EnableWindow(FALSE);
					m_button_router_settime.EnableWindow(FALSE);

					m_combo_three.SetWindowTextW(str);
					
					OnBnClickedServerGetid();
					OnBnClickedButtonServerGettime();

					return;
				}
				if(m_bGetRouter)
				{
					//连接到设备
					m_bConnect = TRUE;
					m_button_connect.SetWindowTextW(TEXT("斷開連接"));
					m_tag_getid.EnableWindow(FALSE);
					m_tag_setid.EnableWindow(FALSE);
					m_tag_gettime.EnableWindow(FALSE);
					m_tag_settime.EnableWindow(FALSE);

					m_server_getid.EnableWindow(FALSE);
					m_server_setid.EnableWindow(FALSE);
					m_button_server_gettime.EnableWindow(FALSE);
					m_button_server_settime.EnableWindow(FALSE);

					m_router_getid.EnableWindow(TRUE);
					m_router_setid.EnableWindow(TRUE);
					m_button_router_gettime.EnableWindow(TRUE);
					m_button_router_settime.EnableWindow(TRUE);
					m_combo_three.SetWindowTextW(str);

					OnBnClickedRouterGetid();
					OnBnClickedButtonRouterGettime();

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

void SetType3::OnBnClickedTagGetid()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bGetTagID = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x02, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
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

void SetType3::OnBnClickedTagSetid()
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
	BYTE cmd[] = {0xFF, 0xFE, 0x01, 0x00, 0x00, 0xFD, 0xFC};
	cmd[3] = m_Tag_ID[0];
	cmd[4] = m_Tag_ID[1];
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
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

		OnBnClickedTagGetid();
	}
	else
	{
		MessageBox(TEXT("設置ID失敗！"));
	}
}

bool SetType3::StringToChar(CString str, BYTE* data)
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

void SetType3::OnBnClickedButtonGettime()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bSleepTime = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x04, 0x00, 0x00, 0xFD, 0xFC};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSleepTime)
	{
		CString str;
		str.Format(TEXT("%d"), m_SleepTime);
		m_edit_tag_gettime.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取時間失敗！"));
	}
}
void SetType3::OnBnClickedButtonSettime()
{
	// TODO: Add your control notification handler code here
	UpdateData(TRUE);
	if(m_edit_settime_value == 0)
	{
		MessageBox(TEXT("發送間隔不能設置為0"));
		return;
	}
	m_bSleepTime = FALSE;
	BYTE cmd[] = {0xFF, 0xFE, 0x03, 0x00, 0x00, 0xFD, 0xFC};
	cmd[3] = (m_edit_settime_value >> 8) & 0xFF;
	cmd[4] = m_edit_settime_value & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSleepTime)
	{
		OnBnClickedButtonGettime();
	}
	else
	{
		MessageBox(TEXT("設置時間失敗！"));
	}
	
}
void SetType3::OnBnClickedServerGetid()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bGetServerID = FALSE;
	BYTE cmd[] = {0xFC, 0x02, 0x00, 0x00, 0xFB};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetServerID)
	{
		CString str;
		str.Format(TEXT("%02X"), m_Server_ID[0]);
		m_edit_server_getid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Server_ID[1]);
		m_edit_server_getid2.SetWindowTextW(str);
		Sleep(10);
	}
	else
	{
		MessageBox(TEXT("獲取ID失敗！"));
	}
}

void SetType3::OnBnClickedServerSetid()
{
	// TODO: 在此添加控件通知处理程序代码
	CString panid1, panid2;
	m_edit_server_setid1.GetWindowTextW(panid1);
	m_edit_server_setid2.GetWindowTextW(panid2);
	if(panid1.GetLength() != 2 || panid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid1, m_Server_ID))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid2, m_Server_ID+1))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(m_Server_ID[0] == 0 && m_Server_ID[1] == 0)
	{
		CString str;
		str = TEXT("不能設置定位卡片的ID號為：00 00");
		MessageBox(str);
		return;
	}
	m_bSetServerID = FALSE;
	BYTE cmd[] = {0xFC, 0x01, 0x00, 0x00, 0xFB};
	cmd[2] = m_Server_ID[0];
	cmd[3] = m_Server_ID[1];
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetServerID)
	{
		//设置成功，ID号自动加1
		if(m_Server_ID[1] == 0xFF)
		{
			m_Server_ID[1] = 0x00;
			if(m_Server_ID[0] == 0xFF)
				m_Server_ID[0] = 0x00;
			else
				m_Server_ID[0]++;
		}
		else
			m_Server_ID[1]++;
		CString str;
		str.Format(TEXT("%02X"), m_Server_ID[0]);
		m_edit_server_setid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Server_ID[1]);
		m_edit_server_setid2.SetWindowTextW(str);

		Sleep(10);
		OnBnClickedServerGetid();
	}
	else
	{
		MessageBox(TEXT("設置ID失敗！"));
	}
}

void SetType3::OnBnClickedRouterGetid()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bGetRouterID = FALSE;
	BYTE cmd[] = {0xFC, 0x02, 0x00, 0x00, 0xFB};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bGetRouterID)
	{
		CString str;
		str.Format(TEXT("%02X"), m_Router_ID[0]);
		m_edit_router_getid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Router_ID[1]);
		m_edit_router_getid2.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取ID失敗！"));
	}
}

void SetType3::OnBnClickedRouterSetid()
{
	// TODO: 在此添加控件通知处理程序代码
	CString panid1, panid2;
	m_edit_router_setid1.GetWindowTextW(panid1);
	m_edit_router_setid2.GetWindowTextW(panid2);
	if(panid1.GetLength() != 2 || panid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid1, m_Router_ID))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!this->StringToChar(panid2, m_Router_ID+1))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(m_Router_ID[0] == 0 && m_Router_ID[1] == 0)
	{
		CString str;
		str = TEXT("不能設置定位卡片的ID號為：00 00");
		MessageBox(str);
		return;
	}
	m_bSetRouterID = FALSE;
	BYTE cmd[] = {0xFC, 0x01, 0x00, 0x00, 0xFB};
	cmd[2] = m_Router_ID[0];
	cmd[3] = m_Router_ID[1];
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bSetRouterID)
	{
		//设置成功，ID号自动加1
		if(m_Router_ID[1] == 0xFF)
		{
			m_Router_ID[1] = 0x00;
			if(m_Router_ID[0] == 0xFF)
				m_Router_ID[0] = 0x00;
			else
				m_Router_ID[0]++;
		}
		else
			m_Router_ID[1]++;
		CString str;
		str.Format(TEXT("%02X"), m_Router_ID[0]);
		m_edit_router_setid1.SetWindowTextW(str);
		str.Format(TEXT("%02X"), m_Router_ID[1]);
		m_edit_router_setid2.SetWindowTextW(str);

		OnBnClickedRouterGetid();
	}
	else
	{
		MessageBox(TEXT("設置ID失敗！"));
	}
}

void SetType3::OnBnClickedButtonServerGettime()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bInformTime = FALSE;
	BYTE cmd[] = {0xFC, 0x04, 0x00, 0x00, 0xFB};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bInformTime)
	{
		CString str;
		str.Format(TEXT("%d"), m_InformTime);
		m_edit_server_gettime.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取時間失敗！"));
	}
}

void SetType3::OnBnClickedButtonServerSettime()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	if(m_edit_server_settime == 0)
	{
		MessageBox(TEXT("發送間隔不能設置為0"));
		return;
	}
	m_bInformTime = FALSE;
	BYTE cmd[] = {0xFC, 0x03, 0x00, 0x00, 0xFB};
	cmd[2] = (m_edit_server_settime >> 8) & 0xFF;
	cmd[3] = m_edit_server_settime & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bInformTime)
	{
		OnBnClickedButtonServerGettime();
	}
	else
	{
		MessageBox(TEXT("設置時間失敗！"));
	}
}

void SetType3::OnBnClickedButtonRouterGettime()
{
	// TODO: 在此添加控件通知处理程序代码
	m_bInformTime = FALSE;
	BYTE cmd[] = {0xFC, 0x04, 0x00, 0x00, 0xFB};
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bInformTime)
	{
		CString str;
		str.Format(TEXT("%d"), m_InformTime);
		m_edit_router_gettime.SetWindowTextW(str);
	}
	else
	{
		MessageBox(TEXT("獲取時間失敗！"));
	}
}

void SetType3::OnBnClickedButtonRouterSettime()
{
	// TODO: 在此添加控件通知处理程序代码
	UpdateData(TRUE);
	if(m_edit_router_settime == 0)
	{
		MessageBox(TEXT("發送間隔不能設置為0"));
		return;
	}
	m_bInformTime = FALSE;
	BYTE cmd[] = {0xFC, 0x03, 0x00, 0x00, 0xFB};
	cmd[2] = (m_edit_router_settime >> 8) & 0xFF;
	cmd[3] = m_edit_router_settime & 0xFF;
	DWORD write;					
	WriteFile(m_hcom, cmd, sizeof(cmd), &write, NULL);
	Sleep(200);
	if(m_bInformTime)
	{
		OnBnClickedButtonRouterGettime();
	}
	else
	{
		MessageBox(TEXT("設置時間失敗！"));
	}
}
