package koserver.game.services;

import java.util.Collections;
import java.util.Iterator;
import java.util.Map;
import java.util.Map.Entry;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.TimeUnit;

import javolution.util.FastMap;
import koserver.common.services.AbstractService;
import koserver.game.configuration.GlobalConfig;
import koserver.game.tasks.AbstractTask;
import koserver.game.tasks.TaskTypeEnum;

import org.apache.log4j.Logger;

public class ThreadPoolService extends AbstractService {

    /** LOGGER */
    private static Logger log = Logger.getLogger(ThreadPoolService.class.getName());

    private final ScheduledExecutorService executor = Executors.newScheduledThreadPool(GlobalConfig.GLOBAL_THREADPOOL_POOL_SIZE);
    private final Map<AbstractTask<?>, ScheduledFuture<?>> tasks = Collections.synchronizedMap(new FastMap<AbstractTask<?>, ScheduledFuture<?>>());

    private ThreadPoolService() {
    }

    @Override
    public void onInit() {
        log.info("ThreadPoolService started");
    }

    @Override
    public void onDestroy() {
        log.info("ThreadPoolService stopped");
    }

    public void scheduleTask(final AbstractTask<?> task, final long delay, final TimeUnit timeUnit) {
        final Runnable runnable = new Runnable() {
            @Override
            public void run() {
                task.execute();
                task.cancel();
            }
        };

        synchronized (tasks) {
            tasks.put(task, executor.schedule(runnable, delay, timeUnit));
        }
    }

    public void scheduleRepeatableTask(final AbstractTask<?> task, final long initialDelay, final long delay, final TimeUnit timeUnit) {
        final Runnable runnable = new Runnable() {
            @Override
            public void run() {
                task.execute();
            }
        };

        synchronized (tasks) {
            tasks.put(task, executor.scheduleAtFixedRate(runnable, initialDelay, delay, timeUnit));
        }
    }

    public Future<?> scheduleAtFixedRate(final Runnable runnable, final long initialDelay, final long period, final TimeUnit timeUnit) {
        return executor.scheduleAtFixedRate(runnable, initialDelay, period, timeUnit);
    }

    public void cancelTask(final Object linkedObject, final TaskTypeEnum taskType) {
        synchronized (tasks) {
            final Iterator<Entry<AbstractTask<?>, ScheduledFuture<?>>> iterator = this.tasks.entrySet().iterator();
            while (iterator.hasNext()) {
                final Entry<AbstractTask<?>, ScheduledFuture<?>> entry = iterator.next();
                if (entry.getKey().getLinkedObject().equals(linkedObject) && entry.getKey().getTaskType() == taskType) {
                    entry.getValue().cancel(true);
                    iterator.remove();
                }
            }
        }
    }

    public void cancelAllTasksByLinkedObject(final Object linkedObject) {
        final Iterator<Entry<AbstractTask<?>, ScheduledFuture<?>>> iterator = this.tasks.entrySet().iterator();
        while (iterator.hasNext()) {
            final Entry<AbstractTask<?>, ScheduledFuture<?>> entry = iterator.next();
            if (entry.getKey().getLinkedObject().equals(linkedObject)) {
                entry.getValue().cancel(true);
                iterator.remove();
            }
        }
    }

    /** SINGLETON */
    public static ThreadPoolService getInstance() {
        return SingletonHolder.instance;
    }

    private static class SingletonHolder {
        protected static final ThreadPoolService instance = new ThreadPoolService();
    }
}
