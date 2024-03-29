﻿// SetTag.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "SetTag.h"
#include "ini.h"
#include <shlwapi.h>
#include "define.h"


// CSetTag dialog

IMPLEMENT_DYNAMIC(CSetTag, CDialog)

CSetTag::CSetTag(CWnd* pParent /*=NULL*/)
	: CDialog(CSetTag::IDD, pParent)
{

}

CSetTag::~CSetTag()
{
}

void CSetTag::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);	
	DDX_Control(pDX, IDC_BUTTON1, m_button_refresh);
	DDX_Control(pDX, IDC_BUTTON2, m_button_save);
	DDX_Control(pDX, IDC_BUTTON3, m_button_del);
	DDX_Control(pDX, IDC_LIST2, m_list);
	DDX_Control(pDX, IDC_BUTTON4, m_button_add);
	DDX_Control(pDX, IDC_EDIT1, m_edit_ID);
	DDX_Control(pDX, IDC_EDIT2, m_edit_name);
}


BEGIN_MESSAGE_MAP(CSetTag, CDialog)
	
	ON_WM_SIZE()
	ON_BN_CLICKED(IDC_BUTTON1, &CSetTag::OnBnClickedButton1)
	ON_NOTIFY(NM_CLICK, IDC_LIST2, &CSetTag::OnNMClickList2)
	ON_BN_CLICKED(IDC_BUTTON5, &CSetTag::OnBnClickedButton5)
	ON_NOTIFY(NM_CUSTOMDRAW, IDC_LIST2, &CSetTag::OnNMCustomdrawList2)
	ON_BN_CLICKED(IDC_BUTTON4, &CSetTag::OnBnClickedButton4)
	ON_BN_CLICKED(IDC_BUTTON3, &CSetTag::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BUTTON2, &CSetTag::OnBnClickedButton2)
	ON_NOTIFY(LVN_COLUMNCLICK, IDC_LIST2, &CSetTag::OnLvnColumnclickList2)
END_MESSAGE_MAP()



BOOL CSetTag::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	DWORD dwStyle = m_list.GetExtendedStyle();
	m_list.SetExtendedStyle(dwStyle | LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_HEADERDRAGDROP);
	
	m_list.InsertColumn(0, TEXT("卡片ID"), LVCFMT_LEFT, 80);
	m_list.InsertColumn(1, TEXT("卡片名稱"), LVCFMT_LEFT, 150);


	InitLoadTagInfo();

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CSetTag::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here
	CRect rs; 
	this->GetClientRect(&rs);
	rs.left += 10;
	rs.top += 50;
	rs.right = rs.left + 360;
	rs.bottom -= 10;
	::MoveWindow(m_list.GetSafeHwnd(), rs.left, rs.top, rs.Width(), rs.Height(), TRUE);
}

void CSetTag::GetModulePath(LPTSTR path, LPCTSTR module)
{
	TCHAR* s;
	HANDLE Handle = NULL;
	if(module)
		Handle = GetModuleHandle(module);
	GetModuleFileName((HMODULE)Handle, path, MAX_PATH);
	s = _tcsrchr(path, '\\');
	if(s) s[1] = 0;
}

void CSetTag::InitLoadTagInfo()
{
	m_list.DeleteAllItems();

	CIni	m_ini;
	WCHAR szPath[MAX_PATH] = {0};
	GetModulePath(szPath, NULL);
	CString filepath;
	filepath = szPath;
	filepath = filepath + TEXT("config\\tag.ini");	
	if(!m_ini.Open(filepath, FALSE))
		return;

	CString str, str_value;
	int tagcount = 0;

	str.Format(TEXT("%d"), tagcount);
	while(m_ini.GetValue(str, TEXT("ID"), str_value))
	{		
		m_list.InsertItem(tagcount, str_value);
		if(m_ini.GetValue(str, TEXT("Addr"), str_value))
		{
			m_list.SetItemText(tagcount, 1, str_value);
		}
		tagcount++;
		str.Format(TEXT("%d"), tagcount);   
	}	
}

void CSetTag::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here
	InitLoadTagInfo();
}

BOOL CSetTag::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN)
	{
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
	}
	return CDialog::PreTranslateMessage(pMsg);
}

void CSetTag::OnNMClickList2(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: Add your control notification handler code here
	if(pResult)
		*pResult = 0;

	POSITION ps;
	int nIndex;

	ps=m_list.GetFirstSelectedItemPosition();
	if(ps == NULL)
		return;
	nIndex=m_list.GetNextSelectedItem(ps);
	//TODO:添加多選的操作。
	if(nIndex >= 0)
	{
		CString str;
		str = m_list.GetItemText(nIndex, 0);
		m_edit_ID.SetWindowTextW(str);
		str = m_list.GetItemText(nIndex, 1);
		m_edit_name.SetWindowTextW(str);	
	}
	
}

BOOL CSetTag::CheckIdOk(CString strid)
{
	char c;
	if(strid.GetLength() != 4)
		return FALSE;
	for(int i=0; i<4; i++)
	{
		c = strid.GetAt(i);
		if((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F'))
			;		//ok
		else
			return FALSE;		
	}
	return TRUE;
}

void CSetTag::OnBnClickedButton5()
{
	// TODO: Add your control notification handler code here
	POSITION ps;
	int nIndex;
	ps=m_list.GetFirstSelectedItemPosition();
	nIndex=m_list.GetNextSelectedItem(ps);
	//TODO:添加多選的操作。
	if(nIndex >= 0)
	{
		//找到選中行，修改選中行的內容
		CString str;
		m_edit_ID.GetWindowTextW(str);
		if(!CheckIdOk(str))
		{
			MessageBox(TEXT("請輸入4位十六進制的ID號，例如：0011，AABB"));
			return;
		}
		m_list.SetItemText(nIndex, 0, str);

		m_edit_name.GetWindowTextW(str);
		m_list.SetItemText(nIndex, 1, str);
	}
}

void CSetTag::OnNMCustomdrawList2(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMCUSTOMDRAW pNMCD = reinterpret_cast<LPNMCUSTOMDRAW>(pNMHDR);
	// TODO: Add your control notification handler code here
	*pResult = 0;

	NMLVCUSTOMDRAW *pLVCD = reinterpret_cast<NMLVCUSTOMDRAW*>(pNMHDR);
	
	switch(pLVCD->nmcd.dwDrawStage)
	{
	case CDDS_PREPAINT:
		*pResult = CDRF_NOTIFYITEMDRAW;          // ask for item otifications.
		break;

	case CDDS_ITEMPREPAINT:
		*pResult = CDRF_NOTIFYSUBITEMDRAW;
		break;

	case CDDS_ITEMPREPAINT | CDDS_SUBITEM:
		{
			int nItem = static_cast<int>(pLVCD->nmcd.dwItemSpec);// nItem表示item的index
			if(m_list.GetItemState(nItem, LVIS_SELECTED) == LVIS_SELECTED )// 這里加入判斷是否被選中的代碼
			{
				pLVCD->clrText = RGB(255, 255, 255);
				pLVCD->clrTextBk = RGB(0, 0, 255); // cr1和cr2分別為高亮顯示時的文本和背景色，自己查一下
			}
			*pResult = CDRF_DODEFAULT;
		}
		break;

	default:
		*pResult = CDRF_DODEFAULT;
	}

}

void CSetTag::OnBnClickedButton4()
{
	// TODO: Add your control notification handler code here
	POSITION ps;
	int nIndex;
	ps=m_list.GetFirstSelectedItemPosition();
	nIndex=m_list.GetNextSelectedItem(ps);
	//TODO:添加多選的操作。
	while(nIndex >= 0)
	{
		m_list.SetItemState(nIndex, 0, LVIS_SELECTED);
		nIndex=m_list.GetNextSelectedItem(ps);
	}

	int count = m_list.GetItemCount();
	m_list.InsertItem(count, TEXT("0000"));
	m_list.SetItemState(count, LVIS_SELECTED, LVIS_SELECTED);
	//滾動到最后一行顯示
	m_list.SendMessage(WM_VSCROLL, SB_BOTTOM, NULL);
	m_list.RedrawWindow();
	OnNMClickList2(NULL, NULL);
}

void CSetTag::OnBnClickedButton3()
{
	// TODO: Add your control notification handler code here
	POSITION ps;
	int nIndex;
	ps=m_list.GetFirstSelectedItemPosition();
	
	//TODO:添加多選的操作。
	while(ps)
	{
		nIndex=m_list.GetNextSelectedItem(ps);
		m_list.DeleteItem(nIndex);
		ps = m_list.GetFirstSelectedItemPosition();
	}

}

void CSetTag::OnBnClickedButton2()
{
	// TODO: Add your control notification handler code here

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
	//創建文件
	CFile	m_cFile;
	CString	m_strFile;
	filepath += TEXT("\\tag.ini");
	if(m_cFile.Open(filepath, CFile::modeCreate|CFile::modeWrite))
	{			
		//先保存2個字節在文檔前面，文檔會被當作Unicode編碼文件處理
		BYTE buf[2] = {0xFF, 0xFE};
		m_cFile.Write(buf, 2);
		//以ini的格式來保存列表
		int count = m_list.GetItemCount();
		int len;
		for(int i=0; i<count; i++)
		{
			m_strFile.Format(TEXT("[%d]"), i);
			m_strFile += TEXT("\r\n");
			m_strFile += TEXT("ID=");
			m_strFile += m_list.GetItemText(i, 0);
			m_strFile += TEXT("\r\n");
			m_strFile += TEXT("Addr=");
			m_strFile += m_list.GetItemText(i, 1);
			m_strFile += TEXT("\r\n");
			len = m_strFile.GetLength();
			m_cFile.Write(m_strFile.GetBuffer(len), len*2);
			m_strFile.ReleaseBuffer();
		}
		m_cFile.Close();

		::PostMessage(m_hwnd_ListShow, WM_SETTAG_SAVE_LISTSHOW, 0, 0);
		::PostMessage(m_hwnd_ImageShow, WM_SETTAG_SAVE_IMAGESHOW, 0, 0);
		return ;
	}
	else
	{
		MessageBox(TEXT("創建文件：") + filepath + TEXT("失敗！無法保存！"));
		return;
	}

}

static int CALLBACK MyCompareProc(LPARAM lParam1, LPARAM lParam2, LPARAM lParamSort)
{
	CString &lp1 = *((CString *)lParam1);
	CString &lp2 = *((CString *)lParam2);
	int &sort = *(int *)lParamSort;
	if (sort == 0)
	{
		return lp1.CompareNoCase(lp2);
	}
	else
	{
		return lp2.CompareNoCase(lp1);
	}
}

void CSetTag::OnLvnColumnclickList2(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLISTVIEW pNMLV = reinterpret_cast<LPNMLISTVIEW>(pNMHDR);
	// TODO: Add your control notification handler code here
	*pResult = 0;

	int Length = m_list.GetItemCount();
	CArray<CString,CString> ItemData;
	ItemData.SetSize(Length);
	for (int i = 0; i < Length; i++)
	{
		ItemData[i] = m_list.GetItemText(i,pNMLV->iSubItem);
		m_list.SetItemData(i,(DWORD_PTR)&ItemData[i]);//設置排序關鍵字
	}
	static int sort = 0;
	static int SubItem = 0;
	if (SubItem != pNMLV->iSubItem)
	{
		sort = 0;
		SubItem = pNMLV->iSubItem;
	}
	else
	{
		if (sort == 0)
		{
			sort = 1;
		}
		else
		{
			sort = 0;
		}
	}
	m_list.SortItems(MyCompareProc,(DWORD_PTR)&sort);//排序
}

void CSetTag::SetListShowHwnd(HWND hwnd)
{
	m_hwnd_ListShow = hwnd;
}
void CSetTag::SetImageShowHwnd(HWND hwnd)
{
	m_hwnd_ImageShow = hwnd;
}