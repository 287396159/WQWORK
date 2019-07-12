// ConfigServer.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "ConfigServer.h"
#include <WinSock2.h>
#include <Iphlpapi.h>
#include <shlwapi.h>

// CConfigServer dialog

IMPLEMENT_DYNAMIC(CConfigServer, CDialog)

CConfigServer::CConfigServer(CWnd* pParent /*=NULL*/)
	: CDialog(CConfigServer::IDD, pParent)
	, m_edit_port(51234)
{

}

CConfigServer::~CConfigServer()
{
}

void CConfigServer::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_SAVE, m_button_save);
	DDX_Control(pDX, IDC_COMBO_IP, m_combobox_ip);
	DDX_Text(pDX, IDC_EDIT_PORT, m_edit_port);
}


BEGIN_MESSAGE_MAP(CConfigServer, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_SAVE, &CConfigServer::OnBnClickedButtonSave)
	ON_BN_CLICKED(IDC_BUTTON_SAVEIP, &CConfigServer::OnBnClickedButtonSaveip)
END_MESSAGE_MAP()


void CConfigServer::Init()
{
	while(m_combobox_ip.GetCount() > 0)
		m_combobox_ip.DeleteString(0);
	m_combobox_ip.SetWindowTextW(TEXT(""));

	CString str;
	//PIP_ADAPTER_INFO結構體指針存儲本機網卡信息
    PIP_ADAPTER_INFO pIpAdapterInfo = new IP_ADAPTER_INFO();
    //得到結構體大小,用于GetAdaptersInfo參數
    unsigned long stSize = sizeof(IP_ADAPTER_INFO);
    //調用GetAdaptersInfo函數,填充pIpAdapterInfo指針變量;其中stSize參數既是一個輸入量也是一個輸出量
    int nRel = GetAdaptersInfo(pIpAdapterInfo,&stSize);
    //記錄網卡數量
    int netCardNum = 0;
    //記錄每張網卡上的IP地址數量
    int IPnumPerNetCard = 0;
    if (ERROR_BUFFER_OVERFLOW == nRel)
    {
        //如果函數返回的是ERROR_BUFFER_OVERFLOW
        //則說明GetAdaptersInfo參數傳遞的內存空間不夠,同時其傳出stSize,表示需要的空間大小
        //這也是說明為什么stSize既是一個輸入量也是一個輸出量
        //釋放原來的內存空間
        delete pIpAdapterInfo;
        //重新申請內存空間用來存儲所有網卡信息
        pIpAdapterInfo = (PIP_ADAPTER_INFO)new BYTE[stSize];
        //再次調用GetAdaptersInfo函數,填充pIpAdapterInfo指針變量
        nRel=GetAdaptersInfo(pIpAdapterInfo,&stSize);    
    }
    if (ERROR_SUCCESS == nRel)
    {
        //輸出網卡信息
         //可能有多網卡,因此通過循環去判斷
		while (pIpAdapterInfo)
		{
			//可能網卡有多IP,因此通過循環去判斷
			IP_ADDR_STRING *pIpAddrString =&(pIpAdapterInfo->IpAddressList);
			IPnumPerNetCard = 0;
			do 
			{
				str = pIpAddrString->IpAddress.String;
				m_combobox_ip.AddString(str);

				pIpAddrString=pIpAddrString->Next;
			} while (pIpAddrString);
			pIpAdapterInfo = pIpAdapterInfo->Next;			
		}    
    }
    //釋放內存空間
    if (pIpAdapterInfo)
    {
        delete[] pIpAdapterInfo;
    }

	//從配置文件中抓取之前的IP
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	CIni cini;
	if(cini.Open(filepath, FALSE))
	{	
		if(cini.GetValue(TEXT("Server"), TEXT("Ip"), str))
		{
			m_combobox_ip.SetWindowTextW(str);			
		}
		if(cini.GetValue(TEXT("Server"), TEXT("Port"), str))
		{
			SetDlgItemText(IDC_EDIT_PORT, str);
		}
	}
}

VOID CConfigServer::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}



// CConfigServer message handlers


BOOL CConfigServer::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	Init();
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CConfigServer::OnBnClickedButtonSave()
{
	// TODO: Add your control notification handler code here
	CString str;
	UpdateData(TRUE);
	if(m_edit_port > 65535)
	{
		MessageBox(TEXT("請輸入正確的端口號：0 -- 65535"));
		return;
	}

	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	CIni cini;
	if(cini.Open(filepath, TRUE))
	{
		str.Format(TEXT("%d"), m_edit_port);
		if(!(cini.SetValue(TEXT("Server"), TEXT("Port"), str)))
		{
			MessageBox(TEXT("保存端口失敗！"));
		}
	}
	else
	{
		MessageBox(TEXT("打開config.ini失敗，保存端口失敗！"));
	}
}



void CConfigServer::OnBnClickedButtonSaveip()
{
	// TODO: Add your control notification handler code here
	CString str;
	m_combobox_ip.GetWindowTextW(str);
	int len = str.GetLength();

	if(str.IsEmpty())
	{
		MessageBox(TEXT("請選擇或輸入IP！"));
		return;
	}

	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	CIni cini;
	if(cini.Open(filepath, TRUE))
	{
		
		if(!(cini.SetValue(TEXT("Server"), TEXT("Ip"), str)))
		{
			MessageBox(TEXT("保存IP失敗！"));
		}
	}
	else
	{
		MessageBox(TEXT("打開config.ini失敗，保存IP失敗！"));
	}
}
