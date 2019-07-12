// Config.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "Config.h"


// CConfig dialog

IMPLEMENT_DYNAMIC(CConfig, CDialog)

CConfig::CConfig(CWnd* pParent /*=NULL*/)
	: CDialog(CConfig::IDD, pParent)
{

}

CConfig::~CConfig()
{
}

void CConfig::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, m_list);
}


BEGIN_MESSAGE_MAP(CConfig, CDialog)
	ON_LBN_SELCHANGE(IDC_LIST1, &CConfig::OnLbnSelchangeList1)
END_MESSAGE_MAP()


// CConfig message handlers

BOOL CConfig::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	m_list.AddString(TEXT("連接設定"));
	m_list.AddString(TEXT("地圖設定"));
	m_list.AddString(TEXT("顯示設定"));
	m_list.AddString(TEXT("報警設定"));


	m_ConfigServer.Create(IDD_DIALOG_CONFIG_SERVER, this);
	m_ConfigMap.Create(IDD_DIALOG_CONFIG_MAP, this);
	m_ConfigDisplay.Create(IDD_DIALOG_CONFIG_DISPLAY, this);
	m_ConfigWarning.Create(IDD_DIALOG_CONFIG_WARNING, this);

	
	m_ConfigServer.MoveWindow(100, 0, 440, 500);
	m_ConfigMap.MoveWindow(100, 0, 440, 500);
	m_ConfigDisplay.MoveWindow(100, 0, 440, 500);
	m_ConfigWarning.MoveWindow(100, 0, 440, 500);

	m_list.SetCurSel(0);
	m_ConfigServer.ShowWindow(SW_SHOW);
	m_ConfigMap.ShowWindow(SW_HIDE);
	m_ConfigDisplay.ShowWindow(SW_HIDE);	
	m_ConfigWarning.ShowWindow(SW_HIDE);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CConfig::OnLbnSelchangeList1()
{
	// TODO: Add your control notification handler code here
	int index = m_list.GetCurSel();
	switch(index)
	{
	case 0:
		m_ConfigServer.ShowWindow(SW_SHOW);
		m_ConfigMap.ShowWindow(SW_HIDE);
		m_ConfigDisplay.ShowWindow(SW_HIDE);
		m_ConfigWarning.ShowWindow(SW_HIDE);
		break;
	case 1:
		m_ConfigServer.ShowWindow(SW_HIDE);
		m_ConfigMap.ShowWindow(SW_SHOW);
		m_ConfigDisplay.ShowWindow(SW_HIDE);
		m_ConfigWarning.ShowWindow(SW_HIDE);
		break;
	case 2:
		m_ConfigServer.ShowWindow(SW_HIDE);
		m_ConfigMap.ShowWindow(SW_HIDE);
		m_ConfigDisplay.ShowWindow(SW_SHOW);
		m_ConfigWarning.ShowWindow(SW_HIDE);
		break;
	case 3:
		m_ConfigServer.ShowWindow(SW_HIDE);
		m_ConfigMap.ShowWindow(SW_HIDE);
		m_ConfigDisplay.ShowWindow(SW_HIDE);
		m_ConfigWarning.ShowWindow(SW_SHOW);
		break;
	}
}

void CConfig::SetMainWindowHwnd(HWND hwnd)
{
	m_MainWindowHwnd = hwnd;
	m_ConfigServer.m_MainWindowHwnd = m_MainWindowHwnd;
	m_ConfigMap.m_MainWindowHwnd = m_MainWindowHwnd;
	m_ConfigDisplay.m_MainWindowHwnd = m_MainWindowHwnd;
	m_ConfigWarning.m_MainWindowHwnd = m_MainWindowHwnd;
}
BOOL CConfig::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
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
	return CDialog::PreTranslateMessage(pMsg);
}
