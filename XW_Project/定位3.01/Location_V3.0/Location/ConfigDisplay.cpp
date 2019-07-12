// ConfigDisplay.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "ConfigDisplay.h"


// CConfigDisplay dialog

IMPLEMENT_DYNAMIC(CConfigDisplay, CDialog)

CConfigDisplay::CConfigDisplay(CWnd* pParent /*=NULL*/)
	: CDialog(CConfigDisplay::IDD, pParent)
	, m_edit_show_TimeOut_Value(60)
	, m_edit_nomovetime(60)
	, m_edit_tag_continuous(3)
{

}

CConfigDisplay::~CConfigDisplay()
{
}

void CConfigDisplay::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_CHECK_SHOW_TIMEOUT, m_check_show_TimeOut);
	DDX_Control(pDX, IDC_EDIT_SHOW_TIME, m_edit_show_TimeOut_Control);
	DDX_Text(pDX, IDC_EDIT_SHOW_TIME, m_edit_show_TimeOut_Value);
	DDX_Control(pDX, IDC_CHECK_AUTO_Emergency, m_check_auto_emergency);
	DDX_Control(pDX, IDC_CHECK_SHOW_PORT, m_check_show_ReferencePoint);
	DDX_Text(pDX, IDC_EDIT_NO_MOVE_TIME, m_edit_nomovetime);
	DDX_Control(pDX, IDC_CHECK_TAG_NOMOVE, m_btn_tag_nomove);
	DDX_Text(pDX, IDC_EDIT_TAG_CONTINUOUS, m_edit_tag_continuous);
	DDX_Control(pDX, IDC_CHECK_TAG_CONTINUOUS, m_btn_tag_continuous);
}


BEGIN_MESSAGE_MAP(CConfigDisplay, CDialog)
	ON_BN_CLICKED(IDC_CHECK_SHOW_TIMEOUT, &CConfigDisplay::OnBnClickedCheckShowTimeout)
	ON_BN_CLICKED(IDC_CHECK_AUTO_Emergency, &CConfigDisplay::OnBnClickedCheckAutoEmergency)
	ON_BN_CLICKED(IDC_CHECK_SHOW_PORT, &CConfigDisplay::OnBnClickedCheckShowPort)
	ON_EN_KILLFOCUS(IDC_EDIT_SHOW_TIME, &CConfigDisplay::OnEnKillfocusEditShowTime)
	ON_BN_CLICKED(IDC_CHECK_TAG_NOMOVE, &CConfigDisplay::OnBnClickedCheckTagNomove)
	ON_EN_KILLFOCUS(IDC_EDIT_NO_MOVE_TIME, &CConfigDisplay::OnEnKillfocusEditNoMoveTime)
	ON_BN_CLICKED(IDC_CHECK_TAG_CONTINUOUS, &CConfigDisplay::OnBnClickedCheckTagContinuous)
	ON_EN_KILLFOCUS(IDC_EDIT_TAG_CONTINUOUS, &CConfigDisplay::OnEnKillfocusEditTagContinuous)
END_MESSAGE_MAP()


// CConfigDisplay message handlers

BOOL CConfigDisplay::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	m_check_show_TimeOut.SetCheck(1);
	m_check_auto_emergency.SetCheck(1);
	m_check_show_ReferencePoint.SetCheck(1);
	m_btn_tag_nomove.SetCheck(1);
	m_btn_tag_continuous.SetCheck(0);


	//從配置文件中抓取之前的路徑
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	CIni cini;
	if(cini.Open(filepath, FALSE))
	{	
		CString str;
		if(cini.GetValue(TEXT("Display"), TEXT("NoShowTimeOutID"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_check_show_TimeOut.SetCheck(1);
			else
				m_check_show_TimeOut.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Display"), TEXT("ShowTimeOutIDTime"), str))
		{
			m_edit_show_TimeOut_Control.SetWindowTextW(str);
		}
		if(cini.GetValue(TEXT("Display"), TEXT("AutoShowEmergency"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_check_auto_emergency.SetCheck(1);
			else
				m_check_auto_emergency.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Display"), TEXT("ShowReferencePoint"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_check_show_ReferencePoint.SetCheck(1);
			else
				m_check_show_ReferencePoint.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Display"), TEXT("ShowTagNoMove"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_btn_tag_nomove.SetCheck(1);
			else
				m_btn_tag_nomove.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Display"), TEXT("ShowTagNoMoveTime"), str))
		{
			SetDlgItemText(IDC_EDIT_NO_MOVE_TIME, str);
		}
		if(cini.GetValue(TEXT("Display"), TEXT("ShowTagContinuous"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_btn_tag_continuous.SetCheck(1);
			else
				m_btn_tag_continuous.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Display"), TEXT("ShowTagContinuousNumber"), str))
		{
			SetDlgItemText(IDC_EDIT_TAG_CONTINUOUS, str);
		}
	}

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

VOID CConfigDisplay::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}


void CConfigDisplay::OnBnClickedCheckShowTimeout()
{
	// TODO: Add your control notification handler code here
	CString str;
	if(m_check_show_TimeOut.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTIMEOUTID_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTIMEOUTID_UPDATE, 0, 0);
	}
	Save(TEXT("Display"), TEXT("NoShowTimeOutID"), str);	
}

void CConfigDisplay::OnBnClickedCheckAutoEmergency()
{
	// TODO: Add your control notification handler code here		
	CString str;
	if(m_check_auto_emergency.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_AUTOEMERGENCY_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_AUTOEMERGENCY_UPDATE, 0, 0);
	}
	Save(TEXT("Display"), TEXT("AutoShowEmergency"), str);
	
}

void CConfigDisplay::Save(CString segment, CString key, CString value)
{
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	CIni cini;
	if(cini.Open(filepath, FALSE))
	{		
		if(cini.SetValue(segment, key, value))
		{
			
		}
		else
			MessageBox(TEXT("保存失敗！"));
		
	}
	else
		MessageBox(TEXT("打開config.ini失敗，保存失敗！"));
}

void CConfigDisplay::OnBnClickedCheckShowPort()
{
	// TODO: Add your control notification handler code here
	CString str;
	if(m_check_show_ReferencePoint.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_REFERENCEPOINT_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_REFERENCEPOINT_UPDATE, 0, 0);
	}
	Save(TEXT("Display"), TEXT("ShowReferencePoint"), str);
}

void CConfigDisplay::OnEnKillfocusEditShowTime()
{
	// TODO: Add your control notification handler code here
	CString str;
	UpdateData(TRUE);
	str.Format(TEXT("%d"), m_edit_show_TimeOut_Value);
	Save(TEXT("Display"), TEXT("ShowTimeOutIDTime"), str);
	::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTIMEOUTTIME_UPDATE, m_edit_show_TimeOut_Value, 0);
}

void CConfigDisplay::OnBnClickedCheckTagNomove()
{
	// TODO: Add your control notification handler code here
	CString str;
	if(m_btn_tag_nomove.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTAGNOMOVE_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTAGNOMOVE_UPDATE, 0, 0);
	}
	Save(TEXT("Display"), TEXT("ShowTagNoMove"), str);
}

void CConfigDisplay::OnEnKillfocusEditNoMoveTime()
{
	// TODO: Add your control notification handler code here
	CString str;
	UpdateData(TRUE);
	str.Format(TEXT("%d"), m_edit_nomovetime);
	Save(TEXT("Display"), TEXT("ShowTagNoMoveTime"), str);
	::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTAGNOMOVETIME_UPDATE, m_edit_nomovetime, 0);
}

void CConfigDisplay::OnBnClickedCheckTagContinuous()
{
	// TODO: 在此添加控件通知處理程序代碼
	CString str;
	if(m_btn_tag_continuous.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTAGCONTINUOUS_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTAGCONTINUOUS_UPDATE, 0, 0);
	}
	Save(TEXT("Display"), TEXT("ShowTagContinuous"), str);

}

void CConfigDisplay::OnEnKillfocusEditTagContinuous()
{
	// TODO: 在此添加控件通知處理程序代碼
	CString str;
	UpdateData(TRUE);
	str.Format(TEXT("%d"), m_edit_tag_continuous);
	Save(TEXT("Display"), TEXT("ShowTagContinuousNumber"), str);
	::PostMessage(m_MainWindowHwnd, WM_DISPLAY_SHOWTAGCONTINUOUSNUMBER_UPDATE, m_edit_tag_continuous, 0);
}
