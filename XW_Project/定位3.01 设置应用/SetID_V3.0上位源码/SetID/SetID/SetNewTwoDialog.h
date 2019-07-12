#pragma once
#include "resource.h"
#include "afxwin.h"
#include "afxcmn.h"
#include "DataTypeConverter.h"

// CSetNewTwoDialog 对话框

class CSetNewTwoDialog : public CDialogEx
{
	DECLARE_DYNAMIC(CSetNewTwoDialog)

public:
	CSetNewTwoDialog(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CSetNewTwoDialog();

// 对话框数据
	enum { IDD = IDD_SET_NEWTWO };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:

	BOOL	m_bConnect;
	HANDLE	m_hcom;
	BOOL OpenCom(CString str_com);
	//DWORD ComReadThread(LPVOID lparam);
	int	m_Receive_Data_Len;
	#define	MAX_RECEIVE_DATA_LEN	2048
	BYTE	m_Receive_Data_Char[MAX_RECEIVE_DATA_LEN];

	afx_msg void OnBnClickedButton1();
	//CMscomm1 m_ctrlComm;
	CEdit m_com_num;
	CButton m_openOrCloceBtn;
	afx_msg void OnBnClickedButtonConnectdrivace();
	CDataTypeConverter DTCter;
	void ParseData(void);

	afx_msg void OnBnClickedButtongetid();
	afx_msg void OnBnClickedButtoncheckdrivacetime();
	afx_msg void OnBnClickedButtonreadwificount();
	afx_msg void OnBnClickedButtonreadwifipassword();
	afx_msg void OnBnClickedButtonreadipadress();
	afx_msg void OnBnClickedButtonreadport();
	afx_msg void OnBnClickedButtonsetid();
	BYTE getByteHex(CString str);

	afx_msg void OnBnClickedButtonsetdrivaceidtime();
	afx_msg void OnBnClickedButtonsetwificount();
	afx_msg void OnBnClickedButtonsetwifipassword();
	afx_msg void OnBnClickedButtonsetipadress();
	CIPAddressCtrl m_ipaddress_setIp;
	afx_msg void OnBnClickedButtonsetport();
	void dealReveData(BYTE data[1024],long len);

	DECLARE_EVENTSINK_MAP();
	void OnCommMscomm1();
	CIPAddressCtrl m_get_serviseIp4;
};
