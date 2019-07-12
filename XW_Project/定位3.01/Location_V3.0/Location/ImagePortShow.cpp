// ImagePortShow.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "ImagePortShow.h"


// CImagePortShow dialog

IMPLEMENT_DYNAMIC(CImagePortShow, CDialog)

CImagePortShow::CImagePortShow(CWnd* pParent /*=NULL*/)
	: CDialog(CImagePortShow::IDD, pParent)
{

}

CImagePortShow::~CImagePortShow()
{
}

void CImagePortShow::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_STATIC_TITAL, m_static_tital);
	DDX_Control(pDX, IDC_LIST1, m_list);
}


BEGIN_MESSAGE_MAP(CImagePortShow, CDialog)
	ON_WM_SIZE()
END_MESSAGE_MAP()


// CImagePortShow message handlers


void CImagePortShow::SetMainHwnd(HWND hwnd)
{
	m_MainWindowHwnd = hwnd;
}

void CImagePortShow::UpdateTitle(CString str)
{
	m_static_tital.SetWindowTextW(str);
}

void CImagePortShow::UpdateSaveTag(ImageShowSaveTagInfo *issti, int count)
{
	m_Cur_SaveTagCount = count;
	for(int i=0; i<count; i++)
	{
		m_SaveTagInfo[i] = issti[i];
	}
}

void CImagePortShow::UpdateSavePort(ImageShowSavePortInfo *isspi, int count)
{
	m_Cur_SavePortCount = count;
	for(int i=0; i<count; i++)
	{
		m_SavePortInfo[i] = isspi[i];
	}
}

void CImagePortShow::UpdateData(ImageShowReceiveInfo *isri, int count)
{
	m_list.DeleteAllItems();
	CString str;
	for(int i=0; i<count; i++)
	{
		//Tag ID
		str.Format(TEXT("%02X%02X"), isri[i].TagId[0], isri[i].TagId[1]);
		m_list.InsertItem(i, str);

		//Tag Name
		for(int j=0; j<m_Cur_SaveTagCount; j++)
		{
			if(0 == str.Compare(m_SaveTagInfo[j].ID))
			{
				str = m_SaveTagInfo[j].Name;
				break;
			}
		}
		m_list.SetItemText(i, 1, str);			
		//Port1 ID
		str.Format(TEXT("%02X%02X"), isri[i].Port1Id[0], isri[i].Port1Id[1]);
		m_list.SetItemText(i, 2, str);
		//Port1 Name		
		for(int j=0; j<m_Cur_SavePortCount; j++)
		{
			if(0 == str.Compare(m_SavePortInfo[j].ID))
			{
				str = m_SavePortInfo[j].Name;
				break;
			}
		}
		m_list.SetItemText(i, 3, str);
		//Port1 Rssi
		str.Format(TEXT("%d"), isri[i].Rssi1);
		m_list.SetItemText(i, 4, str);

		//Port2 ID
		str.Format(TEXT("%02X%02X"), isri[i].Port2Id[0], isri[i].Port2Id[1]);
		m_list.SetItemText(i, 5, str);
		//Port2 Name		
		for(int j=0; j<m_Cur_SavePortCount; j++)
		{
			if(0 == str.Compare(m_SavePortInfo[j].ID))
			{
				str = m_SavePortInfo[j].Name;
				break;
			}
		}
		m_list.SetItemText(i, 6, str);
		//Port2 Rssi
		str.Format(TEXT("%d"), isri[i].Rssi2);
		m_list.SetItemText(i, 7, str);

		//Port3 ID
		str.Format(TEXT("%02X%02X"), isri[i].Port3Id[0], isri[i].Port3Id[1]);
		m_list.SetItemText(i, 8, str);
		//Port3 Name		
		for(int j=0; j<m_Cur_SavePortCount; j++)
		{
			if(0 == str.Compare(m_SavePortInfo[j].ID))
			{
				str = m_SavePortInfo[j].Name;
				break;
			}
		}
		m_list.SetItemText(i, 9, str);
		//Port3 Rssi
		str.Format(TEXT("%d"), isri[i].Rssi3);
		m_list.SetItemText(i, 10, str);
		
		//type
		if(isri[i].Type == 1)
			str = TEXT("普通定位");
		else
			str = TEXT("緊急定位");
		m_list.SetItemText(i, 11, str);

		//Battery
		str.Format(TEXT("%d"), isri[i].Battery);
		m_list.SetItemText(i, 12, str);

		//sensor time
		str.Format(TEXT("%d"), isri[i].SensorTime);
		m_list.SetItemText(i, 13, str);
				
		//距離上一次接收到數據包的間隔時間
		DWORD time = GetTickCount() - isri[i].FirstReceiveTime;
		if(time < 60000)
		{
			str.Format(TEXT("%d秒"), time/1000);
		}
		else if(time < 3600000)
			str.Format(TEXT("%d分%d秒"), time/60000, (time%60000)/1000);
		else 
			str.Format(TEXT("%d小時%d分%d秒"), time/3600000, (time%3600000)/60000, (time%60000)/1000);
		m_list.SetItemText(i, 14, str);
	}
}
BOOL CImagePortShow::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	DWORD dwStyle = m_list.GetExtendedStyle();
	m_list.SetExtendedStyle(dwStyle | LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_HEADERDRAGDROP);
	
	m_list.SetExtendedStyle(dwStyle | LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_HEADERDRAGDROP);
	m_list.InsertColumn(0,TEXT("卡片ID"), LVCFMT_CENTER, 50);
	m_list.InsertColumn(1,TEXT("卡片名稱"), LVCFMT_LEFT, 100);
	m_list.InsertColumn(2,TEXT("參考點1ID"), LVCFMT_LEFT, 70);
	m_list.InsertColumn(3,TEXT("參考點1名稱"), LVCFMT_LEFT, 100);
	m_list.InsertColumn(4,TEXT("相對距離1"), LVCFMT_CENTER, 70);
	m_list.InsertColumn(5,TEXT("參考點2ID"), LVCFMT_LEFT, 70);
	m_list.InsertColumn(6,TEXT("參考點2名稱"), LVCFMT_LEFT, 100);
	m_list.InsertColumn(7,TEXT("相對距離2"), LVCFMT_CENTER, 70);
	m_list.InsertColumn(8,TEXT("參考點3ID"), LVCFMT_LEFT, 70);
	m_list.InsertColumn(9,TEXT("參考點3名稱"), LVCFMT_LEFT, 100);
	m_list.InsertColumn(10,TEXT("相對距離3"), LVCFMT_CENTER, 70);
	m_list.InsertColumn(11,TEXT("定位類型"), LVCFMT_CENTER, 70);
	m_list.InsertColumn(12,TEXT("電池電量"), LVCFMT_CENTER, 70);
	m_list.InsertColumn(13,TEXT("卡片沒有移動的時間（秒）"), LVCFMT_CENTER, 170);
	m_list.InsertColumn(14,TEXT("距離上一次接收到數據包的時間"), LVCFMT_CENTER, 180);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CImagePortShow::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	::PostMessage(m_MainWindowHwnd, WM_DLG_PORTSHOW_CLOSE, 0, 0);
	CDialog::OnCancel();
}

void CImagePortShow::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here
	CRect rs; 
	this->GetClientRect(&rs);
	rs.left += 10;
	rs.top += 30;
	rs.right -= 10;
	rs.bottom -= 10;
	::MoveWindow(m_list.GetSafeHwnd(), rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
}

BOOL CImagePortShow::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN)
	{
		switch (pMsg->wParam)
		{
		case VK_RETURN:
			//MessageBox(_T("回車鍵按下"));
			return TRUE;		
		default:
			break;
		}
	}
	return CDialog::PreTranslateMessage(pMsg);
}
