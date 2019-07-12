#pragma once
#include "afxwin.h"
#include "afxcmn.h"
#include "define.h"

// CTagOnLine dialog

class CTagOnLine : public CDialog
{
	DECLARE_DYNAMIC(CTagOnLine)

public:
	CTagOnLine(CWnd* pParent = NULL);   // standard constructor
	virtual ~CTagOnLine();

// Dialog Data
	enum { IDD = IDD_DIALOG_TAG_ONLINE };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	CStatic m_static_SetCount;
	CStatic m_static_OnLine;
	CStatic m_static_OffLine;
	CListCtrl m_list_OnLine;
	CListCtrl m_list_OffLine;

	HWND	m_MainWindowHwnd;

	int		m_Cur_SaveTagCount;
	ImageShowSaveTagInfo		m_SaveTagInfo[MAX_SAVE_TAG_COUNT];
	BYTE	m_SaveTagIsOnLine[MAX_SAVE_TAG_COUNT];
	
	void UpdateSaveTag(ImageShowSaveTagInfo *issti, int count);	
	void SetMainHwnd(HWND hwnd);	
	void UpdateData(ImageShowReceiveInfo *isri, int count);

	virtual BOOL OnInitDialog();
protected:
	virtual void OnCancel();
	virtual void OnOK();
public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
};
