// TagOnLine.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "TagOnLine.h"


// CTagOnLine dialog

IMPLEMENT_DYNAMIC(CTagOnLine, CDialog)

CTagOnLine::CTagOnLine(CWnd* pParent /*=NULL*/)
	: CDialog(CTagOnLine::IDD, pParent)
{

}

CTagOnLine::~CTagOnLine()
{
}

void CTagOnLine::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_STATIC_SETCOUNT, m_static_SetCount);
	DDX_Control(pDX, IDC_STATIC_ONLINE, m_static_OnLine);
	DDX_Control(pDX, IDC_STATIC_OFFLINE, m_static_OffLine);
	DDX_Control(pDX, IDC_LIST_ONLINE, m_list_OnLine);
	DDX_Control(pDX, IDC_LIST_OFFLINE, m_list_OffLine);
}


BEGIN_MESSAGE_MAP(CTagOnLine, CDialog)
END_MESSAGE_MAP()


// CTagOnLine message handlers

void CTagOnLine::UpdateSaveTag(ImageShowSaveTagInfo *issti, int count)
{
	m_Cur_SaveTagCount = count;
	for(int i=0; i<count; i++)
		m_SaveTagInfo[i] = issti[i];
	CString str;
	str.Format(TEXT("%d"), count);
	m_static_SetCount.SetWindowTextW(TEXT("設定保存卡片的總數量：") + str);
}

void CTagOnLine::SetMainHwnd(HWND hwnd)
{
	m_MainWindowHwnd = hwnd;
}

void CTagOnLine::UpdateData(ImageShowReceiveInfo *isri, int count)
{
	int offlinecount = 0;
	memset(m_SaveTagIsOnLine, 0, MAX_SAVE_TAG_COUNT);

	m_list_OnLine.DeleteAllItems();
	CString str;
	str.Format(TEXT("在線卡片（%d）"), count);
	m_static_OnLine.SetWindowTextW(str);
	for(int i=0; i<count; i++)
	{
		str.Format(TEXT("%02X%02X"), isri[i].TagId[0], isri[i].TagId[1]);
		m_list_OnLine.InsertItem(i, str);
		for(int j=0; j<m_Cur_SaveTagCount; j++)
		{
			if(str.Compare(m_SaveTagInfo[j].ID) == 0)
			{
				str = m_SaveTagInfo[j].Name;
				m_SaveTagIsOnLine[j] = 1;
				break;
			}
		}
		m_list_OnLine.SetItemText(i, 1, str);

		DWORD time = GetTickCount() - isri[i].FirstReceiveTime;
		if(time < 60000)
		{
			str.Format(TEXT("%d秒"), time/1000);
		}
		else if(time < 3600000)
			str.Format(TEXT("%d分%d秒"), time/60000, (time%60000)/1000);
		else 
			str.Format(TEXT("%d小時%d分%d秒"), time/3600000, (time%3600000)/60000, (time%60000)/1000);
		m_list_OnLine.SetItemText(i, 2, str);
	}

	count = 0;
	m_list_OffLine.DeleteAllItems();
	for(int i=0; i<m_Cur_SaveTagCount; i++)
	{
		if(m_SaveTagIsOnLine[i] == 0)
		{
			m_list_OffLine.InsertItem(count, m_SaveTagInfo[i].ID);
			m_list_OffLine.SetItemText(count, 1, m_SaveTagInfo[i].Name);
			count++;
		}
	}
	str.Format(TEXT("不在線卡片（%d）"), count);
	m_static_OffLine.SetWindowTextW(str);
	
}

BOOL CTagOnLine::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	DWORD dwStyle = m_list_OnLine.GetExtendedStyle();
	m_list_OnLine.SetExtendedStyle(dwStyle | LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_HEADERDRAGDROP);
	
	m_list_OnLine.InsertColumn(0, TEXT("卡片ID"), LVCFMT_LEFT, 50);
	m_list_OnLine.InsertColumn(1, TEXT("卡片名稱"), LVCFMT_LEFT, 70);
	m_list_OnLine.InsertColumn(2, TEXT("距離最近一次收到定位的時間"), LVCFMT_LEFT, 170);


	dwStyle = m_list_OffLine.GetExtendedStyle();
	m_list_OffLine.SetExtendedStyle(dwStyle | LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_HEADERDRAGDROP);
	
	m_list_OffLine.InsertColumn(0, TEXT("卡片ID"), LVCFMT_LEFT, 80);
	m_list_OffLine.InsertColumn(1, TEXT("卡片名稱"), LVCFMT_LEFT, 150);
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CTagOnLine::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	::PostMessage(m_MainWindowHwnd, WM_ONLINE_DLG_OFF, 0, 0);
	CDialog::OnCancel();
}

void CTagOnLine::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class

	CDialog::OnOK();
}

BOOL CTagOnLine::PreTranslateMessage(MSG* pMsg)
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
