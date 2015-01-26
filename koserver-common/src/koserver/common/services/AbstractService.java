package koserver.common.services;

import koserver.common.services.events.ServiceEvent;

public abstract class AbstractService implements ServiceEvent {

    private boolean started;

    public AbstractService() {
    }

    public final void start() {
        if (this.started) {
            return;
        }

        this.onInit();
        this.started = true;
    }

    public final void stop() {
        if (!this.started) {
            return;
        }

        this.onDestroy();
        this.started = false;
    }

    public final void restart() {
        this.stop();
        this.start();
    }
}
