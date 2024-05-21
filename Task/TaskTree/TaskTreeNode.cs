namespace TTask.TaskTree
{
    public class TaskTreeNode
    {
        public string Name { get; set; }
        public List<TaskTreeNode> Children { get; private set; }
        public List<string> Items { get; private set; }

        public TaskTreeNode(string name)
        {
            Name = name;
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
                    Children.Where(n => n.Name == root).First().AddPath(root);
                }
                else
                {
                    TaskTreeNode newNode = new TaskTreeNode(root);
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
            return Name;
        }
    }
}
