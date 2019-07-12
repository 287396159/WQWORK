// LocationDlg.cpp : 實現文件
//

#include "stdafx.h"
#include "Location.h"
#include "LocationDlg.h"
#include <shlwapi.h>


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 用于應用程序“關于”菜單項的 CAboutDlg 對話框

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// 對話框數據
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 實現
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


// CLocationDlg 對話框




CLocationDlg::CLocationDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLocationDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CLocationDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TAB1, m_tab);
}

BEGIN_MESSAGE_MAP(CLocationDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB1, &CLocationDlg::OnTcnSelchangeTab1)
	ON_WM_SIZE()
END_MESSAGE_MAP()


// CLocationDlg 消息處理程序

BOOL CLocationDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// 將“關于...”菜單項添加到系統菜單中。

	// IDM_ABOUTBOX 必須在系統命令范圍內。
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

	// 設置此對話框的圖標。當應用程序主窗口不是對話框時，框架將自動
	//  執行此操作
	SetIcon(m_hIcon, TRUE);			// 設置大圖標
	SetIcon(m_hIcon, FALSE);		// 設置小圖標

	// TODO: 在此添加額外的初始化代碼
	SetWindowText(TEXT("長高科技定位系統V3.0"));

	//調整窗口的寬高為系統的顯示區（出去任務欄高度的部分）
	RECT  r;
	SystemParametersInfo(SPI_GETWORKAREA, 0, &r, 0);	
	MoveWindow(r.left, r.top, r.right - r.left, r.bottom-r.top);
//	MoveWindow(100, 50, 800, 600);
	
	//調整tab控件的位置
	CRect rs; 
//	this->GetClientRect(&rs);
//	m_tab.MoveWindow(&rs);

	m_tab.InsertItem(0, TEXT("設定位置參考點"));
	m_tab.InsertItem(1, TEXT("設定定位卡片"));
	//m_tab.InsertItem(2, TEXT("設定數據節點"));
	m_tab.InsertItem(2, TEXT("列表顯示定位結果"));
	m_tab.InsertItem(3, TEXT("圖形顯示定位結果"));

	m_tab.SetCurSel(3);
	m_SetPort.Create(IDD_DIALOG_SETPORT, GetDlgItem(IDC_TAB1));
	m_SetTag.Create(IDD_DIALOG_SETTAG, GetDlgItem(IDC_TAB1));
	//m_SetNet.Create(IDD_DIALOG_SETNET, GetDlgItem(IDC_TAB1));
	m_ListShow.Create(IDD_DIALOG_LISTSHOW, GetDlgItem(IDC_TAB1));
	m_ImageShow.Create(IDD_DIALOG_IMAGESHOW, GetDlgItem(IDC_TAB1));
	
	
	m_tab.GetClientRect(&rs); 
	rs.left += 1;
	rs.top += 20;
	rs.right -= 1;
	rs.bottom -= 1;

	m_SetPort.MoveWindow(&rs);
	m_SetTag.MoveWindow(&rs);
	//m_SetNet.MoveWindow(&rs);
	m_ListShow.MoveWindow(&rs);
	m_ImageShow.MoveWindow(&rs);
	
	m_SetPort.ShowWindow(SW_HIDE);
	m_SetTag.ShowWindow(SW_HIDE);
	//m_SetNet.ShowWindow(SW_HIDE);
	m_ListShow.ShowWindow(SW_HIDE);
	m_ImageShow.ShowWindow(SW_SHOW);
	
	//創建config文件夾
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config");
	//判斷文件夾不存在，創建文件夾
	if(!PathFileExists(filepath))
	{
		CreateDirectory(filepath, NULL); 
	}

	//創建SaveData文件夾	
	filepath = szPath;
	filepath = filepath + TEXT("SaveData");
	//判斷文件夾不存在，創建文件夾
	if(!PathFileExists(filepath))
	{
		CreateDirectory(filepath, NULL); 
	}

	m_hwnd_SetPort = m_SetPort.GetSafeHwnd();
	m_hwnd_SetTag = m_SetTag.GetSafeHwnd();
	m_hwnd_ListShow = m_ListShow.GetSafeHwnd();
	m_hwnd_ImageShow = m_ImageShow.GetSafeHwnd();

	m_SetPort.SetListShowHwnd(m_hwnd_ListShow);
	m_SetPort.SetImageShowHwnd(m_hwnd_ImageShow);
	m_SetTag.SetListShowHwnd(m_hwnd_ListShow);
	m_SetTag.SetImageShowHwnd(m_hwnd_ImageShow);
	//m_SetNet.SetImageShowHwnd(m_hwnd_ImageShow);

	return TRUE;  // 除非將焦點設置到控件，否則返回 TRUE
}

void CLocationDlg::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}

void CLocationDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

// 如果向對話框添加最小化按鈕，則需要下面的代碼
//  來繪制該圖標。對于使用文檔/視圖模型的 MFC 應用程序，
//  這將由框架自動完成。

void CLocationDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于繪制的設備上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使圖標在工作矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 繪制圖標
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

//當用戶拖動最小化窗口時系統調用此函數取得光標顯示。
//
HCURSOR CLocationDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CLocationDlg::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: Add your control notification handler code here
	*pResult = 0;

	int sel = m_tab.GetCurSel();
	switch(sel)
	{
	case 0:
		m_SetPort.ShowWindow(SW_SHOW);
		m_SetTag.ShowWindow(SW_HIDE);
		//m_SetNet.ShowWindow(SW_HIDE);
		m_ListShow.ShowWindow(SW_HIDE);
		m_ImageShow.ShowWindow(SW_HIDE);
		break;
	case 1:
		m_SetPort.ShowWindow(SW_HIDE);
		m_SetTag.ShowWindow(SW_SHOW);	
		//m_SetNet.ShowWindow(SW_HIDE);
		m_ListShow.ShowWindow(SW_HIDE);
		m_ImageShow.ShowWindow(SW_HIDE);
		break;	
	case 2:
		m_SetPort.ShowWindow(SW_HIDE);
		m_SetTag.ShowWindow(SW_HIDE);	
		//m_SetNet.ShowWindow(SW_SHOW);
		m_ListShow.ShowWindow(SW_SHOW);
		m_ImageShow.ShowWindow(SW_HIDE);
		break;
	case 3:
		m_SetPort.ShowWindow(SW_HIDE);
		m_SetTag.ShowWindow(SW_HIDE);	
		//m_SetNet.ShowWindow(SW_HIDE);
		m_ListShow.ShowWindow(SW_HIDE);
		m_ImageShow.ShowWindow(SW_SHOW);
		break;
/*	case 4:
		m_SetPort.ShowWindow(SW_HIDE);
		m_SetTag.ShowWindow(SW_HIDE);	
		m_SetNet.ShowWindow(SW_HIDE);
		m_ListShow.ShowWindow(SW_HIDE);
		m_ImageShow.ShowWindow(SW_SHOW);
		break;*/
	}

}

void CLocationDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here
	if(nType == 2 || nType == 0)
	{
		CRect rs; 
		this->GetClientRect(&rs);		
		::MoveWindow(m_tab.GetSafeHwnd(),rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
		m_tab.GetClientRect(&rs); 
		rs.left += 1;
		rs.top += 20;
		rs.right -= 1;
		rs.bottom -= 1;
	
		::MoveWindow(m_SetPort.GetSafeHwnd(),rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
		::MoveWindow(m_SetTag.GetSafeHwnd(),rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
		//::MoveWindow(m_SetNet.GetSafeHwnd(),rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
		::MoveWindow(m_ListShow.GetSafeHwnd(),rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
		::MoveWindow(m_ImageShow.GetSafeHwnd(),rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
	}
}

BOOL CLocationDlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	switch (pMsg->wParam)
	{
	case VK_RETURN:
		//MessageBox(_T("回車鍵按下"));
		return TRUE;
	case VK_ESCAPE:
		//MessageBox(_T("ESC鍵按下"));
		return TRUE;
	default:
		break;
	}
	return CDialog::PreTranslateMessage(pMsg);
}
