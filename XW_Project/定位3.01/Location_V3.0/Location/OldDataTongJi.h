#pragma once
#include "afxdtctl.h"
#include "afxwin.h"
#include "define.h"
#include "ini.h"


// COldDataTongJi dialog

class COldDataTongJi : public CDialog
{
	DECLARE_DYNAMIC(COldDataTongJi)

public:
	COldDataTongJi(CWnd* pParent = NULL);   // standard constructor
	virtual ~COldDataTongJi();

// Dialog Data
	enum { IDD = IDD_DIALOG_OLDDATA_TONGJI };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	HDC m_hdc;
	HDC m_mdc;
	HBITMAP m_bmp;
	CFont	m_font;
	int	m_DrawLeft;
	int m_DrawTop;
	int	m_DrawWidth;
	int m_DrawHeight;

	int	m_DrawLeft_TongJi;
	int m_DrawTop_TongJi;
	int	m_DrawWidth_TongJi;
	int m_DrawHeight_TongJi;

	int m_DrawPortStart;
	int m_DrawXTimeStart;
	int m_DrawXTimeCount;

	CIni	m_ini;


	HBRUSH m_Brush_white;
	HBRUSH m_Brush_blue;
	HBRUSH m_Brush_red;
	HBRUSH m_Brush_black;
	HBRUSH m_Brush_violet;
	
	CTime m_time_start;
	CTime m_time_end;
	CTime m_time_start_draw;
	CTime m_time_end_draw;

	int	m_NormalTime;
	int	m_WarringTime;
	int	m_NoMoveTime;
	int m_LostTime;

	int m_CurSelTag;
	BOOL m_bLBtnDown;
	CPoint m_LBtnDownPoint;

//	ReadSaveData	*m_pReadSaveData;
	TongJiJieGuo	m_TongJiJieGuo[MAX_SAVE_TAG_COUNT];
	int				m_CurTongJiCount;
	DWORD TongJiLen;
	BYTE		m_TongJiPort[MAX_SAVE_PORT_COUNT][2];
	int			m_CurTongJiPortCount;

	DWORD		m_PortTime[MAX_SAVE_PORT_COUNT];

	ImageShowSaveTagInfo		m_SaveTagInfo[MAX_SAVE_TAG_COUNT];
	ImageShowSavePortInfo		m_SavePortInfo[MAX_SAVE_PORT_COUNT];
	int		m_Cur_SaveTagCount;
	int		m_Cur_SavePortCount;

	CDateTimeCtrl m_datatime_start;
	CComboBox m_combo_hour_start;
	CComboBox m_combo_minute_start;
	CDateTimeCtrl m_datatime_end;
	CComboBox m_combo_hour_end;
	CComboBox m_combo_minute_end;
	CListBox m_list;

	BOOL GetSavePortInfo(void);
	BOOL GetSaveTagInfo(void);
	BOOL GetTagName(CString &str_ID);
	BOOL GetPortName(CString &str_ID);
	virtual BOOL OnInitDialog();
	void Draw();
	void GetModulePath(LPTSTR path, LPCTSTR module);
	int FindTongJiTag(BYTE id[]);
	int FindTongJiPort(BYTE id[]);
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnShowWindow(BOOL bShow, UINT nStatus);
	afx_msg void OnBnClickedButtonStart();
	
	afx_msg void OnLbnSelchangeList1();
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg BOOL OnMouseWheel(UINT nFlags, short zDelta, CPoint pt);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	virtual BOOL PreTranslateMessage(MSG* pMsg);
};
