// SetImagePort.cpp : implementation file
//

#include "stdafx.h"
#include "Location.h"
#include "SetImagePort.h"
#include "ImageShow.h"


// CSetImagePort dialog

IMPLEMENT_DYNAMIC(CSetImagePort, CDialog)

CSetImagePort::CSetImagePort(CWnd* pParent /*=NULL*/)
	: CDialog(CSetImagePort::IDD, pParent)
{

}

CSetImagePort::~CSetImagePort()
{
}

void CSetImagePort::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT_ID1, m_edit_id1);
	DDX_Control(pDX, IDC_ID2, m_edit_id2);
	DDX_Control(pDX, IDC_BUTTON_SETID, m_button_setid);
	DDX_Control(pDX, IDC_BUTTON_DELETE, m_button_delete);
	DDX_Control(pDX, IDC_BUTTON_BACK, m_button_back);
}


BEGIN_MESSAGE_MAP(CSetImagePort, CDialog)
	ON_BN_CLICKED(IDC_BUTTON_SETID, &CSetImagePort::OnBnClickedButtonSetid)
	ON_BN_CLICKED(IDC_BUTTON_DELETE, &CSetImagePort::OnBnClickedButtonDelete)
	ON_BN_CLICKED(IDC_BUTTON_BACK, &CSetImagePort::OnBnClickedButtonBack)
END_MESSAGE_MAP()


// CSetImagePort message handlers

BOOL CSetImagePort::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	m_button_setid.SetWindowTextW(TEXT("設置 ID"));
	m_button_delete.SetWindowTextW(TEXT("刪  除"));
	m_button_back.SetWindowTextW(TEXT("返  回"));
	this->SetWindowTextW(TEXT("設置位置參考點"));

	CString str;
	str.Format(TEXT("%02X"), m_id1);
	m_edit_id1.SetWindowTextW(str);
	str.Format(TEXT("%02X"), m_id2);
	m_edit_id2.SetWindowTextW(str);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CSetImagePort::SetParent(CImageShow *p)
{
	pf = p;
}

void CSetImagePort::SetID(BYTE id1, BYTE id2)
{
	m_id1 = id1;
	m_id2 = id2;	
}
void CSetImagePort::OnBnClickedButtonSetid()
{
	// TODO: Add your control notification handler code here
	CString panid1, panid2;
	BYTE id[2];
	m_edit_id1.GetWindowTextW(panid1);
	m_edit_id2.GetWindowTextW(panid2);
	if(panid1.GetLength() != 2 || panid2.GetLength() != 2)
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!StringToChar(panid1, id))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(!StringToChar(panid2, id+1))
	{
		CString str;
		str = TEXT("請輸入2位十六進制的數字，例如：19 9B");
		MessageBox(str);
		return;
	}
	if(id[0] == 0 && id[1] == 0)
	{
		CString str;
		str = TEXT("不能設置參考位置的ID號為：00 00");
		MessageBox(str);
		return;
	}
	pf->SetImagePort(id[0], id[1]);
	OnBnClickedButtonBack();
}

bool CSetImagePort::StringToChar(CString str, BYTE* data)
{
	int strlen = str.GetLength();
	int datalen = 0;
	int value;
	for(int i=0; i<strlen; i++)
	{
		if(str.GetAt(i) == ' ')
			continue;
		switch(str.GetAt(i))
		{
		case '0': value = 0x0; break;
		case '1': value = 0x1; break;
		case '2': value = 0x2; break;
		case '3': value = 0x3; break;
		case '4': value = 0x4; break;
		case '5': value = 0x5; break;
		case '6': value = 0x6; break;
		case '7': value = 0x7; break;
		case '8': value = 0x8; break;
		case '9': value = 0x9; break;
		case 'a':
		case 'A': value = 0xA; break;
		case 'b':
		case 'B': value = 0xB; break;
		case 'c':
		case 'C': value = 0xC; break;
		case 'd':
		case 'D': value = 0xD; break;
		case 'e':
		case 'E': value = 0xE; break;
		case 'f':
		case 'F': value = 0xF; break;	
		default: return false; 
		}
		data[datalen] = (char) (value<<4);
		if(i+1 >= strlen)
			return false;
		switch(str.GetAt(i+1))
		{
		case '0': value = 0x0; break;
		case '1': value = 0x1; break;
		case '2': value = 0x2; break;
		case '3': value = 0x3; break;
		case '4': value = 0x4; break;
		case '5': value = 0x5; break;
		case '6': value = 0x6; break;
		case '7': value = 0x7; break;
		case '8': value = 0x8; break;
		case '9': value = 0x9; break;
		case 'a':
		case 'A': value = 0xA; break;
		case 'b':
		case 'B': value = 0xB; break;
		case 'c':
		case 'C': value = 0xC; break;
		case 'd':
		case 'D': value = 0xD; break;
		case 'e':
		case 'E': value = 0xE; break;
		case 'f':
		case 'F': value = 0xF; break;	
		default: return false; 
		}
		data[datalen] = (char) (data[datalen] + value);
		return true;			
	}		
	return false;
}

void CSetImagePort::OnBnClickedButtonDelete()
{
	// TODO: Add your control notification handler code here
	pf->DeleteImagePort();
	OnCancel();
}

void CSetImagePort::OnBnClickedButtonBack()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}
