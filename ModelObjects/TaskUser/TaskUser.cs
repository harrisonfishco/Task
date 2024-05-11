using Task.MODef;

namespace Task.TaskUser
{
    public class TaskUser : DataBoundModelObject
    {
        #region Properties
        private const string TABLE = "TASK_USER";

        public const string PROPERTY_USERGU = "USER_GU";
        public string UserGU
        {
            get { return GetProperty(PROPERTY_USERGU); }
            set { SetProperty(PROPERTY_USERGU, value); }
        }

        public const string PROPERTY_USERNAME = "USERNAME";
        public string Username
        {
            get { return GetProperty(PROPERTY_USERNAME); }
            set { SetProperty(PROPERTY_USERNAME, value); }
        }

        public const string PROPERTY_PASSWORD = "PASSWORD";
        public string Password
        {
            get { return GetProperty(PROPERTY_PASSWORD); }
            set { SetProperty(PROPERTY_PASSWORD, value); }
        }

        public const string PROPERTY_EMAIL = "EMAIL";
        public string Email
        {
            get { return GetProperty(PROPERTY_EMAIL); }
            set { SetProperty(PROPERTY_EMAIL, value); }
        }

        public const string PROPERTY_ADDTIMESTAMP = "ADD_TIMESTAMP";
        public string AddTimestamp
        {
            get { return GetProperty(PROPERTY_ADDTIMESTAMP); }
            set { SetProperty(PROPERTY_ADDTIMESTAMP, value); }
        }

        public const string PROPERTY_UPDATETIMESTAMP = "UPDATE_TIMESTAMP";
        public string UpdateTimestamp
        {
            get { return GetProperty(PROPERTY_UPDATETIMESTAMP); }
            set { SetProperty(PROPERTY_UPDATETIMESTAMP, value); }
        }
        #endregion

        public TaskUser()
            : base(PROPERTY_USERGU, TABLE)
        {
            
        }
    }
}
