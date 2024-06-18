using Task.ModelObjects;

namespace Task.Components.Layout
{
    public partial class ModelLayout<T> where T : ModelObject
    {
        protected Mode CurrentMode { get; set; } = Mode.Find;

        public void SetIdentity(string? identity)
        {
            if(TypeCheck.NotEmpty(identity))
            {
                if (Guid.TryParse(identity, out Guid id))
                {
                    this.Identity = id;
                }
                CurrentMode = Mode.Update;
            }
        }

        public void SetIdentity(Guid? identity)
        {
            if(TypeCheck.NotEmpty(identity))
            {
                this.Identity = identity;
                CurrentMode = Mode.Update;
            }
        }

        public Guid? GetIdentity()
        {
            return this.Identity;
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
