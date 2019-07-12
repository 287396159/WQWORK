#pragma once
#include "afxwin.h"


// CConfigWarning dialog

class CConfigWarning : public CDialog
{
	DECLARE_DYNAMIC(CConfigWarning)

public:
	CConfigWarning(CWnd* pParent = NULL);   // standard constructor
	virtual ~CConfigWarning();

// Dialog Data
	enum { IDD = IDD_DIALOG_CONFIG_WARNING };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	HWND			m_MainWindowHwnd;

	VOID GetModulePath(LPTSTR path, LPCTSTR module);
	void Save(CString segment, CString key, CString value);

	virtual BOOL OnInitDialog();
	CButton m_check_emergency;
	CButton m_check_servernet;
	CButton m_check_tagtimeout;
	CEdit m_servernet_timeout_control;
	DWORD m_servernet_timeout_value;
	afx_msg void OnBnClickedCheckEmergency();
	afx_msg void OnBnClickedCheckServerNet();
	afx_msg void OnBnClickedCheckTagtimeout();
	afx_msg void OnEnKillfocusEditServernetTime();
	CButton m_button_battery;
	CEdit m_edit_battery;
	DWORD m_edit_battery_value;
	afx_msg void OnEnKillfocusEditBatteryValue();
	afx_msg void OnBnClickedCheckBattery();
};
