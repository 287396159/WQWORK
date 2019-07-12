#pragma once
#include "afxwin.h"
#include "afxcmn.h"


#define	MAX_RECEIVE_DATA_LEN	2048

// SetType1 对话框

class SetType1 : public CDialog
{
	DECLARE_DYNAMIC(SetType1)

public:
	SetType1(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~SetType1();

// 对话框数据
	enum { IDD = IDD_SET_ONE };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()

private:
	bool StringToChar(CString str, BYTE* data);

public:

	BOOL	m_bConnect;
	BOOL	m_bGetTag;
	BOOL	m_bGetPort;
	BOOL	m_bGetServer;

	BOOL	m_bSetPortID;
	BOOL	m_bGetPortID;
	BOOL	m_bSetTagID;
	BOOL	m_bGetTagID;

	BOOL	m_bGetVersion;
	CString	m_StrVersion;

	BOOL	m_bDHCP;
	BYTE	m_dhcpvalue;

	BOOL	m_bGetLocalIp;
	BYTE	m_LocalIp[4];

	BOOL	m_bGetLocalPort;
	unsigned short m_LocalPort;

	BOOL	m_bGetServerIp;
	BYTE	m_ServerIp[4];

	BOOL	m_bGetServerPort;
	unsigned short m_ServerPort;

	BOOL	m_bGetLocalMac;
	BYTE	m_LocalMac[6];

	BOOL	m_bGetLocalID;
	BYTE	m_LocalID[2];

	BOOL	m_bGetServerIpMode;
	int		m_ServerIpMode;

	BOOL	m_bSubMask;
	BYTE	m_SubMask[4];

	BOOL	m_bGateWay;
	BYTE	m_GateWay[4];

	BOOL	m_bGetSendNameTime;
	DWORD	m_SendNameTime;

	BOOL	m_bSleepTime;
	BOOL	m_bWorkTime;
	DWORD	m_SleepTime;
	DWORD	m_WorkTime;
	BOOL	m_bSetServerID;
	BOOL	m_bGetServerID;
	BOOL	m_bGetRouterID;
	BOOL	m_bSetRouterID;
	DWORD m_edit_settime_value;

	BYTE	m_Tag_ID[2];
	BYTE	m_Port_ID[2];

	BOOL	m_bSetPower;
	BOOL	m_bGetPower;
	BYTE	m_SendPower;
	BYTE	number;
	BOOL	m_bSetResponse;
	BOOL	m_bGetResponse;
	DWORD	m_Response;

	HANDLE	m_hcom;
	int		m_Receive_Data_Len;
	BYTE	m_Receive_Data_Char[MAX_RECEIVE_DATA_LEN];

	CButton m_button_connect;
	CComboBox m_combo_one;
	//port
	CButton m_button_port_getid;
	CButton m_button_port_setid;
	CEdit m_edit_port_getid1;
	CEdit m_edit_port_getid2;
	CEdit m_edit_port_setid1;
	CEdit m_edit_port_setid2;
	//tag
	CButton m_button_tag_getid;
	CButton m_button_tag_setid;
	CButton m_button_tag_getworktime;
	CButton m_button_tag_setworktime;
	CButton m_button_tag_getsleeptime;
	CButton m_button_tag_setsleeptime;
	CEdit m_edit_tag_getid1;
	CEdit m_edit_tag_getid2;
	CEdit m_edit_tag_setid1;
	CEdit m_edit_tag_setid2;
	CEdit m_edit_tag_getworktime;
	DWORD m_edit_tag_seteworktime;
	CEdit m_edit_tag_getsleeptime;
	DWORD m_edit_tag_setsleeptime;
	//server
	CButton m_button_local_dhcp;
	CButton m_button_local_staticip;
	CButton m_button_local_setip;
	CButton m_button_local_settime;
	CButton m_button_local_submask;
	CButton m_button_local_gateway;
	CButton m_button_local_setport;
	CButton m_button_server_dhcp;
	CButton m_button_server_staticip;
	CButton m_button_server_setip;
	CButton m_button_server_setport;
	CButton m_button_local_setid;
	
	CEdit m_edit_local_dhcp;
	DWORD m_edit_local_setport;
	DWORD m_edit_local_settime;
	DWORD m_edit_server_setport;
	CEdit m_edit_local_port;
	CEdit m_edit_local_mac;
	CEdit m_edit_local_time;
	CEdit m_edit_server_dhcp;
	CEdit m_edit_server_port;
	CEdit m_edit_local_setid1;
	CEdit m_edit_local_id1;
	CEdit m_edit_local_id2;
	CEdit m_edit_local_setid2;

	CIPAddressCtrl m_ipaddress_local_ip;
	CIPAddressCtrl m_ipaddress_local_setip;
	CIPAddressCtrl m_ipaddress_local_submask;
	CIPAddressCtrl m_ipaddress_local_setsubmask;
	CIPAddressCtrl m_ipaddress_local_gateway;
	CIPAddressCtrl m_ipaddress_local_setgateway;
	CIPAddressCtrl m_ipaddress_server_ip;
	CIPAddressCtrl m_ipaddress_server_setip;
	
	afx_msg void OnBnClickedButtonPortGetid();
	afx_msg void OnBnClickedButtonPortSetid();
	afx_msg void OnBnClickedButtonConnect();

	virtual BOOL OnInitDialog();
	BOOL OpenCom(CString str_com);
	int FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2);
	int FindChar(BYTE *str, int start, int end, BYTE c1);
	void ParseData(void);
	void GetInfo();
	void GetTag();
	
	
	afx_msg void OnBnClickedButtonTagGetid();
	afx_msg void OnBnClickedButtonTagGetworktime();
	afx_msg void OnBnClickedButtonTagGetsleeptime();
	afx_msg void OnBnClickedButtonTagSetid();
	afx_msg void OnBnClickedButtonTagSetworktime();
	afx_msg void OnBnClickedButtonTagSetsleeptime();
	afx_msg void OnBnClickedButtonLocalDhcp();
	afx_msg void OnBnClickedButtonLocalStaticip();
	afx_msg void OnBnClickedButtonLocalSetip();
	afx_msg void OnBnClickedButtonLocalSetport();
	afx_msg void OnBnClickedButtonLocalSetid();
	afx_msg void OnBnClickedButtonLocalSettime();
	afx_msg void OnBnClickedButtonLocalSubmask();
	afx_msg void OnBnClickedButtonLocalGateway();
	afx_msg void OnBnClickedButtonServerDhcp();
	afx_msg void OnBnClickedButtonServerStaticip();
	afx_msg void OnBnClickedButtonServerSetip();
	afx_msg void OnBnClickedButtonServerSetport();
	afx_msg void OnBnClickedButton4();
	CButton m_button_getpower;
	CButton m_button_setpower;
	CButton m_button_getresponse;
	CButton m_button_setresponse;
	CEdit m_edit_getpower;
	CComboBox m_combo_setpower;
	CEdit m_edit_getresponse;
	DWORD m_edit_setresponse;
	afx_msg void OnBnClickedButtonGetpower();
	afx_msg void OnBnClickedButtonSetpower();
	afx_msg void OnBnClickedButtonGetresponse();
	afx_msg void OnBnClickedButtonSetresponse();
	
};
