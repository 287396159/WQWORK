// SetIDDlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "SetID.h"
#include "SetIDDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif




// ����Ӧ�ó��򡰹��ڡ��˵���� CAboutDlg �Ի���

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// �Ի�������
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

// ʵ��
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


// CSetIDDlg �Ի���




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


// CSetIDDlg ��Ϣ�������

BOOL CSetIDDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// ��������...���˵�����ӵ�ϵͳ�˵��С�

	// IDM_ABOUTBOX ������ϵͳ���Χ�ڡ�
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

	SetWindowTextW(TEXT("��λϵͳ�������"));

	// ���ô˶Ի����ͼ�ꡣ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��

	// TODO: �ڴ���Ӷ���ĳ�ʼ������
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


	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
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

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void CSetIDDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ����������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
		dc.DrawIcon(x, y, m_hIcon);
	}else{
		CDialog::OnPaint();
	}
}

//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù��
//��ʾ��
HCURSOR CSetIDDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CSetIDDlg::OnTcnSelchangeTab1(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
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
