namespace Task
{
    public class TaskError
    {
        public static void HandleError(Exception e)
        {
            throw e;
        }

        public static void CreateUserError(string message)
        {
            throw new Exception(message);
        }
    }
}
