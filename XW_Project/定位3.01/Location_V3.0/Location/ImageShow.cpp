// ImageShow.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "ImageShow.h"
#include <math.h>
#include <shlwapi.h>


#pragma comment(lib,"Iphlpapi.lib") //需要添加Iphlpapi.lib庫

int		g_DrawTagXYOffset[MAX_DRAWTAG][2] = {
	{0, -28},	//12點方向
	{14, -14},	//1.5點方向
	{28, 0},	//3
	{14, 14},	//4.5
	{0, 28},	//6
	{-14, 14},	//7.5
	{-28, 0},	//9
	{-14, -14}	//10.5
};

int		g_DrawTagXYChongDieQu[2] = {0, 0};	//中間圓心

// CImageShow dialog
IMPLEMENT_DYNAMIC(CImageShow, CDialog)

CImageShow::CImageShow(CWnd* pParent /*=NULL*/)
	: CDialog(CImageShow::IDD, pParent)
{

}

CImageShow::~CImageShow()
{
}

void CImageShow::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_CONNECT, m_button_connect);
	DDX_Control(pDX, IDC_BUTTON_SET, m_button_set);	
	DDX_Control(pDX, IDC_EDIT_WARNING_INFO, m_edit_warning_info);
	DDX_Control(pDX, IDC_BUTTON_WARNING_ALL, m_btn_warning_show);
	DDX_Control(pDX, IDC_LIST1, m_CheckList);
	DDX_Control(pDX, IDC_STATIC_Show_Tag, m_static_show_tag);
	DDX_Control(pDX, IDC_CHECK_ALL, m_check_all);
	DDX_Control(pDX, IDC_BUTTON_OldData, m_button_olddata);
	DDX_Control(pDX, IDC_BUTTON_TAG_ONLINE, m_button_tag_online);
}


BEGIN_MESSAGE_MAP(CImageShow, CDialog)
	ON_WM_PAINT()
	ON_WM_LBUTTONDBLCLK()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_BN_CLICKED(IDC_BUTTON_CONNECT, &CImageShow::OnBnClickedButtonConnect)
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_BUTTON_SET, &CImageShow::OnBnClickedButtonSet)	
	ON_BN_CLICKED(IDC_BUTTON_WARNING_ALL, &CImageShow::OnBnClickedButtonWarningAll)
	ON_WM_SIZE()
	ON_BN_CLICKED(IDC_CHECK_ALL, &CImageShow::OnBnClickedCheckAll)
	ON_LBN_SELCHANGE(IDC_LIST1, &CImageShow::OnLbnSelchangeList1)
	ON_BN_CLICKED(IDC_BUTTON_TAG_ONLINE, &CImageShow::OnBnClickedButtonTagOnline)
	ON_BN_CLICKED(IDC_BUTTON_OldData, &CImageShow::OnBnClickedButtonOlddata)
END_MESSAGE_MAP()


// CImageShow message handlers

CRITICAL_SECTION    g_cs_ImageShow;

//串口讀線程函數
DWORD ReadThread_ImageShow(LPVOID lparam)
{	
	int	actualReadLen=0;	//實際讀取的字節數
	int	willReadLen;
	SOCKADDR_IN tmpAddr;
	int tmpRecvLen;
	
	tmpRecvLen = sizeof(tmpAddr);

	CImageShow *pdlg;
	pdlg = (CImageShow *)lparam;

	pdlg->m_Receive_Data_Len = 0;
		
	while(pdlg->m_UDPSocket != NULL)
	{ 		
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
			Sleep(10);
			continue;
		}
		pdlg->m_Receive_Data_Len += actualReadLen;

		EnterCriticalSection(&g_cs_ImageShow); 
		pdlg->ParseData();
		LeaveCriticalSection(&g_cs_ImageShow); 
		
	}
	return 0;
}


#define		PACKAGE_LEN		13
void CImageShow::ParseData(void)
{
	int start;
	while(m_Receive_Data_Len >= 3)
	{
		//查找開始標志FC
		start = FindChar(m_Receive_Data_Char, 0, m_Receive_Data_Len, 0xFC);
		if(start < 0){
			//全部丟掉
			m_Receive_Data_Len = 0;		
			break;
		}

		//找到開始標志
		//判斷從開始標志開始，是否有接收封包類型的字節
		if(start + 1 <= m_Receive_Data_Len)
		{
			//判斷數據封包類型
			switch(m_Receive_Data_Char[start+1])
			{
			case 1:		//普通定位
			case 2:		//緊急定位，固定13Byte
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				if(start + 13 <= m_Receive_Data_Len)
				{
					//有接收到一個包的長度，判斷包尾格式		
					if(0xFB == m_Receive_Data_Char[start+PACKAGE_LEN-1])
					{
						//包尾格式匹配，計算校驗和
						BYTE sum = 0;
						for(int i=0; i<PACKAGE_LEN-2; i++)
							sum += m_Receive_Data_Char[start+i];
						if(sum == m_Receive_Data_Char[start+PACKAGE_LEN-2])
						{
							//校驗和匹配，認為是正確的數據包								
							//定位數據
							//更新參考點接收到數據包的時間
							int id;
							id = FindImagePortID(m_Receive_Data_Char+start+4);
							if(id >= 0)
							{
								m_ImagePortInfo[id].LastReceData = GetTickCount();
							}
							//把數據抓取出來
							m_TagID[0] = m_Receive_Data_Char[start+2];
							m_TagID[1] = m_Receive_Data_Char[start+3];
							//判斷這個TAG之前是否保存過
							id = -1;
							for(int i=0; i<m_Cur_ReceiveCount; i++)
							{
								if(memcmp(m_TagID, m_ReceiveInfo[i].TagId, 2) == 0)
								{
									id = i;
									break;
								}
							}
							CString str;
							if(id >= 0)
							{
								//判斷接收到的序列號是否一樣，
								if(m_Receive_Data_Char[start+10] == m_ReceiveInfo[id].index)
								{
									//兩次的序列號一樣，是同一批的數據包，從不同的數據節點發送來
									//比較新的參考點，是否有比舊的點更近
									if(m_Receive_Data_Char[start+6] < m_ReceiveInfo[id].Rssi1)
									{
										//新參考點比舊的第一個參考點近，
										//以前保存的參考點信息后移
										m_ReceiveInfo[id].Rssi3 = m_ReceiveInfo[id].Rssi2;
										m_ReceiveInfo[id].Port3Id[0] = m_ReceiveInfo[id].Port2Id[0];
										m_ReceiveInfo[id].Port3Id[1] = m_ReceiveInfo[id].Port2Id[1];

										m_ReceiveInfo[id].Rssi2 = m_ReceiveInfo[id].Rssi1;
										m_ReceiveInfo[id].Port2Id[0] = m_ReceiveInfo[id].Port1Id[0];
										m_ReceiveInfo[id].Port2Id[1] = m_ReceiveInfo[id].Port1Id[1];

										//替換第一個點
										m_ReceiveInfo[id].Port1Id[0] = m_Receive_Data_Char[start+4];
										m_ReceiveInfo[id].Port1Id[1] = m_Receive_Data_Char[start+5];
										m_ReceiveInfo[id].Rssi1 = m_Receive_Data_Char[start+6];
									}
									else if(m_Receive_Data_Char[start+6] < m_ReceiveInfo[id].Rssi2)
									{
										//新參考點比舊的第2個參考點近，
										//以前保存的參考點信息后移
										m_ReceiveInfo[id].Rssi3 = m_ReceiveInfo[id].Rssi2;
										m_ReceiveInfo[id].Port3Id[0] = m_ReceiveInfo[id].Port2Id[0];
										m_ReceiveInfo[id].Port3Id[1] = m_ReceiveInfo[id].Port2Id[1];

										//替換第2個點
										m_ReceiveInfo[id].Port2Id[0] = m_Receive_Data_Char[start+4];
										m_ReceiveInfo[id].Port2Id[1] = m_Receive_Data_Char[start+5];
										m_ReceiveInfo[id].Rssi2 = m_Receive_Data_Char[start+6];
									}
									else if(m_Receive_Data_Char[start+6] < m_ReceiveInfo[id].Rssi3)
									{
										//新參考點比舊的第3個參考點近										
										//替換第3個點
										m_ReceiveInfo[id].Port3Id[0] = m_Receive_Data_Char[start+4];
										m_ReceiveInfo[id].Port3Id[1] = m_Receive_Data_Char[start+5];
										m_ReceiveInfo[id].Rssi3 = m_Receive_Data_Char[start+6];
									}
									else
									{
										//比以前保存的都遠，不處理它
										;
									}
								}
								else
								{
									//兩次的序列號不一樣，是新一批次的數據包，把數據包直接保存下來
									m_ReceiveInfo[id].Type = isJinJiType(m_Receive_Data_Char[start+1]);//m_Receive_Data_Char[start+1];

									if(m_bAutoEmergency && m_ReceiveInfo[id].Type == 2)
									{
										m_CurShowType = SHOW_TYPE_EMERGENCY;									
									}
									if(m_bWarning_ShowEmergencyTag && m_ReceiveInfo[id].Type == 2)
									{
										CTime t = CTime::GetCurrentTime();
										CString str_name;
										str_name.Format(TEXT("%02X%02X"), m_ReceiveInfo[id].TagId[0], m_ReceiveInfo[id].TagId[1]);
										GetTagName(str_name);
										str.Format(TEXT("%04d-%02d-%02d %02d:%02d:%02d, %s 緊急定位信息\r\n"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond(), str_name);
										m_WarningInfo.Add(str);
									}

									m_ReceiveInfo[id].Port1Id[0] = m_Receive_Data_Char[start+4];
									m_ReceiveInfo[id].Port1Id[1] = m_Receive_Data_Char[start+5];
									m_ReceiveInfo[id].Rssi1 = m_Receive_Data_Char[start+6];

									m_ReceiveInfo[id].Battery = m_Receive_Data_Char[start+7];
									m_ReceiveInfo[id].SensorTime = (m_Receive_Data_Char[start+8] << 8) | m_Receive_Data_Char[start+9];

									//檢測低電量報警
									if(m_bWarning_ShowLowBattery && m_ReceiveInfo[id].Battery < m_Warning_LowBatteryValue)
									{
										CTime t = CTime::GetCurrentTime();
										CString str_name;
										str_name.Format(TEXT("%02X%02X"), m_ReceiveInfo[id].TagId[0], m_ReceiveInfo[id].TagId[1]);
										GetTagName(str_name);
										str.Format(TEXT("%04d-%02d-%02d %02d:%02d:%02d, %s 低電量報警，電量剩餘：%d\r\n"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond(), str_name, m_ReceiveInfo[id].Battery);
										m_WarningInfo.Add(str);
									}

									//如果是普通定位，且序列號是符合遞加規則的，那么統計出卡片發送數據的間隔時間
									if(m_ReceiveInfo[id].Type == 1 && (BYTE)(m_ReceiveInfo[id].index+1) == m_Receive_Data_Char[start+10])
									{
										m_ReceiveInfo[id].SendTimeOut = GetTickCount() - m_ReceiveInfo[id].FirstReceiveTime;
									}

									m_ReceiveInfo[id].index = m_Receive_Data_Char[start+10];
									m_ReceiveInfo[id].IsUpdate = TRUE;
									m_ReceiveInfo[id].FirstReceiveTime = GetTickCount();
									m_ReceiveInfo[id].Port2Id[0] = 0;
									m_ReceiveInfo[id].Port2Id[1] = 0;
									m_ReceiveInfo[id].Rssi2 = 0xFF;
									m_ReceiveInfo[id].Port3Id[0] = 0;
									m_ReceiveInfo[id].Port3Id[1] = 0;
									m_ReceiveInfo[id].Rssi3 = 0xFF;
									m_ReceiveInfo[id].IsAladyWarringTimeOut = FALSE;
									m_ReceiveInfo[id].IsAladySaveData = FALSE;
								}								
							}
							else
							{
								//之前沒有接收過這個TAG，添加
								if(m_Cur_ReceiveCount < MAX_RECEIVE_COUNT)
								{
									id = m_Cur_ReceiveCount;
									m_Cur_ReceiveCount++;
									m_ReceiveInfo[id].Type = isJinJiType(m_Receive_Data_Char[start+1]);//m_Receive_Data_Char[start+1];

									m_ReceiveInfo[id].TagId[0] = m_Receive_Data_Char[start+2];
									m_ReceiveInfo[id].TagId[1] = m_Receive_Data_Char[start+3];

									if(m_bAutoEmergency && m_ReceiveInfo[id].Type == 2)
									{
										m_CurShowType = SHOW_TYPE_EMERGENCY;									
									}
									if(m_bWarning_ShowEmergencyTag && m_ReceiveInfo[id].Type == 2)
									{
										CTime t = CTime::GetCurrentTime();
										CString str_name;
										str_name.Format(TEXT("%02X%02X"), m_ReceiveInfo[id].TagId[0], m_ReceiveInfo[id].TagId[1]);
										GetTagName(str_name);
										str.Format(TEXT("%04d-%02d-%02d %02d:%02d:%02d, %s 緊急定位信息\r\n"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond(), str_name);
										m_WarningInfo.Add(str);
									}

									m_ReceiveInfo[id].Port1Id[0] = m_Receive_Data_Char[start+4];
									m_ReceiveInfo[id].Port1Id[1] = m_Receive_Data_Char[start+5];
									m_ReceiveInfo[id].Rssi1 = m_Receive_Data_Char[start+6];

									m_ReceiveInfo[id].Battery = m_Receive_Data_Char[start+7];
									m_ReceiveInfo[id].SensorTime = (m_Receive_Data_Char[start+8] << 8) | m_Receive_Data_Char[start+9];

									//檢測低電量報警
									if(m_bWarning_ShowLowBattery && m_ReceiveInfo[id].Battery < m_Warning_LowBatteryValue)
									{
										CTime t = CTime::GetCurrentTime();
										CString str_name;
										str_name.Format(TEXT("%02X%02X"), m_ReceiveInfo[id].TagId[0], m_ReceiveInfo[id].TagId[1]);
										GetTagName(str_name);
										str.Format(TEXT("%04d-%02d-%02d %02d:%02d:%02d, %s 低電量報警，電量剩餘：%d\r\n"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond(), str_name, m_ReceiveInfo[id].Battery);
										m_WarningInfo.Add(str);
									}

									m_ReceiveInfo[id].index = m_Receive_Data_Char[start+10];
									m_ReceiveInfo[id].IsUpdate = TRUE;
									m_ReceiveInfo[id].FirstReceiveTime = GetTickCount();
									m_ReceiveInfo[id].ReceivData = TRUE;
									m_ReceiveInfo[id].Port2Id[0] = 0;
									m_ReceiveInfo[id].Port2Id[1] = 0;
									m_ReceiveInfo[id].Rssi2 = 0xFF;
									m_ReceiveInfo[id].Port3Id[0] = 0;
									m_ReceiveInfo[id].Port3Id[1] = 0;
									m_ReceiveInfo[id].Rssi3 = 0xFF;	

									m_ReceiveInfo[id].LastPortId[0] = 0;
									m_ReceiveInfo[id].LastPortId[1] = 0;
									m_ReceiveInfo[id].SendTimeOut = 0;
									m_ReceiveInfo[id].IsAladyWarringTimeOut = FALSE;
									m_ReceiveInfo[id].IsAladySaveData = FALSE;
									m_ReceiveInfo[id].ChangePortId[0] = 0;
									m_ReceiveInfo[id].ChangePortId[1] = 0;
									m_ReceiveInfo[id].ChangePortCount = 0;
									m_ReceiveInfo[id].NearPortId[0] = 0;
									m_ReceiveInfo[id].NearPortId[1] = 0;
								}
								else
								{
									//沒有空位置了，不添加
								}
							}
							//處理完了，丟掉處理過的數據包
							m_Receive_Data_Len = m_Receive_Data_Len - start - PACKAGE_LEN;
							memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+PACKAGE_LEN, m_Receive_Data_Len);
						}
						else
						{
							//校驗和失敗，丟掉數據包
							m_Receive_Data_Len = m_Receive_Data_Len - start - PACKAGE_LEN;
							memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+PACKAGE_LEN, m_Receive_Data_Len);
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
							return;
						}
					}
				}
				else
				{
					//未接收到整個封包的字節，等待收滿再處理
					if(start > 0)
					{
						//在數據包頭前，還有數據，把這些數據清除掉
						m_Receive_Data_Len = m_Receive_Data_Len - start;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start, m_Receive_Data_Len);
					}
					return;
				}
				break;
			case 3:		//位置參考點自動上報數據，固定6Byte
				if(start + 6 <= m_Receive_Data_Len)
				{
					//一個包的所有的數據已經收到
					//判斷包尾是否符合
					if(m_Receive_Data_Char[start+5] == 0xFB)
					{
						//包尾符合，計算校驗和
						BYTE sum = 0;
						int i;
						for(i=0; i<4; i++)
							sum += m_Receive_Data_Char[start+i];
						if(sum == m_Receive_Data_Char[start+4])
						{
							//校驗和一樣，認為是正確的包
							CString str;
							str.Format(TEXT("%02X%02X"), m_Receive_Data_Char[start+2], m_Receive_Data_Char[start+3]);
								
							for(i=0; i<m_ReceServerNet.GetCount(); i++)
							{
								if(0 == str.Compare(m_ReceServerNet.GetAt(i)))
								{
									//已經保存過，不添加
									break;
								}
							}
							if(i >= m_ReceServerNet.GetCount())
							{
								//沒有保存過，添加
								m_ReceServerNet.Add(str);
							}
						}
						else																
						{
							//校驗和不一樣，丟掉，不處理
						}
						//處理完畢，丟掉處理過的數據
						m_Receive_Data_Len = m_Receive_Data_Len - start - 6;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+6, m_Receive_Data_Len);
					}
					else
					{
						//包尾不符合，丟掉包頭和類型2個Byte
						m_Receive_Data_Len = m_Receive_Data_Len - start - 2;
						memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start+2, m_Receive_Data_Len);
					}
				}
				else
				{
					//一個包的數據沒有收到，等待接收完成
					return;
				}							
				break;
			default:	//位置數據包
				//沒有標志頭，全部丟掉
				m_Receive_Data_Len = 0;		
				break;
			}
		}
		else
		{
			//未接收到封包類型的字節，等待收滿再處理
			if(start > 0)
			{
				//在數據包頭前，還有數據，把這些數據清除掉
				m_Receive_Data_Len = m_Receive_Data_Len - start;
				memcpy(m_Receive_Data_Char, m_Receive_Data_Char+start, m_Receive_Data_Len);
			}else
				m_Receive_Data_Len = 0;
			break;
		}
		//}		
	}
}


////判断是否是紧急定位
BYTE CImageShow::isJinJiType(byte itemType){
	if (itemType == 0x02 || itemType == 0x06 || itemType == 0x08){
		return 2;
	}
	return 1;
}



int CImageShow::FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2)
{
	for(int i=start; i<end-1; i++)
	{
		if(str[i] == c1 && str[i+1] == c2)
			return i;
	}
	return -1;
}

int CImageShow::FindChar(BYTE *str, int start, int end, BYTE c1)
{
	for(int i=start; i<end; i++)
	{
		if(str[i] == c1)
			return i;
	}
	return -1;
}



VOID CImageShow::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}


BOOL CImageShow::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here

	m_button_connect.MoveWindow(5, 5, 100, 25);	
	m_button_set.MoveWindow(115, 5, 100, 25);
	m_button_tag_online.MoveWindow(225, 5, 100, 25);
	m_button_olddata.MoveWindow(335, 5, 100, 25);
	m_btn_warning_show.MoveWindow(445, 5, 100, 25);	
	m_edit_warning_info.MoveWindow(555, 7, 490, 20);
	

	m_Config.Create(IDD_DIALOG_CONFIG);
	m_WarningMessage.Create(IDD_DIALOG_WARNING_INFO);
	m_ImagePortShow.Create(IDD_DIALOG_IMAGESHOW_PORTSHOW);
	m_TagOnLine.Create(IDD_DIALOG_TAG_ONLINE);
	m_OldData.Create(IDD_DIALOG_OLDDATA);

	m_Config.ShowWindow(SW_HIDE);
	m_WarningMessage.ShowWindow(SW_HIDE);
	m_ImagePortShow.ShowWindow(SW_HIDE);
	m_TagOnLine.ShowWindow(SW_HIDE);
	m_OldData.ShowWindow(SW_HIDE);

	m_Config.SetMainWindowHwnd(this->GetSafeHwnd());
	m_WarningMessage.SetMainHwnd(this->GetSafeHwnd());
	m_ImagePortShow.SetMainHwnd(this->GetSafeHwnd());
	m_TagOnLine.SetMainHwnd(this->GetSafeHwnd());

	m_font.CreateFont(12, 0, 0, 0, 0, 0, 0, 0, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,	DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, TEXT("宋體"));

	m_bConnect = FALSE;
	m_bLoadImage = FALSE;
	m_bShowTagOnLineDlg = FALSE;
	m_SaveDataHandle = NULL;
	
	m_CheckList.SetCheckStyle(BS_CHECKBOX);
	
	m_StaticShow_left = 5;
	m_StaticShow_top = 40;
	m_StaticShow_width = 200;

	m_ImageLeft = m_StaticShow_width+5;
	m_ImageTop = m_StaticShow_top;

	m_bCheckBox_all = TRUE;
	m_check_all.SetCheck(1);
	
	m_Cur_DlgPortShowPortIndex = -2;
	m_bDlgPortShowLButtonDown = FALSE;

	m_Brush_white = ::CreateSolidBrush(RGB(255, 255, 255));
	m_Brush_Port = ::CreateSolidBrush(RGB(0, 255, 0));
	m_Brush_Tag_Emergency = ::CreateSolidBrush(RGB(255, 0, 0));
	m_Brush_Tag_Normal = ::CreateSolidBrush(RGB(0, 0, 255));
	m_Brush_Tag_Warring = ::CreateSolidBrush(RGB(255, 0, 255));
	m_Brush_Tag_NoMove = ::CreateSolidBrush(RGB(0, 0, 0));

	m_Pen_Emergency = ::CreatePen(PS_SOLID, 1, RGB(255, 0, 0));
	m_Pen_Normal = ::CreatePen(PS_SOLID, 1, RGB(0, 0, 255));
	m_Pen_Warring = ::CreatePen(PS_SOLID, 1, RGB(255, 0, 255));
	m_Pen_NoMove = ::CreatePen(PS_SOLID, 1, RGB(0, 0, 0));

	m_hdc = ::GetDC(this->GetSafeHwnd());
	m_mdc = CreateCompatibleDC(m_hdc);
	m_ndc = CreateCompatibleDC(m_hdc);

	srand(GetTickCount());

	m_CurImagePortCount = 0;	
	m_bImagePortLButtonDown = FALSE;

	InitializeCriticalSection(&g_cs_ImageShow);

	//裝載保存的信息
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	if(m_ini.Open(filepath, FALSE))
	{		
		CString str, str2;
		if(m_ini.GetValue(TEXT("Map"), TEXT("FilePath"), m_ImageFile))
		{
			m_SaveImageFile = m_ImageFile;			
		//	MyLoadImage();
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowReferencePoint"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_bShowReferencePort = TRUE;
			else
				m_bShowReferencePort = FALSE;
		}
		else
		{
			m_bShowReferencePort = TRUE;
		}
	}

	m_SaveImageWidth = 0;
	m_SaveImageHeight = 0;
	m_CurImagePortCount = 0;	
	filepath = szPath;
	filepath = filepath + TEXT("config\\SavePort.ini");	
	if(m_ini.Open(filepath, FALSE))
	{
		CString str, str2;
		m_CurImagePortCount = 0;	
		if(m_ini.GetValue(TEXT("Picture"), TEXT("Width"), str))
		{		
			m_SaveImageWidth = _ttoi(str);
		}		
		if(m_ini.GetValue(TEXT("Picture"), TEXT("Height"), str))
		{		
			m_SaveImageHeight = _ttoi(str);
		}		

		for(; m_CurImagePortCount<MAX_IMAGEPORT_COUNT; m_CurImagePortCount++)
		{
			str2.Format(TEXT("Port%d"), m_CurImagePortCount);
			if(!m_ini.GetValue(str2, TEXT("ID"), str))
			{		
				//讀完了						
				break;
			}
			if(str.GetLength() != 4)
			{				
				break;
			}
			if(!StringToChar(str, m_ImagePortInfo[m_CurImagePortCount].Id))
			{				
				break;
			}
			if(!m_ini.GetValue(str2, TEXT("x"), str))
			{		
				//讀完了				
				break;					
			}
			m_ImagePortInfo[m_CurImagePortCount].x = _ttoi(str);
			if(!m_ini.GetValue(str2, TEXT("y"), str))
			{		
				//讀完了				
				break;					
			}
			m_ImagePortInfo[m_CurImagePortCount].y = _ttoi(str);
			memset(m_ImagePortInfo[m_CurImagePortCount].DrawTagId, 0, sizeof(m_ImagePortInfo[m_CurImagePortCount].DrawTagId));
		}
	}

	m_Cur_ReceiveCount = 0;

	//1.初始化socket資源
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2,2),&wsa) != 0)
	{
		return TRUE;//代表失敗
	}

	GetSaveTagInfo();
	GetSavePortInfo();
	GetSaveServerNetInfo();

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

BOOL CImageShow::GetSavePortInfo(void)
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

	m_ImagePortShow.UpdateSavePort(m_SavePortInfo, m_Cur_SavePortCount);
	return TRUE;
}

bool CImageShow::StringToChar(CString str, BYTE* data)
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
		datalen++;
		i++;
	//	return true;			
	}		
	return true;
}


void CImageShow::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	// TODO: Add your message handler code here
	PaintImage();
	// Do not call CDialog::OnPaint() for painting messages
}

int	CImageShow::IsShowTag(BYTE id1, BYTE id2)
{
	CString str;
	str.Format(TEXT("%02X%02X"), id1, id2);
	for(int i=0; i<m_Cur_SaveTagCount; i++)
	{
		if(str.Compare(m_SaveTagInfo[i].ID) == 0)
			return m_SaveTagInfo[i].Show;
	}
	return SHOW_TAG_UNKNOW;
}

void CImageShow::PaintImage()
{
	int x, y;
	RECT r;	
	CRect cr;		
	CString str;
	BYTE bshow;
	m_ShowTagLeftTopCount = 0;
	int count;
	int jinjicount;
	DWORD lmr;

	//每次畫之前，要把位置參考點的“重疊區”的數量清空
	for(int i=0; i<m_CurImagePortCount; i++)
		m_ImagePortInfo[i].DrawTagCount = 0;
		
	if(m_bLoadImage)
	{
		//先添加背景
		BitBlt(m_mdc, 0, 0, m_ImageWidth, m_ImageHeight, m_ndc, 0, 0, SRCCOPY);
		//畫Port
		SelectObject(m_mdc, (HFONT)m_font);
	//	SetBkMode(m_mdc, TRANSPARENT);
		SetTextColor(m_mdc, RGB(0, 255, 0));
		if(m_bShowReferencePort)
		{			
			for(int i=0; i<m_CurImagePortCount; i++)
			{
				r.left = m_ImagePortInfo[i].x-10;
				r.top = m_ImagePortInfo[i].y-7;
				r.right = m_ImagePortInfo[i].x+10;
				r.bottom = m_ImagePortInfo[i].y+7;
				if(GetTickCount() - m_ImagePortInfo[i].LastReceData > m_Warning_ServerNetTime*1000)
				{
					//位置參考點超時沒有收到數據包，用紅色顯示
					SetTextColor(m_mdc, RGB(255, 0, 0));
					::FillRect(m_mdc, &r, m_Brush_Tag_Emergency);
				}
				else
				{
					SetTextColor(m_mdc, RGB(0, 255, 0));
					::FillRect(m_mdc, &r, m_Brush_Port);
				}
				cr.SetRect(r.right+2, r.top, r.right+100, r.bottom);
				str.Format(TEXT("%02X%02X"), m_ImagePortInfo[i].Id[0], m_ImagePortInfo[i].Id[1]);
				GetPortName(str);
				::DrawText(m_mdc, str, -1, &cr, DT_LEFT|DT_VCENTER|DT_SINGLELINE);
			}
		}
		//畫Tag
		if(m_bConnect)
		{	if(m_CurShowType == SHOW_TYPE_ALL)
			{		
				//顯示所有的點
				for(int i=0; i<m_Cur_ReceiveCount; i++)
				{	
					//判斷每一個卡片是否是在允許顯示的列表中的
					bshow = IsShowTag(m_ReceiveInfo[i].TagId[0], m_ReceiveInfo[i].TagId[1]);
					if(bshow == SHOW_TAG_YES || bshow == SHOW_TAG_UNKNOW)
					{
						//顯示TAG
						//獲取畫圖坐標
						if(GetTagCoordinate(m_ReceiveInfo[i], x, y, count, lmr))
						{
							if(count == 0)
							{
								//單個顯示
								//根據定位信息的類型來用不同顏色來畫圖
								if(m_ReceiveInfo[i].Type == 2)
								{
									//緊急定位
									SetTextColor(m_mdc, RGB(255, 0, 0));
									SelectObject(m_mdc, m_Pen_Emergency);
									SelectObject(m_mdc, m_Brush_Tag_Emergency);
								}
								else
								{
									//普通定位，
									if(m_bShowNoMoveTag && m_ReceiveInfo[i].SensorTime != 0xFFFF && m_ReceiveInfo[i].SensorTime > m_ShowNoMoveTagTime)
									{
										//卡片超時未移動，用黑色顯示
										SetTextColor(m_mdc, RGB(0, 0, 0));
										SelectObject(m_mdc, m_Pen_NoMove);
										SelectObject(m_mdc, m_Brush_Tag_NoMove);
									}
									else
									{
										//檢查接收卡片的定位信息是否有超時
										if(m_ReceiveInfo[i].SendTimeOut != 0 && (GetTickCount() - m_ReceiveInfo[i].FirstReceiveTime) > m_ReceiveInfo[i].SendTimeOut*2)
										{
											//超時沒有接收到下一次的定位數據，以警告顏色顯示
											SetTextColor(m_mdc, RGB(255, 0, 255));
											SelectObject(m_mdc, m_Pen_Warring);
											SelectObject(m_mdc, m_Brush_Tag_Warring);
										}
										else
										{
											//正常接收到了數據包，以正常顏色顯示
											SetTextColor(m_mdc, RGB(0, 0, 255));
											SelectObject(m_mdc, m_Pen_Normal);
											SelectObject(m_mdc, m_Brush_Tag_Normal);
										}
									}
								}
							}
							else
							{
								//多個顯示
								SetBkMode(m_mdc, TRANSPARENT);
								SetTextColor(m_mdc, RGB(255, 255, 255));
								SelectObject(m_mdc, m_Pen_Normal);
								SelectObject(m_mdc, m_Brush_Tag_Normal);
							}
							Ellipse(m_mdc, x-7, y-7, x+7, y+7);
							//顯示定位卡片的名稱	
							
							if(count == 0)
							{
								if(lmr == DT_LEFT)
									cr.SetRect(x+10, y-7, x+150, y+7);
								else
									cr.SetRect(x-150, y-7, x-10, y+7);
								str.Format(TEXT("%02X%02X"), m_ReceiveInfo[i].TagId[0], m_ReceiveInfo[i].TagId[1]);
								GetTagName(str);													
							}
							else
							{
								cr.SetRect(x-10, y-7, x+10, y+7);
								str.Format(TEXT("%d"), count);												
							}
							::DrawText(m_mdc, str, -1, &cr, lmr|DT_VCENTER|DT_SINGLELINE);		
						}	
					}
					else
					{
						//對于設定了不顯示的卡片，要檢查它之前是否有顯示到了某個參考點周圍
						if(m_ReceiveInfo[i].LastPortId[0] != 0 || m_ReceiveInfo[i].LastPortId[1] != 0)
						{
							//之前有顯示在某個參考點周圍，把該參考點的顯示位置信息抹掉
							ClearPortDrawTagId(m_ReceiveInfo[i].LastPortId, m_ReceiveInfo[i].TagId);
							//信息抹掉后，把上一次的位置參考點的ID抹掉
							m_ReceiveInfo[i].LastPortId[0] = 0;
							m_ReceiveInfo[i].LastPortId[1] = 0;
						}
					}
				}
			}
			else if(m_CurShowType == SHOW_TYPE_EMERGENCY)
			{
				jinjicount = 0;
				//只顯示緊急的定位信息
				for(int i=0; i<m_Cur_ReceiveCount; i++)
				{	
					if(m_ReceiveInfo[i].Type == 2 && GetTagCoordinate(m_ReceiveInfo[i], x, y, count, lmr))
					{	
						jinjicount++;

						SetTextColor(m_mdc, RGB(255, 0, 0));
						SelectObject(m_mdc, m_Pen_Emergency);
						SelectObject(m_mdc, m_Brush_Tag_Emergency);	
						Ellipse(m_mdc, x-7, y-7, x+7, y+7);
						//顯示定位卡片的名稱							
						if(count == 0)
						{
							if(lmr == DT_LEFT)
								cr.SetRect(x+10, y-7, x+150, y+7);
							else
								cr.SetRect(x-150, y-7, x-10, y+7);
							str.Format(TEXT("%02X%02X"), m_ReceiveInfo[i].TagId[0], m_ReceiveInfo[i].TagId[1]);
							GetTagName(str);							
						}
						else
						{
							SetBkMode(m_mdc, TRANSPARENT);
							SetTextColor(m_mdc, RGB(255, 255, 255));
							cr.SetRect(x-10, y-7, x+10, y+7);
							str.Format(TEXT("%d"), count);							
						}
						::DrawText(m_mdc, str, -1, &cr, lmr|DT_VCENTER|DT_SINGLELINE);								
					}				
				}
				if(jinjicount == 0)
				{
					//沒有發現緊急定位，退出這個模式
					m_CurShowType = SHOW_TYPE_ALL;
				}
			}			
		}
		SetBkMode(m_mdc, OPAQUE);
		//
		BitBlt(m_hdc, m_ImageLeft, m_ImageTop, m_ImageWidth, m_ImageHeight, m_mdc, 0, 0, SRCCOPY);
	}
	else
	{		
		r.left = m_ImageLeft;
		r.top = m_ImageTop;
		r.right = m_ImageWidth + m_ImageLeft;
		r.bottom = m_ImageHeight + m_ImageTop;
		::FillRect(m_hdc, &r, m_Brush_white);
		::MoveToEx(m_hdc, r.left, r.top, NULL);
		::LineTo(m_hdc, r.right-1, r.top);
		::LineTo(m_hdc, r.right-1, r.bottom-1);
		::LineTo(m_hdc, r.left, r.bottom-1);
		::LineTo(m_hdc, r.left, r.top);
		SetBkMode(m_hdc, TRANSPARENT);		
		cr.SetRect(r.left, r.top, r.right, r.bottom);
		::DrawText(m_hdc, TEXT("請裝載背景圖片"), -1, &cr, DT_CENTER|DT_VCENTER|DT_SINGLELINE);
	}
}

BOOL CImageShow::GetTagCoordinate(ImageShowReceiveInfo &isri, int &x, int &y, int &count, DWORD &lmr)
{
//	int index;
	int i;
	int ipi;
	//先檢查，最後一次獲取到新數據的時間，與現在時間的間隔
	if(GetTickCount() - isri.FirstReceiveTime < 200)
	{
		//等收到第一個數據包后200毫秒再畫
		//判斷上一次的參考點是否存在
		if(isri.LastPortId[0] != 0 || isri.LastPortId[1] != 0)
		{			
			//上一次有參考點，用上一次的坐標畫圖
			ipi = FindImagePortID(isri.LastPortId);	
			if(ipi >= 0)
			{
				//查看上次的畫點在什么地方
				for(i=0; i<MAX_DRAWTAG; i++)
				{
					if(isri.TagId[0] == m_ImagePortInfo[ipi].DrawTagId[i][0] && isri.TagId[1] == m_ImagePortInfo[ipi].DrawTagId[i][1])
					{
						//找到上次畫點的位置，本次還是使用上次點位的位置
						x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset[i][0];
						y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset[i][1];
						count = 0;
						if(i>= 5)
							lmr = DT_RIGHT;
						else
							lmr = DT_LEFT;
						return TRUE;
					}
				}
				//找完一圈，沒有找到，說明上次是顯示在“重疊區”的
				//再來，先查找一次現在這個參考點周圍，有沒有空位置了
				for(i=0; i<MAX_DRAWTAG; i++)
				{
					if(0 == m_ImagePortInfo[ipi].DrawTagId[i][0] && 0 == m_ImagePortInfo[ipi].DrawTagId[i][1])
					{
						//找到一個空位置，使用這個空位置，把卡片的ID號存入，占位
						m_ImagePortInfo[ipi].DrawTagId[i][0] = isri.TagId[0];
						m_ImagePortInfo[ipi].DrawTagId[i][1] = isri.TagId[1];
						//返回數據
						x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset[i][0];
						y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset[i][1];
						count = 0;
						if(i>= 5)
							lmr = DT_RIGHT;
						else
							lmr = DT_LEFT;
						return TRUE;
					}
				}
				//找完一圈，沒有找到空位置，還是使用“重疊區”
				x = m_ImagePortInfo[ipi].x + g_DrawTagXYChongDieQu[0];
				y = m_ImagePortInfo[ipi].y + g_DrawTagXYChongDieQu[1];
				m_ImagePortInfo[ipi].DrawTagCount++;
				count = m_ImagePortInfo[ipi].DrawTagCount;
				lmr = DT_CENTER;
			}
			else
			{
				//參考點不在地圖上，顯示在左上角
				m_ShowTagLeftTopCount++;
				x = 10; y = 10;	
				count = m_ShowTagLeftTopCount;	//只顯示數字
				lmr = DT_CENTER;	
			}
			return TRUE;	
		}
		else
		{
			//上一次無參考點，極有可能是第一次顯示的卡片，讓下面按照正常步驟來畫
			;		
		}
	}
	//否則，現在距離上一次畫點已經超過200，開始畫圖

	//查找，這個參考點，是否有設定到了地圖上？
	ipi = FindImagePortID(isri.NearPortId);	
	if(ipi >= 0)
	{
		//參考點有在地圖上
		//先查找這個卡片，上次畫的參考點的ID，是否和這一次的是否一樣
		if(isri.LastPortId[0] == isri.NearPortId[0] && isri.LastPortId[1] == isri.NearPortId[1])
		{
			//上次顯示的參考點和這次的一樣，查看上次的畫點在什么地方
			for(i=0; i<MAX_DRAWTAG; i++)
			{
				if(isri.TagId[0] == m_ImagePortInfo[ipi].DrawTagId[i][0] && isri.TagId[1] == m_ImagePortInfo[ipi].DrawTagId[i][1])
				{
					//找到上次畫點的位置，本次還是使用上次點位的位置
					x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset[i][0];
					y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset[i][1];
					count = 0;
					if(i>= 5)
						lmr = DT_RIGHT;
					else
						lmr = DT_LEFT;
					return TRUE;
				}
			}
			//找完一圈，沒有找到，說明上次是顯示在“重疊區”的
			//再來，先查找一次現在這個參考點周圍，有沒有空位置了
			for(i=0; i<MAX_DRAWTAG; i++)
			{
				if(0 == m_ImagePortInfo[ipi].DrawTagId[i][0] && 0 == m_ImagePortInfo[ipi].DrawTagId[i][1])
				{
					//找到一個空位置，使用這個空位置，把卡片的ID號存入，占位
					m_ImagePortInfo[ipi].DrawTagId[i][0] = isri.TagId[0];
					m_ImagePortInfo[ipi].DrawTagId[i][1] = isri.TagId[1];
					//返回數據
					x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset[i][0];
					y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset[i][1];
					count = 0;
					if(i>= 5)
						lmr = DT_RIGHT;
					else
						lmr = DT_LEFT;
					return TRUE;
				}
			}
			//找完一圈，沒有找到空位置，還是使用“重疊區”
			x = m_ImagePortInfo[ipi].x + g_DrawTagXYChongDieQu[0];
			y = m_ImagePortInfo[ipi].y + g_DrawTagXYChongDieQu[1];
			m_ImagePortInfo[ipi].DrawTagCount++;
			count = m_ImagePortInfo[ipi].DrawTagCount;
			lmr = DT_CENTER;
			return TRUE;			
		}
		else
		{
			//上一次顯示的點和本次顯示的點，不一樣，重新查找點來顯示
			//在查找新點之前，先把舊點位的占用的位置信息抹掉			
			ClearPortDrawTagId(isri.LastPortId, isri.TagId);			
			//把在之前參考點的占有的畫點位置信息抹掉后，在新參考點周圍查找空位來占有
			for(i=0; i<MAX_DRAWTAG; i++)
			{
				if(0 == m_ImagePortInfo[ipi].DrawTagId[i][0] && 0 == m_ImagePortInfo[ipi].DrawTagId[i][1])
				{
					//找到一個空位置，使用這個空位置，把卡片的ID號存入，占位
					m_ImagePortInfo[ipi].DrawTagId[i][0] = isri.TagId[0];
					m_ImagePortInfo[ipi].DrawTagId[i][1] = isri.TagId[1];
					isri.LastPortId[0] = isri.NearPortId[0];
					isri.LastPortId[1] = isri.NearPortId[1];
					//返回數據
					x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset[i][0];
					y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset[i][1];
					count = 0;
					if(i>= 5)
						lmr = DT_RIGHT;
					else
						lmr = DT_LEFT;
					return TRUE;
				}
			}
			//找完一圈，沒有找到空位置，使用“重疊區”
			isri.LastPortId[0] = isri.NearPortId[0];
			isri.LastPortId[1] = isri.NearPortId[1];

			x = m_ImagePortInfo[ipi].x + g_DrawTagXYChongDieQu[0];
			y = m_ImagePortInfo[ipi].y + g_DrawTagXYChongDieQu[1];
			m_ImagePortInfo[ipi].DrawTagCount++;
			count = m_ImagePortInfo[ipi].DrawTagCount;
			lmr = DT_CENTER;
			return TRUE;
		}
	}
	//在地圖上沒有找到這個參考點，顯示在地圖的左上角
	m_ShowTagLeftTopCount++;
	x = 10; y = 10;	
	count = m_ShowTagLeftTopCount;	//只顯示數字
	lmr = DT_CENTER;	

	return TRUE;
}
int CImageShow::FindImagePortID(BYTE id[])
{
	for(int i=0; i<m_CurImagePortCount; i++)
	{
		if(id[0] == m_ImagePortInfo[i].Id[0] && id[1] == m_ImagePortInfo[i].Id[1])
		{						
			return i;
		}
	}
	return -1;
}

void CImageShow::ClearPortDrawTagId(BYTE portid[], BYTE tagid[])
{
	//先找到上一次顯示點的參考點
	int index = FindImagePortID(portid);
	if(index >= 0)
	{
		//上一次顯示的點存在，把上一次顯示點的信息抹掉
		//查找在上一個參考點，是畫在什么點位上的
		for(int i=0; i<MAX_DRAWTAG; i++)
		{
			if(tagid[0] == m_ImagePortInfo[index].DrawTagId[i][0] && tagid[1] == m_ImagePortInfo[index].DrawTagId[i][1])
			{
				//找到上次畫點的位置，把上一次顯示點的信息抹掉，空出位置來
				m_ImagePortInfo[index].DrawTagId[i][0] = 0;
				m_ImagePortInfo[index].DrawTagId[i][1] = 0;
				break;	//跳出查找
			}
		}
		//找完一圈，沒找到，說明上次是顯示在“重疊區”的，無需抹去操作
	}
}

void CImageShow::SrandOnePortXY(ImagePortInfo ipi, int &x, int &y)
{
	int r = 20;				
	x = rand()%(r+1);
	y = (int)sqrt((double)(r*r - x*x));
	if(rand()%2 == 0)
		x = ipi.x + x;
	else
		x = ipi.x - x;
	if(rand()%2 == 0)
		y = ipi.y + y;
	else
		y = ipi.y - y;
}



BOOL CImageShow::MyLoadImage()
{
	HBITMAP hBitmap = (HBITMAP)::LoadImage(NULL, m_ImageFile, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE|LR_CREATEDIBSECTION|LR_DEFAULTSIZE);
	if(hBitmap == NULL)
	{
		m_bLoadImage = FALSE;
		return FALSE;
	}
	BITMAP bm;
    GetObject( hBitmap, sizeof(BITMAP), &bm);
	int width = bm.bmWidth;
	int height = bm.bmHeight;
	//計算圖片等比例縮放
	int left = 0;
	int top = 0;
	
/*
	if(width < m_ImageWidth && height < m_ImageHeight)
	{
		//以寬為標準來放大
		if((height*m_ImageWidth)/width > m_ImageHeight)
		{
			//放大后的高度超出畫框，那么以高為標準來放大
			width = width * m_ImageHeight/height;
			m_ImageLeft = m_ImageLeft + (m_ImageWidth - width)/2;
			m_ImageWidth = width;
		}
		else
		{
			//放大后的高度不超出畫框，符合條件
			height = height * m_ImageWidth/width;
			m_ImageTop = m_ImageTop + (m_ImageHeight - height)/2;
			m_ImageHeight = height;
		}
	}
	else if(width >= m_ImageWidth && height < m_ImageHeight)
	{
		//以寬為標準來縮小
		height = height * m_ImageWidth/width;
		m_ImageTop = m_ImageTop + (m_ImageHeight - height)/2;
		m_ImageHeight = height;
	}
	else if(width < m_ImageWidth && height >= m_ImageHeight)
	{
		//以高位標準來縮小
		width = width * m_ImageHeight/height;
		m_ImageLeft = m_ImageLeft + (m_ImageWidth - width)/2;
		m_ImageWidth = width;
	}
	else	//寬和高都大于畫框
	{
		//以寬來縮小試探
		if((height*m_ImageWidth)/width > m_ImageHeight)
		{
			//縮小后的高，大于畫框，換以高位標準縮小
			width = width * m_ImageHeight/height;
			m_ImageLeft = m_ImageLeft + (m_ImageWidth - width)/2;
			m_ImageWidth = width;
		}
		else
		{
			//以寬為標準來縮小
			height = height * m_ImageWidth/width;
			m_ImageTop = m_ImageTop + (m_ImageHeight - height)/2;
			m_ImageHeight = height;
		}
	}
	*/
	m_bmp_ndc = CreateCompatibleBitmap(m_hdc, m_ImageWidth, m_ImageHeight);
	m_bmp_mdc = CreateCompatibleBitmap(m_hdc, m_ImageWidth, m_ImageHeight);
	SelectObject(m_ndc, m_bmp_ndc);
	SelectObject(m_mdc, hBitmap);
	StretchBlt(m_ndc, 0, 0, m_ImageWidth, m_ImageHeight, m_mdc, 0, 0, bm.bmWidth, bm.bmHeight, SRCCOPY);
	::MoveToEx(m_ndc, 0, 0, NULL);
	::LineTo(m_ndc, m_ImageWidth-1, 0);
	::LineTo(m_ndc, m_ImageWidth-1, m_ImageHeight-1);
	::LineTo(m_ndc, 0, m_ImageHeight-1);
	::LineTo(m_ndc, 0, 0);
	SelectObject(m_mdc, m_bmp_mdc);
//	BitBlt(m_hdc, m_ImageLeft, m_ImageTop, m_ImageWidth, m_ImageHeight, m_ndc, 0, 0, SRCCOPY);

	m_bLoadImage = TRUE;
	return TRUE;
}

void CImageShow::OnBnClickedButtonLoadimage()
{
	// TODO: Add your control notification handler code here
	CFileDialog dlg( TRUE, _T("bmp"), NULL, OFN_FILEMUSTEXIST|OFN_HIDEREADONLY, _T( "bmp|*.bmp||" ) );
	if(dlg.DoModal() == IDOK)
	{
		m_ImageFile = dlg.GetPathName();		
		m_ImageLeft = 5;
		m_ImageTop = 40;
		m_ImageWidth = 985;
		m_ImageHeight = 520;
		if(MyLoadImage())
		{
			if(m_ImageFile.Compare(m_SaveImageFile) != 0)
			{
				//背景更換了，重設ImagePort的位置				
				int left = 10, top = 10;
				for(int i=0; i<m_CurImagePortCount; i++)
				{
					if(top > m_ImageHeight -10)
					{
						left += 100;
						top = 10;
					}
					m_ImagePortInfo[i].x = left;
					m_ImagePortInfo[i].y = top;
					top += 20;
				}
			}
			RedrawWindow();
			SaveImage();
		}
		else
			MessageBox(TEXT("裝載圖片失敗！"));
	}
}

BOOL CImageShow::SaveImage()
{
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\SavePort.ini");	
	CFile	cFile;
	if(!cFile.Open(filepath, CFile::modeCreate|CFile::modeWrite))
	{
		MessageBox(TEXT("Create file fail!"));
		return FALSE;
	}
	BYTE buf[] = {0xFF, 0xFE};
	cFile.Write(buf, 2);
	CString str_save, str;	

	//先儲存一個當前的圖片的寬和高
	str_save = TEXT("[Picture]\r\n");
	str.Format(TEXT("Width=%d\r\n"), m_ImageWidth);
	str_save += str;
	str.Format(TEXT("Height=%d\r\n"), m_ImageHeight);
	str_save += str;
	cFile.Write(str_save, str_save.GetLength()*2);
	m_SaveImageWidth = m_ImageWidth;
	m_ImageHeight = m_ImageHeight;

	str_save = TEXT("");
	for(int i=0; i<m_CurImagePortCount; i++)
	{
		str.Format(TEXT("[Port%d]\r\n"), i);
		str_save += str;
		str.Format(TEXT("ID=%02X%02X\r\n"), m_ImagePortInfo[i].Id[0], m_ImagePortInfo[i].Id[1]);
		str_save += str;
		str.Format(TEXT("x=%d\r\n"), m_ImagePortInfo[i].x);
		str_save += str;
		str.Format(TEXT("y=%d\r\n"), m_ImagePortInfo[i].y);
		str_save += str;		
	}

	str_save += TEXT("\r\n");
	cFile.Write(str_save, str_save.GetLength()*2);
	cFile.Close();
	return TRUE;
}

void CImageShow::OnLButtonDblClk(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	//雙擊，添加位置參考點
	CRect rect(m_ImageLeft, m_ImageTop, m_ImageWidth + m_ImageLeft, m_ImageHeight + m_ImageTop);
	if(!m_bConnect && m_bLoadImage && rect.PtInRect(point))
	{
		//雙擊，添加位置參考點
		if(m_CurImagePortCount < MAX_IMAGEPORT_COUNT)
		{
			m_CurImagePortIndex = m_CurImagePortCount;
			m_ImagePortInfo[m_CurImagePortIndex].x = point.x - m_ImageLeft;
			m_ImagePortInfo[m_CurImagePortIndex].y = point.y - m_ImageTop;
			m_ImagePortInfo[m_CurImagePortIndex].Id[0] = 0;
			m_ImagePortInfo[m_CurImagePortIndex].Id[1] = 0;
			memset(m_ImagePortInfo[m_CurImagePortIndex].DrawTagId, 0, sizeof(m_ImagePortInfo[m_CurImagePortIndex].DrawTagId));
			m_CurImagePortCount++;
			m_SetImagePort.SetParent(this);
			m_SetImagePort.SetID(0, 0);

			m_bShowReferencePort = TRUE;

			RedrawWindow();
			m_SetImagePort.DoModal();
			
		}
	}

	CDialog::OnLButtonDblClk(nFlags, point);
}

void CImageShow::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	if(m_bConnect)
	{
		//開始了監控，查看是否點擊了參考點
		CRect rect;
		for(int i=0; i<m_CurImagePortCount; i++)
		{
			rect.SetRect(m_ImagePortInfo[i].x-10 + m_ImageLeft, m_ImagePortInfo[i].y-6 + m_ImageTop, m_ImagePortInfo[i].x + m_ImageLeft+10, m_ImagePortInfo[i].y + m_ImageTop+6);
			if(rect.PtInRect(point))
			{
				m_bDlgPortShowLButtonDown = TRUE;
				m_DlgPortShowPort_LButtonDownIndex = i;
				return;
			}
		}
		//檢查是否移動到左上角位置
		rect.SetRect(m_ImageLeft, 4 + m_ImageTop, m_ImageLeft+20, 16 + m_ImageTop);
		if(rect.PtInRect(point))
		{
			m_bDlgPortShowLButtonDown = TRUE;
			m_DlgPortShowPort_LButtonDownIndex = -1;	// -1 = 左上角的點
			return;
		}
	}
	else if(!m_bConnect && m_bLoadImage && m_bShowReferencePort)
	{
		CRect rect;
		for(int i=0; i<m_CurImagePortCount; i++)
		{
			rect.SetRect(m_ImagePortInfo[i].x-10 + m_ImageLeft, m_ImagePortInfo[i].y-6 + m_ImageTop, m_ImagePortInfo[i].x + m_ImageLeft+10, m_ImagePortInfo[i].y + m_ImageTop+6);
			if(rect.PtInRect(point))
			{
				m_ImagePortLButtonDownPoint = point;
				m_bImagePortLButtonDown = TRUE;
				m_CurImagePortIndex = i;
				return;
			}
		}
	}
	CDialog::OnLButtonDown(nFlags, point);
}

void CImageShow::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	if(m_bDlgPortShowLButtonDown)
	{
		m_bDlgPortShowLButtonDown = FALSE;
		CRect rect;
		if(m_DlgPortShowPort_LButtonDownIndex >= 0)
		{
			rect.SetRect(m_ImagePortInfo[m_DlgPortShowPort_LButtonDownIndex].x-10 + m_ImageLeft, m_ImagePortInfo[m_DlgPortShowPort_LButtonDownIndex].y-6 + m_ImageTop, m_ImagePortInfo[m_DlgPortShowPort_LButtonDownIndex].x + m_ImageLeft+10, m_ImagePortInfo[m_DlgPortShowPort_LButtonDownIndex].y + m_ImageTop+6);
			if(rect.PtInRect(point))
			{
				m_Cur_DlgPortShowPortIndex = m_DlgPortShowPort_LButtonDownIndex;
				UpdateShowPortDlgData(TRUE);
				return;
			}
		}
		//檢查是否移動到左上角位置
		rect.SetRect(m_ImageLeft, 4 + m_ImageTop, m_ImageLeft+20, 16 + m_ImageTop);
		if(rect.PtInRect(point) && m_DlgPortShowPort_LButtonDownIndex == -1)
		{
			m_Cur_DlgPortShowPortIndex = -1;	// -1 = 左上角的點
			UpdateShowPortDlgData(TRUE);
			return;
		}
	}
	else if(m_bImagePortLButtonDown)
	{
		if(m_ImagePortLButtonDownPoint.x  == point.x && m_ImagePortLButtonDownPoint.y == point.y)
		{
			//在點擊Port
			m_bImagePortLButtonDown = FALSE;
			m_SetImagePort.SetParent(this);
			m_SetImagePort.SetID(m_ImagePortInfo[m_CurImagePortIndex].Id[0], m_ImagePortInfo[m_CurImagePortIndex].Id[1]);
			m_SetImagePort.DoModal();
		}
		else
		{		
			//移動Port的釋放
			m_bImagePortLButtonDown = FALSE;
			m_ImagePortInfo[m_CurImagePortIndex].x = point.x - m_ImageLeft;
			m_ImagePortInfo[m_CurImagePortIndex].y = point.y - m_ImageTop;
			SaveImage();
			PaintImage();
			SetClassLong(this->GetSafeHwnd(),GCL_HCURSOR, (LONG)(LoadCursor(NULL, IDC_ARROW)));
		}
	}
	CDialog::OnLButtonUp(nFlags, point);
}

void CImageShow::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	if(m_bConnect)
	{
		//已經開始監控了，檢查鼠標是否有移動到了參考點上面
		CRect rect;
		for(int i=0; i<m_CurImagePortCount; i++)
		{
			rect.SetRect(m_ImagePortInfo[i].x-10 + m_ImageLeft, m_ImagePortInfo[i].y-6 + m_ImageTop, m_ImagePortInfo[i].x + m_ImageLeft+10, m_ImagePortInfo[i].y + m_ImageTop+6);
			if(rect.PtInRect(point))
			{
				SetClassLong(this->GetSafeHwnd(),GCL_HCURSOR ,(LONG)LoadCursor(NULL , IDC_HAND));
				return;
			}
		}
		//檢查是否移動到左上角位置
		rect.SetRect(m_ImageLeft, 4 + m_ImageTop, m_ImageLeft+20, 16 + m_ImageTop);
		if(rect.PtInRect(point))
		{
			SetClassLong(this->GetSafeHwnd(),GCL_HCURSOR ,(LONG)LoadCursor(NULL , IDC_HAND));
			return;
		}
		SetClassLong(this->GetSafeHwnd(),GCL_HCURSOR ,(LONG)LoadCursor(NULL , IDC_ARROW));
	}
	else if(!m_bImagePortLButtonDown && !m_bConnect && m_bLoadImage && m_bShowReferencePort)
	{
		//在可以移動參考點的模式下
		CRect rect;
		for(int i=0; i<m_CurImagePortCount; i++)
		{
			rect.SetRect(m_ImagePortInfo[i].x-10 + m_ImageLeft, m_ImagePortInfo[i].y-6 + m_ImageTop, m_ImagePortInfo[i].x + m_ImageLeft+10, m_ImagePortInfo[i].y + m_ImageTop+6);
			if(rect.PtInRect(point))
			{
				SetClassLong(this->GetSafeHwnd(),GCL_HCURSOR ,(LONG)LoadCursor(NULL , IDC_HAND));
				return;
			}
		}
		SetClassLong(this->GetSafeHwnd(),GCL_HCURSOR ,(LONG)LoadCursor(NULL , IDC_ARROW));
	}
	else if(m_bImagePortLButtonDown)
	{
		//在移動位置參考點
		m_ImagePortInfo[m_CurImagePortIndex].x = point.x - m_ImageLeft;
		m_ImagePortInfo[m_CurImagePortIndex].y = point.y - m_ImageTop;
		PaintImage();
	}


	CDialog::OnMouseMove(nFlags, point);
}

void CImageShow::UpdateShowPortDlgData(BOOL UpdateTitle)
{
	if(UpdateTitle)
	{
		CRect rs; 
		this->GetWindowRect(&rs);
		::MoveWindow(m_ImagePortShow.GetSafeHwnd(), rs.left, rs.top, rs.Width(), rs.Height(), TRUE);

		CString str_name;
		if(m_Cur_DlgPortShowPortIndex >= 0)
		{
			str_name.Format(TEXT("%02X%02X"), m_ImagePortInfo[m_Cur_DlgPortShowPortIndex].Id[0], m_ImagePortInfo[m_Cur_DlgPortShowPortIndex].Id[1]);
			GetPortName(str_name);		
			str_name = TEXT("以下為參考點“") + str_name + TEXT("”周圍，所有定位卡片的詳細信息：");
		}
		else
		{
			str_name = TEXT("以下為參考點未標示在地圖上的所有定位卡片的詳細信息：");
		}
		m_ImagePortShow.UpdateTitle(str_name);
	}
	//從接收到的所有定位數據中，查找符合的卡片信息來更新顯示。			
	m_Cur_DlgPortShowCount = 0;
	if(m_Cur_DlgPortShowPortIndex == -1)
	{
		//這個是要查找所有參考點不在地圖上的卡片
		for(int i=0; i<m_Cur_ReceiveCount; i++)
		{
			if(FindImagePortID(m_ReceiveInfo[i].Port1Id) < 0)
			{
				//卡片靠近的參考點，沒有標註在地圖上
				m_DlgPortShowReceiveInfo[m_Cur_DlgPortShowCount++] = m_ReceiveInfo[i];
			}
		}
	}
	else if(m_Cur_DlgPortShowPortIndex >= 0)
	{
		//查找指定位置參考點周圍的卡片
		for(int i=0; i<m_Cur_ReceiveCount; i++)
		{
			if(memcmp(m_ReceiveInfo[i].Port1Id, m_ImagePortInfo[m_Cur_DlgPortShowPortIndex].Id, 2) == 0)
			{
				//參考點ID相同，是要顯示的卡片
				m_DlgPortShowReceiveInfo[m_Cur_DlgPortShowCount++] = m_ReceiveInfo[i];
			}
		}
	}
	m_ImagePortShow.UpdateData(m_DlgPortShowReceiveInfo, m_Cur_DlgPortShowCount);

	m_ImagePortShow.ShowWindow(SW_SHOW);
}

void CImageShow::SetImagePort(BYTE id1, BYTE id2)
{
	m_ImagePortInfo[m_CurImagePortIndex].Id[0] = id1;
	m_ImagePortInfo[m_CurImagePortIndex].Id[1] = id2;
	SaveImage();
	RedrawWindow();
}
void CImageShow::DeleteImagePort()
{
	for(int i=m_CurImagePortIndex; i<m_CurImagePortCount-1; i++)
	{
		m_ImagePortInfo[i] = m_ImagePortInfo[i+1];		
	}
	m_CurImagePortCount--;
	SaveImage();
	RedrawWindow();
}

void CImageShow::LoadConfigInfo()
{
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	if(m_ini.Open(filepath, FALSE))
	{		
		CString str, str2;
		if(m_ini.GetValue(TEXT("Server"), TEXT("Ip"), str))
		{								
			m_ServerIp = str;
		}
		else
		{
			m_ServerIp = TEXT("");				
		}
		if(m_ini.GetValue(TEXT("Server"), TEXT("Port"), str))
		{								
			m_ServerPort = _ttoi(str);
		}
		else
		{
			m_ServerPort = 0;
		}			
		if(m_ini.GetValue(TEXT("Display"), TEXT("NoShowTimeOutID"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bNoShowTimeOutID = TRUE;
			else
				m_bNoShowTimeOutID = FALSE;
		}
		else
		{
			m_bNoShowTimeOutID = TRUE;
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowTimeOutIDTime"), str))
		{								
			m_ShowTimeOutTime = _ttoi(str);
		}
		else
		{
			m_ShowTimeOutTime = 60;
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowTagNoMove"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bShowNoMoveTag = TRUE;
			else
				m_bShowNoMoveTag = FALSE;
		}
		else
		{
			m_bShowNoMoveTag = TRUE;
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowTagNoMoveTime"), str))
		{								
			m_ShowNoMoveTagTime = _ttoi(str);
		}
		else
		{
			m_ShowNoMoveTagTime = 60;
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowTagContinuous"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bShowTagContinuous = TRUE;
			else
				m_bShowTagContinuous = FALSE;
		}
		else
		{
			m_bShowTagContinuous = FALSE;
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowTagContinuousNumber"), str))
		{								
			m_ShowTagContinuousNumber = _ttoi(str);
		}
		else
		{
			m_ShowTagContinuousNumber = 3;
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("AutoShowEmergency"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bAutoEmergency = TRUE;
			else
				m_bAutoEmergency = FALSE;
		}
		else
		{
			m_bAutoEmergency = TRUE;
		}
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowReferencePoint"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bShowReferencePort = TRUE;
			else
				m_bShowReferencePort = FALSE;
		}
		else
		{
			m_bShowReferencePort = TRUE;
		}
		if(m_ini.GetValue(TEXT("Warning"), TEXT("ShowEmergencyTag"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bWarning_ShowEmergencyTag = TRUE;
			else
				m_bWarning_ShowEmergencyTag = FALSE;
		}
		else
		{
			m_bWarning_ShowEmergencyTag = TRUE;
		}
		if(m_ini.GetValue(TEXT("Warning"), TEXT("ShowServerNetTimeOut"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bWarning_ShowServerNetTimeout = TRUE;
			else
				m_bWarning_ShowServerNetTimeout = FALSE;
		}
		else
		{
			m_bWarning_ShowServerNetTimeout = TRUE;
		}
		if(m_ini.GetValue(TEXT("Warning"), TEXT("ShowTagTimeOut"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bWarning_ShowTagTimeout = TRUE;
			else
				m_bWarning_ShowTagTimeout = FALSE;
		}
		else
		{
			m_bWarning_ShowTagTimeout = TRUE;
		}
		if(m_ini.GetValue(TEXT("Warning"), TEXT("ServerNetTimeOutTime"), str))
		{								
			m_Warning_ServerNetTime = _ttoi(str);
		}
		else
		{
			m_Warning_ServerNetTime = 60;
		}

		if(m_ini.GetValue(TEXT("Warning"), TEXT("LowBatteryValue"), str))
		{								
			m_Warning_LowBatteryValue = _ttoi(str);
		}
		else
		{
			m_Warning_LowBatteryValue = 10;
		}
		if(m_ini.GetValue(TEXT("Warning"), TEXT("ShowLowBattery"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				m_bWarning_ShowLowBattery = TRUE;
			else
				m_bWarning_ShowLowBattery = FALSE;
		}
		else
		{
			m_bWarning_ShowLowBattery = TRUE;
		}
	}
	else
	{
		//沒有設置文件，使用默認值
		m_bCOMConnectAuto = TRUE;
		m_bNoShowTimeOutID = TRUE;
		m_ShowTimeOutTime = 60;
		m_bAutoEmergency = TRUE;
		m_bShowReferencePort = TRUE;
		m_bWarning_ShowEmergencyTag = TRUE;
		m_bWarning_ShowServerNetTimeout = TRUE;
		m_Warning_ServerNetTime = 60;
		m_Warning_LowBatteryValue = 10;
		m_bWarning_ShowLowBattery = TRUE;
	}
}

void CImageShow::OnBnClickedButtonConnect()
{
	// TODO: Add your control notification handler code here
	if(m_bConnect)
	{
		m_button_connect.SetWindowTextW(TEXT("開始監控"));
		m_bConnect = FALSE;
		closesocket(m_UDPSocket);
		m_UDPSocket = NULL;		
		KillTimer(1);
		CloseHandle(m_SaveDataHandle);
		m_SaveDataHandle = NULL;
		PaintImage();
	}
	else
	{
		//加載配置文件
		LoadConfigInfo();
		if(m_ServerPort == 0 || m_ServerIp.IsEmpty())
		{
			MessageBox(TEXT("請先設定Server的IP和Port！"));
			return;
		}
		
		m_CurShowType = SHOW_TYPE_ALL;

		//創建UDP Server
		//創建UDP套接字
		m_UDPSocket = socket(AF_INET,SOCK_DGRAM,IPPROTO_UDP);
		if (m_UDPSocket == INVALID_SOCKET)
		{
			MessageBox(TEXT("Create UDP socket error!"));
			return;
		}		
		char ip[16];
		memset(ip, 0, sizeof(ip));
		wcstombs(ip, m_ServerIp, 16);

		struct sockaddr_in localAddr;
		localAddr.sin_family = AF_INET;
		localAddr.sin_port = htons(m_ServerPort);
		localAddr.sin_addr.s_addr= inet_addr(ip);
		
		//綁定地址
		if(bind(m_UDPSocket,(sockaddr*)&localAddr,sizeof(localAddr))!=0)
		{
			closesocket(m_UDPSocket);
			MessageBox(TEXT("bind port fail!"));
			return;
		}
		
		//設置非堵塞通訊
		DWORD ul= 1;
		ioctlsocket(m_UDPSocket,FIONBIO,&ul);

		AfxBeginThread((AFX_THREADPROC)ReadThread_ImageShow, this);

		
		m_button_connect.SetWindowTextW(TEXT("停止監控"));
						
		for(int j=0; j<m_CurImagePortCount; j++)
			memset(m_ImagePortInfo[j].DrawTagId, 0, sizeof(m_ImagePortInfo[j].DrawTagId));

		//打開儲存數據的文件
		if(!OpenSaveDataFile())
		{
			MessageBox(TEXT("創建保存文件失敗！"));
		}


		//每秒刷新一次列表信息
		SetTimer(1, 500, NULL);
		m_bConnect = TRUE;

		m_Cur_ReceiveCount = 0;
		m_WarningInfo.RemoveAll();
		m_Last_ServerNetTime = GetTickCount();

	}
}

BOOL CImageShow::OpenSaveDataFile()
{
	CloseHandle(m_SaveDataHandle);

	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("SaveData");

	CTime t = CTime::GetCurrentTime();
	CString str;
	str.Format(TEXT("%d%02d%02d"), t.GetYear(), t.GetMonth(), t.GetDay());
	filepath = filepath + TEXT("\\") + str;

	//判斷文件夾不存在，創建文件夾
	if(!PathFileExists(filepath))
	{
		CreateDirectory(filepath, NULL); 
	}

	str.Format(TEXT("\\%02d.dat"), t.GetHour());
	filepath = filepath + str;
	if(!PathFileExists(filepath))
	{
		//創建文件
		m_SaveDataHandle = CreateFile(filepath, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ|FILE_SHARE_WRITE|FILE_SHARE_DELETE, NULL, CREATE_NEW, FILE_FLAG_BACKUP_SEMANTICS, NULL); 
	}
	else
	{
		//共享打開
		m_SaveDataHandle = CreateFile(filepath, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ|FILE_SHARE_WRITE|FILE_SHARE_DELETE, NULL, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, NULL); 
	}
	m_SaveDataHour = t.GetHour();
	m_SaveDataDay = t.GetDay();

	if(m_SaveDataHandle == INVALID_HANDLE_VALUE)
		return FALSE;
	SetFilePointer(m_SaveDataHandle, 0, 0, FILE_END);
	return TRUE;
}

void CImageShow::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: Add your message handler code here and/or call default
	if(nIDEvent == 1)
	{
		CString str;
		CTime t = CTime::GetCurrentTime();
		if(t.GetHour() != m_SaveDataHour)
			OpenSaveDataFile();
		DWORD Write;

		EnterCriticalSection(&g_cs_ImageShow);
		
		if(m_bShowTagContinuous == 1)
		{
			//判斷卡片是否有切換參考點和保存數據
			for(int i=0; i<m_Cur_ReceiveCount; i++)
			{				
				if(!m_ReceiveInfo[i].IsAladySaveData && (GetTickCount() - m_ReceiveInfo[i].FirstReceiveTime >= 40))
				{
					//離第一次接收到數據之后，已經有40ms了。認為同一次的數據都已經到了
					//判斷當前的參考點和保存的切換參考點是否一致
					if(m_ReceiveInfo[i].ChangePortId[0] == m_ReceiveInfo[i].Port1Id[0] && m_ReceiveInfo[i].ChangePortId[1] == m_ReceiveInfo[i].Port1Id[1])
					{
						m_ReceiveInfo[i].ChangePortCount++;
						if(m_ReceiveInfo[i].ChangePortCount >= m_ShowTagContinuousNumber)
						{
							//達到切換參考點的條件
							if(((m_ReceiveInfo[i].ChangePortId[0]<<8)|(m_ReceiveInfo[i].ChangePortId[1])) == ((m_ReceiveInfo[i].NearPortId[0]<<8)|(m_ReceiveInfo[i].NearPortId[1])))
							{
								//相等，說明是在同一個
							}
							else
							{
								//兩次的參考點不一樣，是真實的切換，要發送232數據
								m_ReceiveInfo[i].NearPortId[0] = m_ReceiveInfo[i].ChangePortId[0];
								m_ReceiveInfo[i].NearPortId[1] = m_ReceiveInfo[i].ChangePortId[1];	
							}
							
							m_ReceiveInfo[i].ChangePortCount = m_ShowTagContinuousNumber;
						}
					}
					else
					{
						//上次保存的點和本次的不一樣，把保存點歸零
						m_ReceiveInfo[i].ChangePortId[0] = m_ReceiveInfo[i].Port1Id[0];
						m_ReceiveInfo[i].ChangePortId[1] = m_ReceiveInfo[i].Port1Id[1];
						m_ReceiveInfo[i].ChangePortCount = 0;
					}
									
					m_ReceiveInfo[i].IsAladySaveData = TRUE;
					if(m_ReceiveInfo[i].SaveDataSecond != t.GetSecond())
					{
						m_ReceiveInfo[i].SaveDataSecond != t.GetSecond();
						m_SaveOldData.Minute = t.GetMinute();
						m_SaveOldData.Secondes = t.GetSecond();
						m_SaveOldData.Type = m_ReceiveInfo[i].Type;
						m_SaveOldData.TagId[0] = m_ReceiveInfo[i].TagId[0]; m_SaveOldData.TagId[1] = m_ReceiveInfo[i].TagId[1];
						m_SaveOldData.PortId[0] = m_ReceiveInfo[i].NearPortId[0]; m_SaveOldData.PortId[1] = m_ReceiveInfo[i].NearPortId[1];
						m_SaveOldData.Rssi = m_ReceiveInfo[i].Rssi1;
						m_SaveOldData.Index = m_ReceiveInfo[i].index;
						m_SaveOldData.Battery = m_ReceiveInfo[i].Battery;
						m_SaveOldData.SensorTime = m_ReceiveInfo[i].SensorTime;
						WriteFile(m_SaveDataHandle, &m_SaveOldData, sizeof(m_SaveOldData), &Write, NULL);
					}
				}
				//否則，可能丟包導致，沒有收到新數據
			}
		}
		else
		{
		//保存數據
			for(int i=0; i<m_Cur_ReceiveCount; i++)
			{		
				m_ReceiveInfo[i].NearPortId[0] = m_ReceiveInfo[i].Port1Id[0];
				m_ReceiveInfo[i].NearPortId[1] = m_ReceiveInfo[i].Port1Id[1];

				if(!m_ReceiveInfo[i].IsAladySaveData && (GetTickCount() - m_ReceiveInfo[i].FirstReceiveTime >= (DWORD)200))
				{
					m_ReceiveInfo[i].IsAladySaveData = TRUE;
					m_SaveOldData.Minute = t.GetMinute();
					m_SaveOldData.Secondes = t.GetSecond();
					m_SaveOldData.Type = m_ReceiveInfo[i].Type;
					m_SaveOldData.TagId[0] = m_ReceiveInfo[i].TagId[0]; m_SaveOldData.TagId[1] = m_ReceiveInfo[i].TagId[1];
					m_SaveOldData.PortId[0] = m_ReceiveInfo[i].NearPortId[0]; m_SaveOldData.PortId[1] = m_ReceiveInfo[i].NearPortId[1];
					m_SaveOldData.Rssi = m_ReceiveInfo[i].Rssi1;
					m_SaveOldData.Index = m_ReceiveInfo[i].index;
					m_SaveOldData.Battery = m_ReceiveInfo[i].Battery;
					m_SaveOldData.SensorTime = m_ReceiveInfo[i].SensorTime;
					WriteFile(m_SaveDataHandle, &m_SaveOldData, sizeof(m_SaveOldData), &Write, NULL);
				}
			}
		}
		//刪除超時沒有接收到的定位信息的點
		if(m_bNoShowTimeOutID || m_bWarning_ShowTagTimeout)
		{
			for(int i=0; i<m_Cur_ReceiveCount; i++)
			{				
				if(GetTickCount() - m_ReceiveInfo[i].FirstReceiveTime > (DWORD)m_ShowTimeOutTime*1000)
				{
					if(m_bWarning_ShowTagTimeout && !m_ReceiveInfo[i].IsAladyWarringTimeOut)
					{		
						m_ReceiveInfo[i].IsAladyWarringTimeOut = TRUE;
						CTime t = CTime::GetCurrentTime();
						CString str_name;
						str_name.Format(TEXT("%02X%02X"), m_ReceiveInfo[i].TagId[0], m_ReceiveInfo[i].TagId[1]);
						GetTagName(str_name);
						str.Format(TEXT("%04d-%02d-%02d %02d:%02d:%02d, %s 超時無定位信息\r\n"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond(), str_name);
						m_WarningInfo.Add(str);
					}
					if(m_bNoShowTimeOutID)
					{
						//刪除之前，需要先刪除之前這個TAG顯示在那個參考點周圍的位置信息
						ClearPortDrawTagId(m_ReceiveInfo[i].LastPortId, m_ReceiveInfo[i].TagId);
						for(int j=i; j<m_Cur_ReceiveCount-1; j++)
						{
							m_ReceiveInfo[j] = m_ReceiveInfo[j+1];
						}
						m_Cur_ReceiveCount--;
						i--;
					}					
				}
			}
		}
		//檢查位置參考點是否在線
		if(m_bWarning_ShowServerNetTimeout)
		{
			if((GetTickCount() - m_Last_ServerNetTime) > (DWORD)(m_Warning_ServerNetTime * 1000))
			{
				m_Last_ServerNetTime = GetTickCount();
				//時間到，檢查一次
				for(int i=0; i<m_SaveServerNet_ID.GetCount(); i++)
				{
					int j=0; 
					for(j=0; j<m_ReceServerNet.GetCount(); j++)
					{
						if(0 == m_SaveServerNet_ID.GetAt(i).Compare(m_ReceServerNet.GetAt(j)))
							break;
					}
					if(j >= m_ReceServerNet.GetCount())
					{
						//在接收中沒有查到，發出報警
						CTime t = CTime::GetCurrentTime();
						str = m_SaveServerNet_Name.GetAt(i);
						GetPortName(str);
						str.Format(TEXT("%04d-%02d-%02d %02d:%02d:%02d, 位置參考點（%s）超時無數據送達\r\n"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond(), str);
						m_WarningInfo.Add(str);
					}
				}
				m_ReceServerNet.RemoveAll();
			}
		}
		//顯示有多少報警信息
		if(m_LastWarnningCount != m_WarningInfo.GetCount())
		{
			m_LastWarnningCount = (int)m_WarningInfo.GetCount();
			if(m_LastWarnningCount > 0)
			{
				m_edit_warning_info.SetWindowTextW(m_WarningInfo.GetAt(m_LastWarnningCount-1));
				str.Format(TEXT("警告信息(%d)"), m_LastWarnningCount);
				m_btn_warning_show.SetWindowTextW(str);
			}
			else
			{
				m_btn_warning_show.SetWindowTextW(TEXT("警告信息"));
				m_edit_warning_info.SetWindowTextW(TEXT(""));
			}
			m_WarningMessage.UpdateWarningMessage(m_WarningInfo);
		}
		PaintImage();
		
		if(m_Cur_DlgPortShowPortIndex >= -1)
			UpdateShowPortDlgData(FALSE);	
		if(m_bShowTagOnLineDlg)
			m_TagOnLine.UpdateData(m_ReceiveInfo, m_Cur_ReceiveCount);

		LeaveCriticalSection(&g_cs_ImageShow); 
	}	
	CDialog::OnTimer(nIDEvent);
}

BOOL CImageShow::GetSaveTagInfo(void)
{
	m_Cur_SaveTagCount = 0;
	while(m_CheckList.GetCount())
		m_CheckList.DeleteString(0);

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
		m_SaveTagInfo[m_Cur_SaveTagCount].Show = SHOW_TAG_NO;
	}

	//更新到列表中
	for(int i=0; i<m_Cur_SaveTagCount; i++)
	{
		str_value = m_SaveTagInfo[i].ID + TEXT("  ") + m_SaveTagInfo[i].Name;
		m_CheckList.AddString(str_value);
		if(m_bCheckBox_all)
		{
			m_CheckList.SetCheck(i, 1);
			m_SaveTagInfo[i].Show = SHOW_TAG_YES;
		}
	}
	m_ImagePortShow.UpdateSaveTag(m_SaveTagInfo, m_Cur_SaveTagCount);
	m_TagOnLine.UpdateSaveTag(m_SaveTagInfo, m_Cur_SaveTagCount);
	return TRUE;
}

BOOL CImageShow::GetSaveServerNetInfo(void)
{
	m_SaveServerNet_ID.RemoveAll();
	m_SaveServerNet_Name.RemoveAll();

	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\port.ini");	
	if(!m_ini.Open(filepath, FALSE))
		return TRUE;

	CString str, str_value;
	int i = 0;
	while(1)
	{
		str.Format(TEXT("%d"), i);
		if(m_ini.GetValue(str, TEXT("ID"), str_value))
		{
			m_SaveServerNet_ID.Add(str_value);
		}
		else
			break;
		if(m_ini.GetValue(str, TEXT("Addr"), str_value))
		{
			m_SaveServerNet_Name.Add(str_value);
		}
		else
			break;
		i++;
	}
	return TRUE;
}

BOOL CImageShow::GetTagName(CString &str_ID)
{
	for(int i=0; i<m_Cur_SaveTagCount; i++)
	{
		if(str_ID.Compare(m_SaveTagInfo[i].ID) == 0)
		{
			str_ID = m_SaveTagInfo[i].Name;
			return TRUE;
		}
	}
	return FALSE;
}

BOOL CImageShow::GetPortName(CString &str_ID)
{
	for(int i=0; i<m_Cur_SavePortCount; i++)
	{
		if(str_ID.Compare(m_SavePortInfo[i].ID) == 0)
		{
			str_ID = m_SavePortInfo[i].Name;
			return TRUE;
		}
	}
	return FALSE;
}

void CImageShow::OnBnClickedButtonSet()
{
	// TODO: Add your control notification handler code here
	m_Config.ShowWindow(SW_SHOW);
}

BOOL CImageShow::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	switch(pMsg->message)
	{
	case WM_MAP_UPDATE:
		{
			WCHAR szPath[MAX_PATH] = {0};
			GetModulePath(szPath, NULL);
			CString filepath;
			filepath = szPath;
			filepath = filepath + TEXT("config\\Config.ini");	
			if(m_ini.Open(filepath, FALSE))
			{		
				CString str, str2;
				if(m_ini.GetValue(TEXT("Map"), TEXT("FilePath"), m_ImageFile))
				{								
					if(MyLoadImage())
					{
						if(m_ImageFile.Compare(m_SaveImageFile) != 0)
						{
							//背景更換了，重設ImagePort的位置				
							int left = 10, top = 10;
							for(int i=0; i<m_CurImagePortCount; i++)
							{
								if(top > m_ImageHeight -10)
								{
									left += 100;
									top = 10;
								}
								m_ImagePortInfo[i].x = left;
								m_ImagePortInfo[i].y = top;
								top += 20;
							}
						}
						RedrawWindow();
						SaveImage();
					}
					m_SaveImageFile = m_ImageFile;
				}

			}
		}
		break;
	case WM_DISPLAY_REFERENCEPOINT_UPDATE:
		m_bShowReferencePort = (pMsg->wParam == 1);
		RedrawWindow();
		break;
	case WM_DISPLAY_AUTOEMERGENCY_UPDATE:
		m_bAutoEmergency = (pMsg->wParam == 1);
		break;
	case WM_DISPLAY_SHOWTIMEOUTID_UPDATE:
		m_bNoShowTimeOutID = (pMsg->wParam == 1);
		break;
	case WM_DISPLAY_SHOWTIMEOUTTIME_UPDATE:
		m_ShowTimeOutTime = pMsg->wParam;
		break;
	case WM_WARNING_SHOWEMERGENCY_UPDATE:
		m_bWarning_ShowEmergencyTag = (pMsg->wParam == 1);
		break;
	case WM_WARNING_SHOWSERVERNETTIMEOUT_UPDATE:
		m_bWarning_ShowServerNetTimeout = (pMsg->wParam == 1);
		break;
	case WM_WARNING_SHOWTAGTIMEOUT_UPDATE:
		m_bWarning_ShowTagTimeout = (pMsg->wParam == 1);
		break;
	case WM_WARNING_SERVERNETTIMEOUTTIME_UPDATE:
		m_Warning_ServerNetTime = pMsg->wParam;
		break;
	case WM_CLEAR_ALL_WARNING_MESSAGE:
		{
			m_LastWarnningCount = 0;
			m_WarningInfo.RemoveAll();
			m_btn_warning_show.SetWindowTextW(TEXT("警告信息"));
			m_edit_warning_info.SetWindowTextW(TEXT(""));
		}
		break;
	case WM_DISPLAY_SHOWTAGNOMOVE_UPDATE:
		m_bShowNoMoveTag = (pMsg->wParam == 1);
		break;
	case WM_DISPLAY_SHOWTAGNOMOVETIME_UPDATE:
		m_ShowNoMoveTagTime = pMsg->wParam;
		break;
	case WM_DLG_PORTSHOW_CLOSE:
		m_Cur_DlgPortShowPortIndex = -2;
		break;
	case WM_SETPORT_SAVE_IMAGESHOW:
		GetSavePortInfo();
		break;
	case WM_SETTAG_SAVE_IMAGESHOW:
		GetSaveTagInfo();
		break;
	case WM_SETNET_SAVE_IMAGESHOW:
		GetSaveServerNetInfo();
		break;
	case WM_ONLINE_DLG_OFF:
		m_bShowTagOnLineDlg = FALSE;
		break;
	case WM_WARNING_SHOWLOWBATTERY_UPDATE:
		m_bWarning_ShowLowBattery = (pMsg->wParam == 1);
		break;
	case WM_WARNING_LOWBATTERYVALUE_UPDATE:
		m_Warning_LowBatteryValue = pMsg->wParam;
		break;
	case WM_DISPLAY_SHOWTAGCONTINUOUS_UPDATE:
		m_bShowTagContinuous = (pMsg->wParam == 1);
		break;
	case WM_DISPLAY_SHOWTAGCONTINUOUSNUMBER_UPDATE:
		m_ShowTagContinuousNumber = pMsg->wParam;
		break;
	}
	return CDialog::PreTranslateMessage(pMsg);
}


void CImageShow::OnBnClickedButtonWarningAll()
{
	// TODO: Add your control notification handler code here
	m_WarningMessage.ShowWindow(SW_SHOW);
}

void CImageShow::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	// TODO: Add your message handler code here
	CRect rs; 
	this->GetClientRect(&rs);
	m_StaticShow_height = rs.Height() - 5 - m_StaticShow_top;
	::MoveWindow(m_static_show_tag.GetSafeHwnd(), m_StaticShow_left, m_StaticShow_top, m_StaticShow_width, m_StaticShow_height, TRUE);
	::MoveWindow(m_check_all, m_StaticShow_left+5, m_StaticShow_top+20, m_StaticShow_width-10, 15, TRUE);
	::MoveWindow(m_CheckList, m_StaticShow_left+5, m_StaticShow_top+20+15+5, m_StaticShow_width-10, m_StaticShow_height-45, TRUE);

	m_ImageLeft = m_StaticShow_left + m_StaticShow_width + 5;
	m_ImageTop = m_StaticShow_top;
	m_ImageWidth = rs.Width() - 5 - m_ImageLeft;
	m_ImageHeight = rs.Height() - 5 - m_ImageTop;

	//重新計算參考點的坐標
	if(m_SaveImageWidth != 0 && m_SaveImageHeight != 0)
	{
		for(int i=0; i<m_CurImagePortCount; i++)
		{
			m_ImagePortInfo[i].x = m_ImagePortInfo[i].x * m_ImageWidth / m_SaveImageWidth;
			m_ImagePortInfo[i].y = m_ImagePortInfo[i].y * m_ImageHeight / m_SaveImageHeight;
		}
	}
	m_SaveImageWidth = m_ImageWidth;
	m_SaveImageHeight = m_ImageHeight;

	MyLoadImage();

	::MoveWindow(m_edit_warning_info, 555, 7, rs.Width() - 5 - 555, 20, TRUE);
}

void CImageShow::OnBnClickedCheckAll()
{
	// TODO: Add your control notification handler code here
	if(m_check_all.GetCheck() == 1)
	{
		m_bCheckBox_all = TRUE;
		int count = m_CheckList.GetCount();
		for(int i=0; i<count; i++)
		{
			m_CheckList.SetCheck(i, 1);
			m_SaveTagInfo[i].Show = SHOW_TAG_YES;
		}
	}
	else
	{
		m_bCheckBox_all = FALSE;
		int count = m_CheckList.GetCount();
		for(int i=0; i<count; i++)
		{
			m_CheckList.SetCheck(i, 0);
			m_SaveTagInfo[i].Show = SHOW_TAG_NO;
		}
	}
}

void CImageShow::OnLbnSelchangeList1()
{
	// TODO: Add your control notification handler code here
	int index = m_CheckList.GetCurSel();
	if(index >= 0)
	{
		m_bCheckBox_all = FALSE;
		m_check_all.SetCheck(0);
		if(m_CheckList.GetCheck(index))
		{
			m_CheckList.SetCheck(index, 0);
			m_SaveTagInfo[index].Show = SHOW_TAG_NO;
		}
		else
		{
			m_CheckList.SetCheck(index, 1);
			m_SaveTagInfo[index].Show = SHOW_TAG_YES;
		}
	}
}

//串口讀線程函數
DWORD ComReadThread_ImageShow(LPVOID lparam)
{	
	DWORD	actualReadLen=0;	//實際讀取的字節數
	DWORD	willReadLen;	
	
	DWORD dwReadErrors;
	COMSTAT	cmState;
	
	CImageShow *pdlg;
	pdlg = (CImageShow *)lparam;

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
			EnterCriticalSection(&g_cs_ImageShow); 
			pdlg->ParseData();
			LeaveCriticalSection(&g_cs_ImageShow); 		
		}
		else
			pdlg->ParseData_Connect();

	}
	return 0;
}

#define PACKET_LEN_CONNECT  5
void CImageShow::ParseData_Connect(void)
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

BOOL CImageShow::OpenCom(CString str_com)
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

	HANDLE m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ComReadThread_ImageShow, this, 0, NULL);
	CloseHandle(m_hThread);

	return TRUE;
}
void CImageShow::OnBnClickedButtonTagOnline()
{
	// TODO: Add your control notification handler code here
	if(m_bShowTagOnLineDlg)
	{
		m_bShowTagOnLineDlg = FALSE;
		m_TagOnLine.ShowWindow(SW_HIDE);
	}
	else
	{
		m_bShowTagOnLineDlg = TRUE;
		EnterCriticalSection(&g_cs_ImageShow); 
		m_TagOnLine.UpdateData(m_ReceiveInfo, m_Cur_ReceiveCount);
		LeaveCriticalSection(&g_cs_ImageShow); 		
		m_TagOnLine.ShowWindow(SW_SHOW);
	}
}

void CImageShow::OnBnClickedButtonOlddata()
{
	// TODO: Add your control notification handler code here
	m_OldData.ShowWindow(SW_SHOW);
}
