#ifndef MY_INI_H
#define MY_INI_H

class CIni
{
public:
	CIni();
	~CIni();
	//打開文件
	BOOL Open(CString File, BOOL Create = TRUE); 
	BOOL GetAllSegment(CStringArray &str);
	BOOL GetAllKey(CString segment, CStringArray &str_key);
	BOOL GetValue(CString segment, CString key, CString &value);
	BOOL SetValue(CString segment, CString key, CString value);
public:
//	HANDLE m_hFile;
	CFile	m_cFile;
	CString m_strFile;
	CString m_FilePath;
};

#endif MY_INI_H