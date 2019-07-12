#pragma once


// CSetNet dialog

class CSetNet : public CDialog
{
	DECLARE_DYNAMIC(CSetNet)

public:
	CSetNet(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSetNet();

// Dialog Data
	enum { IDD = IDD_DIALOG_SETNET };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	HWND			m_hwnd_ImageShow;

	void SetImageShowHwnd(HWND hwnd);
	void InitLoadPortInfo();
	void GetModulePath(LPTSTR path, LPCTSTR module);
	BOOL CheckIdOk(CString strid);
	
	virtual BOOL OnInitDialog();
	CListCtrl m_list;
	CEdit m_edit_ID;
	CEdit m_edit_name;
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnBnClickedButtonRefresh();
	afx_msg void OnBnClickedButtonAdd();
	afx_msg void OnNMClickList2(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnNMCustomdrawList2(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnLvnColumnclickList2(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedButtonDel();
	afx_msg void OnBnClickedButtonSave();
	afx_msg void OnBnClickedButtonUpdatetolist();
	virtual BOOL PreTranslateMessage(MSG* pMsg);

};
