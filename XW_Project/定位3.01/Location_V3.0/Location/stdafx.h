// stdafx.h : 標準系統包含文件的包含文件，
// 或是經常使用但不常更改的
// 特定于項目的包含文件

#pragma once

#ifndef _SECURE_ATL
#define _SECURE_ATL 1
#endif

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN		// 從 Windows 頭中排除極少使用的資料
#endif

// 如果您必須使用下列所指定的平臺之前的平臺，則修改下面的定義。
// 有關不同平臺的相應值的最新信息，請參考 MSDN。
#ifndef WINVER				// 允許使用特定于 Windows XP 或更高版本的功能。
#define WINVER 0x0501		// 將此值更改為相應的值，以適用于 Windows 的其他版本。
#endif

#ifndef _WIN32_WINNT		// 允許使用特定于 Windows XP 或更高版本的功能。
#define _WIN32_WINNT 0x0501	// 將此值更改為相應的值，以適用于 Windows 的其他版本。
#endif						

#ifndef _WIN32_WINDOWS		// 允許使用特定于 Windows 98 或更高版本的功能。
#define _WIN32_WINDOWS 0x0410 // 將它更改為適合 Windows Me 或更高版本的相應值。
#endif

#ifndef _WIN32_IE			// 允許使用特定于 IE 6.0 或更高版本的功能。
#define _WIN32_IE 0x0600	// 將此值更改為相應的值，以適用于 IE 的其他版本。值。
#endif

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// 某些 CString 構造函數將是顯式的

// 關閉 MFC 對某些常見但經常可放心忽略的警告消息的隱藏
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC 核心組件和標準組件
#include <afxext.h>         // MFC 擴展


#include <afxdisp.h>        // MFC 自動化類



#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>		// MFC 對 Internet Explorer 4 公共控件的支持
#endif
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC 對 Windows 公共控件的支持
#endif // _AFX_NO_AFXCMN_SUPPORT









#ifdef _UNICODE
#if defined _M_IX86
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='x86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_IA64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='ia64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_X64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='amd64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#else
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif
#endif


