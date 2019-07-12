#pragma once
#include "afxwin.h"


#define	MAX_RECEIVE_DATA_LEN	2048

// SetType3 对话框

class SetType3 : public CDialog
{
	DECLARE_DYNAMIC(SetType3)

public:
	SetType3(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~SetType3();

// 对话框数据
	enum { IDD = IDD_SET_THREE };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()

private:
	bool StringToChar(CString str, BYTE* data);

public:

	BOOL	m_bConnect;
	BOOL	m_bGetTag;
	BOOL	m_bSetTagID;
	BOOL	m_bGetTagID;
	BOOL	m_bSleepTime;
	DWORD	m_SleepTime;
	BOOL	m_bGetServer;
	BOOL	m_bGetRouter;
	BOOL	m_bSetServerID;
	BOOL	m_bGetServerID;
	BOOL	m_bGetRouterID;
	BOOL	m_bSetRouterID;
	BOOL	m_bInformTime;
	DWORD	m_InformTime;

	BYTE	m_Tag_ID[2];
	BYTE	m_Server_ID[2];
	BYTE	m_Router_ID[2];

	HANDLE	m_hcom;
	int		m_Receive_Data_Len;
	BYTE	m_Receive_Data_Char[MAX_RECEIVE_DATA_LEN];

	DWORD m_edit_settime_value;
	CButton m_button_connect;
	CButton m_tag_getid;
	CButton m_tag_setid;
	CButton m_tag_gettime;
	CButton m_tag_settime;
	CButton m_server_getid;
	CButton m_server_setid;
	CButton m_router_getid;
	CButton m_router_setid;
	CEdit m_edit_tag_getid1;
	CEdit m_edit_tag_getid2;
	CEdit m_edit_tag_setid1;
	CEdit m_edit_tag_setid2;
	CEdit m_edit_tag_gettime;
	CEdit m_edit_server_getid1;
	CEdit m_edit_server_getid2;
	CEdit m_edit_server_setid1;
	CEdit m_edit_server_setid2;
	CEdit m_edit_router_getid1;
	CEdit m_edit_router_getid2;
	CEdit m_edit_router_setid1;
	CEdit m_edit_router_setid2;
	CComboBox m_combo_three;
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedButtonConnect();

	BOOL OpenCom(CString str_com);
	int FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2);
	int FindChar(BYTE *str, int start, int end, BYTE c1);
	void ParseData(void);
	afx_msg void OnBnClickedTagGetid();
	afx_msg void OnBnClickedTagSetid();
	afx_msg void OnBnClickedButtonGettime();
	afx_msg void OnBnClickedButtonSettime();
	afx_msg void OnBnClickedServerGetid();
	afx_msg void OnBnClickedServerSetid();
	afx_msg void OnBnClickedRouterGetid();
	afx_msg void OnBnClickedRouterSetid();
	
	afx_msg void OnBnClickedButtonServerGettime();
	afx_msg void OnBnClickedButtonServerSettime();
	afx_msg void OnBnClickedButtonRouterGettime();
	afx_msg void OnBnClickedButtonRouterSettime();
	CButton m_button_server_gettime;
	CButton m_button_server_settime;
	CButton m_button_router_gettime;
	CButton m_button_router_settime;
	CEdit m_edit_router_gettime;
	DWORD m_edit_router_settime;
	CEdit m_edit_server_gettime;
	DWORD m_edit_server_settime;
	
};
