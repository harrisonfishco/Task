using Task.ModelObjects;

namespace Task.Components.Layout
{
    public partial class ModelLayout<T> where T : ModelObject
    {
        protected Mode CurrentMode { get; set; }

        public void SetNumber(string number)
        {
            this.Number = number;
        }

        /// <summary>
        /// Grab the title of the page depending on the current Mode
        /// </summary>
        /// <returns></returns>
        protected string GetTitle()
        {
            string res = string.Empty;
            switch(CurrentMode)
            {
                default:
                case Mode.Find:
                    res = $"Find {ModelObject.GetNamePlural<T>()}";
                    break;
                case Mode.Insert:
                    res = $"Add {ModelObject.GetName<T>()}";
                    break;
                case Mode.Update:
                    res = $"Update {ModelObject.GetName<T>()}";
                    break;
            }
            return res;
        }
    }

    public enum Mode
    {
        Find,
        Update,
        Insert
    }
}
