// ListShow.cpp : implementation file
//
#include "stdafx.h"
#include "Location.h"
#include "ListShow.h"
#include <io.h>
#include <fcntl.h>
#include <stdio.h>
#include <iostream>


#pragma comment(lib,"Iphlpapi.lib") //需要添加Iphlpapi.lib庫
// CListShow dialog

IMPLEMENT_DYNAMIC(CListShow, CDialog)

CListShow::CListShow(CWnd* pParent /*=NULL*/)
	: CDialog(CListShow::IDD, pParent)
	, m_edit_port(51234)
{
}

CListShow::~CListShow()
{
	FreeConsole();
}

void InitConsole()  
{  
    int nRet= 0;  
    FILE* fp;  
    AllocConsole();  
    nRet= _open_osfhandle((long)GetStdHandle(STD_OUTPUT_HANDLE), _O_TEXT);  
    fp = _fdopen(nRet, "w");  
    *stdout = *fp;  
    setvbuf(stdout, NULL, _IONBF, 0);  
}  

void CListShow::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_CONNECT, m_button_connect);
	DDX_Control(pDX, IDC_COMBO_COMPORT, m_combo_comport);
	DDX_Control(pDX, IDC_LIST_Info, m_list_info);
	DDX_Text(pDX, IDC_EDIT_ServerPort, m_edit_port);
	DDX_Control(pDX, IDC_BUTTON_COMStart, m_button_com_start);
	DDX_Control(pDX, IDC_BUTTON_Clean_List, m_button_clean_list);
}


BEGIN_MESSAGE_MAP(CListShow, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_CONNECT, &CListShow::OnBnClickedButtonConnect)
	ON_WM_TIMER()
	ON_WM_SIZE()
	ON_BN_CLICKED(IDC_BUTTON_COMStart, &CListShow::OnBnClickedButtonComstart)
	ON_BN_CLICKED(IDC_BUTTON_Clean_List, &CListShow::OnBnClickedButtonCleanList)
END_MESSAGE_MAP()


// CListShow message handlers

void CListShow::PreInitDialog()
{
	// TODO: Add your specialized code here and/or call the base class

	CDialog::PreInitDialog();
}

CRITICAL_SECTION    g_cs;

//網口讀數據線程函數
DWORD ReadThread_ListShow(LPVOID lparam)
{	
	int	actualReadLen=0;	//實際讀取的字節數
	int	willReadLen;
	SOCKADDR_IN tmpAddr;
	int tmpRecvLen;
	
	tmpRecvLen = sizeof(tmpAddr);

	CListShow *pdlg;
	pdlg = (CListShow *)lparam;
	DWORD time ;
	pdlg->m_Receive_Data_Len = 0;
	time = GetTickCount();	
	while(pdlg->m_UDPSocket != NULL){ 		
		tmpRecvLen = sizeof(tmpAddr);
		willReadLen = MAX_RECEIVE_DATA_LEN - pdlg->m_Receive_Data_Len;
		actualReadLen = recvfrom(pdlg->m_UDPSocket, (char *)(pdlg->m_Receive_Data_Char + pdlg->m_Receive_Data_Len), willReadLen,0, (SOCKADDR*)&tmpAddr,&tmpRecvLen); 
		
		if (actualReadLen == 0 && pdlg->m_Receive_Data_Len == 0)
		{
			Sleep(10);
			continue;
		}
		else if(actualReadLen < 0)
		{			
			if(pdlg->m_Receive_Data_Len >= MAX_RECEIVE_DATA_LEN ) {
				pdlg->m_Receive_Data_Len = 0;
				time = GetTickCount();
			}
			Sleep(10);
			continue;
		}
		pdlg->m_Receive_Data_Len += actualReadLen;

		EnterCriticalSection(&g_cs); 
		pdlg->ParseData();
		LeaveCriticalSection(&g_cs); 
		time = GetTickCount();
	}
	return 0;
}


#define PACKET_LEN		13
#define PACKET_HANDLE	0XFE
#define PACKET_END	0XFD
void CListShow::ParseData(void){
	int start;
	while(m_Receive_Data_Len >= 3){
		//查找開始標志FE
		start = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, PACKET_HANDLE);
		if(start < 0){
			//全部丟掉
			m_Receive_Data_Len = 0;		
			break;
		}
		//找到開始標志
		//判斷從開始標志開始，是否有接收封包類型的長度
		if(start + 1 > m_Receive_Data_Len){
			break;
		}
		//接收到封包類型，判斷封包類型
		switch(m_Receive_Data_Char[start+1]){
			case 1:		//普通定位
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				{	//緊急定位，固定長度13
				if(start + PACKET_LEN > m_Receive_Data_Len){
					//未收滿，等待收滿再處理
					return;
				}
				//收到一個數據包的長度，處理它
				//有接收到一個包的長度，判斷包尾格式	
				//CString msg;
				//msg = "1";
				using namespace std;
				//cout<<msg<<endl;
				cout<<"dec:"<<dec<<PACKET_END<<endl;  //以十进制形式输出整数

				if(PACKET_END != m_Receive_Data_Char[start+PACKET_LEN-1]){
					//包尾格式不正確
					//查找下一個開始標志頭，
					int start2 = FindChar(m_Receive_Data_Char, start+1, m_Receive_Data_Len, PACKET_HANDLE);
					if(start2 >= 0){
						//有下一個開始標志頭，把這個頭前的所有數據丟掉
						m_Receive_Data_Len = m_Receive_Data_Len - start2;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start2, m_Receive_Data_Len);
					}else
						//全部丟掉
						m_Receive_Data_Len = 0;								
					break;
				}

				//包尾格式匹配，計算校驗和
				BYTE sum = 0;
				for(int i=0; i<PACKET_LEN-2; i++)
					sum += m_Receive_Data_Char[start+i];
				if(sum != m_Receive_Data_Char[start+PACKET_LEN-2])
				{
					//校驗和失敗，丟掉數據包  //
					m_Receive_Data_Len = m_Receive_Data_Len - start - PACKET_LEN;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+PACKET_LEN, m_Receive_Data_Len);
					break;
				}

				//把數據抓取出來
				m_TagID[0] = m_Receive_Data_Char[start+2];
				m_TagID[1] = m_Receive_Data_Char[start+3];
				//判斷這個TAG之前是否保存過
				int id = -1;
				for(int i=0; i<m_Cur_ReceiveCount; i++)
				{
					if(memcmp(m_TagID, m_ReceiveInfo[i].TagId, 2) == 0)
					{
						id = i;
						break;
					}
				}if(id >= 0){
					//之前有接收過這個TAG
					//判斷接收到的序列號是否一樣，
					if(m_Receive_Data_Char[start+10] == m_ReceiveInfo[id].index){
						//兩次的序列號一樣，是同一批的數據包，從不同的數據節點發送來
						//比較新的參考點，是否有比舊的點更近
						if(m_Receive_Data_Char[start+6] < m_ReceiveInfo[id].Port1Rssi){
							//新參考點比舊的第一個參考點近，
							//以前保存的參考點信息后移
							m_ReceiveInfo[id].Port3Rssi = m_ReceiveInfo[id].Port2Rssi;
							m_ReceiveInfo[id].Port3ID[0] = m_ReceiveInfo[id].Port2ID[0];
							m_ReceiveInfo[id].Port3ID[1] = m_ReceiveInfo[id].Port2ID[1];

							m_ReceiveInfo[id].Port2Rssi = m_ReceiveInfo[id].Port1Rssi;
							m_ReceiveInfo[id].Port2ID[0] = m_ReceiveInfo[id].Port1ID[0];
							m_ReceiveInfo[id].Port2ID[1] = m_ReceiveInfo[id].Port1ID[1];

							//替換第一個點
							m_ReceiveInfo[id].Port1ID[0] = m_Receive_Data_Char[start+4];
							m_ReceiveInfo[id].Port1ID[1] = m_Receive_Data_Char[start+5];
							m_ReceiveInfo[id].Port1Rssi = m_Receive_Data_Char[start+6];
						}
						else if(m_Receive_Data_Char[start+6] < m_ReceiveInfo[id].Port2Rssi)
						{
							//新參考點比舊的第2個參考點近，
							//以前保存的參考點信息后移
							m_ReceiveInfo[id].Port3Rssi = m_ReceiveInfo[id].Port2Rssi;
							m_ReceiveInfo[id].Port3ID[0] = m_ReceiveInfo[id].Port2ID[0];
							m_ReceiveInfo[id].Port3ID[1] = m_ReceiveInfo[id].Port2ID[1];

							//替換第2個點
							m_ReceiveInfo[id].Port2ID[0] = m_Receive_Data_Char[start+4];
							m_ReceiveInfo[id].Port2ID[1] = m_Receive_Data_Char[start+5];
							m_ReceiveInfo[id].Port2Rssi = m_Receive_Data_Char[start+6];
						}
						else if(m_Receive_Data_Char[start+6] < m_ReceiveInfo[id].Port3Rssi)
						{
							//新參考點比舊的第3個參考點近										
							//替換第3個點
							m_ReceiveInfo[id].Port3ID[0] = m_Receive_Data_Char[start+4];
							m_ReceiveInfo[id].Port3ID[1] = m_Receive_Data_Char[start+5];
							m_ReceiveInfo[id].Port3Rssi = m_Receive_Data_Char[start+6];
						}else{
							//比以前保存的都遠，不處理它							
						}
					}else{

						/*if(GetTickCount() - m_ReceiveInfo[id].FirstReceiveTime < 200){ //做缓存
														
							m_ReceiveInfo2[id] = m_ReceiveInfo[id];		
						}*/

						//兩次的序列號不一樣，是新一批次的數據包，把數據包直接保存下來
						m_ReceiveInfo[id].Type = m_Receive_Data_Char[start+1];

						m_ReceiveInfo[id].Port1ID[0] = m_Receive_Data_Char[start+4];
						m_ReceiveInfo[id].Port1ID[1] = m_Receive_Data_Char[start+5];
						m_ReceiveInfo[id].Port1Rssi = m_Receive_Data_Char[start+6];

						m_ReceiveInfo[id].Battery = m_Receive_Data_Char[start+7];
						m_ReceiveInfo[id].SensorTime = (m_Receive_Data_Char[start+8] << 8) | m_Receive_Data_Char[start+9];

						m_ReceiveInfo[id].TotalCount++;
						if(((BYTE)(m_ReceiveInfo[id].index+1) != m_Receive_Data_Char[start+10]) && (m_ReceiveInfo[id].index != m_Receive_Data_Char[start+10]))
						{
							if(m_Receive_Data_Char[start+10] < m_ReceiveInfo[id].index)
								m_ReceiveInfo[id].LostCount += m_Receive_Data_Char[start+10] + 256 - m_ReceiveInfo[id].index - 1;
							else
								m_ReceiveInfo[id].LostCount += m_Receive_Data_Char[start+10] - m_ReceiveInfo[id].index - 1;
						}
						m_ReceiveInfo[id].index = m_Receive_Data_Char[start+10];
						m_ReceiveInfo[id].IsUpdate = TRUE;
						//if(GetTickCount() -m_ReceiveInfo[id].FirstReceiveTime <=200) {

						m_ReceiveInfo[id].FirstReceiveTime = GetTickCount();									
						m_ReceiveInfo[id].Port2ID[0] = 0;
						m_ReceiveInfo[id].Port2ID[1] = 0;
						m_ReceiveInfo[id].Port2Rssi = 0xFF;
						m_ReceiveInfo[id].Port3ID[0] = 0;
						m_ReceiveInfo[id].Port3ID[1] = 0;
						m_ReceiveInfo[id].Port3Rssi = 0xFF;}
					//}								
				}
				else
				{
					//之前沒有接收過這個TAG，添加
					if(m_Cur_ReceiveCount < MAX_RECEIVE_COUNT){

						id = m_Cur_ReceiveCount;
						m_Cur_ReceiveCount++;
						m_ReceiveInfo[id].Type = isJinJiType(m_Receive_Data_Char[start+1]);//
						m_ReceiveInfo[id].DrivaceType = m_Receive_Data_Char[start+1];

						m_ReceiveInfo[id].TagId[0] = m_Receive_Data_Char[start+2];
						m_ReceiveInfo[id].TagId[1] = m_Receive_Data_Char[start+3];
											
						m_ReceiveInfo[id].Port1ID[0] = m_Receive_Data_Char[start+4];
						m_ReceiveInfo[id].Port1ID[1] = m_Receive_Data_Char[start+5];
						m_ReceiveInfo[id].Port1Rssi = m_Receive_Data_Char[start+6];
											
						m_ReceiveInfo[id].Battery = m_Receive_Data_Char[start+7];
						m_ReceiveInfo[id].SensorTime = (m_Receive_Data_Char[start+8] << 8) | m_Receive_Data_Char[start+9];

						m_ReceiveInfo[id].index = m_Receive_Data_Char[start+10];
						m_ReceiveInfo[id].IsUpdate = TRUE;
						m_ReceiveInfo[id].FirstReceiveTime = GetTickCount() ;//- 200
						m_ReceiveInfo[id].ReceivData = TRUE;
						m_ReceiveInfo[id].Port2ID[0] = 0;
						m_ReceiveInfo[id].Port2ID[1] = 0;
						m_ReceiveInfo[id].Port2Rssi = 0xFF;
						m_ReceiveInfo[id].Port3ID[0] = 0;
						m_ReceiveInfo[id].Port3ID[1] = 0;
						m_ReceiveInfo[id].Port3Rssi = 0xFF;

						m_ReceiveInfo[id].TotalCount = 1;
						m_ReceiveInfo[id].LostCount = 0;	
					}
					else
					{
						//沒有空位置了，不添加
					}									
				}
				m_Receive_Data_Len = m_Receive_Data_Len - start - PACKET_LEN;
				memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+PACKET_LEN, m_Receive_Data_Len);		
				break;
				}
			/*case 3:		//參考點自動上報的數據，這里不處理
				//沒有標志頭，全部丟掉
				m_Receive_Data_Len = 0;		
				break;*/
			default:
				//查找下一個開始標志頭，
				int start2 = FindChar(m_Receive_Data_Char, start+1, m_Receive_Data_Len, PACKET_HANDLE);
				if(start2 >= 0)
				{
					//有下一個開始標志頭，把這個頭前的所有數據丟掉
					m_Receive_Data_Len = m_Receive_Data_Len - start2;
					memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start2, m_Receive_Data_Len);
				}else{
					//沒有標志頭，全部丟掉
					m_Receive_Data_Len = 0;						
					break;
				}
				break;
		}		
	}
}

////判断是否是紧急定位
BYTE CListShow::isJinJiType(byte itemType){
	if (itemType == 0x02 || itemType == 0x06 || itemType == 0x08){
		return 2;
	}
	return 1;
}


int CListShow::FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2)
{
	for(int i=start; i<end-1; i++)
	{
		if(str[i] == c1 && str[i+1] == c2)
			return i;
	}
	return -1;
}

int CListShow::FindChar(BYTE *str, int start, int end, BYTE c1)
{
	for(int i=start; i<end; i++)
	{
		if(str[i] == c1)
			return i;
	}
	return -1;
}

void InitConsoleWindow()
{
    int nCrt = 0;
    FILE* fp;
    AllocConsole();
    nCrt = _open_osfhandle((long)GetStdHandle(STD_OUTPUT_HANDLE), _O_TEXT);
    fp = _fdopen(nCrt, "w");
    *stdout = *fp;
    setvbuf(stdout, NULL, _IONBF, 0);
}

BOOL CListShow::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here	

	m_bConnect = FALSE;

	DWORD dwStyle = m_list_info.GetExtendedStyle();
	m_list_info.SetExtendedStyle(dwStyle | LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_HEADERDRAGDROP);
	m_list_info.InsertColumn(0,TEXT("卡片ID"), LVCFMT_CENTER, 50);
	m_list_info.InsertColumn(1,TEXT("卡片名稱"), LVCFMT_LEFT, 100);
	m_list_info.InsertColumn(2,TEXT("參考點1ID"), LVCFMT_LEFT, 70);
	m_list_info.InsertColumn(3,TEXT("參考點1名稱"), LVCFMT_LEFT, 100);
	m_list_info.InsertColumn(4,TEXT("相對距離1"), LVCFMT_CENTER, 70);
	m_list_info.InsertColumn(5,TEXT("參考點2ID"), LVCFMT_LEFT, 70);
	m_list_info.InsertColumn(6,TEXT("參考點2名稱"), LVCFMT_LEFT, 100);
	m_list_info.InsertColumn(7,TEXT("相對距離2"), LVCFMT_CENTER, 70);
	m_list_info.InsertColumn(8,TEXT("參考點3ID"), LVCFMT_LEFT, 70);
	m_list_info.InsertColumn(9,TEXT("參考點3名稱"), LVCFMT_LEFT, 100);
	m_list_info.InsertColumn(10,TEXT("相對距離3"), LVCFMT_CENTER, 70);
	m_list_info.InsertColumn(11,TEXT("定位類型"), LVCFMT_CENTER, 70);
	m_list_info.InsertColumn(12,TEXT("電池電量"), LVCFMT_CENTER, 70);
	m_list_info.InsertColumn(13,TEXT("卡片沒有移動的時間（秒）"), LVCFMT_CENTER, 170);
	m_list_info.InsertColumn(14,TEXT("最後一次接收數據的時間"), LVCFMT_CENTER, 160);
	m_list_info.InsertColumn(15,TEXT("距離上一次接收到數據包的時間"), LVCFMT_CENTER, 180);
	m_list_info.InsertColumn(16,TEXT("接收封包的數量"), LVCFMT_CENTER, 90);
	m_list_info.InsertColumn(17,TEXT("丟包的數量"), LVCFMT_CENTER, 90);	
	m_list_info.InsertColumn(18,TEXT("設備類型"), LVCFMT_CENTER, 90);	

	InitializeCriticalSection(&g_cs);
	
//	InitConsoleWindow();	

	//抓取電腦IP
	//1.初始化socket資源
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2,2),&wsa) != 0)
	{
		return TRUE;//代表失敗
	}

	CString str, str_add, str_mac;
	//PIP_ADAPTER_INFO結構體指針存儲本機網卡信息
    PIP_ADAPTER_INFO pIpAdapterInfo = new IP_ADAPTER_INFO();
    //得到結構體大小,用于GetAdaptersInfo參數
    unsigned long stSize = sizeof(IP_ADAPTER_INFO);
    //調用GetAdaptersInfo函數,填充pIpAdapterInfo指針變量;其中stSize參數既是一個輸入量也是一個輸出量
    int nRel = GetAdaptersInfo(pIpAdapterInfo,&stSize);
    //記錄網卡數量
    int netCardNum = 0;
    //記錄每張網卡上的IP地址數量
    int IPnumPerNetCard = 0;
    if (ERROR_BUFFER_OVERFLOW == nRel)
    {
        //如果函數返回的是ERROR_BUFFER_OVERFLOW
        //則說明GetAdaptersInfo參數傳遞的內存空間不夠,同時其傳出stSize,表示需要的空間大小
        //這也是說明為什么stSize既是一個輸入量也是一個輸出量
        //釋放原來的內存空間
        delete pIpAdapterInfo;
        //重新申請內存空間用來存儲所有網卡信息
        pIpAdapterInfo = (PIP_ADAPTER_INFO)new BYTE[stSize];
        //再次調用GetAdaptersInfo函數,填充pIpAdapterInfo指針變量
        nRel=GetAdaptersInfo(pIpAdapterInfo,&stSize);    
    }
    if (ERROR_SUCCESS == nRel)
    {
        //輸出網卡信息
         //可能有多網卡,因此通過循環去判斷
		while (pIpAdapterInfo)
		{
			
			printf("網卡數量：%d \r\n", ++netCardNum);
			printf("網卡名稱：%s \r\n", pIpAdapterInfo->AdapterName);
			printf("網卡描述：%s \r\n", pIpAdapterInfo->Description);
			switch(pIpAdapterInfo->Type)
			{
			case MIB_IF_TYPE_OTHER:
				printf("網卡類型： OTHER \r\n");
				break;
			case MIB_IF_TYPE_ETHERNET:
				printf("網卡類型： ETHERNET \r\n");
				break;
			case MIB_IF_TYPE_TOKENRING:
				printf("網卡類型： TOKENRING \r\n");
				break;
			case MIB_IF_TYPE_FDDI:
				printf("網卡類型： FDDI \r\n");
				break;
			case MIB_IF_TYPE_PPP:
				printf("PP\n");
				printf("網卡類型： PPP \r\n");
				break;
			case MIB_IF_TYPE_LOOPBACK:
				printf("網卡類型： LOOPBACK \r\n");
				break;
			case MIB_IF_TYPE_SLIP:
				printf("網卡類型： SLIP \r\n");
				break;
			default:

				break;
			}
			printf("網卡MAC地址：");
			
			str_mac = TEXT("");
			for (DWORD i = 0; i < pIpAdapterInfo->AddressLength; i++)
			{
				
				if (i < pIpAdapterInfo->AddressLength-1)
				{
					str.Format(TEXT("%02X-"), pIpAdapterInfo->Address[i]);
					printf("%02X-", pIpAdapterInfo->Address[i]);
				}
				else
				{
					str.Format(TEXT("%02X"), pIpAdapterInfo->Address[i]);
					printf("%02X\n", pIpAdapterInfo->Address[i]);
				}
				str_add += str;
			}
			str_mac = TEXT("(") + str_mac + TEXT(")");
			printf("網卡IP地址如下：\r\n");
			//可能網卡有多IP,因此通過循環去判斷
			IP_ADDR_STRING *pIpAddrString =&(pIpAdapterInfo->IpAddressList);
			IPnumPerNetCard = 0;
			do 
			{
				str_add = pIpAddrString->IpAddress.String;
				m_combo_comport.AddString(str_add);

				printf("該網卡上的IP數量：%d \r\n", ++IPnumPerNetCard);
				printf("IP 地址：%s \r\n", pIpAddrString->IpAddress.String);
				printf("子網地址：%s \r\n", pIpAddrString->IpMask.String);
				printf("網關地址：%s \r\n", pIpAdapterInfo->GatewayList.IpAddress.String);
				pIpAddrString=pIpAddrString->Next;
			} while (pIpAddrString);
			pIpAdapterInfo = pIpAdapterInfo->Next;
			printf("--------------------------------------------------------------------\r\n");
		}    
    }
    //釋放內存空間
    if (pIpAdapterInfo)
    {
        delete[] pIpAdapterInfo;
    }

	m_combo_comport.SetCurSel(0);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}


VOID GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}

BOOL CListShow::GetSaveTagInfo(void)
{
	m_Cur_SaveTagCount = 0;

	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\tag.ini");	
	if(!m_ini.Open(filepath, FALSE))
		return TRUE;

	CString str, str_value;

	for(; m_Cur_SaveTagCount<MAX_SAVE_TAG_COUNT; m_Cur_SaveTagCount++)
	{
		str.Format(TEXT("%d"), m_Cur_SaveTagCount);
		if(m_ini.GetValue(str, TEXT("ID"), str_value))
		{
			if(str_value.GetLength() != 4)
			{
				str.Format(TEXT("tag.ini中，[%d]的ID號長度不正確！"), m_Cur_SaveTagCount);				
				MessageBox(str);
				return FALSE;
			}
			m_SaveTagInfo[m_Cur_SaveTagCount].ID = str_value;
		}
		else
			break;
		if(m_ini.GetValue(str, TEXT("Addr"), str_value))
		{
			m_SaveTagInfo[m_Cur_SaveTagCount].Name = str_value;
		}
		else
			break;			
	}
	return TRUE;
}

BOOL CListShow::GetSavePortInfo(void)
{
	m_Cur_SavePortCount = 0;

	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\port.ini");	
	if(!m_ini.Open(filepath, FALSE))
		return TRUE;

	CString str, str_value;

	for(; m_Cur_SavePortCount<MAX_SAVE_PORT_COUNT; m_Cur_SavePortCount++)
	{
		str.Format(TEXT("%d"), m_Cur_SavePortCount);
		if(m_ini.GetValue(str, TEXT("ID"), str_value))
		{
			if(str_value.GetLength() != 4)
			{
				str.Format(TEXT("port.ini中，[%d]的ID號長度不正確！"), m_Cur_SavePortCount);				
				MessageBox(str);
				return FALSE;
			}
			m_SavePortInfo[m_Cur_SavePortCount].ID = str_value;
		}
		else
			break;
		if(m_ini.GetValue(str, TEXT("Addr"), str_value))
		{
			m_SavePortInfo[m_Cur_SavePortCount].Name = str_value;
		}
		else
			break;			
	}
	return TRUE;
}


void CListShow::OnBnClickedButtonConnect()
{
	// TODO: Add your control notification handler code here
	if(m_bConnect)
	{
		closesocket(m_UDPSocket);
		m_UDPSocket = NULL;
		KillTimer(1);
		m_bConnect = FALSE;
		m_button_connect.SetWindowTextW(TEXT("開始監控"));
		m_button_com_start.EnableWindow(TRUE);
	}
	else
	{
		UpdateData(TRUE);
		//創建UDP套接字
		m_UDPSocket = socket(AF_INET,SOCK_DGRAM,IPPROTO_UDP);
		if (m_UDPSocket == INVALID_SOCKET)
		{
			MessageBox(TEXT("Create UDP socket error!"));
			return;
		}
		CString str;
		m_combo_comport.GetWindowTextW(str);
		char ip[16];
		memset(ip, 0, sizeof(ip));
		wcstombs(ip, str, 16);

		struct sockaddr_in localAddr;
		localAddr.sin_family = AF_INET;
		localAddr.sin_port = htons(m_edit_port);
		localAddr.sin_addr.s_addr= inet_addr(ip);
		
		//綁定地址
		if(bind(m_UDPSocket,(sockaddr*)&localAddr,sizeof(localAddr))!=0)
		{
			closesocket(m_UDPSocket);
			MessageBox(TEXT("bind port fail!"));
			return;
		}
		
		//設置非堵塞通訊
		DWORD ul= 1;	// 0 = 阻塞；1 = 非阻塞
		ioctlsocket(m_UDPSocket,FIONBIO,&ul);

		AfxBeginThread((AFX_THREADPROC)ReadThread_ListShow, this);
		GetSaveTagInfo();
		GetSavePortInfo();
		//每秒刷新一次列表信息
		SetTimer(1, 500, NULL);
		m_button_connect.SetWindowTextW(TEXT("停止監控"));
		m_button_com_start.EnableWindow(FALSE);
		m_bConnect = TRUE;
		m_Cur_ReceiveCount = 0;
		m_list_info.SetExtendedStyle(LVS_EX_DOUBLEBUFFER);
		m_list_info.DeleteAllItems();		
	}
}


void CListShow::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: Add your message handler code here and/or call default
	if(nIDEvent == 1)
	{	
		
		DWORD time;
		EnterCriticalSection(&g_cs); 		
		//更新List列表		
		CString str;

		for(int id=0; id < m_Cur_ReceiveCount; id++)
		{
			//m_ReceiveInfo2[id] = m_ReceiveInfo[id];
						
			if(!m_ReceiveInfo[id].IsUpdate)
			{
				time = GetTickCount() - m_ReceiveInfo[id].FirstReceiveTime;
				if(time < 60000)
				{
					str.Format(TEXT("%d秒"), time/1000);
				}
				else if(time < 3600000)
					str.Format(TEXT("%d分%d秒"), time/60000, (time%60000)/1000);
				else 
					str.Format(TEXT("%d小時%d分%d秒"), time/3600000, (time%3600000)/60000, (time%60000)/1000);
				m_list_info.SetItemText(id, 15, str);
				continue;
			}

			ReceiveInfo receiveInfo;
			if((m_ReceiveInfo[id].Port2ID[0] ==0  && m_ReceiveInfo[id].Port2ID[1] == 0) || 
				(m_ReceiveInfo[id].Port3ID[0] ==0  && m_ReceiveInfo[id].Port3ID[1] == 0) ){
					if (m_ReceiveInfo2[id].FirstReceiveTime < GetTickCount() - 400 )
					{
						receiveInfo = m_ReceiveInfo2[id];
					}else
					{
						continue;
					}
			}else{
				receiveInfo = m_ReceiveInfo[id];
			}

			if(GetTickCount() - m_ReceiveInfo2[id].FirstReceiveTime < 200)
			{
				//接收到第一個包之后，200毫秒，再顯示
				continue;
			}
			receiveInfo.IsUpdate = FALSE;
			//Tag ID
			str.Format(TEXT("%02X%02X"), receiveInfo.TagId[0], receiveInfo.TagId[1]);
			if(id+1 > m_list_info.GetItemCount())
			{
				m_list_info.InsertItem(id, str);
			}
			//Tag Name
			for(int i=0; i<m_Cur_SaveTagCount; i++)
			{
				if(0 == str.Compare(m_SaveTagInfo[i].ID))
				{
					str = m_SaveTagInfo[i].Name;
					break;
				}
			}
			m_list_info.SetItemText(id, 1, str);			
			//Port1 ID
			str.Format(TEXT("%02X%02X"), receiveInfo.Port1ID[0], receiveInfo.Port1ID[1]);
			m_list_info.SetItemText(id, 2, str);
			//Port1 Name
			for(int i=0; i<m_Cur_SavePortCount; i++)
			{
				if(0 == str.Compare(m_SavePortInfo[i].ID))
				{
					str = m_SavePortInfo[i].Name;
					break;
				}
			}
			m_list_info.SetItemText(id, 3, str);
			//Port1 Rssi
			str.Format(TEXT("%d"), receiveInfo.Port1Rssi);
			m_list_info.SetItemText(id, 4, str);

			//Port2 ID
			str.Format(TEXT("%02X%02X"), receiveInfo.Port2ID[0], receiveInfo.Port2ID[1]);
			m_list_info.SetItemText(id, 5, str);
			//Port2 Name
			for(int i=0; i<m_Cur_SavePortCount; i++)
			{
				if(0 == str.Compare(m_SavePortInfo[i].ID))
				{
					str = m_SavePortInfo[i].Name;
					break;
				}
			}
			m_list_info.SetItemText(id, 6, str);
			//Port1 Rssi
			str.Format(TEXT("%d"), receiveInfo.Port2Rssi);
			m_list_info.SetItemText(id, 7, str);

			//Port3 ID
			str.Format(TEXT("%02X%02X"), receiveInfo.Port3ID[0], receiveInfo.Port3ID[1]);
			m_list_info.SetItemText(id, 8, str);
			//Port3 Name
			for(int i=0; i<m_Cur_SavePortCount; i++)
			{
				if(0 == str.Compare(m_SavePortInfo[i].ID))
				{
					str = m_SavePortInfo[i].Name;
					break;
				}
			}
			m_list_info.SetItemText(id, 9, str);
			//Port3 Rssi
			str.Format(TEXT("%d"), receiveInfo.Port3Rssi);
			m_list_info.SetItemText(id, 10, str);
			
			//type
			if(receiveInfo.Type != 2)
				str = TEXT("普通定位");
			else
				str = TEXT("緊急定位");
			m_list_info.SetItemText(id, 11, str);

			//Battery
			str.Format(TEXT("%d"), receiveInfo.Battery);
			m_list_info.SetItemText(id, 12, str);

			//sensor time
			str.Format(TEXT("%d"), receiveInfo.SensorTime);
			m_list_info.SetItemText(id, 13, str);
			
			//刷新時間
			CTime t = CTime::GetCurrentTime();
			str.Format(TEXT("%d-%d-%d %02d:%02d:%02d"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond());
			m_list_info.SetItemText(id, 14, str);
			
			//距離上一次接收到數據包的間隔時間
			time = GetTickCount() - receiveInfo.FirstReceiveTime;
			if(time < 60000)
			{
				str.Format(TEXT("%d秒"), time/1000);
			}
			else if(time < 3600000)
				str.Format(TEXT("%d分%d秒"), time/60000, (time%60000)/1000);
			else 
				str.Format(TEXT("%d小時%d分%d秒"), time/3600000, (time%3600000)/60000, (time%60000)/1000);
			m_list_info.SetItemText(id, 15, str);

			//total count
			str.Format(TEXT("%d"), receiveInfo.TotalCount);
			m_list_info.SetItemText(id, 16, str);

			//lost count
			str.Format(TEXT("%d"), receiveInfo.LostCount);
			m_list_info.SetItemText(id, 17, str);

			str = "TAG";
			BYTE DrivaceType = receiveInfo.DrivaceType;
			if (DrivaceType == 0x01 || DrivaceType == 0x02 ){
				str = "sl03";
			}
			if (DrivaceType == 0x05 || DrivaceType == 0x06 ){
				str = "pTAG-H01";
			}
			/*if (DrivaceType == 0x07 || DrivaceType == 0x08 ){
				str = "";
			}*/
			//getDrivaceTypeFromByte(str,m_ReceiveInfo[id].DrivaceType);
			m_list_info.SetItemText(id, 18, str);

		}
		LeaveCriticalSection(&g_cs); 
	}
	else if(nIDEvent == 2)
	{
		KillTimer(2);
		int i=0;
		CString str;
		for(i=0; i<1000; i++)
		{
			str.Format(TEXT("COM%d"), i);
			if(OpenCom(str))
			{
				m_bConnect = FALSE;
				Sleep(10);
				DWORD write;
				BYTE buf[5];
				buf[0] = 0xFC;
				buf[1] = 0x40;
				buf[2] = 0x00;
				buf[3] = buf[0] + buf[1] + buf[2];
				buf[4] = 0xFB;
				WriteFile(m_hcom, buf, sizeof(buf), &write, NULL);
				Sleep(200);
				if(m_bConnect)
				{
					//連接到設備
					m_button_com_start.SetWindowTextW(TEXT("停止監控"));
					m_button_connect.EnableWindow(FALSE);

					GetSaveTagInfo();
					GetSavePortInfo();
					//每秒刷新一次列表信息
					SetTimer(1, 500, NULL);
					EnterCriticalSection(&g_cs); 
					m_Cur_ReceiveCount = 0;
					m_list_info.DeleteAllItems();
					LeaveCriticalSection(&g_cs); 
					return;
				}
				CloseHandle(m_hcom);
				m_hcom = NULL;
				Sleep(10);
			}			
		}
		MessageBox(TEXT("連接硬體設備失敗！"));
		m_button_com_start.SetWindowTextW(TEXT("開始監控"));
	}
	CDialog::OnTimer(nIDEvent);
}

BOOL CListShow::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN)
	{
		switch (pMsg->wParam)
		{
		case VK_RETURN:
			//MessageBox(_T("回車鍵按下"));
			return TRUE;
		case VK_ESCAPE:
			//MessageBox(_T("ESC鍵按下"));
			return TRUE;
		default:
			break;
		}
	}
	else if(pMsg->message == WM_SETPORT_SAVE_LISTSHOW)
	{
		GetSavePortInfo();
	}
	else if(pMsg->message == WM_SETTAG_SAVE_LISTSHOW)
	{
		GetSaveTagInfo();
	}
	return CDialog::PreTranslateMessage(pMsg);
}

void CListShow::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	// TODO: Add your message handler code here

	CRect rs; 
	this->GetClientRect(&rs);
	rs.left += 10;
	rs.top += 85;
	rs.right -= 10;
	rs.bottom -= 10;	
	::MoveWindow(m_list_info.GetSafeHwnd(), rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
}



//串口讀線程函數
DWORD ComReadThread_ListShow(LPVOID lparam)
{	
	DWORD	actualReadLen=0;	//實際讀取的字節數
	DWORD	willReadLen;	
	
	DWORD dwReadErrors;
	COMSTAT	cmState;
	
	CListShow *pdlg;
	pdlg = (CListShow *)lparam;

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
		if(pdlg->m_bConnect)
		{
			EnterCriticalSection(&g_cs); 
			pdlg->ParseData();
			LeaveCriticalSection(&g_cs); 		
		}
		else
			pdlg->ParseData_Connect();

	}
	return 0;
}

#define PACKET_LEN_CONNECT  5
void CListShow::ParseData_Connect(void)
{
	int start;
	while(m_Receive_Data_Len >= PACKET_LEN_CONNECT)
	{
		//查找開始標志FC
		start = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, 0xFA);
		if(start >= 0)
		{
			//找到開始標志
			//判斷從開始標志開始，是否有接收一個包的長度
			if(start + PACKET_LEN_CONNECT <= m_Receive_Data_Len)
			{
				//有接收到一個包的長度，判斷包尾格式		
				if(0xF8 == m_Receive_Data_Char[start+PACKET_LEN_CONNECT-1])
				{
					//包尾格式匹配，計算校驗和
					BYTE sum = 0;
					for(int i=0; i<PACKET_LEN_CONNECT-2; i++)
						sum += m_Receive_Data_Char[start+i];
					if(sum == m_Receive_Data_Char[start+PACKET_LEN_CONNECT-2])
					{
						//校驗和匹配，認為是正確的數據包
						if(m_Receive_Data_Char[start+1] == 0x41 && m_Receive_Data_Char[start+2] == 0x01)
						{
							//Server
							m_bConnect = TRUE;
							m_Receive_Data_Len = 0;
							return;
						}
						else
						{
							//其它類型的數據暫不處理
						}
						//處理完成，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - PACKET_LEN_CONNECT;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+PACKET_LEN_CONNECT, m_Receive_Data_Len);
					}
					else
					{
						//校驗和失敗，丟掉數據包
						m_Receive_Data_Len = m_Receive_Data_Len - start - PACKET_LEN_CONNECT;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+PACKET_LEN_CONNECT, m_Receive_Data_Len);
					}
				}
				else
				{
					//包尾格式不正確
					//查找下一個開始標志頭，
					int start2 = FindChar(m_Receive_Data_Char, start+1, m_Receive_Data_Len, 0xFC);
					if(start2 >= 0)
					{
						//有下一個開始標志頭，把這個頭前的所有數據丟掉
						m_Receive_Data_Len = m_Receive_Data_Len - start2;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start2, m_Receive_Data_Len);
					}
					else
					{
						//全部丟掉
						m_Receive_Data_Len = 0;						
						break;
					}
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
			//全部丟掉
			m_Receive_Data_Len = 0;		
			break;
		}
	}
}

BOOL CListShow::OpenCom(CString str_com)
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

	HANDLE m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ComReadThread_ListShow, this, 0, NULL);
	CloseHandle(m_hThread);

	return TRUE;
}
void CListShow::OnBnClickedButtonComstart()
{
	// TODO: Add your control notification handler code here
	if(m_bConnect)
	{
		CloseHandle(m_hcom);
		m_hcom = NULL;
		m_button_com_start.SetWindowTextW(TEXT("開始監控"));
		m_button_connect.EnableWindow(TRUE);
		m_bConnect = FALSE;
		KillTimer(1);
	}
	else
	{
		m_button_com_start.SetWindowTextW(TEXT("連接設備中..."));
		SetTimer(2, 1, NULL);		
	}
}

void CListShow::OnBnClickedButtonCleanList()
{
	// TODO: Add your control notification handler code here
	EnterCriticalSection(&g_cs); 			
	m_Cur_ReceiveCount = 0;
	m_list_info.DeleteAllItems();
	LeaveCriticalSection(&g_cs); 	
}
