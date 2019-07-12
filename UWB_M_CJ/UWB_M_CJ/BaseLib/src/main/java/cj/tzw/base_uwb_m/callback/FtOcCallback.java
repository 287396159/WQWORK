package cj.tzw.base_uwb_m.callback;

public interface FtOcCallback {

    void ftOpened();

    void ftOpenFail(String failMsg);

    void ftClosed();

    void ftSended();

    void ftSendFail(String failMsg);

    void ftRecevied(byte[] receviedBytes);

    void ftRecevieOutTime();

}
