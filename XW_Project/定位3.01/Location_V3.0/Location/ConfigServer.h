#pragma once
#include "afxwin.h"
#include "Ini.h"

// CConfigServer dialog

class CConfigServer : public CDialog
{
	DECLARE_DYNAMIC(CConfigServer)

public:
	CConfigServer(CWnd* pParent = NULL);   // standard constructor
	virtual ~CConfigServer();

// Dialog Data
	enum { IDD = IDD_DIALOG_CONFIG_SERVER };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
//	CComboBox m_combo_ip;
//	DWORD m_port_value;
	HWND			m_MainWindowHwnd;
	
	void Init();
	VOID GetModulePath(LPTSTR path, LPCTSTR module);

	virtual BOOL OnInitDialog();

	afx_msg void OnBnClickedButtonSave();
	CButton m_button_save;

	CComboBox m_combobox_ip;
	DWORD m_edit_port;
	afx_msg void OnBnClickedButtonSaveip();
};
