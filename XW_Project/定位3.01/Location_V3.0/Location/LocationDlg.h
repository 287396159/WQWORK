// LocationDlg.h : 頭文件
//

#pragma once
#include "afxcmn.h"
#include "SetPort.h"
#include "SetTag.h"
#include "ListShow.h"
#include "ImageShow.h"
#include "SetNet.h"

// CLocationDlg 對話框
class CLocationDlg : public CDialog
{
// 構造
public:
	CLocationDlg(CWnd* pParent = NULL);	// 標準構造函數

// 對話框數據
	enum { IDD = IDD_LOCATION_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 實現
protected:
	HICON m_hIcon;

	// 生成的消息映射函數
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	HWND			m_hwnd_SetPort;
	HWND			m_hwnd_SetTag;
	HWND			m_hwnd_ListShow;
	HWND			m_hwnd_ImageShow;

	CSetPort		m_SetPort;
	CSetTag			m_SetTag;
	CSetNet			m_SetNet;
	CListShow		m_ListShow;
	CImageShow		m_ImageShow;
	

	CTabCtrl m_tab;
	afx_msg void OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	virtual BOOL PreTranslateMessage(MSG* pMsg);

	void GetModulePath(LPTSTR path, LPCTSTR module);
};
