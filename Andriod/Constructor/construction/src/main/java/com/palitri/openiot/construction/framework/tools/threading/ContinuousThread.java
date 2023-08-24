package com.palitri.openiot.construction.framework.tools.threading;

public class ContinuousThread extends Thread {
    private volatile boolean running;
    private volatile boolean done;

    public volatile int interval;

    public ContinuousThread() {
        this.interval = 50;
    }

    @Override
    public void run() {
        this.running = true;
        this.done = false;

        try {
            while (this.running && this.action()) {
                Thread.sleep(this.interval);
            }
        }
        catch (InterruptedException ex) {
            return;
        }

        this.done = true;
    }

    public void terminate(boolean wait) {
        this.running = false;

        while (wait && !this.done)
            try {
                Thread.sleep(this.interval);
            }
            catch (InterruptedException ex) {
                return;
            }

    }

    public boolean action()
    {
        return false;
    }
}
