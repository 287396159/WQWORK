#ifndef _DEFINE_H__
#define _DEFINE_H__

#define	WM_MAP_UPDATE								WM_APP+1
#define	WM_DISPLAY_UPDATE							WM_APP+2
#define	WM_DISPLAY_SHOWTIMEOUTID_UPDATE				WM_APP+3
#define	WM_DISPLAY_SHOWTIMEOUTTIME_UPDATE			WM_APP+4
#define	WM_DISPLAY_AUTOEMERGENCY_UPDATE				WM_APP+5
#define	WM_DISPLAY_REFERENCEPOINT_UPDATE			WM_APP+6
#define WM_WARNING_SHOWEMERGENCY_UPDATE				WM_APP+7
#define WM_WARNING_SHOWSERVERNETTIMEOUT_UPDATE		WM_APP+8
#define WM_WARNING_SERVERNETTIMEOUTTIME_UPDATE		WM_APP+9
#define WM_WARNING_SHOWTAGTIMEOUT_UPDATE			WM_APP+10
#define WM_CLEAR_ALL_WARNING_MESSAGE				WM_APP+11
#define WM_DISPLAY_SHOWTAGNOMOVE_UPDATE				WM_APP+12
#define WM_DISPLAY_SHOWTAGNOMOVETIME_UPDATE			WM_APP+13
#define WM_DLG_PORTSHOW_CLOSE						WM_APP+14
#define WM_SETPORT_SAVE_LISTSHOW					WM_APP+15
#define WM_SETPORT_SAVE_IMAGESHOW					WM_APP+16
#define WM_SETTAG_SAVE_LISTSHOW						WM_APP+17
#define WM_SETTAG_SAVE_IMAGESHOW					WM_APP+18
#define WM_ONLINE_DLG_OFF							WM_APP+19
#define WM_WARNING_LOWBATTERYVALUE_UPDATE			WM_APP+20
#define WM_WARNING_SHOWLOWBATTERY_UPDATE			WM_APP+21
#define WM_SETNET_SAVE_IMAGESHOW					WM_APP+22
#define WM_DISPLAY_SHOWTAGCONTINUOUS_UPDATE			WM_APP+23
#define WM_DISPLAY_SHOWTAGCONTINUOUSNUMBER_UPDATE	WM_APP+24


#define	MAX_RECEIVE_DATA_LEN	2048

#define MAX_SAVE_TAG_COUNT		1000
#define MAX_SAVE_PORT_COUNT		1000
#define MAX_RECEIVE_COUNT		2000

struct ImageShowReceiveInfo
{
	BOOL ReceivData;			//是否接受到資料
	BYTE Type;					//定位信息類型
	BYTE TagId[2];			//卡片ID
	BYTE Port1Id[2];			//參考點ID
	BYTE Rssi1;				//相對距離
	BYTE Port2Id[2];			//參考點ID
	BYTE Rssi2;				//相對距離
	BYTE Port3Id[2];			//參考點ID
	BYTE Rssi3;				//相對距離
	BYTE index;					//本次資料的序列號	
	BYTE Battery;		//電池電量	
	BOOL IsUpdate;				//數據是否有更新，True為更新
	unsigned short SensorTime;
	DWORD FirstReceiveTime;		//第一次接受到數據的時間
	BYTE LastPortId[2];		//上一次該卡片挨近的位置參考點的ID
	DWORD SendTimeOut;		//每個卡片的數據發送間隔時間
	BOOL IsAladyWarringTimeOut;		//TRUE = 已經發送了超時的報警信息
	BOOL IsAladySaveData;			//TRUE = 已經保存到了文件中
	BYTE ChangePortId[2];		//上一次靠近的點的ID
	BYTE ChangePortCount;		//上一次靠近的點，連續出現的次數
	BYTE NearPortId[2];		//當前定位到的參考點ID
	BYTE SaveDataSecond;	//上一次存儲數據的時間秒，1秒存一次
};

struct ImageShowSaveTagInfo
{
	CString ID;
	CString Name;
	BYTE	Show;	//是否顯示在地圖上，TRUE = 顯示
};

struct ImageShowSavePortInfo
{
	CString ID;
	CString Name;	
};

struct SaveOldData	//保存歷史數據的結構體，歷史數據以天為單位建立文件夾，以小時為單位來建立文件
{
	BYTE  Minute;		//保存的分
	BYTE  Secondes;		//保存的秒
	BYTE  Type;			//定位類型 1 = 普通； 2 = 緊急
	BYTE  TagId[2];		//定位卡片ID
	BYTE  PortId[2];	//參考點ID
	BYTE  Rssi;			//相對距離
	BYTE  Index;		//接收數據的序列號
	BYTE  Battery;		//電池電量
	unsigned short SensorTime;	//Sensor未移動的時間
};

struct ReadSaveData
{
	short Year;
	BYTE Month;
	BYTE Day;
	BYTE Hour;
	SaveOldData *SaveData;
	int SaveDataCount;
	ReadSaveData *pNext;
};

#define	TONGJILEIXING_NODATA		0	//未收到定位數據
#define TONGJILEIXING_NORMAL		1	//普通定位，藍色
#define	TONGJILEIXING_WARRING		2	//緊急定位，紅色
#define	TONGJILEIXING_NOMOVE		3	//未移動，黑色
#define	TONGJILEIXING_LOST			4	//丟包，紫色
#define TONGJILEIXING_COUNT			5	//總共的個數


struct TongJiJieGuo
{
	BYTE TagId[2];		//以卡片的ID為單位來統計
	unsigned short *Data;	//每個數據的高13位代表參考點數組的序列號，低3位代表統計類型。以秒為單位，每秒儲存一次
	DWORD LastDataIndex;	//上一次有定位信息時的保存序列號
	BYTE LastType;		//上一次的類型
	BYTE LastReceIndex;	//上一次的接收序列號
};


#define		MAX_IMAGEPORT_COUNT		1000
#define	MAX_RECEIVE_DATA_LEN	2048



#define SHOW_TYPE_ALL			0	//顯示所有
#define SHOW_TYPE_EMERGENCY		1	//只顯示緊急定位的TAG
#define SHOW_TYPE_SINGLE		2	//只顯示單獨的一個TAG

#define SHOW_TAG_YES		1	//允許顯示TAG
#define SHOW_TAG_NO			0	//不允許顯示TAG
#define SHOW_TAG_UNKNOW		2	//授權未知，應該是保存的TAG列表中沒有這個ID

#define MAX_DRAWTAG			8	//從0 - 7分別對應時鐘的12點、1.5、3、4.5、6、7.5、9、10.5位置

struct ImagePortInfo
{
	BYTE	Id[2];
	int		x;
	int		y;
//	BYTE ImageAddr[8];	//位置參考點周圍的幾個畫卡片的點，是否有被占用，1-6分別對應6個點位，1-5的值：1 = 占用；0 = 未用；6的值：表示有多少個卡片的數量
	BYTE	DrawTagId[MAX_DRAWTAG][2];	//每個位置參考點周圍畫上的TAG的ID
	int		DrawTagCount;	//剩余參考點的顯示個數
	DWORD	LastReceData;	//最后一次收到數據的時間
};

#define TONGJI_DRAW_LEFT_PORT_WIDTH		120
#define TONGJI_DRAW_RIGHT_TEXT_WIDTH	100
#define TONGJI_DRAW_PORT_HEIGHT			30



#endif