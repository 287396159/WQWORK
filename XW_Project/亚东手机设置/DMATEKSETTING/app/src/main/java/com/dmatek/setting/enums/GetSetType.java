package com.dmatek.setting.enums;

public enum GetSetType {

    /**
     * 空的，什么都不是
     */
    Nothing,
    /**
     * serverIp
     */
    SERVER_IP,
    SERVER_PORT,
    WIFI_NAME,
    WIFI_PASSWORD,
    WIFI_MODEL,
    WIFI_IP,
    NODE_MODEL,
    NODE_IP,
    /**
     * subMask,子網掩碼
     */
    SUBMASK,
    /**
     * gateWay,
     */
    GATEWAY,
    RESERT, // 复位
    WIFI_RESSI, // wifi信号强度
    WIFI_LAST_CONNECTION_TYPE, // WiFi最后一次连接状态

    CKD_THRESHOLD,
    CKD_K1,


    CARD_UPTIME,
    CARD_POWER,
    CARD_STATICSLEEPTIME,
    CARD_SENSITIVITY,
}
