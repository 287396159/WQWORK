// SetNewTwoDialog.cpp : 实现文件
//

#include "stdafx.h"
#include "SetNewTwoDialog.h"
#include "SetID.h"
#include "afxdialogex.h"



// CSetNewTwoDialog 对话框

IMPLEMENT_DYNAMIC(CSetNewTwoDialog, CDialogEx)

CSetNewTwoDialog::CSetNewTwoDialog(CWnd* pParent /*=NULL*/)
	: CDialogEx(CSetNewTwoDialog::IDD, pParent)
{
}

CSetNewTwoDialog::~CSetNewTwoDialog()
{
}

void CSetNewTwoDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT5, m_com_num);
	DDX_Control(pDX, IDC_BUTTON1, m_openOrCloceBtn);
	DDX_Control(pDX, IDC_IPADDRESS2, m_ipaddress_setIp);
	DDX_Control(pDX, IDC_IPADDRESS1, m_get_serviseIp4);
	m_bConnect = false;
}


BEGIN_MESSAGE_MAP(CSetNewTwoDialog, CDialogEx)
	ON_BN_CLICKED(IDC_BUTTON1, &CSetNewTwoDialog::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON_CONNECTDrivace, &CSetNewTwoDialog::OnBnClickedButtonConnectdrivace)
	ON_BN_CLICKED(IDC_BUTTON_getID, &CSetNewTwoDialog::OnBnClickedButtongetid)
	ON_BN_CLICKED(IDC_BUTTON_checkDrivaceTime, &CSetNewTwoDialog::OnBnClickedButtoncheckdrivacetime)
	ON_BN_CLICKED(IDC_BUTTON_readWifiCount, &CSetNewTwoDialog::OnBnClickedButtonreadwificount)
	ON_BN_CLICKED(IDC_BUTTON_readWifiPassword, &CSetNewTwoDialog::OnBnClickedButtonreadwifipassword)
	ON_BN_CLICKED(IDC_BUTTON_readIPadress, &CSetNewTwoDialog::OnBnClickedButtonreadipadress)
	ON_BN_CLICKED(IDC_BUTTON_readPort, &CSetNewTwoDialog::OnBnClickedButtonreadport)
	ON_BN_CLICKED(IDC_BUTTON_setID, &CSetNewTwoDialog::OnBnClickedButtonsetid)
	ON_BN_CLICKED(IDC_BUTTON_setDrivaceIDTime, &CSetNewTwoDialog::OnBnClickedButtonsetdrivaceidtime)
	ON_BN_CLICKED(IDC_BUTTON_setWifiCount, &CSetNewTwoDialog::OnBnClickedButtonsetwificount)
	ON_BN_CLICKED(IDC_BUTTON_setWifiPassword, &CSetNewTwoDialog::OnBnClickedButtonsetwifipassword)
	ON_BN_CLICKED(IDC_BUTTON_setIpAdress, &CSetNewTwoDialog::OnBnClickedButtonsetipadress)
	ON_BN_CLICKED(IDC_BUTTON_setPort, &CSetNewTwoDialog::OnBnClickedButtonsetport)
END_MESSAGE_MAP()
DWORD ComReadThread_newTwo(LPVOID lparam)
{	
	DWORD	actualReadLen=0;	//实际读取的字节数
	DWORD	willReadLen;	
	
	DWORD dwReadErrors;
	COMSTAT	cmState;
	
	CSetNewTwoDialog *pdlg;
	pdlg = (CSetNewTwoDialog *)lparam;

	pdlg->m_Receive_Data_Len = 0;
		
	// 清空缓冲，并检查串口是否打开。
	ASSERT(pdlg->m_hcom != INVALID_HANDLE_VALUE); 	
	//清空串口
//	PurgeComm(pdlg->m_hcom, PURGE_RXCLEAR | PURGE_TXCLEAR );	
	SetCommMask (pdlg->m_hcom, EV_RXCHAR | EV_CTS | EV_DSR);
	while(pdlg->m_hcom != NULL && pdlg->m_hcom != INVALID_HANDLE_VALUE){ 			
		ClearCommError(pdlg->m_hcom,&dwReadErrors,&cmState);
		willReadLen = cmState.cbInQue ;
		if (willReadLen <= 0){
			Sleep(10);
			continue;
		}			
		if(willReadLen + pdlg->m_Receive_Data_Len > MAX_RECEIVE_DATA_LEN)
			willReadLen = MAX_RECEIVE_DATA_LEN - pdlg->m_Receive_Data_Len;

		ReadFile(pdlg->m_hcom, pdlg->m_Receive_Data_Char + pdlg->m_Receive_Data_Len, willReadLen, &actualReadLen, 0);
		if (actualReadLen <= 0 && pdlg->m_Receive_Data_Len == 0){
			Sleep(10);
			continue;
		}
		pdlg->m_Receive_Data_Len += actualReadLen;		
		pdlg->ParseData();
	}
	return 0;
}


// CSetNewTwoDialog 消息处理程序
int m_servisePort_intValue = 0;

void CSetNewTwoDialog::OnBnClickedButton1(){
	if(m_bConnect){
		m_bConnect = FALSE;
		if(m_hcom != NULL)CloseHandle(m_hcom);
		m_hcom = NULL;
		m_openOrCloceBtn.SetWindowTextW(_T("連接設備"));//GetWindowTextW("打开");
		return;
	}

	CString str;
	m_com_num.GetWindowText(str);
	int m_servisePort_intValue2 = _wtoi(str);
	if (m_servisePort_intValue2 != m_servisePort_intValue && m_servisePort_intValue != 0)
	{
		if(OpenCom(_T("com")+str)){
			m_openOrCloceBtn.SetWindowTextW(_T("關閉設備"));	
			m_bConnect = TRUE;
			DTCter.ReadDrivaceType();
			return;
		}
	}
	
	for (int i = m_servisePort_intValue+1; i < 100; i++){		
		CString str2;
		str2.Format(_T("com%d"),i);
		//str2 = "com"+;
		if(OpenCom(str2)) {
			m_openOrCloceBtn.SetWindowTextW(_T("關閉設備"));	
			m_bConnect = TRUE;						
			m_servisePort_intValue = i;
			CString str3;
			str3.Format(_T("%d"),i);
			m_com_num.SetWindowTextW(str3);
			DTCter.ReadDrivaceType();
			return;
		}
		m_servisePort_intValue = 0;
	}
}


BOOL CSetNewTwoDialog::OpenCom(CString str_com)
{	
	str_com = TEXT("\\\\.\\") + str_com;
	m_hcom = CreateFile(str_com, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
	///配置串口
	if(m_hcom == INVALID_HANDLE_VALUE && m_hcom != NULL)
	{	
		return FALSE;
	}

	DCB  dcb;    
	dcb.DCBlength = sizeof(DCB); 
	// 默认串口参数
	GetCommState(m_hcom, &dcb);

	dcb.BaudRate = 115200;					// 设置波特率 
	dcb.fBinary = TRUE;						// 设置二进制模式，此处必须设置TRUE
	dcb.fParity = TRUE;						// 支持奇偶校验 
	dcb.ByteSize = 8;						// 数据位,范围:4-8 
	dcb.Parity = NOPARITY;					// 校验模式
	dcb.StopBits = ONESTOPBIT;				// 停止位 0,1,2 = 1, 1.5, 2

	dcb.fOutxCtsFlow = FALSE;				// No CTS output flow control 
	dcb.fOutxDsrFlow = FALSE;				// No DSR output flow control 
	dcb.fDtrControl = DTR_CONTROL_ENABLE; 
	// DTR flow control type 
	dcb.fDsrSensitivity = FALSE;			// DSR sensitivity 
	dcb.fTXContinueOnXoff = TRUE;			// XOFF continues Tx 
	dcb.fOutX = FALSE;					// No XON/XOFF out flow control 
	dcb.fInX = FALSE;						// No XON/XOFF in flow control 
	dcb.fErrorChar = FALSE;				// Disable error replacement 
	dcb.fNull = FALSE;					// Disable null stripping 
	dcb.fRtsControl = RTS_CONTROL_ENABLE; 
	// RTS flow control 
	dcb.fAbortOnError = FALSE;			// 当串口发生错误，并不终止串口读写


	if (!SetCommState(m_hcom, &dcb))
	{
		///L"配置串口失败";			
		return FALSE;
	}
	////配置超时值
	COMMTIMEOUTS  cto;
	GetCommTimeouts(m_hcom, &cto);
	cto.ReadIntervalTimeout = MAXDWORD;  
	cto.ReadTotalTimeoutMultiplier = 10;  
	cto.ReadTotalTimeoutConstant = 10;    
	cto.WriteTotalTimeoutMultiplier = 50;  
	cto.WriteTotalTimeoutConstant = 100;    
	if (!SetCommTimeouts(m_hcom, &cto))
	{
		///L"不能设置超时参数";		
		return FALSE;
	}	

	//指定端口监测的事件集
	SetCommMask (m_hcom, EV_RXCHAR);
		
	//分配设备缓冲区
//	SetupComm(m_hcom,8192,8192);

	//初始化缓冲区中的信息
	PurgeComm(m_hcom,PURGE_TXCLEAR|PURGE_RXCLEAR);

	DTCter.m_hcom = m_hcom;
	HANDLE m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)ComReadThread_newTwo, this, 0, NULL);
	CloseHandle(m_hThread);

	return TRUE;
}


void CSetNewTwoDialog::ParseData(void){
	dealReveData(m_Receive_Data_Char,m_Receive_Data_Len);
	m_Receive_Data_Len = 0;
}


void CSetNewTwoDialog::OnBnClickedButtonConnectdrivace(){
	// TODO: 在此添加控件通知处理程序代码
	CStatic *pStatic=(CStatic*)GetDlgItem(IDC_STATIC_connect);
	pStatic->SetWindowText(_T("未與設備連接"));
	DTCter.ReadDrivaceType();
	
}




void CSetNewTwoDialog::OnBnClickedButtongetid(){	
	DTCter.ReadCardSendData();	
}


void CSetNewTwoDialog::OnBnClickedButtoncheckdrivacetime()
{
	DTCter.ReadUpTimeData();
}


void CSetNewTwoDialog::OnBnClickedButtonreadwificount()
{
	DTCter.readDrviserAcount();
	//m_ctrlComm.put_Output(DTCter.readDrviserAcount());//发送数据
}


void CSetNewTwoDialog::OnBnClickedButtonreadwifipassword()
{
	DTCter.readDrviserPassWord();
}


void CSetNewTwoDialog::OnBnClickedButtonreadipadress()
{
	DTCter.readServiseIP();
	//m_ctrlComm.put_Output(DTCter.readServiseIP());//发送数据
}


void CSetNewTwoDialog::OnBnClickedButtonreadport()
{
	DTCter.readServisePort();
}


void CSetNewTwoDialog::OnBnClickedButtonsetid()//設置設備ID
{
	byte  lp[2]; 
	CEdit* pBoxOne;
    pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_setId1);	
	CString str;
    //pBoxOne-> GetWindowText(str,2);
	pBoxOne->GetWindowTextW(str);  
	if (str.GetLength() <=0)
	{
		return;
	}
	lp[0] = getByteHex(str);	

	CEdit* pBoxOne2;
    pBoxOne2 = (CEdit*) GetDlgItem(IDC_EDIT_setId2);	
	CString str2;
    pBoxOne2-> GetWindowText(str2);
	if (str2.GetLength() <=0)
	{
		return;
	}
	lp[1] = getByteHex(str2);
	DTCter.setCardIDSendData(lp);
}

BYTE CSetNewTwoDialog::getByteHex(CString str){
	char pstr[2];

	for (int i = 0; i < 2; i++){
		//pstr[i] = str[i];
		if (str[i] > 47 && str[i] < 58){
			pstr[i] = str[i] - 48;
		}
		if (str[i] > 64 && str[i] < 71){
			pstr[i] = str[i] - 65 +10;
		}
		if (str[i] > 96 && str[i] < 103){
			pstr[i] = str[i] - 97 +10;
		}
	}
	BYTE bt = 0;
	if (str.GetLength() == 1)
	{
		bt = pstr[0];
	}else if (str.GetLength() == 2)
	{
		bt = (pstr[0]*16)+pstr[1];
	}
	
	return bt;
}

void CSetNewTwoDialog::OnBnClickedButtonsetdrivaceidtime()
{
	CEdit* pBoxOne;
    pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_setIdTime);	
	CString str;
    pBoxOne-> GetWindowText(str);
	int upTime_int = _wtoi(str);
	if(upTime_int > 65535) {
		MessageBox(_T("取值範圍：0~65535"),_T("錯誤"),MB_OK);  
		return;
	}
	DTCter.setUpTimeData(upTime_int);
}


void CSetNewTwoDialog::OnBnClickedButtonsetwificount()
{
	CEdit* pBoxOne;
    pBoxOne = (CEdit*) GetDlgItem(IDC_EDITsetWifiCount);	
	CString str;
    pBoxOne-> GetWindowText(str);
	int length = str.GetLength();
	if(length > 20 || length < 2) {
		MessageBox(_T("輸入賬號長度大於20或者小於2"),_T("錯誤"),MB_OK);  
		return;
	}
	char*  lp;     
	lp = (char*)str.GetBuffer(length);   
	DTCter.setDrviserAcount(lp,length);
}


void CSetNewTwoDialog::OnBnClickedButtonsetwifipassword()
{
	CEdit* pBoxOne;
    pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_setWifiPassword);	
	CString str;
    pBoxOne-> GetWindowText(str);
	int length = str.GetLength();
	if(length > 20 || length < 2) {
		MessageBox(_T("輸入賬號長度大於20或者小於2"),_T("錯誤"),MB_OK);  
		return;
	}
	char*  lp;     
	lp = (char*)str.GetBuffer(length);   
	DTCter.setDrviserPassWord(lp,length);
}


void CSetNewTwoDialog::OnBnClickedButtonsetipadress()
{
	BYTE m_ServerIp[4];
	m_ipaddress_setIp.GetAddress(m_ServerIp[0],m_ServerIp[1],m_ServerIp[2],m_ServerIp[3]);
	char  lp[4]; 
	for (int i = 0; i < 4; i++){
		lp[i] = m_ServerIp[i];
	}
	DTCter.setServiseIP(lp,4);
	//m_ctrlComm.put_Output(DTCter.setServiseIP(lp,4));
}


void CSetNewTwoDialog::OnBnClickedButtonsetport(){
	char  lp[2]; 
	CEdit* pBoxOne;
    pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_setPort);	
	CString str;
    pBoxOne-> GetWindowText(str);
	int m_servisePort_intValue = _wtoi(str);

	if(m_servisePort_intValue > 65535) {
		MessageBox(_T("取值範圍：0~65535"),_T("錯誤"),MB_OK);  
		return;
	}

	lp[0] = m_servisePort_intValue/256;
	lp[1] = m_servisePort_intValue%256;
	DTCter.setServisePort(lp,2);

	//m_ctrlComm.put_Output(DTCter.setServisePort(lp,2));
}



void CSetNewTwoDialog::dealReveData(BYTE data[1024],long len){
	if (data[0] != 0xfb && data[1] != 0xfa && data[len - 2] != 0xf9 && data[len - 1] != 0xf8){
		return;
	}
	CString str;
	m_com_num.GetWindowText(str);
	int m_servisePort_intValue2 = _wtoi(str);
	m_servisePort_intValue = m_servisePort_intValue2 -1;

	CEdit* pBoxOne;    
	CString msg;
	switch (data[2]){
	case 0x80:
		if (data[3] == 0x05 && data[4] == 0x00){
			CStatic *pStatic=(CStatic*)GetDlgItem(IDC_STATIC_connect);
			pStatic->SetWindowText(_T("成功連接設備"));
		}
		break;
	case 0x81:
		if (len < 7) break;
		msg.Format(_T("%02X"),data[3]);
		pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_getId1);	
		pBoxOne->SetWindowTextW(msg);
		CEdit* pBoxOne2;    
		msg.Format(_T("%02X"),data[4]);
		pBoxOne2 = (CEdit*) GetDlgItem(IDC_EDIT_getId2);	
		pBoxOne2->SetWindowTextW(msg);	

		break;	
	case 0x83:
		if (len != 7) break;
		int upTime2[2];
		upTime2[0] = data[3];
		upTime2[1] = data[4];
		pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_checkIdTime);	
		int upTime_int;
		upTime_int = upTime2[0]*256 + upTime2[1];
		msg.Format(_T("%d"),upTime_int);
		pBoxOne->SetWindowTextW(msg);
		break;
	case 0x85:
		if (len < 9) break;
		BYTE ip4Bt[4];
		for (int i = 3; i < 7; i++){
			ip4Bt[i-3] = data[i];
		}
		m_get_serviseIp4.SetAddress(ip4Bt[0],ip4Bt[1],ip4Bt[2],ip4Bt[3]);
		break;
	case 0x87:
		if (len != 7)break;
		int port2[2];
		port2[0] = data[3];
		port2[1] = data[4];

		pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_readPort);	
		int port_int;
		port_int = port2[0]*256 + port2[1];
		msg.Format(_T("%d"),port_int);
		pBoxOne->SetWindowTextW(msg);
		break;
	case 0x89:
		if (len < 6) break;
		for (int i = 3; i < len - 2; i++){
			char dt = data[i];
			msg += dt;
		}
		pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_readWifiCount);	
		pBoxOne->SetWindowTextW(msg);

		break;
	case 0x8b:
		if (len < 6) break;
		for (int i = 3; i < len - 2; i++){
			char dt = data[i];
			msg += dt;
		}
		pBoxOne = (CEdit*) GetDlgItem(IDC_EDIT_readPassWord);	
		pBoxOne->SetWindowTextW(msg);
		break;
	default:
		break;
	}
}

BEGIN_EVENTSINK_MAP(CSetNewTwoDialog, CDialogEx)
	END_EVENTSINK_MAP()
