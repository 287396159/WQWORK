#pragma once
#include "afxwin.h"

class CImageShow;

// CSetImagePort dialog

class CSetImagePort : public CDialog
{
	DECLARE_DYNAMIC(CSetImagePort)

public:
	CSetImagePort(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSetImagePort();

// Dialog Data
	enum { IDD = IDD_DIALOG_SETIMAGEPORT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CImageShow	*pf;

	BYTE	m_id1, m_id2;

	void SetParent(CImageShow *p);
	void SetID(BYTE id1, BYTE id2);
	bool StringToChar(CString str, BYTE* data);

	CEdit m_edit_id1;
	CEdit m_edit_id2;
	CButton m_button_setid;
	CButton m_button_delete;
	CButton m_button_back;
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedButtonSetid();
	afx_msg void OnBnClickedButtonDelete();
	afx_msg void OnBnClickedButtonBack();
};
