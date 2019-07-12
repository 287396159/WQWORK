#pragma once
#include "afxwin.h"
#include "afxcmn.h"


#define	MAX_RECEIVE_DATA_LEN	2048
// SetServer dialog

class SetServer : public CDialog
{
	DECLARE_DYNAMIC(SetServer)

public:
	SetServer(CWnd* pParent = NULL);   // standard constructor
	virtual ~SetServer();

// Dialog Data
	enum { IDD = IDD_DIALOG_SETSERVER };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	BOOL	m_bConnect;

	HANDLE	m_hcom;
	int		m_Receive_Data_Len;
	BYTE	m_Receive_Data_Char[MAX_RECEIVE_DATA_LEN];

	BOOL	m_bGetDevice;

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

	BOOL	m_bGetLocalName;
	BYTE	m_LocalName[42];

	BOOL	m_bGetServerIpMode;
	int		m_ServerIpMode;

	BOOL	m_bSubMask;
	BYTE	m_SubMask[4];

	BOOL	m_bGateWay;
	BYTE	m_GateWay[4];

	BOOL	m_bGetSendNameTime;
	DWORD	m_SendNameTime;
	
	BOOL OpenCom(CString str_com);
	void ParseData(void);
	int FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2);
	void GetInfo();


	CButton m_button_connect;
	CComboBox m_combo_comport;
	CButton m_check_manual;
	CEdit m_edit_version;
	CEdit m_edit_dhcp;
	CIPAddressCtrl m_ip_local_show;
	CEdit m_edit_local_port_show;
	CEdit m_edit_local_mac_show;
	CIPAddressCtrl m_ip_server_show;
	CEdit m_edit_server_port_show;
	CButton m_btn_set_dhcp;
	CButton m_btn_set_static_ip;
	CButton m_btn_set_local_ip;
	CButton m_btn_set_local_port;
	CButton m_btn_set_server_ip;
	CButton m_btn_set_server_port;
	CIPAddressCtrl m_ip_local_set;
	DWORD m_port_local_set;
	CIPAddressCtrl m_ip_server_set;
	DWORD m_port_server_set;
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedCheck();
	afx_msg void OnBnClickedButtonConnect();
	afx_msg void OnBnClickedButtonDhcp();
	afx_msg void OnBnClickedButtonStaticIp();
	afx_msg void OnBnClickedButtonSetLocalIp();
	afx_msg void OnBnClickedButtonSetLocalPort();
	afx_msg void OnBnClickedButtonServerSetIp();
	afx_msg void OnBnClickedButtonSetServerPort();
	CEdit m_edit_local_name;
	CButton m_btn_set_local_name;
	CEdit m_edit_local_name_set;
	afx_msg void OnBnClickedButtonSetLocalName();
	CEdit m_edit_sendname_time_show;
	CButton m_btn_set_sendname;
	CEdit m_edit_sendname_time_set;
	CIPAddressCtrl m_ipaddr_submask_show;
	CButton m_btn_set_submask;
	CIPAddressCtrl m_ipaddr_submask_set;
	CIPAddressCtrl m_ipaddr_gateway_show;
	CButton m_btn_set_gateway;
	CIPAddressCtrl m_ipaddr_gateway_set;
	CEdit m_edit_server_ipmode_show;
	CButton m_btn_set_serverip_dhcp;
	CButton m_btn_server_ip_static;
	DWORD m_sendname_time_set;
	afx_msg void OnBnClickedButtonSetSendnameTime();
	afx_msg void OnBnClickedButtonSetSubmusk();
	afx_msg void OnBnClickedButtonSetGateway();
	afx_msg void OnBnClickedButtonSetServerIpDhcp();
	afx_msg void OnBnClickedButtonServerIpStatic();
};
