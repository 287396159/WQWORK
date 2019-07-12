// OldData.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "OldData.h"


// COldData dialog

IMPLEMENT_DYNAMIC(COldData, CDialog)

COldData::COldData(CWnd* pParent /*=NULL*/)
	: CDialog(COldData::IDD, pParent)
{

}

COldData::~COldData()
{
}

void COldData::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TAB1, m_tab);
}


BEGIN_MESSAGE_MAP(COldData, CDialog)
	ON_WM_SHOWWINDOW()
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, &COldData::OnTcnSelchangeTab1)
END_MESSAGE_MAP()


// COldData message handlers

BOOL COldData::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here

	//調整窗口的寬高為系統的顯示區（出去任務欄高度的部分）
	RECT  r;
	SystemParametersInfo(SPI_GETWORKAREA, 0, &r, 0);	
	MoveWindow(r.left, r.top, r.right - r.left, r.bottom-r.top);

	CRect rs; 
	this->GetClientRect(&rs);		
	::MoveWindow(m_tab.GetSafeHwnd(),rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
	m_tab.GetClientRect(&rs); 
	rs.left += 1;
	rs.top += 20;
	rs.right -= 1;
	rs.bottom -= 1;

	m_tab.InsertItem(0, TEXT("數據統計"));
	m_tab.InsertItem(1, TEXT("定位回放"));

	m_tab.SetCurSel(0);

	m_OldDataTongJi.Create(IDD_DIALOG_OLDDATA_TONGJI, GetDlgItem(IDC_TAB1));
	m_OldDataRePlay.Create(IDD_DIALOG_OLDDATE_REPLAY, GetDlgItem(IDC_TAB1));

	m_OldDataTongJi.MoveWindow(&rs);
	m_OldDataRePlay.MoveWindow(&rs);

	m_OldDataTongJi.ShowWindow(SW_HIDE);
	m_OldDataRePlay.ShowWindow(SW_HIDE);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void COldData::OnShowWindow(BOOL bShow, UINT nStatus)
{
	CDialog::OnShowWindow(bShow, nStatus);

	// TODO: Add your message handler code here
	switch(m_tab.GetCurSel())
	{
	case 0:
		if(bShow)
			m_OldDataTongJi.ShowWindow(SW_SHOW);
		else
			m_OldDataTongJi.ShowWindow(SW_HIDE);
		m_OldDataRePlay.ShowWindow(SW_HIDE);
		break;
	case 1:
		if(bShow)
			m_OldDataRePlay.ShowWindow(SW_SHOW);
		else
			m_OldDataRePlay.ShowWindow(SW_HIDE);
		m_OldDataTongJi.ShowWindow(SW_HIDE);
		break;
	}
}

void COldData::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	m_OldDataTongJi.ShowWindow(SW_HIDE);
	m_OldDataRePlay.ShowWindow(SW_HIDE);
	CDialog::OnCancel();
}

void COldData::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: Add your control notification handler code here
	*pResult = 0;

	switch(m_tab.GetCurSel())
	{
	case 0:		
		m_OldDataTongJi.ShowWindow(SW_SHOW);		
		m_OldDataRePlay.ShowWindow(SW_HIDE);
		break;
	case 1:				
		m_OldDataTongJi.ShowWindow(SW_HIDE);
		m_OldDataRePlay.ShowWindow(SW_SHOW);
		break;
	}
}
