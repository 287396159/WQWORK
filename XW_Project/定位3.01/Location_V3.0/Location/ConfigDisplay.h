#pragma once
#include "afxwin.h"
#include "ini.h"
#include "define.h"

// CConfigDisplay dialog

class CConfigDisplay : public CDialog
{
	DECLARE_DYNAMIC(CConfigDisplay)

public:
	CConfigDisplay(CWnd* pParent = NULL);   // standard constructor
	virtual ~CConfigDisplay();

// Dialog Data
	enum { IDD = IDD_DIALOG_CONFIG_DISPLAY };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	HWND			m_MainWindowHwnd;

	CButton m_check_show_TimeOut;
	CEdit m_edit_show_TimeOut_Control;
	DWORD m_edit_show_TimeOut_Value;
	CButton m_check_auto_emergency;
	CButton m_check_show_ReferencePoint;
	virtual BOOL OnInitDialog();
	VOID GetModulePath(LPTSTR path, LPCTSTR module);
	afx_msg void OnBnClickedCheckShowTimeout();
	afx_msg void OnBnClickedCheckAutoEmergency();
	void Save(CString segment, CString key, CString value);
	afx_msg void OnBnClickedCheckShowPort();
	afx_msg void OnEnKillfocusEditShowTime();
	DWORD m_edit_nomovetime;
	CButton m_btn_tag_nomove;
	afx_msg void OnBnClickedCheckTagNomove();
	afx_msg void OnEnKillfocusEditNoMoveTime();
	afx_msg void OnBnClickedCheckTagContinuous();
	afx_msg void OnEnKillfocusEditTagContinuous();
	DWORD m_edit_tag_continuous;
	CButton m_btn_tag_continuous;
};
