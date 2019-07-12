// WarningMessage.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "WarningMessage.h"
#include "define.h"


// CWarningMessage dialog

IMPLEMENT_DYNAMIC(CWarningMessage, CDialog)

CWarningMessage::CWarningMessage(CWnd* pParent /*=NULL*/)
	: CDialog(CWarningMessage::IDD, pParent)
{

}

CWarningMessage::~CWarningMessage()
{
}

void CWarningMessage::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT_INFO, m_edit_info);
}


BEGIN_MESSAGE_MAP(CWarningMessage, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_ClearAll, &CWarningMessage::OnBnClickedButtonClearall)
	ON_BN_CLICKED(IDC_BUTTON_CLOSE, &CWarningMessage::OnBnClickedButtonClose)
END_MESSAGE_MAP()


// CWarningMessage message handlers

BOOL CWarningMessage::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CWarningMessage::UpdateWarningMessage(CStringArray &WarningInfo)
{	
	CString str;
	str = TEXT("");
	for(int i=0; i<WarningInfo.GetCount(); i++)
	{
		str += WarningInfo.GetAt(i);
	}
	m_edit_info.SetWindowTextW(str);
	m_edit_info.SetSel(str.GetLength(), str.GetLength());
}

void CWarningMessage::SetMainHwnd(HWND hwnd)
{
	m_MainWindowHwnd = hwnd;
}

void CWarningMessage::OnBnClickedButtonClearall()
{
	// TODO: Add your control notification handler code here
	::PostMessage(m_MainWindowHwnd, WM_CLEAR_ALL_WARNING_MESSAGE, 0, 0);
	m_edit_info.SetWindowTextW(TEXT(""));
}

void CWarningMessage::OnBnClickedButtonClose()
{
	// TODO: Add your control notification handler code here
	this->ShowWindow(SW_HIDE);
}
