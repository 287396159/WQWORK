// onfigMap.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "onfigMap.h"
#include <shlwapi.h>

// ConfigMap dialog

IMPLEMENT_DYNAMIC(ConfigMap, CDialog)

ConfigMap::ConfigMap(CWnd* pParent /*=NULL*/)
	: CDialog(ConfigMap::IDD, pParent)
{

}

ConfigMap::~ConfigMap()
{
}

void ConfigMap::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT_MAP, m_edit_filepath);
}


BEGIN_MESSAGE_MAP(ConfigMap, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_OPEN, &ConfigMap::OnBnClickedButtonOpen)
	ON_WM_PAINT()
END_MESSAGE_MAP()


// ConfigMap message handlers

BOOL ConfigMap::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here

	m_edit_filepath.SetWindowTextW(TEXT(""));
	m_ImageFile = TEXT("");

	//從配置文件中抓取之前的路徑
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\Config.ini");	
	CIni cini;
	if(cini.Open(filepath, FALSE))
	{	
		CString str;
		if(cini.GetValue(TEXT("Map"), TEXT("FilePath"), str))
		{
			m_edit_filepath.SetWindowTextW(str);
			m_ImageFile = str;
		}
	}

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

VOID ConfigMap::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}



void ConfigMap::OnBnClickedButtonOpen()
{
	// TODO: Add your control notification handler code here
	CString str_file;
	CFileDialog dlg( TRUE, _T("bmp"), NULL, OFN_FILEMUSTEXIST|OFN_HIDEREADONLY, _T( "bmp|*.bmp||" ) );
	if(dlg.DoModal() == IDOK)
	{
		str_file = dlg.GetPathName();	

		WCHAR szPath[MAX_PATH] = {0};
		GetModulePath(szPath, NULL);
		CString filepath;
		filepath = szPath;		
		filepath = filepath + TEXT("config\\Config.ini");	
		CIni cini;
		if(cini.Open(filepath, TRUE))
		{
			
			if(!(cini.SetValue(TEXT("Map"), TEXT("FilePath"), str_file)))
			{
				MessageBox(TEXT("保存地圖失敗！"));
			}
		}
		else
		{
			MessageBox(TEXT("打開config.ini失敗，保存地圖失敗！"));
		}
		m_edit_filepath.SetWindowTextW(str_file);
		m_ImageFile = str_file;
		RedrawWindow();
		::PostMessage(m_MainWindowHwnd, WM_MAP_UPDATE, 0, 0);
	}
}

BOOL ConfigMap::MyLoadImage()
{
	if(m_ImageFile.IsEmpty())
		return FALSE;

	HBITMAP hBitmap = (HBITMAP)::LoadImage(NULL, m_ImageFile, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE|LR_CREATEDIBSECTION|LR_DEFAULTSIZE);
	if(hBitmap == NULL)
	{		
		return FALSE;
	}
	BITMAP bm;
    GetObject( hBitmap, sizeof(BITMAP), &bm);
	int width = bm.bmWidth;
	int height = bm.bmHeight;
	//計算圖片等比例縮放
	int left = 0;
	int top = 0;

	int m_ImageLeft = 10;
	int	m_ImageTop = 70;
	int	m_ImageWidth = 425;
	int	m_ImageHeight = 355;

	if(width < m_ImageWidth && height < m_ImageHeight)
	{
		//以寬為標準來放大
		if((height*m_ImageWidth)/width > m_ImageHeight)
		{
			//放大后的高度超出畫框，那么以高為標準來放大
			width = width * m_ImageHeight/height;
			m_ImageLeft = m_ImageLeft + (m_ImageWidth - width)/2;
			m_ImageWidth = width;
		}
		else
		{
			//放大后的高度不超出畫框，符合條件
			height = height * m_ImageWidth/width;
			m_ImageTop = m_ImageTop + (m_ImageHeight - height)/2;
			m_ImageHeight = height;
		}
	}
	else if(width >= m_ImageWidth && height < m_ImageHeight)
	{
		//以寬為標準來縮小
		height = height * m_ImageWidth/width;
		m_ImageTop = m_ImageTop + (m_ImageHeight - height)/2;
		m_ImageHeight = height;
	}
	else if(width < m_ImageWidth && height >= m_ImageHeight)
	{
		//以高位標準來縮小
		width = width * m_ImageHeight/height;
		m_ImageLeft = m_ImageLeft + (m_ImageWidth - width)/2;
		m_ImageWidth = width;
	}
	else	//寬和高都大于畫框
	{
		//以寬來縮小試探
		if((height*m_ImageWidth)/width > m_ImageHeight)
		{
			//縮小后的高，大于畫框，換以高位標準縮小
			width = width * m_ImageHeight/height;
			m_ImageLeft = m_ImageLeft + (m_ImageWidth - width)/2;
			m_ImageWidth = width;
		}
		else
		{
			//以寬為標準來縮小
			height = height * m_ImageWidth/width;
			m_ImageTop = m_ImageTop + (m_ImageHeight - height)/2;
			m_ImageHeight = height;
		}
	}
	HDC	m_hdc = ::GetDC(this->GetSafeHwnd());
	HDC m_mdc = CreateCompatibleDC(m_hdc);
	HDC m_ndc = CreateCompatibleDC(m_hdc);

	HBITMAP m_bmp_mdc = CreateCompatibleBitmap(m_hdc, m_ImageWidth, m_ImageHeight);
	HBITMAP oldndc = (HBITMAP)SelectObject(m_ndc, hBitmap);
	HBITMAP oldmdc = (HBITMAP)SelectObject(m_mdc, m_bmp_mdc);
	StretchBlt(m_mdc, 0, 0, m_ImageWidth, m_ImageHeight, m_ndc, 0, 0, bm.bmWidth, bm.bmHeight, SRCCOPY);
	
	BitBlt(m_hdc, m_ImageLeft, m_ImageTop, m_ImageWidth, m_ImageHeight, m_mdc, 0, 0, SRCCOPY);
	SelectObject(m_ndc, oldndc);
	SelectObject(m_mdc, oldmdc);
	DeleteObject(m_mdc);
	DeleteObject(m_ndc);
	DeleteObject(m_bmp_mdc);
	::ReleaseDC(this->GetSafeHwnd(), m_hdc);

	return TRUE;
}


void ConfigMap::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	// TODO: Add your message handler code here
	MyLoadImage();
	// Do not call CDialog::OnPaint() for painting messages
}
