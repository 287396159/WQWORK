// OldDataTongJi.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "OldDataTongJi.h"


// COldDataTongJi dialog

IMPLEMENT_DYNAMIC(COldDataTongJi, CDialog)

COldDataTongJi::COldDataTongJi(CWnd* pParent /*=NULL*/)
	: CDialog(COldDataTongJi::IDD, pParent)
{

}

COldDataTongJi::~COldDataTongJi()
{
}

void COldDataTongJi::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_DATETIMEPICKER_START, m_datatime_start);
	DDX_Control(pDX, IDC_COMBO_HOUR_START, m_combo_hour_start);
	DDX_Control(pDX, IDC_COMBO_MINUTE_START, m_combo_minute_start);
	DDX_Control(pDX, IDC_DATETIMEPICKER_END, m_datatime_end);
	DDX_Control(pDX, IDC_COMBO_HOUR_END, m_combo_hour_end);
	DDX_Control(pDX, IDC_COMBO_MINUTE_END, m_combo_minute_end);
	DDX_Control(pDX, IDC_LIST1, m_list);
}


BEGIN_MESSAGE_MAP(COldDataTongJi, CDialog)
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_WM_SHOWWINDOW()
	ON_BN_CLICKED(IDC_BUTTON_START, &COldDataTongJi::OnBnClickedButtonStart)
	ON_LBN_SELCHANGE(IDC_LIST1, &COldDataTongJi::OnLbnSelchangeList1)
	ON_WM_MOUSEMOVE()
	ON_WM_MOUSEWHEEL()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
END_MESSAGE_MAP()


// COldDataTongJi message handlers

BOOL COldDataTongJi::OnInitDialog() {
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here

	m_Brush_white = ::CreateSolidBrush(RGB(255, 255, 255));
	m_Brush_blue = ::CreateSolidBrush(RGB(0, 0, 255));
	m_Brush_red = ::CreateSolidBrush(RGB(255, 0, 0));
	m_Brush_black = ::CreateSolidBrush(RGB(0, 0, 0));
	m_Brush_violet = ::CreateSolidBrush(RGB(255, 0, 255));
		
	m_time_start = CTime::GetCurrentTime();
	m_time_end = CTime::GetCurrentTime();
	m_NormalTime = 0;
	m_WarringTime = 0;
	m_NoMoveTime = 0;
	m_LostTime = 0;

	m_CurSelTag = -1;

//	m_pReadSaveData = NULL;
	m_CurTongJiCount = 0;

	m_bLBtnDown = FALSE;

	CTime t = CTime::GetCurrentTime();
	CTime t2 = t - CTimeSpan(1, 0, 0, 0);
	m_datatime_end.SetTime(&t);
	m_combo_hour_end.SetCurSel(t.GetHour());
	m_combo_minute_end.SetCurSel(t.GetMinute());

	m_datatime_start.SetTime(&t2);
	m_combo_hour_start.SetCurSel(t.GetHour());
	m_combo_minute_start.SetCurSel(t.GetMinute());

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void COldDataTongJi::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	// TODO: Add your message handler code here
	// Do not call CDialog::OnPaint() for painting messages
	Draw();
}

void COldDataTongJi::Draw()
{
	int i, j, x, y;
	RECT r;
	r.left = 0;
	r.top = 0;
	r.right = m_DrawWidth;
	r.bottom = m_DrawHeight;
	
	::FillRect(m_mdc, &r, m_Brush_white);
	::MoveToEx(m_mdc, 0, 0, NULL);
	::LineTo(m_mdc, m_DrawWidth-1, 0);
	::LineTo(m_mdc, m_DrawWidth-1, m_DrawHeight-1);
	::LineTo(m_mdc, 0, m_DrawHeight-1);
	::LineTo(m_mdc, 0, 0);

	SetBkMode(m_mdc, TRANSPARENT);
	CString str;
	DWORD time = m_time_end.GetTime() - m_time_start.GetTime();
	//顯示統計時間
	if(time >= 24*3600)
		str.Format(TEXT("統計時間：%d-%d-%d %d:%d --- %d-%d-%d %d:%d，總共：%d天%d小時%d分%d秒"), m_time_start.GetYear(), m_time_start.GetMonth(), m_time_start.GetDay(), m_time_start.GetHour(), m_time_start.GetMinute(), 
																								m_time_end.GetYear(), m_time_end.GetMonth(), m_time_end.GetDay(), m_time_end.GetHour(), m_time_end.GetMinute(),
																								time/(24*3600), (time%(24*3600))/3600, (time%(3600))/60, (time%(60)));
	else if(time >= 3600)
		str.Format(TEXT("統計時間：%d-%d-%d %d:%d --- %d-%d-%d %d:%d，總共：%d小時%d分%d秒"), m_time_start.GetYear(), m_time_start.GetMonth(), m_time_start.GetDay(), m_time_start.GetHour(), m_time_start.GetMinute(), 
																								m_time_end.GetYear(), m_time_end.GetMonth(), m_time_end.GetDay(), m_time_end.GetHour(), m_time_end.GetMinute(),
																								(time)/3600, (time%(3600))/60, (time%(60)));
	else if(time >= 60)
		str.Format(TEXT("統計時間：%d-%d-%d %d:%d --- %d-%d-%d %d:%d，總共：%d分%d秒"), m_time_start.GetYear(), m_time_start.GetMonth(), m_time_start.GetDay(), m_time_start.GetHour(), m_time_start.GetMinute(), 
																								m_time_end.GetYear(), m_time_end.GetMonth(), m_time_end.GetDay(), m_time_end.GetHour(), m_time_end.GetMinute(),
																								(time)/60, (time%(60)));
	else
		str.Format(TEXT("統計時間：%d-%d-%d %d:%d --- %d-%d-%d %d:%d，總共：%d秒"), m_time_start.GetYear(), m_time_start.GetMonth(), m_time_start.GetDay(), m_time_start.GetHour(), m_time_start.GetMinute(), 
																								m_time_end.GetYear(), m_time_end.GetMonth(), m_time_end.GetDay(), m_time_end.GetHour(), m_time_end.GetMinute(),
																								(time));
	r.left = 5; r.top = 1; r.right = 500; r.bottom = 20;
	::DrawText(m_mdc, str, -1, &r, DT_LEFT|DT_VCENTER|DT_SINGLELINE);	

	//顯示不同顏色的統計結果
	r.left = 5; r.top = 24; r.right = 17; r.bottom = 36;
	::FillRect(m_mdc, &r, m_Brush_blue);
	if(m_NormalTime >= 24*3600)
		str.Format(TEXT("正常定位時間%d天%d小時%d分%d秒"), m_NormalTime/(24*3600), (m_NormalTime%(24*3600))/3600, (m_NormalTime%(3600))/60, (m_NormalTime%(60)));
	else if(m_NormalTime >= 3600)
		str.Format(TEXT("正常定位時間%d小時%d分%d秒"), (m_NormalTime)/3600, (m_NormalTime%(3600))/60, (m_NormalTime%(60)));
	else if(m_NormalTime >= 60)
		str.Format(TEXT("正常定位時間%d分%d秒"), (m_NormalTime)/60, (m_NormalTime%(60)));
	else
		str.Format(TEXT("正常定位時間%d秒"), (m_NormalTime));
	r.left = 20; r.top = 20; r.right = 220; r.bottom = 40;
	::DrawText(m_mdc, str, -1, &r, DT_LEFT|DT_VCENTER|DT_SINGLELINE);

	r.left = 220; r.top = 24; r.right = 232; r.bottom = 36;
	::FillRect(m_mdc, &r, m_Brush_black);
	if(m_NoMoveTime >= 24*3600)
		str.Format(TEXT("卡片未移動時間%d天%d小時%d分%d秒"), m_NoMoveTime/(24*3600), (m_NoMoveTime%(24*3600))/3600, (m_NoMoveTime%(3600))/60, (m_NoMoveTime%(60)));
	else if(m_NoMoveTime >= 3600)
		str.Format(TEXT("卡片未移動時間%d小時%d分%d秒"), (m_NoMoveTime)/3600, (m_NoMoveTime%(3600))/60, (m_NoMoveTime%(60)));
	else if(m_NoMoveTime >= 60)
		str.Format(TEXT("卡片未移動時間%d分%d秒"), (m_NoMoveTime)/60, (m_NoMoveTime%(60)));
	else
		str.Format(TEXT("卡片未移動時間%d秒"), (m_NoMoveTime));
	r.left = 235; r.top = 20; r.right = 435; r.bottom = 40;
	::DrawText(m_mdc, str, -1, &r, DT_LEFT|DT_VCENTER|DT_SINGLELINE);	

	r.left = 435; r.top = 24; r.right = 447; r.bottom = 36;
	::FillRect(m_mdc, &r, m_Brush_violet);
	if(m_LostTime >= 24*3600)
		str.Format(TEXT("卡片丟包時間%d天%d小時%d分%d秒"), m_LostTime/(24*3600), (m_LostTime%(24*3600))/3600, (m_LostTime%(3600))/60, (m_LostTime%(60)));
	else if(m_LostTime >= 3600)
		str.Format(TEXT("卡片丟包時間%d小時%d分%d秒"), (m_LostTime)/3600, (m_LostTime%(3600))/60, (m_LostTime%(60)));
	else if(m_LostTime >= 60)
		str.Format(TEXT("卡片丟包時間%d分%d秒"), (m_LostTime)/60, (m_LostTime%(60)));
	else
		str.Format(TEXT("卡片丟包時間%d秒"), (m_LostTime));
	r.left = 450; r.top = 20; r.right = 650; r.bottom = 40;
	::DrawText(m_mdc, str, -1, &r, DT_LEFT|DT_VCENTER|DT_SINGLELINE);	

	r.left = 650; r.top = 24; r.right = 662; r.bottom = 36;
	::FillRect(m_mdc, &r, m_Brush_red);
	if(m_WarringTime >= 24*3600)
		str.Format(TEXT("緊急報警時間%d天%d小時%d分%d秒"), m_WarringTime/(24*3600), (m_WarringTime%(24*3600))/3600, (m_WarringTime%(3600))/60, (m_WarringTime%(60)));
	else if(m_WarringTime >= 3600)
		str.Format(TEXT("緊急報警時間%d小時%d分%d秒"), (m_WarringTime)/3600, (m_WarringTime%(3600))/60, (m_WarringTime%(60)));
	else if(m_WarringTime >= 60)
		str.Format(TEXT("緊急報警時間%d分%d秒"), (m_WarringTime)/60, (m_WarringTime%(60)));
	else
		str.Format(TEXT("緊急報警時間%d秒"), (m_WarringTime));
	r.left = 665; r.top = 20; r.right = 865; r.bottom = 40;
	::DrawText(m_mdc, str, -1, &r, DT_LEFT|DT_VCENTER|DT_SINGLELINE);	

	//畫統計圖
	if(m_CurSelTag >= 0)
	{
		int left, top, width, height;
		int portheight = TONGJI_DRAW_PORT_HEIGHT;
		int drawportcount;
		m_DrawPortStart = 0;

		left = TONGJI_DRAW_LEFT_PORT_WIDTH;	//左邊留來畫參考點ID
		width = m_DrawWidth - TONGJI_DRAW_RIGHT_TEXT_WIDTH - TONGJI_DRAW_LEFT_PORT_WIDTH;	//右邊留100來寫字等
		height = m_DrawHeight - 40 - 40;	//下方留40來寫字
		drawportcount = height/portheight;
		height = drawportcount*portheight;
		top = m_DrawHeight - 40 - height;

		
		//畫出L型的邊框
		::MoveToEx(m_mdc, left, top, NULL);
		::LineTo(m_mdc, left, top+height);
		::LineTo(m_mdc, left+width, top+height);
		//參考點間隔之間，用虛線分割
		for(i=0; i<drawportcount; i++)
		{
			//從上往下畫
			y = top + i * portheight;
			for(j=1; j<width-5; j=j+10)
			{
				x = left + j;
				::MoveToEx(m_mdc, x, y, NULL);
				::LineTo(m_mdc, x+5, y);
			}
		}
		//從下到上，寫上參考點的ID
		for(i=0; i<drawportcount; i++)
		{
			if(m_DrawPortStart+i >= m_CurTongJiPortCount)
				break;
			str.Format(TEXT("%02X%02X"), m_TongJiPort[m_DrawPortStart+i][0], m_TongJiPort[m_DrawPortStart+i][1]);
			GetPortName(str);
			r.left = 1; r.right = 115; r.bottom = top+height-i*portheight; r.top = r.bottom - portheight;
			::DrawText(m_mdc, str, -1, &r, DT_RIGHT|DT_VCENTER|DT_SINGLELINE);	
			//在每個參考點，右邊畫上該參考點的時間
			if(m_PortTime[m_DrawPortStart+i] >= 24*3600)
				str.Format(TEXT("%d天%d小時%d分%d秒"), m_PortTime[m_DrawPortStart+i]/(24*3600), (m_PortTime[m_DrawPortStart+i]%(24*3600))/3600, (m_PortTime[m_DrawPortStart+i]%(3600))/60, (m_PortTime[m_DrawPortStart+i]%(60)));
			else if(m_PortTime[m_DrawPortStart+i] >= 3600)
				str.Format(TEXT("%d小時%d分%d秒"), (m_PortTime[m_DrawPortStart+i])/3600, (m_PortTime[m_DrawPortStart+i]%(3600))/60, (m_PortTime[m_DrawPortStart+i]%(60)));
			else if(m_PortTime[m_DrawPortStart+i] >= 60)
				str.Format(TEXT("%d分%d秒"), (m_PortTime[m_DrawPortStart+i])/60, (m_PortTime[m_DrawPortStart+i]%(60)));
			else
				str.Format(TEXT("%d秒"), (m_PortTime[m_DrawPortStart+i]));
			r.left = left+width+5; r.right = m_DrawWidth; 
			::DrawText(m_mdc, str, -1, &r, DT_LEFT|DT_VCENTER|DT_SINGLELINE);	
		}		
		//從左到右，寫上X軸的時間，每隔150像素寫一次
		str.Format(TEXT("%d-%02d-%02d %02d:%02d:%02d"), m_time_start_draw.GetYear(), m_time_start_draw.GetMonth(), m_time_start_draw.GetDay(), m_time_start_draw.GetHour(), m_time_start_draw.GetMinute(), m_time_start_draw.GetSecond());
		r.left = left-65; r.right = left+65; r.top = top+height+10; r.bottom = r.top + 20;
		::DrawText(m_mdc, str, -1, &r, DT_CENTER|DT_VCENTER|DT_SINGLELINE);	
		CTime t;
		m_DrawXTimeStart = (DWORD)(m_time_start_draw.GetTime() - m_time_start.GetTime());
		m_DrawXTimeCount = (DWORD)(m_time_end_draw.GetTime() - m_time_start_draw.GetTime());
		DWORD second;		
		int xw = 130;
		int xc = width/xw;
		xw = width/xc;
		for(i=1; i<=xc; i=i+1)
		{
			//畫一個小豎線
			x = i*xw + left;
			::MoveToEx(m_mdc, x, top+height, NULL);
			::LineTo(m_mdc, x, top+height+5);
			//計算時間
			second = i*xw*m_DrawXTimeCount/width;
			t = m_time_start_draw + CTimeSpan(second);
			str.Format(TEXT("%d-%02d-%02d %02d:%02d:%02d"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), t.GetMinute(), t.GetSecond());
			r.left = x-xw/2; r.right = x+xw/2; r.top = top+height+10; r.bottom = r.top + 20;
			::DrawText(m_mdc, str, -1, &r, DT_CENTER|DT_VCENTER|DT_SINGLELINE);	
		}
		int port[MAX_SAVE_PORT_COUNT];
		int tagtype[TONGJILEIXING_COUNT];
		int pi, ti;
		//根據時間點來畫圖
		if(m_DrawXTimeCount >= width)
		{
			//要顯示在圖上的時間點多餘可以畫圖的像素，也就是1個像素要顯示多個秒的數據
			x = m_DrawXTimeStart;
			for(i=1; i<=width; i++)
			{
				second = i*m_DrawXTimeCount/width + m_DrawXTimeStart;
				//從x到second這個時間段，先統計在那個參考點的時間多
				memset(port, 0, sizeof(port));
				memset(tagtype, 0, sizeof(tagtype));
				for(j=x; j<second; j++)
				{
					port[(m_TongJiJieGuo[m_CurSelTag].Data[j])>>3] += 1;					
				}
				//找到最多的參考點
				pi = 0;
				for(j=1; j<m_CurTongJiPortCount; j++)
					if(port[j] > port[pi])
						pi = j;
				//在最多的參考點上找到最多的顯示類型
				for(j=x; j<second; j++)
				{
					if(((m_TongJiJieGuo[m_CurSelTag].Data[j])>>3) == pi)
						tagtype[(m_TongJiJieGuo[m_CurSelTag].Data[j])&7] += 1;
				}
				ti = 0;
				for(j=1; j<TONGJILEIXING_COUNT; j++)
					if(tagtype[j] > tagtype[ti])
						ti = j;
				//根據pi和ti的值來畫圖
				if(ti > TONGJILEIXING_NODATA)
				{
					r.left = left+i; r.right = r.left +1 ;
					r.bottom = top+height-pi*portheight;
					r.top = r.bottom - portheight + 1;
					switch(ti)
					{
					case TONGJILEIXING_NORMAL:  ::FillRect(m_mdc, &r, m_Brush_blue);
						break;
					case TONGJILEIXING_WARRING:  ::FillRect(m_mdc, &r, m_Brush_red);
						break;
					case TONGJILEIXING_NOMOVE:  ::FillRect(m_mdc, &r, m_Brush_black);
						break;
					case TONGJILEIXING_LOST:  ::FillRect(m_mdc, &r, m_Brush_violet);
						break;					
					}
				}
				x = second;
			}
		}
		else
		{
			//1秒的數據要顯示在多個像素點上
			for(i=0; i<m_DrawXTimeCount; i++)
			{
				r.left = left + i*width/m_DrawXTimeCount;
				r.right = left + (i+1)*width/m_DrawXTimeCount;
				pi = (m_TongJiJieGuo[m_CurSelTag].Data[m_DrawXTimeStart+i])>>3;
				ti = (m_TongJiJieGuo[m_CurSelTag].Data[m_DrawXTimeStart+i])&7;
				r.bottom = top+height-pi*portheight;
				r.top = r.bottom - portheight + 1;
				switch(ti)
				{
				case TONGJILEIXING_NORMAL:  ::FillRect(m_mdc, &r, m_Brush_blue);
					break;
				case TONGJILEIXING_WARRING:  ::FillRect(m_mdc, &r, m_Brush_red);
					break;
				case TONGJILEIXING_NOMOVE:  ::FillRect(m_mdc, &r, m_Brush_black);
					break;
				case TONGJILEIXING_LOST:  ::FillRect(m_mdc, &r, m_Brush_violet);
					break;					
				}
			}
		}		
	}
	

	BitBlt(m_hdc, m_DrawLeft, m_DrawTop, m_DrawWidth, m_DrawHeight, m_mdc, 0, 0, SRCCOPY);

}
void COldDataTongJi::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	// TODO: Add your message handler code here
	CRect rs; 
	this->GetClientRect(&rs);	

	::MoveWindow(m_list.GetSafeHwnd(), 10, 80, 150, rs.Height()-90, TRUE);

	m_DrawLeft = 170;
	m_DrawTop = 80;
	m_DrawWidth = rs.Width() - 180;
	m_DrawHeight = rs.Height() - 90;

	m_DrawLeft_TongJi = m_DrawLeft + TONGJI_DRAW_LEFT_PORT_WIDTH;	//左邊留來畫參考點ID
	m_DrawWidth_TongJi = m_DrawWidth - TONGJI_DRAW_RIGHT_TEXT_WIDTH - TONGJI_DRAW_LEFT_PORT_WIDTH;	//右邊留100來寫字等

	m_hdc = ::GetDC(this->GetSafeHwnd());
	m_mdc = ::CreateCompatibleDC(m_hdc);
	m_bmp = ::CreateCompatibleBitmap(m_hdc, m_DrawWidth, m_DrawHeight);
	SelectObject(m_mdc, m_bmp);
	m_font.CreateFont(12, 0, 0, 0, 0, 0, 0, 0, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,	DEFAULT_QUALITY, DEFAULT_PITCH | FF_SWISS, TEXT("宋體"));
	SelectObject(m_mdc, (HFONT)m_font);
}

void COldDataTongJi::OnShowWindow(BOOL bShow, UINT nStatus)
{
	CDialog::OnShowWindow(bShow, nStatus);

	// TODO: Add your message handler code here	
	if(bShow)
		this->SetFocus();
	else
		m_list.SetFocus();
}

void COldDataTongJi::OnBnClickedButtonStart()
{
	// TODO: Add your control notification handler code here
	BOOL	bNoMove	= TRUE;	//是否檢查未移動的卡片。
	int		NoMoveTime  = 60;
	CString str;
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	if(m_ini.Open(filepath, FALSE))
	{
		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowTagNoMove"), str))
		{								
			if(0 == str.Compare(TEXT("YES")))
				bNoMove = TRUE;
			else
				bNoMove = FALSE;
		}

		if(m_ini.GetValue(TEXT("Display"), TEXT("ShowTagNoMoveTime"), str))
		{								
			NoMoveTime = _ttoi(str);
		}
	}

	while(m_list.GetCount() > 0)
		m_list.DeleteString(0);
	m_CurSelTag = -1;

	CTime t;	
	m_datatime_start.GetTime(t);
	m_time_start = CTime(t.GetYear(), t.GetMonth(), t.GetDay(), m_combo_hour_start.GetCurSel(), m_combo_minute_start.GetCurSel(), 0);
	m_datatime_end.GetTime(t);
	m_time_end = CTime(t.GetYear(), t.GetMonth(), t.GetDay(), m_combo_hour_end.GetCurSel(), m_combo_minute_end.GetCurSel(), 0);
	if(m_time_start >= m_time_end) {
		Draw();
		return;
	}

	TongJiLen = (DWORD)(m_time_end.GetTime() - m_time_start.GetTime()) + 1;
	//結束前1個小時的文件，最后一個小時的文件單獨讀取
	t = m_time_end - CTimeSpan(0, 1, 0, 0);
	CTime time_end_before1hour = CTime(t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), 59, 59);	

	//先清空以前裝載的文件
	int i;
	for(i=0; i<m_CurTongJiCount; i++)
	{
		delete[] m_TongJiJieGuo[i].Data;
	}
	memset(m_TongJiJieGuo, 0, sizeof(m_TongJiJieGuo));
	m_CurTongJiCount = 0;
	m_CurTongJiPortCount = 0;
	
	//讀取保存的數據文件	
	GetModulePath(szPath, NULL);
	filepath = szPath;
	filepath = filepath + TEXT("SaveData\\");

	HANDLE hfile;
	DWORD filesize;	
	DWORD Read;
	SaveOldData *psod;
	int SaveDataCount;
	int index, indexport;
	CTime tcur;
	DWORD CurDataIndex;
	int j;
	BOOL bReadLastOne = FALSE;
	
	t = m_time_start;
	while(t <= time_end_before1hour)
	{
		//查找當前這個時間點是否存在文件
		str.Format(TEXT("%d%02d%02d\\%02d.dat"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour());
		str = filepath + str;
		hfile = CreateFile(str, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ|FILE_SHARE_WRITE|FILE_SHARE_DELETE, NULL, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, NULL); 
		if(hfile != INVALID_HANDLE_VALUE)
		{
			//文件存在，申請內存來保存文件
			filesize = ::GetFileSize(hfile, NULL);
			SaveDataCount = filesize / sizeof(SaveOldData);
			psod = new SaveOldData[SaveDataCount];
			if(psod == NULL)
			{
				MessageBox(TEXT("內存不足，讀取數據文件失敗，請縮短統計時間，再重試！"));
				CloseHandle(hfile);				
				return;
			}
			ReadFile(hfile, psod, filesize, &Read, NULL);
			if(filesize != Read)
			{
				MessageBox(TEXT("讀取文件長度不符合"));
			}
			CloseHandle(hfile);			
			if(t == m_time_start)
			{
				//是讀取的第一個文件，檢查開始時間的分鐘數是否為0，
				if(t.GetMinute() != 0)
				{
					//開始時間的分鐘數不為0，那麼把設定分鐘數之前的數據都丟掉
					for(i=0; i<SaveDataCount; i++)
					{
						if(psod[i].Minute >= t.GetMinute())
							break;
					}
					SaveDataCount = SaveDataCount - i;
					memcpy(psod, psod + i, SaveDataCount*sizeof(SaveOldData));
				}
			}
			//從讀取到的文件，進行分析
DOSAVE:
			for(i=0; i<SaveDataCount; i++)
			{
				//查找這個Port
				indexport = FindTongJiPort(psod[i].PortId);
				if(indexport >= 0)
				{
					//這個參考點已經保存
				}
				else
				{
					//這個參考點未保存，先保存起來
					if(m_CurTongJiPortCount < MAX_SAVE_PORT_COUNT)
					{
						indexport = m_CurTongJiPortCount;
						m_CurTongJiPortCount++;
						m_TongJiPort[indexport][0] = psod[i].PortId[0];
						m_TongJiPort[indexport][1] = psod[i].PortId[1];
					}
					else
					{
						MessageBox(TEXT("參考點太多"));
						return;
					}
				}
				//先查找這個TAG，是否有統計過了
				index = FindTongJiTag(psod[i].TagId);
				if(index >=0 )
				{
					//以前已經統計過了
					//計算當前的儲存數據的序列號
					tcur = CTime(t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), psod[i].Minute, psod[i].Secondes);
					CurDataIndex = (DWORD)(tcur.GetTime() - m_time_start.GetTime());
							
					//比對新收到的數據的時間點和上一次的比較
					if(CurDataIndex > m_TongJiJieGuo[index].LastDataIndex)
					{
						//新的位置，比以前的位置大，需要存儲數據
						//查看上一次的定位類型
						if(m_TongJiJieGuo[index].LastType == 1)
						{
							//普通定位
							//比較2次的序列號
							if((BYTE)(m_TongJiJieGuo[index].LastReceIndex + 1) == psod[i].Index)
							{
								//序列號正常遞加，查看是否超時為未移動
								//本次存儲點之前的點存儲為上一次的定位數據
								for(j=m_TongJiJieGuo[index].LastDataIndex+1; j<CurDataIndex; j++)
									m_TongJiJieGuo[index].Data[j] = m_TongJiJieGuo[index].Data[m_TongJiJieGuo[index].LastDataIndex];
								if(bNoMove)
								{
									//查找未移動的卡片
									if(psod[i].SensorTime != 0xFFFF && psod[i].SensorTime >= NoMoveTime)
									{
										m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NOMOVE;
									}
									else
										m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
								}
								else
								{
									m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
								}
								//更新資訊
								m_TongJiJieGuo[index].LastDataIndex = CurDataIndex;
								m_TongJiJieGuo[index].LastReceIndex = psod[i].Index;
								m_TongJiJieGuo[index].LastType = psod[i].Type;
							}
							else if((BYTE)(m_TongJiJieGuo[index].LastReceIndex) == psod[i].Index)
							{
								//序列號相同，不處理
								m_TongJiJieGuo[index].LastType = psod[i].Type;
							}
							else
							{
								//兩次的序列號不為遞加，出現了丟包現象
								int losti = (256+psod[i].Index - m_TongJiJieGuo[index].LastReceIndex)%256;
								int losts = CurDataIndex - m_TongJiJieGuo[index].LastDataIndex;
								unsigned short value = m_TongJiJieGuo[index].Data[m_TongJiJieGuo[index].LastDataIndex];
								//填充上一次接收信息的數據點
								for(j=m_TongJiJieGuo[index].LastDataIndex+1; j<m_TongJiJieGuo[index].LastDataIndex+losts/losti; j++)
									m_TongJiJieGuo[index].Data[j] = value;
								//剩下的點做為丟包填充
								value = (value & 0xFFF8 | TONGJILEIXING_LOST);
								for(; j<CurDataIndex; j++)
									m_TongJiJieGuo[index].Data[j] = value;
								
								//填寫新點的類型
								if(psod[i].Type == 1)
								{
									//普通定位
									if(bNoMove)
									{
										//查找未移動的卡片
										if(psod[i].SensorTime != 0xFFFF && psod[i].SensorTime >= NoMoveTime)
										{
											m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NOMOVE;
										}
										else
											m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
									}
									else
									{
										m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
									}
								}
								else
									m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|psod[i].Type;
								//更新資訊
								m_TongJiJieGuo[index].LastDataIndex = CurDataIndex;
								m_TongJiJieGuo[index].LastReceIndex = psod[i].Index;
								m_TongJiJieGuo[index].LastType = psod[i].Type;
							}
						}
						else
						{
							//緊急定位
							//把之前的所有要儲存的點都保存為上一個參考點的緊急定位模式
							for(j=m_TongJiJieGuo[index].LastDataIndex+1; j<CurDataIndex; j++)
							{
								m_TongJiJieGuo[index].Data[j] = m_TongJiJieGuo[index].Data[m_TongJiJieGuo[index].LastDataIndex];
							}
							//新儲存點為新的定位模式
							if(psod[i].Type == 1)
							{
								//普通定位
								if(bNoMove)
								{
									//查找未移動的卡片
									if(psod[i].SensorTime != 0xFFFF && psod[i].SensorTime >= NoMoveTime)
									{
										m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NOMOVE;
									}
									else
										m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
								}
								else
								{
									m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
								}
							}
							else
								m_TongJiJieGuo[index].Data[CurDataIndex] = (indexport<<3)|psod[i].Type;							
							//更新資訊
							m_TongJiJieGuo[index].LastDataIndex = CurDataIndex;
							m_TongJiJieGuo[index].LastReceIndex = psod[i].Index;
							m_TongJiJieGuo[index].LastType = psod[i].Type;
						}
					}
					else
					{
						//兩次的位置相同，只需要修改類型即可
						m_TongJiJieGuo[index].LastType = psod[i].Type;
						m_TongJiJieGuo[index].LastReceIndex = psod[i].Index;
					}
				}
				else
				{
					//以前未統計過，添加
					if(m_CurTongJiCount < MAX_SAVE_TAG_COUNT)
					{
						m_TongJiJieGuo[m_CurTongJiCount].TagId[0] = psod[i].TagId[0];
						m_TongJiJieGuo[m_CurTongJiCount].TagId[1] = psod[i].TagId[1];
						m_TongJiJieGuo[m_CurTongJiCount].Data = new unsigned short[TongJiLen];
						if(m_TongJiJieGuo[m_CurTongJiCount].Data == NULL)
						{
							MessageBox(TEXT("記憶體不足，請縮短統計時間，重試！"));
							return;
						}
						memset(m_TongJiJieGuo[m_CurTongJiCount].Data, 0, TongJiLen*2);
						tcur = CTime(t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour(), psod[i].Minute, psod[i].Secondes);
						CurDataIndex = (DWORD)(tcur.GetTime() - m_time_start.GetTime());
						m_TongJiJieGuo[m_CurTongJiCount].LastDataIndex = CurDataIndex;
						m_TongJiJieGuo[m_CurTongJiCount].LastReceIndex = psod[i].Index;
						m_TongJiJieGuo[m_CurTongJiCount].LastType = psod[i].Type;
						
						//新儲存點為新的定位模式
						if(psod[i].Type == 1)
						{
							//普通定位
							if(bNoMove)
							{
								//查找未移動的卡片
								if(psod[i].SensorTime != 0xFFFF && psod[i].SensorTime >= NoMoveTime)
								{
									m_TongJiJieGuo[m_CurTongJiCount].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NOMOVE;
								}
								else
									m_TongJiJieGuo[m_CurTongJiCount].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
							}
							else
							{
								m_TongJiJieGuo[m_CurTongJiCount].Data[CurDataIndex] = (indexport<<3)|TONGJILEIXING_NORMAL;
							}
						}
						else
							m_TongJiJieGuo[m_CurTongJiCount].Data[CurDataIndex] = (indexport<<3)|psod[i].Type;						

						m_CurTongJiCount++;
					}
				}
			}
			//讀取的文件分析完畢，刪除文件內存
			delete[] psod;
		}
		//繼續查找下一個文件
		t = t + CTimeSpan(0, 1, 0, 0);
	}

	//讀取最後一個小時的文件	
	if(!bReadLastOne)
	{
		bReadLastOne = TRUE;
		t = m_time_end;
		//查找當前這個時間點是否存在文件
		str.Format(TEXT("%d%02d%02d\\%02d.dat"), t.GetYear(), t.GetMonth(), t.GetDay(), t.GetHour());
		str = filepath + str;
		hfile = CreateFile(str, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ|FILE_SHARE_WRITE|FILE_SHARE_DELETE, NULL, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, NULL); 
		if(hfile != INVALID_HANDLE_VALUE)
		{
			//文件存在，申請內存來保存文件
			filesize = ::GetFileSize(hfile, NULL);
			SaveDataCount = filesize / sizeof(SaveOldData);
			psod = new SaveOldData[SaveDataCount];
			if(psod == NULL)
			{
				MessageBox(TEXT("內存不足，讀取數據文件失敗，請縮短統計時間，再重試！"));
				CloseHandle(hfile);				
				return;
			}
			ReadFile(hfile, psod, filesize, &Read, NULL);
			if(filesize != Read)
			{
				MessageBox(TEXT("讀取文件長度不符合"));
			}
			CloseHandle(hfile);			
			
			if(t.GetYear() == m_time_start.GetYear() && t.GetMonth() == m_time_start.GetMonth() && t.GetDay() == m_time_start.GetDay() && t.GetHour() == m_time_start.GetHour())
			{
				//是讀取的第一個文件，檢查開始時間的分鐘數是否為0，
				if(m_time_start.GetMinute() != 0)
				{
					//開始時間的分鐘數不為0，那麼把設定分鐘數之前的數據都丟掉
					for(i=0; i<SaveDataCount; i++)
					{
						if(psod[i].Minute >= m_time_start.GetMinute())
							break;
					}
					SaveDataCount = SaveDataCount - i;
					memcpy(psod, psod + i, SaveDataCount*sizeof(SaveOldData));
				}
			}

			//是讀取的最后一個文件，把開始時間之前的數據清掉，			
			for(i=0; i<SaveDataCount; i++)
			{
				if(psod[i].Minute >= t.GetMinute())
					break;
			}
			SaveDataCount = i;

			goto DOSAVE;
		}
	}

	GetSaveTagInfo();
	GetSavePortInfo();

	//裝載完文件，檢索有哪些卡片ID，把卡片ID顯示到列表中
	while(m_list.GetCount() > 0)
		m_list.DeleteString(0);
	for(i=0; i<m_CurTongJiCount; i++)
	{
		str.Format(TEXT("%02X%02X"), m_TongJiJieGuo[i].TagId[0], m_TongJiJieGuo[i].TagId[1]);
		GetTagName(str);
		m_list.AddString(str);
	}

	m_list.SetCurSel(0);
	OnLbnSelchangeList1();
	
}

int COldDataTongJi::FindTongJiTag(BYTE id[])
{
	for(int i=0; i<m_CurTongJiCount; i++)
	{
		if(m_TongJiJieGuo[i].TagId[0] == id[0] && m_TongJiJieGuo[i].TagId[1] == id[1])
			return i;
	}
	return -1;
}

int COldDataTongJi::FindTongJiPort(BYTE id[])
{
	for(int i=0; i<m_CurTongJiPortCount; i++)
	{
		if(m_TongJiPort[i][0] == id[0] && m_TongJiPort[i][1] == id[1])
			return i;
	}
	return -1;
}

void COldDataTongJi::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}

void COldDataTongJi::OnLbnSelchangeList1()
{
	// TODO: Add your control notification handler code here
	m_CurSelTag = m_list.GetCurSel();
	if(m_CurSelTag < 0)
		return;
	m_time_start_draw = m_time_start;
	m_time_end_draw = m_time_end;

	m_NormalTime = 0;
	m_WarringTime = 0;
	m_NoMoveTime = 0;
	m_LostTime = 0;
	memset(m_PortTime, 0, sizeof(m_PortTime));
	for(int i=0; i<TongJiLen; i++)
	{		
		switch(m_TongJiJieGuo[m_CurSelTag].Data[i] & 0x7)
		{		
		case TONGJILEIXING_NORMAL:	//
			m_NormalTime++;
			break;
		case TONGJILEIXING_WARRING:	//
			m_WarringTime++;
			break;
		case TONGJILEIXING_NOMOVE:	//
			m_NoMoveTime++;
			break;
		case TONGJILEIXING_LOST:	//
			m_LostTime++;
			break;
		case TONGJILEIXING_NODATA:	//無定位數據
		default:
			continue;
			break;
		}
		m_PortTime[m_TongJiJieGuo[m_CurSelTag].Data[i]>>3] += 1;
	}

	Draw();
}


BOOL COldDataTongJi::GetSavePortInfo(void)
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

BOOL COldDataTongJi::GetSaveTagInfo(void)
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
		m_SaveTagInfo[m_Cur_SaveTagCount].Show = SHOW_TAG_NO;
	}
	return TRUE;
}

BOOL COldDataTongJi::GetTagName(CString &str_ID)
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

BOOL COldDataTongJi::GetPortName(CString &str_ID)
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
void COldDataTongJi::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	if(m_bLBtnDown)
	{
		//左右移動，移動統計畫圖的位置
		DWORD second_all = (DWORD)(m_time_end.GetTime() - m_time_start.GetTime());
		DWORD second_draw = (DWORD)(m_time_end_draw.GetTime() - m_time_start_draw.GetTime());
		CTime start = m_time_start_draw;
		CTime end = m_time_end_draw;
		if(second_draw < second_all)
		{
			//當有處于放大顯示的模式時，左右移動才有意義
			DWORD second_move;
			if(point.x > m_LBtnDownPoint.x)
			{
				//鼠標右移，
				second_move = second_draw*(point.x - m_LBtnDownPoint.x)/m_DrawWidth_TongJi;
				start = m_time_start_draw - CTimeSpan(second_move);
				if(start <= m_time_start)
				{
					//移過頭了
					start = m_time_start;
				}
				end = start + CTimeSpan(second_draw);				
			}
			else if(point.x < m_LBtnDownPoint.x)
			{
				//鼠標左移，
				second_move = second_draw*(m_LBtnDownPoint.x - point.x)/m_DrawWidth_TongJi;
				end = m_time_end_draw + CTimeSpan(second_move);
				if(end >= m_time_end)
				{
					//移過頭了
					end = m_time_end;
				}
				start = end - CTimeSpan(second_draw);				
			}
		}
		if(start != m_time_start_draw)
		{
			m_time_start_draw = start;
			m_time_end_draw = end;
			m_LBtnDownPoint = point;
		}
		Draw();

	}
	CDialog::OnMouseMove(nFlags, point);
}

BOOL COldDataTongJi::OnMouseWheel(UINT nFlags, short zDelta, CPoint pt)
{
	// TODO: Add your message handler code here and/or call default
	if(m_CurSelTag >= 0 && (pt.x >= m_DrawLeft_TongJi && pt.x <= m_DrawLeft_TongJi+m_DrawWidth_TongJi))
	{
		CTime start = m_time_start_draw;
		CTime end = m_time_end_draw;
		CTime point;
		DWORD seconds = (DWORD)(end.GetTime() - start.GetTime());
		DWORD pt_second;
		//放大縮小畫圖區
		if(zDelta > 0)
		{
			//放大一倍，
			if(seconds*5 > m_DrawWidth_TongJi)
			{
				//最大，放大到5個像素寬度顯示1秒的信息
				pt_second = seconds*(pt.x - m_DrawLeft_TongJi)/m_DrawWidth_TongJi;
				point = start + CTimeSpan(pt_second);
				start = point - CTimeSpan(pt_second/2);
				end = point + CTimeSpan((seconds - pt_second)/2);	
				if(start > end || start > m_time_end_draw)
					return TRUE;
				if(end > m_time_end_draw)
					end = m_time_end_draw;
				
				m_time_start_draw = start;
				m_time_end_draw = end;
			}
		}
		else
		{
			//縮小一倍
			if(seconds >= (DWORD)(m_time_end.GetTime() - m_time_start.GetTime()))
			{
				m_time_start_draw = m_time_start;
				m_time_end_draw = m_time_end;
				Draw();
				return TRUE;	//已經縮小到最小了
			}
			seconds = seconds * 2;	//縮小一倍
			if(seconds >= (DWORD)(m_time_end.GetTime() - m_time_start.GetTime()))
			{
			//	seconds = (DWORD)(m_time_end.GetTime() - m_time_start.GetTime()); //最大縮小到原始大小
				m_time_start_draw = m_time_start;
				m_time_end_draw = m_time_end;
				Draw();
				return TRUE;
			}
			//根據鼠標的位置來決定縮小點的時間
			pt_second = seconds*(pt.x - m_DrawLeft_TongJi)/m_DrawWidth_TongJi;
			point = start + CTimeSpan(pt_second/2);
			start = point - CTimeSpan(pt_second);
			end = point + CTimeSpan((seconds - pt_second));	
			if(start > end || start > m_time_end)
				return TRUE;
			if(start < m_time_start)
				start = m_time_start;
			if(end > m_time_end)
				end = m_time_end;
			
			m_time_start_draw = start;
			m_time_end_draw = end;

		}
		Draw();
	}

	return CDialog::OnMouseWheel(nFlags, zDelta, pt);
}

void COldDataTongJi::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	this->SetFocus();
	if(m_CurSelTag >= 0 && (point.x >= m_DrawLeft_TongJi && point.x <= m_DrawLeft_TongJi+m_DrawWidth_TongJi))
	{
		m_LBtnDownPoint = point;
		m_bLBtnDown = TRUE;
	}
	CDialog::OnLButtonDown(nFlags, point);
}

void COldDataTongJi::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO: Add your message handler code here and/or call default
	m_bLBtnDown = FALSE;
	CDialog::OnLButtonUp(nFlags, point);
}

BOOL COldDataTongJi::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
//	DWORD hwnd;
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
	
	return CDialog::PreTranslateMessage(pMsg);
}
