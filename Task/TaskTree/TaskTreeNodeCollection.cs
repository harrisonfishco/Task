using System.Collections;

namespace TTask.TaskTree
{
    public class TaskTreeNodeCollection : IEnumerable<TaskTreeNode>
    {
        private List<TaskTreeNode> roots;

        public TaskTreeNodeCollection()
        {
            roots = new List<TaskTreeNode>();
        }

        public void AddPath(string path)
        {
            string root = path.Substring(0, path.IndexOf('.'));

            if (roots.Any(r => r.Name == root))
            {
                roots.Where(r => r.Name == root).First().AddPath(path.Substring(path.IndexOf(".") + 1));
            }
            else
            {
                TaskTreeNode newNode = new TaskTreeNode(root);
                newNode.AddPath(path.Substring(path.IndexOf(".") + 1));
                roots.Add(newNode);
            }
        }

        public IEnumerator<TaskTreeNode> GetEnumerator()
        {
            return roots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
