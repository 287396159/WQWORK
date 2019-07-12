#pragma once
#include "afxwin.h"
#include "afxcmn.h"
#include "define.h"


// CImagePortShow dialog

class CImagePortShow : public CDialog
{
	DECLARE_DYNAMIC(CImagePortShow)

public:
	CImagePortShow(CWnd* pParent = NULL);   // standard constructor
	virtual ~CImagePortShow();

// Dialog Data
	enum { IDD = IDD_DIALOG_IMAGESHOW_PORTSHOW };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CStatic m_static_tital;
	CListCtrl m_list;

	HWND	m_MainWindowHwnd;

	ImageShowSaveTagInfo		m_SaveTagInfo[MAX_SAVE_TAG_COUNT];
	ImageShowSavePortInfo		m_SavePortInfo[MAX_SAVE_PORT_COUNT];
	int		m_Cur_SaveTagCount;
	int		m_Cur_SavePortCount;

	void UpdateSaveTag(ImageShowSaveTagInfo *issti, int count);
	void UpdateSavePort(ImageShowSavePortInfo *isspi, int count);
	void SetMainHwnd(HWND hwnd);
	void UpdateTitle(CString str);
	void UpdateData(ImageShowReceiveInfo *isri, int count);

	virtual BOOL OnInitDialog();
protected:
	virtual void OnCancel();
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
	virtual BOOL PreTranslateMessage(MSG* pMsg);
};
