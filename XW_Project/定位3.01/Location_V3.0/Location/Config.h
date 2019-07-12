#pragma once
#include "afxwin.h"
#include "ConfigServer.h"
#include "onfigMap.h"
#include "ConfigDisplay.h"
#include "ConfigWarning.h"


// CConfig dialog

class CConfig : public CDialog
{
	DECLARE_DYNAMIC(CConfig)

public:
	CConfig(CWnd* pParent = NULL);   // standard constructor
	virtual ~CConfig();

// Dialog Data
	enum { IDD = IDD_DIALOG_CONFIG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CConfigServer	m_ConfigServer;
	ConfigMap		m_ConfigMap;
	CConfigDisplay	m_ConfigDisplay;
	CConfigWarning	m_ConfigWarning;

	HWND			m_MainWindowHwnd;

	CListBox m_list;
	
	void SetMainWindowHwnd(HWND hwnd);

	virtual BOOL OnInitDialog();
	afx_msg void OnLbnSelchangeList1();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
};
