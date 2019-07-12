// SetIDDlg.h : 头文件
//

#pragma once
#include "afxcmn.h"
#include "SetType1.h"
#include "SetNewTwoDialog.h"

// CSetIDDlg 对话框
class CSetIDDlg : public CDialog
{
// 构造
public:
	CSetIDDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_SETID_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	SetType1	m_SetType1;
	CSetNewTwoDialog	m_SetType2;
	//SetType3	m_SetType3;

	CTabCtrl	m_tab;
	afx_msg void OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult);
};
