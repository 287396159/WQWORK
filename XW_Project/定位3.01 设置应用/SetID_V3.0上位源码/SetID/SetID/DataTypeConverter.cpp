// DataTypeConverter.cpp : 实现文件
//

#include "stdafx.h"
#include "DataTypeConverter.h"


// CDataTypeConverter

IMPLEMENT_DYNAMIC(CDataTypeConverter, CWnd)

CDataTypeConverter::CDataTypeConverter()
{

}


CDataTypeConverter::~CDataTypeConverter()
{
}


BEGIN_MESSAGE_MAP(CDataTypeConverter, CWnd)
END_MESSAGE_MAP()


//校验位的计算，将校验位前面的byte叠加得到，index 叠加的起始位，一般为0，length为校验位的位置
void CDataTypeConverter::getCheakCode(BYTE sendBt[100],int index,int length){
	for (int i = index; i < length; i++){
		sendBt[length] += sendBt[i];
	}
}


/////发送 读取设备相關的数据
void CDataTypeConverter::readDataDeal(BYTE packData) { 
	BYTE sendByte[7] = {0xff,0xfe,packData,0x00,0x00,0xfd,0xfc};
	//getCheakCode(sendByte,0,4);
	DWORD write;
	if(m_hcom != NULL) WriteFile(m_hcom, sendByte, sizeof(sendByte), &write, NULL);
}


/////发送 設置设备相關数据
void CDataTypeConverter::setDataDeal(BYTE packData,char * wifiAccount,int length) { 
	BYTE sendByte[100] = {0xff,0xfe,packData};	//數據前3位，2位包頭和數據類型
	int j = 3;
	for (int i = 0; i < length*2; i++){
		if (wifiAccount[i] == '\0' && i != 0 && packData != 0x02 && packData != 0x04 && packData != 0x06 && packData != 0x08) continue;
		sendByte[j] = wifiAccount[i];
		j++;
	}	
	//getCheakCode(sendByte,0,length + 3);         //校驗位設置
	sendByte[length + 3] = 0xfd;				
	sendByte[length + 4] = 0xfc;

	//byte數組建完，開始封裝數據
	DWORD write;
	if(m_hcom != NULL) WriteFile(m_hcom, sendByte, length+5, &write, NULL);

	CByteArray cbTe ;							
	for (int i = 0; i < length+5; i++){
		cbTe.Add(sendByte[i]);
	}
	//delete sendByte;
	//delete wifiAccount;
    //return cbTe;
}


CString CDataTypeConverter::Dec2Hex(unsigned int intDec){
    CString strHex;
    char charHex[255];
    sprintf(charHex,"%x",intDec);
    strHex=charHex;
    if(strHex.GetLength()==1)
        strHex=_T("0")+strHex;
    return strHex;
}



/////发送读取设备ID的数据
void CDataTypeConverter::ReadCardSendData() { 
    //return 
		readDataDeal(0x01);
}


/////发送 设置数据，数据内容为wifi账号
void CDataTypeConverter::setDrviserAcount(char * wifiAccount,int length) { 	
    //return 
		setDataDeal(0x0a,wifiAccount,length);
}


/////发送 读取数据，需要读取wifi账号
void CDataTypeConverter::readDrviserAcount() { 	
    //return 
		readDataDeal(0x09);
}


/////发送 设置数据，数据内容为wifi密码
void CDataTypeConverter::setDrviserPassWord(char * wifiAccount,int length) { 
	//return 
		setDataDeal(0x0c,wifiAccount,length);
}


/////发送 读取数据，需要读取wifi密码
void CDataTypeConverter::readDrviserPassWord() { 
	//return 
		readDataDeal(0x0b);
}


/////发送 设置数据，数据内容为服务IP
void CDataTypeConverter::setServiseIP(char * wifiAccount,int length) { 
	//return 
		setDataDeal(0x06,wifiAccount,length);
}


/////发送 读取数据，需要读取服务IP
void CDataTypeConverter::readServiseIP() { 
	//return 
		readDataDeal(0x05);
}


/////发送 设置数据，数据内容为服务端口
void CDataTypeConverter::setServisePort(char * wifiAccount,int length) { 
	//return 
		setDataDeal(0x08,wifiAccount,length);
}


/////发送 读取数据，需要读取服务端口
void CDataTypeConverter::readServisePort() { 
	//return 
		readDataDeal(0x07);
}


//FF FE 00 00 00 FD FC 
void CDataTypeConverter::ReadDrivaceType(){
    //return 
		readDataDeal(0x00);
}


///設置本機ID
void CDataTypeConverter::setCardIDSendData(BYTE id[2]){
	char wifiAccount[2];
	wifiAccount[0] = id[0];
	wifiAccount[1] = id[1];
    //return 
		setDataDeal(0x02,wifiAccount,2);
}


///讀取上報ID時間
void CDataTypeConverter::ReadUpTimeData(){
    //return 
		readDataDeal(0x03);
}


///設置設備上報ID時間
void CDataTypeConverter::setUpTimeData(int upTime){
	char wifiAccount[2]= {upTime/256,upTime%256};
    //return 
		setDataDeal(0x04,wifiAccount,2);
}