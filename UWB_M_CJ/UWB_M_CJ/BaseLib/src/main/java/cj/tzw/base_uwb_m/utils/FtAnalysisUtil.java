package cj.tzw.base_uwb_m.utils;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.HashMap;

import cj.tzw.base_uwb_m.model.Device;

public class FtAnalysisUtil {

    public static boolean isCorrectData(byte[] dataBytes){
        if(dataBytes!=null && dataBytes.length>2 && dataBytes[0]==(byte)0xF1 && dataBytes[dataBytes.length-1]==(byte)0x1F ){
            return true;
        }
        return false;
    }

    /**
     * 解析搜索到的设备数据
     * @param dataBytes
     * @return
     */
    public static ArrayList<Device> analysisDeviceListData(byte[] dataBytes){
        ArrayList<Device> deviceList = null;
        if(isCorrectData(dataBytes)){
            deviceList = new ArrayList<>();
            int deviceCount = dataBytes[2];
            int index = 3;
            for(int i=0;i<deviceCount;i++){
                byte[] idBytes = new byte[2];
                idBytes[0] = dataBytes[index];
                idBytes[1] = dataBytes[index+1];
                byte type = dataBytes[index+2];
                byte[] fVertion = new byte[4];
                fVertion[0] = dataBytes[index+3];
                fVertion[1] = dataBytes[index+4];
                fVertion[2] = dataBytes[index+5];
                fVertion[3] = dataBytes[index+6];
                byte hVertion = dataBytes[index+7];
                Device device = new Device();
                device.setAlerterID(idBytes);
                device.setType(type);
                device.setFirmware_version(ByteUtil.bytesToHexFun3(fVertion));
                device.sethVersion(hVertion+"");
                deviceList.add(device);
                index = index + 8;
            }
        }
        return deviceList;

    }


    public static HashMap<String,byte[]> analysisData(byte[] dataBytes){
        HashMap<String,byte[]> retMap = null;
        if(isCorrectData(dataBytes)){
            retMap = new HashMap<>();
            byte order = dataBytes[1];
            int startIndex = 5;
            int endIndex = dataBytes.length-2;
            int dataLen = endIndex-startIndex;
            byte[] retBytes = new byte[dataLen];
            System.arraycopy(dataBytes,startIndex,retBytes,0,dataLen);
            switch (order){
            //********************SET********************
                case (byte)0x40:
                    retMap.put("fork_id_set",retBytes);
                    break;
                case (byte)0x47:
                    retMap.put("tag_alarm_en_set",retBytes);
                    break;
                case (byte)0x49:
                    retMap.put("tag_alarm_range_set",retBytes);
                    break;
                case (byte)0x56:
                    retMap.put("tag_safe_range_set",retBytes);
                    break;
                case (byte)0x58:
                    retMap.put("tag_safe_range_extend_set",retBytes);
                    break;
                case (byte)0x4B:
                    retMap.put("forklift_alarm_en_fork_set",retBytes);
                    break;
                case (byte)0x4D:
                    retMap.put("forklift_alarm_range_fork_set",retBytes);
                    break;
                case (byte)0x4F:
                    retMap.put("fix_alarm_en_set",retBytes);
                    break;
                case (byte)0x51:
                    retMap.put("low_freq_distance_set",retBytes);
                    break;
                case (byte)0x53:
                    retMap.put("pan_id_fork_set",retBytes);
                    break;
                case (byte)0xFD:
                    retMap.put("led_alarm_en_set",retBytes);
                    break;
                case (byte)0xFB:
                    retMap.put("sound_alarm_en_set",retBytes);
                    break;
                case (byte)0xF9:
                    retMap.put("sound_volume_set",retBytes);
                    break;
                case (byte)0xF7:
                    retMap.put("sound_alarm_mode_set",retBytes);
                    break;
                case (byte)0xF5:
                    retMap.put("fixed_mode_ontime_set",retBytes);
                    break;
                case (byte)0xF3:
                    retMap.put("fixed_mode_offtime_set",retBytes);
                    break;
                case (byte)0x80:
                    retMap.put("fix_id_set",retBytes);
                    break;
                case (byte)0x85:
                    retMap.put("forklift_alarm_en_fix_set",retBytes);
                    break;
                case (byte)0x87:
                    retMap.put("forklift_alarm_range_fix_set",retBytes);
                    break;
                case (byte)0x89:
                    retMap.put("pan_id_fix_set",retBytes);
                    break;
            //********************READ********************
                case (byte)0x41:
                    retMap.put("reset_forklift_alerter",retBytes);
                    break;
                case (byte)0x81:
                    retMap.put("reset_fix_alerter",retBytes);
                    break;
                case (byte)0x46:
                    retMap.put("tag_alarm_en_read",retBytes);
                    break;
                case (byte)0x48:
                    retMap.put("tag_alarm_range_read",retBytes);
                    break;
                case (byte)0x55:
                    retMap.put("tag_safe_range_read",retBytes);
                    break;
                case (byte)0x57:
                    retMap.put("tag_safe_range_extend_read",retBytes);
                    break;
                case (byte)0x4A:
                    retMap.put("forklift_alarm_en_fork_read",retBytes);
                    break;
                case (byte)0x4C:
                    retMap.put("forklift_alarm_range_fork_read",retBytes);
                    break;
                case (byte)0x4E:
                    retMap.put("fix_alarm_en_read",retBytes);
                    break;
                case (byte)0x50:
                    retMap.put("low_freq_distance_read",retBytes);
                    break;
                case (byte)0x52:
                    retMap.put("pan_id_fork_read",retBytes);
                    break;
                case (byte)0x54:
                    retMap.put("pan_id_fork_reset",retBytes);
                    break;
                case (byte)0xFE:
                    retMap.put("led_alarm_en_read",retBytes);
                    break;
                case (byte)0xFC:
                    retMap.put("sound_alarm_en_read",retBytes);
                    break;
                case (byte)0xFA:
                    retMap.put("sound_volume_read",retBytes);
                    break;
                case (byte)0xF8:
                    retMap.put("sound_alarm_mode_read",retBytes);
                    break;
                case (byte)0xF6:
                    retMap.put("fixed_mode_ontime_read",retBytes);
                    break;
                case (byte)0xF4:
                    retMap.put("fixed_mode_offtime_read",retBytes);
                    break;
                case (byte)0x84:
                    retMap.put("forklift_alarm_en_fix_read",retBytes);
                    break;
                case (byte)0x86:
                    retMap.put("forklift_alarm_range_fix_read",retBytes);
                    break;
                case (byte)0x88:
                    retMap.put("pan_id_fix_read",retBytes);
                    break;
                case (byte)0x8A:
                    retMap.put("pan_id_fix_reset",retBytes);
                    break;

            }
        }
        return retMap;
    }


}
