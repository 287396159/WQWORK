// Location.cpp : 定義應用程序的類行為。
//

#include "stdafx.h"
#include "Location.h"
#include "LocationDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CLocationApp

BEGIN_MESSAGE_MAP(CLocationApp, CWinApp)
	ON_COMMAND(ID_HELP, &CWinApp::OnHelp)
END_MESSAGE_MAP()


// CLocationApp 構造

CLocationApp::CLocationApp()
{
	// TODO: 在此處添加構造代碼，
	// 將所有重要的初始化放置在 InitInstance 中
}


// 唯一的一個 CLocationApp 對象

CLocationApp theApp;


// CLocationApp 初始化

BOOL CLocationApp::InitInstance()
{
	// 如果一個運行在 Windows XP 上的應用程序清單指定要
	// 使用 ComCtl32.dll 版本 6 或更高版本來啟用可視化方式，
	//則需要 InitCommonControlsEx()。否則，將無法創建窗口。
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwSize = sizeof(InitCtrls);
	// 將它設置為包括所有要在應用程序中使用的
	// 公共控件類。
	InitCtrls.dwICC = ICC_WIN95_CLASSES;
	InitCommonControlsEx(&InitCtrls);

	CWinApp::InitInstance();

	AfxEnableControlContainer();

	// 標準初始化
	// 如果未使用這些功能并希望減小
	// 最終可執行文件的大小，則應移除下列
	// 不需要的特定初始化例程
	// 更改用于存儲設置的注冊表項
	// TODO: 應適當修改該字符串，
	// 例如修改為公司或組織名
	SetRegistryKey(_T("應用程序向導生成的本地應用程序"));

	CLocationDlg dlg;
	m_pMainWnd = &dlg;
	INT_PTR nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
		// TODO: 在此處放置處理何時用“確定”來關閉
		//  對話框的代碼
	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: 在此放置處理何時用“取消”來關閉
		//  對話框的代碼
	}

	// 由于對話框已關閉，所以將返回 FALSE 以便退出應用程序，
	//  而不是啟動應用程序的消息泵。
	return FALSE;
}
