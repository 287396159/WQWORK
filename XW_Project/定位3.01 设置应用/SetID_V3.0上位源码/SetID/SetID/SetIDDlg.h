// SetIDDlg.h : ͷ�ļ�
//

#pragma once
#include "afxcmn.h"
#include "SetType1.h"
#include "SetNewTwoDialog.h"

// CSetIDDlg �Ի���
class CSetIDDlg : public CDialog
{
// ����
public:
	CSetIDDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_SETID_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��


// ʵ��
protected:
	HICON m_hIcon;

	// ���ɵ���Ϣӳ�亯��
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
