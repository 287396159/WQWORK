#include "StdAfx.h"
#include "Ini.h"

CIni::CIni()
{
	m_strFile = TEXT("");
	m_FilePath = TEXT("");
//	m_hFile = INVALID_HANDLE_VALUE;
}

CIni::~CIni()
{
}

/*******************************************************
打開文件
File：文件路徑和名稱
Create：當該文件不存在時，是否創建該文件，TRUE：創建
返回：讀取文件或創建成功返回TRUE
*******************************************************/
BOOL CIni::Open(CString File, BOOL Create)
{
	m_FilePath = File;
	WIN32_FIND_DATA wfd;
	HANDLE hFile=NULL;
	hFile=FindFirstFile(File,&wfd);
	if((hFile == INVALID_HANDLE_VALUE || hFile == NULL) && Create)
	{
		if(m_cFile.Open(File, CFile::modeCreate|CFile::modeWrite))
		{			
			int size;
			m_strFile = TEXT("");
			BYTE buf[2] = {0xFF, 0xFE};
			m_cFile.Write(buf, 2);
			m_cFile.Close();
			m_cFile.Open(File, CFile::modeRead);			
			size = 1;
			m_cFile.Read(m_strFile.GetBuffer(size), 2);
			m_strFile.ReleaseBuffer(size);		
			m_cFile.Close();
			return TRUE;
		}
	//	m_hFile = CreateFile(File, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ, NULL, CREATE_NEW, FILE_ATTRIBUTE_NORMAL, NULL);
	}
	if(hFile != INVALID_HANDLE_VALUE && hFile != NULL)
	{		
	//	m_hFile = CreateFile(File, GENERIC_READ|GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

		if(m_cFile.Open(File, CFile::modeReadWrite))
		{
			int size = (m_cFile.GetLength())/2;
			m_cFile.Read(m_strFile.GetBuffer(size), m_cFile.GetLength());
			m_strFile.ReleaseBuffer(size);				
			m_cFile.Close();
		//	RETAILMSG(1, (TEXT("CIni:%s\r\n"), m_strFile));
			return TRUE;
		}
		else
			return FALSE;
	}
	return FALSE;
}

/***************************************************
獲取所有的段名
str：返回段名
返回值：獲取成功返回TRUE
***************************************************/
BOOL CIni::GetAllSegment(CStringArray &str_array)
{
	Open(m_FilePath);
	str_array.RemoveAll();
	CString str;
	int start = -1;
	int end = -1;
	str = m_strFile;
	start = str.Find(_T("["));
	while(start >= 0)
	{
		str = str.Right(str.GetLength() - start-1);
		end = str.Find(_T("]"));
		str_array.Add(str.Left(end));
		start = str.Find(_T("["));
	}
	return TRUE;
}

/**********************************************
獲取指定段的全部鍵名
segment：段名
str_key：鍵名
返回值：獲取成功返回TRUE，獲取失敗（沒有該段）返回FALSE
**********************************************/
BOOL CIni::GetAllKey(CString segment, CStringArray &str_key)
{
	Open(m_FilePath);
	str_key.RemoveAll();
	CString str;
	str = m_strFile;
	CString str_segment;
	str_segment = _T("[") + segment + _T("]");
	int seg = str.Find(str_segment);
	if(seg != -1)
	{
		str = str.Right(str.GetLength()-seg);
		seg = str.Find(_T("\r\n"));
		if( seg != -1)
		{
			str = str.Right(str.GetLength()-seg-2);
			seg = str.Find(_T("["));
			if(seg != -1)
			{
				str = str.Left(seg);
			}
			if(str.IsEmpty())
				return FALSE;
			seg = str.Find(_T("="));
			while(seg != -1)
			{
				while(str[0] == _T('\r') || str[0] == _T('\n'))
				{
					str = str.Right(str.GetLength() - 1);
				}
				seg = str.Find(_T("="));
				str_key.Add(str.Left(seg));
				seg = str.Find(_T("\r\n"));
				str = str.Right(str.GetLength()-seg-2);
				seg = str.Find(_T("="));
			}
			return TRUE;
		}
	}
	else
	{
		return FALSE;
	}
	return FALSE;
}

/**********************************************
獲取指定的鍵值
segment：段名
key：鍵名
valude：鍵值
返回值：獲取成功返回TRUE，獲取失敗（沒有該鍵）返回FALSE
**********************************************/
BOOL CIni::GetValue(CString segment, CString key, CString &value)
{
	Open(m_FilePath);
	CString str_segment;
	CString str_key;
	
	CStringArray key_array;
	if(GetAllKey(segment, key_array))
	{
		for(int i=0; i<key_array.GetCount(); i++)
		{
			if(key.Compare(key_array.GetAt(i)) == 0)
			{
				str_segment = _T("[") + segment + _T("]");
				str_key = key + _T("=");
				CString str;
				str = m_strFile.Right(m_strFile.GetLength() - m_strFile.Find(str_segment));
				str = str.Right(str.GetLength() - str.Find(str_key) - str_key.GetLength());
				value = str.Left(str.Find(_T("\r\n")));
				return TRUE;
			}
		}
		return FALSE;
	}
	else
	{
		return FALSE;
	}
	return FALSE;
}

/**********************************************
設定指定的鍵值
segment：段名
key：鍵名
valude：鍵值
返回值：設定成功返回TRUE
**********************************************/
BOOL CIni::SetValue(CString segment, CString key, CString value)
{
	CString str_segment;
	CString str_key;	
	CStringArray key_array;
	str_segment = TEXT("[") + segment + TEXT("]");
	str_key = key + TEXT("=");

	if(GetAllKey(segment, key_array))
	{
		//找到段
		//判斷是否存在鍵
		int i=0;
		for(i=0; i<key_array.GetCount(); i++)
		{
			if(key.Compare(key_array.GetAt(i)) == 0)
			{
				//找到鍵，修改鍵值
				int index;
				CString str_left, str_right;
				index = m_strFile.Find(str_segment);
				index = m_strFile.Find(str_key, index);
				str_left = m_strFile.Left(index);
				index = m_strFile.Find(_T("\r\n"), index);
				str_right = m_strFile.Right(m_strFile.GetLength() - index);
				m_strFile = str_left + str_key + value + str_right;
				break;
			}
		}
		if(i >= key_array.GetCount())
		{
			//有段，但是無鍵，添加鍵
			int index;
			CString str_left, str_right;
			index = m_strFile.Find(str_segment);
			index = m_strFile.Find(_T("\r\n"), index);
			index += 2;
			str_left = m_strFile.Left(index);			
			str_right = m_strFile.Right(m_strFile.GetLength() - index);
			m_strFile = str_left + str_key + value + TEXT("\r\n") + str_right;
		}
	}
	else
	{
		//沒有段，添加段和鍵
		m_strFile = m_strFile + str_segment + TEXT("\r\n");
		m_strFile = m_strFile + str_key + value + TEXT("\r\n");		
		
	}
	//保存
	if(m_cFile.Open(m_FilePath, CFile::modeCreate|CFile::modeWrite))
	{	
		int len = m_strFile.GetLength();
		m_cFile.Write(m_strFile.GetBuffer(len), len*2);
		m_strFile.ReleaseBuffer();
		m_cFile.Close();
		return TRUE;
	}
	return FALSE;
}