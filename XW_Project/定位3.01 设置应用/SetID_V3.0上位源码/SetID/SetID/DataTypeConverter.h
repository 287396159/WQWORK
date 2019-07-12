#pragma once


// CDataTypeConverter

class CDataTypeConverter : public CWnd{
	DECLARE_DYNAMIC(CDataTypeConverter)

public:
	CDataTypeConverter();
	virtual ~CDataTypeConverter();

	CString Dec2Hex(unsigned int intDec);
	void HexM2OleVariant(CString strHexM);
	HANDLE	m_hcom;

	void getCheakCode(BYTE sendBt[100],int index,int length);
	//CByteArray forGetValue(BYTE sendBts[100],int index,int length);

	void ReadDrivaceType();
	void ReadCardSendData();
	void setCardIDSendData(BYTE id[2]);
	void ReadUpTimeData();
	void setUpTimeData(int upTime);
	void setDrviserAcount(char wifiAccount[100],int length);
	void readDrviserAcount();
	void setDrviserPassWord(char * wifiAccount,int length);
	void readDrviserPassWord();
	void setServiseIP(char * wifiAccount,int length);
	void readServiseIP();
	void setServisePort(char * wifiAccount,int length);
	void readServisePort();
	void readDataDeal(BYTE packData);
	void setDataDeal(BYTE packData,char * wifiAccount,int length);
	

protected:
	DECLARE_MESSAGE_MAP()
};


