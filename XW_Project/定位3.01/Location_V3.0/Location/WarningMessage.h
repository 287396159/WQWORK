#pragma once
#include "afxwin.h"


// CWarningMessage dialog

class CWarningMessage : public CDialog
{
	DECLARE_DYNAMIC(CWarningMessage)

public:
	CWarningMessage(CWnd* pParent = NULL);   // standard constructor
	virtual ~CWarningMessage();

// Dialog Data
	enum { IDD = IDD_DIALOG_WARNING_INFO };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	HWND	m_MainWindowHwnd;

	void UpdateWarningMessage(CStringArray &WarningInfo);
	void SetMainHwnd(HWND hwnd);
	virtual BOOL OnInitDialog();
	CEdit m_edit_info;
	afx_msg void OnBnClickedButtonClearall();
	afx_msg void OnBnClickedButtonClose();
};
