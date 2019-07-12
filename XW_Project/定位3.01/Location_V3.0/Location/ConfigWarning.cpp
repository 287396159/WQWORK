// ConfigWarning.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "ConfigWarning.h"
#include "Ini.h"
#include "define.h"

// CConfigWarning dialog

IMPLEMENT_DYNAMIC(CConfigWarning, CDialog)

CConfigWarning::CConfigWarning(CWnd* pParent /*=NULL*/)
	: CDialog(CConfigWarning::IDD, pParent)
	, m_servernet_timeout_value(60)
	, m_edit_battery_value(10)
{

}

CConfigWarning::~CConfigWarning()
{
}

void CConfigWarning::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_CHECK_EMERGENCY, m_check_emergency);
	DDX_Control(pDX, IDC_CHECK_SERVER_NET, m_check_servernet);
	DDX_Control(pDX, IDC_CHECK_TAGTIMEOUT, m_check_tagtimeout);
	DDX_Control(pDX, IDC_EDIT_SERVERNET_TIME, m_servernet_timeout_control);
	DDX_Text(pDX, IDC_EDIT_SERVERNET_TIME, m_servernet_timeout_value);
	DDX_Control(pDX, IDC_CHECK_BATTERY, m_button_battery);
	DDX_Control(pDX, IDC_EDIT_BATTERY_VALUE, m_edit_battery);
	DDX_Text(pDX, IDC_EDIT_BATTERY_VALUE, m_edit_battery_value);
}


BEGIN_MESSAGE_MAP(CConfigWarning, CDialog)
	ON_BN_CLICKED(IDC_CHECK_EMERGENCY, &CConfigWarning::OnBnClickedCheckEmergency)
	ON_BN_CLICKED(IDC_CHECK_SERVER_NET, &CConfigWarning::OnBnClickedCheckServerNet)
	ON_BN_CLICKED(IDC_CHECK_TAGTIMEOUT, &CConfigWarning::OnBnClickedCheckTagtimeout)
	ON_EN_KILLFOCUS(IDC_EDIT_SERVERNET_TIME, &CConfigWarning::OnEnKillfocusEditServernetTime)
	ON_EN_KILLFOCUS(IDC_EDIT_BATTERY_VALUE, &CConfigWarning::OnEnKillfocusEditBatteryValue)
	ON_BN_CLICKED(IDC_CHECK_BATTERY, &CConfigWarning::OnBnClickedCheckBattery)
END_MESSAGE_MAP()


// CConfigWarning message handlers

BOOL CConfigWarning::OnInitDialog()
{
	CDialog::OnInitDialog();
	
	// TODO:  Add extra initialization here
	m_check_emergency.SetCheck(1);
	m_check_servernet.SetCheck(1);
	m_check_tagtimeout.SetCheck(1);
	m_button_battery.SetCheck(1);

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
		if(cini.GetValue(TEXT("Warning"), TEXT("ShowEmergencyTag"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_check_emergency.SetCheck(1);
			else
				m_check_emergency.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Warning"), TEXT("ServerNetTimeOutTime"), str))
		{
			m_servernet_timeout_control.SetWindowTextW(str);
		}
		if(cini.GetValue(TEXT("Warning"), TEXT("ShowServerNetTimeOut"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_check_servernet.SetCheck(1);
			else
				m_check_servernet.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Warning"), TEXT("ShowTagTimeOut"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_check_tagtimeout.SetCheck(1);
			else
				m_check_tagtimeout.SetCheck(0);
		}
		if(cini.GetValue(TEXT("Warning"), TEXT("LowBatteryValue"), str))
		{
			m_edit_battery.SetWindowTextW(str);
		}
		if(cini.GetValue(TEXT("Warning"), TEXT("ShowLowBattery"), str))
		{
			if(0 == str.Compare(TEXT("YES")))
				m_button_battery.SetCheck(1);
			else
				m_button_battery.SetCheck(0);
		}
	}


	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

VOID CConfigWarning::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}



void CConfigWarning::OnBnClickedCheckEmergency()
{
	// TODO: Add your control notification handler code here
	CString str;
	if(m_check_emergency.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWEMERGENCY_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWEMERGENCY_UPDATE, 0, 0);
	}
	Save(TEXT("Warning"), TEXT("ShowEmergencyTag"), str);	
}

void CConfigWarning::OnBnClickedCheckServerNet()
{
	// TODO: Add your control notification handler code here
	CString str;
	if(m_check_servernet.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWSERVERNETTIMEOUT_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWSERVERNETTIMEOUT_UPDATE, 0, 0);
	}
	Save(TEXT("Warning"), TEXT("ShowServerNetTimeOut"), str);	
}

void CConfigWarning::OnBnClickedCheckTagtimeout()
{
	// TODO: Add your control notification handler code here
	CString str;
	if(m_check_tagtimeout.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWTAGTIMEOUT_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWTAGTIMEOUT_UPDATE, 0, 0);
	}
	Save(TEXT("Warning"), TEXT("ShowTagTimeOut"), str);	
}

void CConfigWarning::OnEnKillfocusEditServernetTime()
{
	// TODO: Add your control notification handler code here
	CString str;
	UpdateData(TRUE);
	str.Format(TEXT("%d"), m_servernet_timeout_value);
	Save(TEXT("Warning"), TEXT("ServerNetTimeOutTime"), str);
	::PostMessage(m_MainWindowHwnd, WM_WARNING_SERVERNETTIMEOUTTIME_UPDATE, m_servernet_timeout_value, 0);
}


void CConfigWarning::Save(CString segment, CString key, CString value)
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

void CConfigWarning::OnEnKillfocusEditBatteryValue()
{
	// TODO: Add your control notification handler code here
	CString str;
	UpdateData(TRUE);
	str.Format(TEXT("%d"), m_edit_battery_value);
	Save(TEXT("Warning"), TEXT("LowBatteryValue"), str);
	::PostMessage(m_MainWindowHwnd, WM_WARNING_LOWBATTERYVALUE_UPDATE, m_edit_battery_value, 0);
}

void CConfigWarning::OnBnClickedCheckBattery()
{
	// TODO: Add your control notification handler code here
	CString str;
	if(m_button_battery.GetCheck())
	{
		str = TEXT("YES");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWLOWBATTERY_UPDATE, 1, 0);
	}
	else
	{
		str = TEXT("NO");
		::PostMessage(m_MainWindowHwnd, WM_WARNING_SHOWLOWBATTERY_UPDATE, 0, 0);
	}
	Save(TEXT("Warning"), TEXT("ShowLowBattery"), str);	
}
