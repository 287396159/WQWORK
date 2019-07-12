#pragma once
#include "afxwin.h"
#include "afxcmn.h"
#include "reportctrl.h"

// CSetTag dialog

class CSetTag : public CDialog
{
	DECLARE_DYNAMIC(CSetTag)

public:
	CSetTag(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSetTag();

// Dialog Data
	enum { IDD = IDD_DIALOG_SETTAG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()

private:
	bool StringToChar(CString str, BYTE* data);

public:
	HWND			m_hwnd_ListShow;
	HWND			m_hwnd_ImageShow;
	
	void SetListShowHwnd(HWND hwnd);
	void SetImageShowHwnd(HWND hwnd);
	void InitLoadTagInfo();
	void GetModulePath(LPTSTR path, LPCTSTR module);
	void LeftButtonDown(int x, int y);
	BOOL CheckIdOk(CString strid);

	virtual BOOL OnInitDialog();
	CButton m_button_refresh;
	CButton m_button_save;
	CButton m_button_del;
	CListCtrl m_list;
	CButton m_button_add;
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnBnClickedButton1();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	CEdit m_edit_ID;
	afx_msg void OnNMClickList2(NMHDR *pNMHDR, LRESULT *pResult);
	CEdit m_edit_name;
	afx_msg void OnBnClickedButton5();

	afx_msg void OnNMCustomdrawList2(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedButton4();
	afx_msg void OnBnClickedButton3();
	afx_msg void OnBnClickedButton2();
	afx_msg void OnLvnColumnclickList2(NMHDR *pNMHDR, LRESULT *pResult);
};
