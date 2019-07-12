#pragma once
#include "afxwin.h"
#include "ini.h"
#include "define.h"

// ConfigMap dialog

class ConfigMap : public CDialog
{
	DECLARE_DYNAMIC(ConfigMap)

public:
	ConfigMap(CWnd* pParent = NULL);   // standard constructor
	virtual ~ConfigMap();

// Dialog Data
	enum { IDD = IDD_DIALOG_CONFIG_MAP };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CString m_ImageFile;
	HWND			m_MainWindowHwnd;

	virtual BOOL OnInitDialog();

	VOID GetModulePath(LPTSTR path, LPCTSTR module);

	BOOL MyLoadImage();
	afx_msg void OnBnClickedButtonOpen();
	CEdit m_edit_filepath;
	afx_msg void OnPaint();
};
