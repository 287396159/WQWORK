// DataTypeConverter.cpp : ʵ���ļ�
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


//У��λ�ļ��㣬��У��λǰ���byte���ӵõ���index ���ӵ���ʼλ��һ��Ϊ0��lengthΪУ��λ��λ��
void CDataTypeConverter::getCheakCode(BYTE sendBt[100],int index,int length){
	for (int i = index; i < length; i++){
		sendBt[length] += sendBt[i];
	}
}


/////���� ��ȡ�豸���P������
void CDataTypeConverter::readDataDeal(BYTE packData) { 
	BYTE sendByte[7] = {0xff,0xfe,packData,0x00,0x00,0xfd,0xfc};
	//getCheakCode(sendByte,0,4);
	DWORD write;
	if(m_hcom != NULL) WriteFile(m_hcom, sendByte, sizeof(sendByte), &write, NULL);
}


/////���� �O���豸���P����
void CDataTypeConverter::setDataDeal(BYTE packData,char * wifiAccount,int length) { 
	BYTE sendByte[100] = {0xff,0xfe,packData};	//����ǰ3λ��2λ���^�͔������
	int j = 3;
	for (int i = 0; i < length*2; i++){
		if (wifiAccount[i] == '\0' && i != 0 && packData != 0x02 && packData != 0x04 && packData != 0x06 && packData != 0x08) continue;
		sendByte[j] = wifiAccount[i];
		j++;
	}	
	//getCheakCode(sendByte,0,length + 3);         //У�λ�O��
	sendByte[length + 3] = 0xfd;				
	sendByte[length + 4] = 0xfc;

	//byte���M���꣬�_ʼ���b����
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



/////���Ͷ�ȡ�豸ID������
void CDataTypeConverter::ReadCardSendData() { 
    //return 
		readDataDeal(0x01);
}


/////���� �������ݣ���������Ϊwifi�˺�
void CDataTypeConverter::setDrviserAcount(char * wifiAccount,int length) { 	
    //return 
		setDataDeal(0x0a,wifiAccount,length);
}


/////���� ��ȡ���ݣ���Ҫ��ȡwifi�˺�
void CDataTypeConverter::readDrviserAcount() { 	
    //return 
		readDataDeal(0x09);
}


/////���� �������ݣ���������Ϊwifi����
void CDataTypeConverter::setDrviserPassWord(char * wifiAccount,int length) { 
	//return 
		setDataDeal(0x0c,wifiAccount,length);
}


/////���� ��ȡ���ݣ���Ҫ��ȡwifi����
void CDataTypeConverter::readDrviserPassWord() { 
	//return 
		readDataDeal(0x0b);
}


/////���� �������ݣ���������Ϊ����IP
void CDataTypeConverter::setServiseIP(char * wifiAccount,int length) { 
	//return 
		setDataDeal(0x06,wifiAccount,length);
}


/////���� ��ȡ���ݣ���Ҫ��ȡ����IP
void CDataTypeConverter::readServiseIP() { 
	//return 
		readDataDeal(0x05);
}


/////���� �������ݣ���������Ϊ����˿�
void CDataTypeConverter::setServisePort(char * wifiAccount,int length) { 
	//return 
		setDataDeal(0x08,wifiAccount,length);
}


/////���� ��ȡ���ݣ���Ҫ��ȡ����˿�
void CDataTypeConverter::readServisePort() { 
	//return 
		readDataDeal(0x07);
}


//FF FE 00 00 00 FD FC 
void CDataTypeConverter::ReadDrivaceType(){
    //return 
		readDataDeal(0x00);
}


///�O�ñ��CID
void CDataTypeConverter::setCardIDSendData(BYTE id[2]){
	char wifiAccount[2];
	wifiAccount[0] = id[0];
	wifiAccount[1] = id[1];
    //return 
		setDataDeal(0x02,wifiAccount,2);
}


///�xȡ�ψ�ID�r�g
void CDataTypeConverter::ReadUpTimeData(){
    //return 
		readDataDeal(0x03);
}


///�O���O���ψ�ID�r�g
void CDataTypeConverter::setUpTimeData(int upTime){
	char wifiAccount[2]= {upTime/256,upTime%256};
    //return 
		setDataDeal(0x04,wifiAccount,2);
}