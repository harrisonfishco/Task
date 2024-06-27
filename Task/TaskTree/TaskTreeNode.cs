using Task;

namespace TTask.TaskTree
{
    public class TaskTreeNode
    {
        public string Name { get; set; }
        public TaskTreeNode? Parent { get; private set; }
        public List<TaskTreeNode> Children { get; private set; }
        public List<string> Items { get; private set; }

        public TaskTreeNode(string name, TaskTreeNode? parent = null)
        {
            Name = name;
            Parent = parent;
            Children = new List<TaskTreeNode>();
            Items = new List<string>();
        }

        public void AddPath(string path)
        {
            if(path.IndexOf('.') == -1)
            {
                AddItem(path);
            }
            else
            {
                string root = path.Substring(0, path.IndexOf("."));

                if(Children.Any(n => n.Name == root))
                {
                    Children.Where(n => n.Name == root).First().AddPath(path.Substring(path.IndexOf(".") + 1));
                }
                else
                {
                    TaskTreeNode newNode = new TaskTreeNode(root, this);
                    newNode.AddPath(path.Substring(path.IndexOf(".") + 1));
                    Children.Add(newNode);
                }
            }
        }

        public void AddItem(string item)
        {
            Items.Add(item);
        }

        public override string ToString() 
        {
            return TypeCheck.NotEmpty(Parent) ? $"{Parent.ToString()}.{Name}" : Name;
        }
    }
}
