// SetIDDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "SetID.h"
#include "SetIDDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif




// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// 对话框数据
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// CSetIDDlg 对话框




CSetIDDlg::CSetIDDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSetIDDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSetIDDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TAB1, m_tab);
}

BEGIN_MESSAGE_MAP(CSetIDDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, &CSetIDDlg::OnTcnSelchangeTab1)
END_MESSAGE_MAP()


// CSetIDDlg 消息处理程序

BOOL CSetIDDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	SetWindowTextW(TEXT("定位系统设置软件"));

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码
	CRect rs; 
	this->GetClientRect(&rs);
	m_tab.MoveWindow(&rs);

//	m_tab.InsertItem(0, TEXT("V2.11"));
	m_tab.InsertItem(0, TEXT("V3.0-1"));
	m_tab.InsertItem(1, TEXT("V3.0-2"));

	m_tab.SetCurSel(0);
	m_SetType1.Create(IDD_SET_ONE, GetDlgItem(IDC_TAB1));
	m_SetType2.Create(IDD_SET_NEWTWO, GetDlgItem(IDC_TAB1));
	//m_SetType3.Create(IDD_SET_THREE, GetDlgItem(IDC_TAB1));

	m_tab.GetClientRect(&rs); 
	rs.left += 1;
	rs.top += 20;
	rs.right -= 1;
	rs.bottom -= 1;

	m_SetType1.MoveWindow(&rs);
	m_SetType2.MoveWindow(&rs);
	//m_SetType3.MoveWindow(&rs);

	m_SetType1.ShowWindow(SW_SHOW);
	m_SetType2.ShowWindow(SW_HIDE);
	//m_SetType3.ShowWindow(SW_HIDE);


	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CSetIDDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CSetIDDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}else{
		CDialog::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CSetIDDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CSetIDDlg::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: 在此添加控件通知处理程序代码
	*pResult = 0;

	int sel = m_tab.GetCurSel();
	switch(sel)
	{
		case 0:
			m_SetType1.ShowWindow(SW_SHOW);
			m_SetType2.ShowWindow(SW_HIDE);
			//m_SetType3.ShowWindow(SW_HIDE);
			break;
		case 1:
			m_SetType1.ShowWindow(SW_HIDE);
			m_SetType2.ShowWindow(SW_SHOW);
			//m_SetType3.ShowWindow(SW_HIDE);
			break;
		case 2:
			m_SetType1.ShowWindow(SW_HIDE);
			m_SetType2.ShowWindow(SW_HIDE);
			//m_SetType3.ShowWindow(SW_SHOW);
			break;
	}
}
