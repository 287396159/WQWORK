// OldDataRePlay.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "OldDataRePlay.h"


int		g_DrawTagXYOffset_RePlay[MAX_DRAWTAG][2] = {
	{0, -28},	//12點方向
	{14, -14},	//1.5點方向
	{28, 0},	//3
	{14, 14},	//4.5
	{0, 28},	//6
	{-14, 14},	//7.5
	{-28, 0},	//9
	{-14, -14}	//10.5
};

int		g_DrawTagXYChongDieQu_RePlay[2] = {0, 0};	//中間圓心

// COldDataRePlay dialog

IMPLEMENT_DYNAMIC(COldDataRePlay, CDialog)

COldDataRePlay::COldDataRePlay(CWnd* pParent /*=NULL*/)
	: CDialog(COldDataRePlay::IDD, pParent)
{

}

COldDataRePlay::~COldDataRePlay()
{
}

void COldDataRePlay::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_DATETIMEPICKER_START, m_datatime_start);
	DDX_Control(pDX, IDC_COMBO_HOUR_START, m_combo_hour_start);
	DDX_Control(pDX, IDC_COMBO_MINUTE_START, m_combo_minute_start);
	DDX_Control(pDX, IDC_DATETIMEPICKER_END, m_datatime_end);
	DDX_Control(pDX, IDC_COMBO_HOUR_END, m_combo_hour_end);
	DDX_Control(pDX, IDC_COMBO_MINUTE_END, m_combo_minute_end);
	DDX_Control(pDX, IDC_STATIC_Show_Tag, m_static_show_tag);
	DDX_Control(pDX, IDC_CHECK_ALL2, m_check_all);
	DDX_Control(pDX, IDC_LIST1, m_CheckList);
	DDX_Control(pDX, IDC_STATIC_TIME, m_static_time);
	DDX_Control(pDX, IDC_STATIC_SPEED, m_static_speed);
	DDX_Control(pDX, IDC_BUTTON_PASUE, m_button_pause);
	DDX_Control(pDX, IDC_BUTTON_START, m_button_start);
}


BEGIN_MESSAGE_MAP(COldDataRePlay, CDialog)
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_BN_CLICKED(IDC_CHECK_ALL2, &COldDataRePlay::OnBnClickedCheckAll)
	ON_LBN_SELCHANGE(IDC_LIST1, &COldDataRePlay::OnLbnSelchangeList1)
	ON_BN_CLICKED(IDC_BUTTON_START, &COldDataRePlay::OnBnClickedButtonStart)
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_BUTTON_SPEED_ADD, &COldDataRePlay::OnBnClickedButtonSpeedAdd)
	ON_BN_CLICKED(IDC_BUTTON_SPEED_DEC, &COldDataRePlay::OnBnClickedButtonSpeedDec)
	ON_BN_CLICKED(IDC_BUTTON_PASUE, &COldDataRePlay::OnBnClickedButtonPasue)
END_MESSAGE_MAP()


// COldDataRePlay message handlers


VOID COldDataRePlay::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}

BOOL COldDataRePlay::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	m_font.CreateFont(12, 0, 0, 0, 0, 0, 0, 0, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,	DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, TEXT("宋體"));
	m_bLoadImage = FALSE;
	
	m_CheckList.SetCheckStyle(BS_CHECKBOX);
	m_StaticShow_left = 5;
	m_StaticShow_top = 80;
	m_StaticShow_width = 200;

	m_ImageLeft = m_StaticShow_width+5;
	m_ImageTop = m_StaticShow_top;

	m_bCheckBox_all = TRUE;
	m_check_all.SetCheck(1);
	

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


	m_CurImagePortCount = 0;	

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

	CTime t = CTime::GetCurrentTime();
	CTime t2 = t - CTimeSpan(1, 0, 0, 0);
	m_datatime_end.SetTime(&t);
	m_combo_hour_end.SetCurSel(t.GetHour());
	m_combo_minute_end.SetCurSel(t.GetMinute());

	m_datatime_start.SetTime(&t2);
	m_combo_hour_start.SetCurSel(t.GetHour());
	m_combo_minute_start.SetCurSel(t.GetMinute());

	m_pSaveOldData = NULL;
	m_CurSaveOldData_Count = 0;
	m_CurSaveOldData_Index = 0;
	m_bPlayStart = FALSE;

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
BOOL COldDataRePlay::GetSavePortInfo(void)
{
	m_Cur_SavePortCount = 0;
	m_SaveServerNet.RemoveAll();

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
			m_SaveServerNet.Add(str_value);
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


bool COldDataRePlay::StringToChar(CString str, BYTE* data)
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


BOOL COldDataRePlay::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class

	return CDialog::PreTranslateMessage(pMsg);
}

void COldDataRePlay::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	// TODO: Add your message handler code here
	// Do not call CDialog::OnPaint() for painting messages
	PaintImage();
}

int	COldDataRePlay::IsShowTag(BYTE id1, BYTE id2)
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

void COldDataRePlay::PaintImage()
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
				::FillRect(m_mdc, &r, m_Brush_Port);
				cr.SetRect(r.right+2, r.top, r.right+100, r.bottom);
				str.Format(TEXT("%02X%02X"), m_ImagePortInfo[i].Id[0], m_ImagePortInfo[i].Id[1]);
				GetPortName(str);
				::DrawText(m_mdc, str, -1, &cr, DT_LEFT|DT_VCENTER|DT_SINGLELINE);
			}
		}
		//畫Tag		
		{	
			if(m_CurShowType == SHOW_TYPE_ALL)
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

BOOL COldDataRePlay::GetTagCoordinate(ImageShowReceiveInfo &isri, int &x, int &y, int &count, DWORD &lmr)
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
						x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset_RePlay[i][0];
						y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset_RePlay[i][1];
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
						x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset_RePlay[i][0];
						y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset_RePlay[i][1];
						count = 0;
						if(i>= 5)
							lmr = DT_RIGHT;
						else
							lmr = DT_LEFT;
						return TRUE;
					}
				}
				//找完一圈，沒有找到空位置，還是使用“重疊區”
				x = m_ImagePortInfo[ipi].x + g_DrawTagXYChongDieQu_RePlay[0];
				y = m_ImagePortInfo[ipi].y + g_DrawTagXYChongDieQu_RePlay[1];
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
	ipi = FindImagePortID(isri.Port1Id);	
	if(ipi >= 0)
	{
		//參考點有在地圖上
		//先查找這個卡片，上次畫的參考點的ID，是否和這一次的是否一樣
		if(isri.LastPortId[0] == isri.Port1Id[0] && isri.LastPortId[1] == isri.Port1Id[1])
		{
			//上次顯示的參考點和這次的一樣，查看上次的畫點在什么地方
			for(i=0; i<MAX_DRAWTAG; i++)
			{
				if(isri.TagId[0] == m_ImagePortInfo[ipi].DrawTagId[i][0] && isri.TagId[1] == m_ImagePortInfo[ipi].DrawTagId[i][1])
				{
					//找到上次畫點的位置，本次還是使用上次點位的位置
					x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset_RePlay[i][0];
					y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset_RePlay[i][1];
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
					x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset_RePlay[i][0];
					y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset_RePlay[i][1];
					count = 0;
					if(i>= 5)
						lmr = DT_RIGHT;
					else
						lmr = DT_LEFT;
					return TRUE;
				}
			}
			//找完一圈，沒有找到空位置，還是使用“重疊區”
			x = m_ImagePortInfo[ipi].x + g_DrawTagXYChongDieQu_RePlay[0];
			y = m_ImagePortInfo[ipi].y + g_DrawTagXYChongDieQu_RePlay[1];
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
					isri.LastPortId[0] = isri.Port1Id[0];
					isri.LastPortId[1] = isri.Port1Id[1];
					//返回數據
					x = m_ImagePortInfo[ipi].x + g_DrawTagXYOffset_RePlay[i][0];
					y = m_ImagePortInfo[ipi].y + g_DrawTagXYOffset_RePlay[i][1];
					count = 0;
					if(i>= 5)
						lmr = DT_RIGHT;
					else
						lmr = DT_LEFT;
					return TRUE;
				}
			}
			//找完一圈，沒有找到空位置，使用“重疊區”
			isri.LastPortId[0] = isri.Port1Id[0];
			isri.LastPortId[1] = isri.Port1Id[1];

			x = m_ImagePortInfo[ipi].x + g_DrawTagXYChongDieQu_RePlay[0];
			y = m_ImagePortInfo[ipi].y + g_DrawTagXYChongDieQu_RePlay[1];
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
int COldDataRePlay::FindImagePortID(BYTE id[])
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

void COldDataRePlay::ClearPortDrawTagId(BYTE portid[], BYTE tagid[])
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


BOOL COldDataRePlay::MyLoadImage()
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


void COldDataRePlay::LoadConfigInfo()
{
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	if(m_ini.Open(filepath, FALSE))
	{		
		CString str, str2;
		
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
	}
	else
	{
		//沒有設置文件，使用默認值		
		m_bNoShowTimeOutID = TRUE;
		m_ShowTimeOutTime = 60;
		m_bAutoEmergency = TRUE;
		m_bShowReferencePort = TRUE;
		m_bWarning_ShowEmergencyTag = TRUE;
		m_bWarning_ShowServerNetTimeout = TRUE;
		m_Warning_ServerNetTime = 60;
	}
}


BOOL COldDataRePlay::GetSaveTagInfo(void)
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
	
	return TRUE;
}


BOOL COldDataRePlay::GetTagName(CString &str_ID)
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

BOOL COldDataRePlay::GetPortName(CString &str_ID)
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


void COldDataRePlay::OnSize(UINT nType, int cx, int cy)
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

}

void COldDataRePlay::OnBnClickedCheckAll()
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

void COldDataRePlay::OnLbnSelchangeList1()
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


void COldDataRePlay::OnBnClickedButtonStart()
{
	// TODO: Add your control notification handler code here
	if(m_bPlayStart)
	{
		KillTimer(1);
		m_button_start.SetWindowTextW(TEXT("開始回放"));
		m_bPlayStart = FALSE;
	}
	else
	{
		if(m_pSaveOldData)
		{
			delete[] m_pSaveOldData;
			m_pSaveOldData = NULL;
			m_CurSaveOldData_Count = 0;
			m_CurSaveOldData_Index = 0;
		}	

		CTime t;	
		m_datatime_start.GetTime(t);
		m_time_start = CTime(t.GetYear(), t.GetMonth(), t.GetDay(), m_combo_hour_start.GetCurSel(), m_combo_minute_start.GetCurSel(), 0);
		m_datatime_end.GetTime(t);
		m_time_end = CTime(t.GetYear(), t.GetMonth(), t.GetDay(), m_combo_hour_end.GetCurSel(), m_combo_minute_end.GetCurSel(), 0);
		m_time_cur = m_time_start;

		if(m_time_start < m_time_end)
		{	
			m_PlaySpeed = 10;
			m_Cur_ReceiveCount = 0;
			m_CurShowType = SHOW_TYPE_ALL;

			m_button_start.SetWindowTextW(TEXT("停止回放"));
			m_button_pause.SetWindowTextW(TEXT("暫停"));
			m_static_speed.SetWindowTextW(TEXT("1X"));
			m_bPlayStart = TRUE;
			m_bPlayPause = FALSE;

			LoadConfigInfo();
			GetSavePortInfo();
			GetSaveTagInfo();
			SetTimer(1, 1000, NULL);
		}
	}
}

void COldDataRePlay::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: Add your message handler code here and/or call default
	if(nIDEvent == 1)
	{
		CString str;
		//先判斷是否到結束的時間
		if(m_time_cur < m_time_end)
		{
			//判斷是否有讀取到文件
			if(m_pSaveOldData == NULL)
			{
				//沒有數據，讀取文件
				WCHAR szPath[MAX_PATH] = {0};
				GetModulePath(szPath, NULL);
				CString filepath;
				filepath = szPath;
				filepath = filepath + TEXT("SaveData\\");				
				str.Format(TEXT("%d%02d%02d\\%02d.dat"), m_time_cur.GetYear(), m_time_cur.GetMonth(), m_time_cur.GetDay(), m_time_cur.GetHour());
				str = filepath + str;
				HANDLE hfile = CreateFile(str, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ|FILE_SHARE_WRITE|FILE_SHARE_DELETE, NULL, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, NULL); 
				if(hfile != INVALID_HANDLE_VALUE)
				{
					//文件存在，申請內存來保存文件
					int filesize = ::GetFileSize(hfile, NULL);
					m_CurSaveOldData_Count = filesize / sizeof(SaveOldData);
					m_CurSaveOldData_Index = 0;
					m_pSaveOldData = new SaveOldData[m_CurSaveOldData_Count];
					if(m_pSaveOldData == NULL)
					{
						MessageBox(TEXT("內存不足，讀取數據文件失敗，請縮短回放時間，再重試！"));
						CloseHandle(hfile);	
						KillTimer(1);
						return;
					}
					DWORD Read;
					ReadFile(hfile, m_pSaveOldData, filesize, &Read, NULL);
					if(filesize != Read)
					{
						MessageBox(TEXT("讀取文件長度不符合"));
					}
					CloseHandle(hfile);	
				}
				else
				{
					//文件不存在，
					m_pSaveOldData = (SaveOldData*)0xFFFFFFFF;	//標識此文件不存在
				}
			}
			if(m_pSaveOldData == (SaveOldData*)0xFFFFFFFF)
			{
				//這個小時，沒有保存文件，直接把時間+1即可
			}
			else
			{
				//有正常的保存的數據文件內容
				//把數據讀取出來，保存到接收的數組中去
				for(; m_CurSaveOldData_Index<m_CurSaveOldData_Count; m_CurSaveOldData_Index++)
				{
					if((m_pSaveOldData[m_CurSaveOldData_Index].Minute < m_time_cur.GetMinute()) ||
						((m_pSaveOldData[m_CurSaveOldData_Index].Minute == m_time_cur.GetMinute()) && (m_pSaveOldData[m_CurSaveOldData_Index].Secondes <= m_time_cur.GetSecond())))
					{
						//時間上符合，把數據保存到接收的數組中去						
						//判斷這個TAG之前是否保存過
						int id = -1;
						for(int i=0; i<m_Cur_ReceiveCount; i++)
						{
							if(memcmp(m_pSaveOldData[m_CurSaveOldData_Index].TagId, m_ReceiveInfo[i].TagId, 2) == 0)
							{
								id = i;
								break;
							}
						}						
						if(id >= 0)
						{						
							//已經保存過了，把數據包直接保存下來
							m_ReceiveInfo[id].Type = m_pSaveOldData[m_CurSaveOldData_Index].Type;

							m_ReceiveInfo[id].Port1Id[0] = m_pSaveOldData[m_CurSaveOldData_Index].PortId[0];
							m_ReceiveInfo[id].Port1Id[1] = m_pSaveOldData[m_CurSaveOldData_Index].PortId[1];
							m_ReceiveInfo[id].Rssi1 = m_pSaveOldData[m_CurSaveOldData_Index].Rssi;

							m_ReceiveInfo[id].Battery = m_pSaveOldData[m_CurSaveOldData_Index].Battery;
							m_ReceiveInfo[id].SensorTime = m_pSaveOldData[m_CurSaveOldData_Index].SensorTime;

							m_ReceiveInfo[id].index = m_pSaveOldData[m_CurSaveOldData_Index].Index;
							m_ReceiveInfo[id].IsUpdate = TRUE;
							m_ReceiveInfo[id].FirstReceiveTime = m_time_cur.GetDay()*24*3600+m_time_cur.GetHour()*3600+m_pSaveOldData[m_CurSaveOldData_Index].Minute*60 + m_pSaveOldData[m_CurSaveOldData_Index].Secondes;
							m_ReceiveInfo[id].Port2Id[0] = 0;
							m_ReceiveInfo[id].Port2Id[1] = 0;
							m_ReceiveInfo[id].Rssi2 = 0xFF;
							m_ReceiveInfo[id].Port3Id[0] = 0;
							m_ReceiveInfo[id].Port3Id[1] = 0;
							m_ReceiveInfo[id].Rssi3 = 0xFF;
							m_ReceiveInfo[id].IsAladyWarringTimeOut = FALSE;
							m_ReceiveInfo[id].IsAladySaveData = FALSE;														
						}
						else
						{
							//之前沒有接收過這個TAG，添加
							if(m_Cur_ReceiveCount < MAX_RECEIVE_COUNT)
							{
								id = m_Cur_ReceiveCount;
								m_Cur_ReceiveCount++;
								m_ReceiveInfo[id].Type = m_pSaveOldData[m_CurSaveOldData_Index].Type;
								m_ReceiveInfo[id].TagId[0] = m_pSaveOldData[m_CurSaveOldData_Index].TagId[0];
								m_ReceiveInfo[id].TagId[1] = m_pSaveOldData[m_CurSaveOldData_Index].TagId[1];

								m_ReceiveInfo[id].Port1Id[0] = m_pSaveOldData[m_CurSaveOldData_Index].PortId[0];
								m_ReceiveInfo[id].Port1Id[1] = m_pSaveOldData[m_CurSaveOldData_Index].PortId[1];
								m_ReceiveInfo[id].Rssi1 = m_pSaveOldData[m_CurSaveOldData_Index].Rssi;

								m_ReceiveInfo[id].Battery = m_pSaveOldData[m_CurSaveOldData_Index].Battery;
								m_ReceiveInfo[id].SensorTime = m_pSaveOldData[m_CurSaveOldData_Index].SensorTime;

								m_ReceiveInfo[id].index = m_pSaveOldData[m_CurSaveOldData_Index].Index;
								m_ReceiveInfo[id].IsUpdate = TRUE;
								m_ReceiveInfo[id].FirstReceiveTime = m_time_cur.GetDay()*24*3600+m_time_cur.GetHour()*3600+m_pSaveOldData[m_CurSaveOldData_Index].Minute*60 + m_pSaveOldData[m_CurSaveOldData_Index].Secondes;
								m_ReceiveInfo[id].Port2Id[0] = 0;
								m_ReceiveInfo[id].Port2Id[1] = 0;
								m_ReceiveInfo[id].Rssi2 = 0xFF;
								m_ReceiveInfo[id].Port3Id[0] = 0;
								m_ReceiveInfo[id].Port3Id[1] = 0;
								m_ReceiveInfo[id].Rssi3 = 0xFF;
								m_ReceiveInfo[id].IsAladyWarringTimeOut = FALSE;
								m_ReceiveInfo[id].IsAladySaveData = FALSE;	

								m_ReceiveInfo[id].LastPortId[0] = 0;
								m_ReceiveInfo[id].LastPortId[1] = 0;
								m_ReceiveInfo[id].SendTimeOut = 0;
								m_ReceiveInfo[id].IsAladyWarringTimeOut = FALSE;
								m_ReceiveInfo[id].IsAladySaveData = FALSE;
							}
							else
							{
								//沒有空位置了，不添加
							}
						}

					}
					else
					{
						//保存的時間在當前顯示的時間的後面，先不顯示
						break;
					}
				}
			}
			//顯示時間
			str.Format(TEXT("%d-%02d-%02d %02d:%02d:%02d"), m_time_cur.GetYear(), m_time_cur.GetMonth(), m_time_cur.GetDay(), m_time_cur.GetHour(), m_time_cur.GetMinute(), m_time_cur.GetSecond());
			m_static_time.SetWindowTextW(str);
			//刪除超時沒有接收到的定位信息的點
			if(m_bNoShowTimeOutID)
			{
				DWORD time = m_time_cur.GetDay()*24*3600+m_time_cur.GetHour()*3600+m_time_cur.GetMinute()*60+m_time_cur.GetSecond();
				for(int i=0; i<m_Cur_ReceiveCount; i++)
				{				
					if(time - m_ReceiveInfo[i].FirstReceiveTime > m_ShowTimeOutTime)
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
			//顯示圖像
			PaintImage();
			
			//時間+1秒
			m_time_cur = m_time_cur + CTimeSpan(0, 0, 0, 1);
			if(m_time_cur.GetMinute() == 0 && m_time_cur.GetSecond() == 0)
			{
				if(m_pSaveOldData != (SaveOldData*)0xFFFFFFFF)
				{
					delete[] m_pSaveOldData;
					m_pSaveOldData = NULL;
				}
			}
		}
		else
		{
			//結束回放
			OnBnClickedButtonStart();
		}		
	}
	CDialog::OnTimer(nIDEvent);
}

void COldDataRePlay::OnBnClickedButtonSpeedAdd()
{
	// TODO: Add your control notification handler code here
	if(m_bPlayStart && !m_bPlayPause)
	{
		m_PlaySpeed++;
		if(m_PlaySpeed > 19)
			m_PlaySpeed = 19;
		CString str;
		if(m_PlaySpeed >= 10)
		{
			KillTimer(1);
			SetTimer(1, 1000/(m_PlaySpeed-9), NULL);
			str.Format(TEXT("%dX"), (m_PlaySpeed-9));
		}
		else
		{
			KillTimer(1);
			SetTimer(1, 1000*(11-m_PlaySpeed), NULL);
			str.Format(TEXT("1/%dX"), (11-m_PlaySpeed));
		}
		m_static_speed.SetWindowTextW(str);
	}
}

void COldDataRePlay::OnBnClickedButtonSpeedDec()
{
	// TODO: Add your control notification handler code here
	if(m_bPlayStart && !m_bPlayPause)
	{
		m_PlaySpeed--;
		if(m_PlaySpeed < 1)
			m_PlaySpeed = 1;
		CString str;
		if(m_PlaySpeed >= 10)
		{
			KillTimer(1);
			SetTimer(1, 1000/(m_PlaySpeed-9), NULL);
			str.Format(TEXT("%dX"), (m_PlaySpeed-9));
		}
		else
		{
			KillTimer(1);
			SetTimer(1, 1000*(11-m_PlaySpeed), NULL);
			str.Format(TEXT("1/%dX"), (11-m_PlaySpeed));
		}
		m_static_speed.SetWindowTextW(str);
	}
}

void COldDataRePlay::OnBnClickedButtonPasue()
{
	// TODO: Add your control notification handler code here
	if(m_bPlayStart)
	{
		if(m_bPlayPause)
		{
			m_bPlayPause = FALSE;
			if(m_PlaySpeed >= 10)
			{	
				SetTimer(1, 1000/(m_PlaySpeed-9), NULL);				
			}
			else
			{	
				SetTimer(1, 1000*(11-m_PlaySpeed), NULL);				
			}
			m_button_pause.SetWindowTextW(TEXT("暫停"));
		}
		else
		{
			KillTimer(1);
			m_bPlayPause = TRUE;
			m_button_pause.SetWindowTextW(TEXT("繼續"));
		}
	}
}
