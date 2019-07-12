#pragma once
#include "afxwin.h"
#include "ini.h"
#include "SetImagePort.h"
#include <WinSock2.h>
#include <Iphlpapi.h>
#include "Config.h"
#include "WarningMessage.h"
#include "ImagePortShow.h"
#include "TagOnLine.h"
#include "olddata.h"

// CImageShow dialog


class CImageShow : public CDialog
{
	DECLARE_DYNAMIC(CImageShow)

public:
	CImageShow(CWnd* pParent = NULL);   // standard constructor
	virtual ~CImageShow();

// Dialog Data
	enum { IDD = IDD_DIALOG_IMAGESHOW };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	BOOL			m_bConnect;
	BOOL			m_bLoadImage;

	int		m_StaticShow_left;
	int		m_StaticShow_width;
	int		m_StaticShow_top;
	int		m_StaticShow_height;

	int		m_ImageLeft;
	int		m_ImageTop;
	int		m_ImageWidth;
	int		m_ImageHeight;

	int		m_SaveImageWidth;
	int		m_SaveImageHeight;

	HDC		m_hdc;
	HDC		m_mdc;
	HDC		m_ndc;
	HBITMAP	m_bmp_ndc;
	HBITMAP m_bmp_mdc;

	CString	m_ImageFile;
	CString m_SaveImageFile;

	HBRUSH m_Brush_white;
	HBRUSH m_Brush_Port;
	HBRUSH m_Brush_Tag_Emergency;
	HBRUSH m_Brush_Tag_Normal;
	HBRUSH m_Brush_Tag_Warring;
	HBRUSH m_Brush_Tag_NoMove;

	HPEN	m_Pen_Emergency;
	HPEN	m_Pen_Normal;
	HPEN	m_Pen_Warring;
	HPEN	m_Pen_NoMove;

	HANDLE	m_hcom;
	
	HANDLE	m_SaveDataHandle;
	int		m_SaveDataHour;
	int		m_SaveDataDay;

	ImagePortInfo	m_ImagePortInfo[MAX_IMAGEPORT_COUNT];
	int		m_CurImagePortCount;
	int		m_CurImagePortIndex;

	CIni	m_ini;
	CSetImagePort m_SetImagePort;
	CConfig		m_Config;
	CWarningMessage	m_WarningMessage;
	CImagePortShow	m_ImagePortShow;
	CTagOnLine		m_TagOnLine;
	COldData		m_OldData;

	BOOL	m_bShowTagOnLineDlg;

	BOOL	m_bImagePortLButtonDown;
	CPoint	m_ImagePortLButtonDownPoint;

	ImageShowReceiveInfo		m_ReceiveInfo[MAX_RECEIVE_COUNT];
	BYTE	m_TagID[2];
	int		m_Cur_ReceiveCount;
	
	ImageShowReceiveInfo		m_DlgPortShowReceiveInfo[MAX_RECEIVE_COUNT];
	int		m_Cur_DlgPortShowPortIndex;
	int		m_Cur_DlgPortShowCount;
	BOOL	m_bDlgPortShowLButtonDown;
	int		m_DlgPortShowPort_LButtonDownIndex;

	int		m_Receive_Data_Len;
	BYTE	m_Receive_Data_Char[MAX_RECEIVE_DATA_LEN];

	BOOL	m_bGetDevice;

	ImageShowSaveTagInfo		m_SaveTagInfo[MAX_SAVE_TAG_COUNT];
	ImageShowSavePortInfo		m_SavePortInfo[MAX_SAVE_PORT_COUNT];
	int		m_Cur_SaveTagCount;
	int		m_Cur_SavePortCount;

	CStringArray	m_SaveServerNet;
	CStringArray	m_SaveServerNet_ID;
	CStringArray	m_SaveServerNet_Name;
	CStringArray	m_ReceServerNet;
	CStringArray	m_WarningInfo;
	int				m_LastWarnningCount;

	int		m_ShowTagLeftTopCount;

	SaveOldData		m_SaveOldData;

	CFont	m_font;

	SOCKET	m_UDPSocket;
	CString	m_ServerIp;
	int		m_ServerPort;
	BOOL	m_bCOMConnectAuto;
	CString	m_StrComPort;
	BOOL	m_bNoShowTimeOutID;
	int		m_ShowTimeOutTime;
	BOOL	m_bShowNoMoveTag;
	int		m_ShowNoMoveTagTime;
	BOOL	m_bAutoEmergency;
	BOOL	m_bShowReferencePort;
	BOOL	m_bWarning_ShowEmergencyTag;
	BOOL	m_bWarning_ShowServerNetTimeout;
	int		m_Warning_ServerNetTime;
	DWORD	m_Last_ServerNetTime;
	BOOL	m_bWarning_ShowTagTimeout;
	BOOL	m_bWarning_ShowLowBattery;
	int		m_Warning_LowBatteryValue;
	int		m_CurShowType;
	BOOL	m_bShowTagContinuous;
	int		m_ShowTagContinuousNumber;

	BOOL	m_bCheckBox_all;

	void	UpdateShowPortDlgData(BOOL UpdateTitle);
	int		IsShowTag(BYTE id1, BYTE id2);
	void	PaintImage();
	BOOL	MyLoadImage();
	BOOL	SaveImage();
	BOOL GetSaveServerNetInfo(void);
	BOOL GetSavePortInfo(void);
	VOID GetModulePath(LPTSTR path, LPCTSTR module);
	void SetImagePort(BYTE id1, BYTE id2);
	void DeleteImagePort();
	bool StringToChar(CString str, BYTE* data);
	int FindChar(BYTE *str, int start, int end, BYTE c1, BYTE c2);
	int FindChar(BYTE *str, int start, int end, BYTE c1);
	void ParseData(void);
	void ParseData_Connect(void);
	BOOL OpenCom(CString str_com);
	BOOL GetTagCoordinate(ImageShowReceiveInfo &isri, int &x, int &y, int &count, DWORD &lmr);
	void SrandOnePortXY(ImagePortInfo ipi, int &x, int &y);	
	void ClearPortDrawTagId(BYTE portid[], BYTE tagid[]);
	BOOL OpenSaveDataFile();

	int FindImagePortID(BYTE id[]);
	BOOL GetSaveTagInfo(void);
	BOOL GetTagName(CString &str_ID);
	BOOL GetPortName(CString &str_ID);
	void LoadConfigInfo();
	BYTE isJinJiType(BYTE c1);


	CButton m_button_connect;
	CButton m_button_set;
	virtual BOOL OnInitDialog();

	afx_msg void OnPaint();
	afx_msg void OnBnClickedButtonLoadimage();
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnBnClickedButtonConnect();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnBnClickedButtonSet();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
//	CComboBox m_combo_show_type;
//	afx_msg void OnCbnSelchangeComboShowType();
	CEdit m_edit_warning_info;
	CButton m_btn_warning_show;
	afx_msg void OnBnClickedButtonWarningAll();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	CCheckListBox m_CheckList;
	CStatic m_static_show_tag;
	CButton m_check_all;
	CButton m_button_olddata;
	afx_msg void OnBnClickedCheckAll();
	afx_msg void OnLbnSelchangeList1();
	CButton m_button_tag_online;
	afx_msg void OnBnClickedButtonTagOnline();
	afx_msg void OnBnClickedButtonOlddata();
};
