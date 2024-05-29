namespace Task.Components.Shared;
public class TaskTreeEventArgs : EventArgs
{
    public string Item { get; private set; }

    public TaskTreeEventArgs(string item)
    {
        Item = item;
    }
}
