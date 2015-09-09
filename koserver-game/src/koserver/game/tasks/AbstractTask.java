package koserver.game.tasks;

import koserver.game.services.ThreadPoolService;

public abstract class AbstractTask<T> {

    protected final T linkedObject;
    protected final TaskTypeEnum taskType;
    protected final boolean repeat = false;

    public AbstractTask(final T linkedObject, final TaskTypeEnum taskType) {
        this.linkedObject = linkedObject;
        this.taskType = taskType;
    }

    public T getLinkedObject() {
        return linkedObject;
    }

    public TaskTypeEnum getTaskType() {
        return taskType;
    }

    public final void cancel() {
        ThreadPoolService.getInstance().cancelTask(this.linkedObject, this.taskType);
    }

    public abstract void execute();
}
