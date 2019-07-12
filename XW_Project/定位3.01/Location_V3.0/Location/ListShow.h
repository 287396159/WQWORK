#pragma once
#include "afxwin.h"
#include "afxcmn.h"
#include "Ini.h"

#include <WinSock2.h>
#include <Iphlpapi.h>

#include "define.h"





// CListShow dialog
struct SaveTagInfo
{
	CString ID;
	CString Name;
};

struct SavePortInfo
{
	CString ID;
	CString Name;
};

struct ReceiveInfo
{
	BOOL ReceivData;			//是否接受到資料
	BYTE Type;					//定位信息類型
	BYTE DrivaceType;			//設備類型
	BYTE TagId[2];			//
	BYTE Port1ID[2];		//保存最近的一個Router的ID
	BYTE Port1Rssi;				//保存最近的一個Router接受數據時候的信號強度
	BYTE Port2ID[2];
	BYTE Port2Rssi;
	BYTE Port3ID[2];
	BYTE Port3Rssi;
	BYTE Battery;		//電池電量
	unsigned short SensorTime;
	BYTE index;					//本次資料的序列號	
	DWORD FirstReceiveTime;		//第一次接受到數據的時間
	//DWORD SecondReceiveTime;		//第一次接受到數據的時間
	//DWORD threadReceiveTime;		//第一次接受到數據的時間
	BOOL IsUpdate;				//數據是否有更新（有新的定位數據到來，需要更新到列表中），True為更新

	DWORD TotalCount;		//總共接收到多少個封包
	DWORD LostCount;		//丟包數量
};


class CListShow : public CDialog
{
	DECLARE_DYNAMIC(CListShow)

public:
	CListShow(CWnd* pParent = NULL);   // standard constructor
	virtual ~CListShow();

// Dialog Data
	enum { IDD = IDD_DIALOG_LISTSHOW };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	HANDLE			m_hcom;

	BOOL			m_bConnect;
	SaveTagInfo		m_SaveTagInfo[MAX_SAVE_TAG_COUNT];
	SavePortInfo	m_SavePortInfo[MAX_SAVE_PORT_COUNT];
	ReceiveInfo		m_ReceiveInfo[MAX_RECEIVE_COUNT];
	ReceiveInfo		m_ReceiveInfo2[MAX_RECEIVE_COUNT];

	SOCKET	m_UDPSocket;

	BYTE	m_TagID[2];

	int	m_Cur_SaveTagCount;
	int	m_Cur_SavePortCount;
	int m_Cur_ReceiveCount;

	CIni	m_ini;
	int		m_Receive_Data_Len;
	BYTE	m_Receive_Data_Char[MAX_RECEIVE_DATA_LEN];


	BOOL GetSaveTagInfo(void);
	BOOL GetSavePortInfo(void);
	int FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2);
	int FindChar(BYTE *str, int start, int end, BYTE c1);
	void ParseData(void);
	void ParseData_Connect(void);
	BOOL OpenCom(CString str_com);
	BYTE isJinJiType(BYTE c1);

	CButton m_button_connect;
	CComboBox m_combo_comport;
	CListCtrl m_list_info;

protected:
	virtual void PreInitDialog();
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedCheck1();
	afx_msg void OnBnClickedButtonConnect();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	DWORD m_edit_port;
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	CButton m_button_com_start;
	CButton m_button_clean_list;
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnBnClickedButtonComstart();
	afx_msg void OnBnClickedButtonCleanList();
};
