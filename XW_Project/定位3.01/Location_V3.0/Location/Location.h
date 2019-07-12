// Location.h : PROJECT_NAME 應用程序的主頭文件
//

#pragma once

#ifndef __AFXWIN_H__
	#error "在包含此文件之前包含“stdafx.h”以生成 PCH 文件"
#endif

#include "resource.h"		// 主符號


// CLocationApp:
// 有關此類的實現，請參閱 Location.cpp
//

class CLocationApp : public CWinApp
{
public:
	CLocationApp();

// 重寫
	public:
	virtual BOOL InitInstance();

// 實現

	DECLARE_MESSAGE_MAP()
};

extern CLocationApp theApp;