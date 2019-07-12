#pragma once
#include "afxdtctl.h"
#include "afxwin.h"
#include "ini.h"
#include "Config.h"


// COldDataRePlay dialog

class COldDataRePlay : public CDialog
{
	DECLARE_DYNAMIC(COldDataRePlay)

public:
	COldDataRePlay(CWnd* pParent = NULL);   // standard constructor
	virtual ~COldDataRePlay();

// Dialog Data
	enum { IDD = IDD_DIALOG_OLDDATE_REPLAY };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
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

	CTime m_time_start;
	CTime m_time_end;
	CTime m_time_cur;

	BOOL	m_bPlayStart;
	BOOL	m_bPlayPause;
	int		m_PlaySpeed;
	SaveOldData	*m_pSaveOldData;
	int		m_CurSaveOldData_Count;
	int		m_CurSaveOldData_Index;

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

	ImagePortInfo	m_ImagePortInfo[MAX_IMAGEPORT_COUNT];
	int		m_CurImagePortCount;
	int		m_CurImagePortIndex;

	CIni	m_ini;

	ImageShowReceiveInfo		m_ReceiveInfo[MAX_RECEIVE_COUNT];
//	BYTE	m_TagID[2];
	int		m_Cur_ReceiveCount;
	

	ImageShowSaveTagInfo		m_SaveTagInfo[MAX_SAVE_TAG_COUNT];
	ImageShowSavePortInfo		m_SavePortInfo[MAX_SAVE_PORT_COUNT];
	int		m_Cur_SaveTagCount;
	int		m_Cur_SavePortCount;

	CStringArray	m_SaveServerNet;
	int		m_ShowTagLeftTopCount;
	CFont	m_font;

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
	int		m_CurShowType;

	BOOL	m_bCheckBox_all;

	int		IsShowTag(BYTE id1, BYTE id2);
	void PaintImage();
	BOOL	MyLoadImage();
	BOOL GetSavePortInfo(void);
	VOID GetModulePath(LPTSTR path, LPCTSTR module);
	bool StringToChar(CString str, BYTE* data);
	BOOL GetTagCoordinate(ImageShowReceiveInfo &isri, int &x, int &y, int &count, DWORD &lmr);
	void ClearPortDrawTagId(BYTE portid[], BYTE tagid[]);
	int FindImagePortID(BYTE id[]);
	BOOL GetSaveTagInfo(void);
	BOOL GetTagName(CString &str_ID);
	BOOL GetPortName(CString &str_ID);
	void LoadConfigInfo();
	CDateTimeCtrl m_datatime_start;
	CComboBox m_combo_hour_start;
	CComboBox m_combo_minute_start;
	CDateTimeCtrl m_datatime_end;
	CComboBox m_combo_hour_end;
	CComboBox m_combo_minute_end;
	CStatic m_static_show_tag;
	CButton m_check_all;
	CCheckListBox m_CheckList;
	virtual BOOL OnInitDialog();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnBnClickedCheckAll();
	afx_msg void OnLbnSelchangeList1();
	afx_msg void OnBnClickedButtonStart();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	CStatic m_static_time;
	CStatic m_static_speed;
	CButton m_button_pause;
	CButton m_button_start;
	afx_msg void OnBnClickedButtonSpeedAdd();
	afx_msg void OnBnClickedButtonSpeedDec();
	afx_msg void OnBnClickedButtonPasue();
};
