#pragma once
#include "afxcmn.h"
#include "OldDataTongJi.h"
#include "OldDataRePlay.h"

// COldData dialog

class COldData : public CDialog
{
	DECLARE_DYNAMIC(COldData)

public:
	COldData(CWnd* pParent = NULL);   // standard constructor
	virtual ~COldData();

// Dialog Data
	enum { IDD = IDD_DIALOG_OLDDATA };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	COldDataTongJi	m_OldDataTongJi;
	COldDataRePlay	m_OldDataRePlay;

	virtual BOOL OnInitDialog();
	CTabCtrl m_tab;
	afx_msg void OnShowWindow(BOOL bShow, UINT nStatus);
protected:
	virtual void OnCancel();
public:
	afx_msg void OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult);
};
