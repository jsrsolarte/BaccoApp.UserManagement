namespace BaccoApp.UserManagement.Domain.Exceptions
{
    [Serializable]
    public class EntityNotExistException : AppException
    {
        public EntityNotExistException(Type type, string id) : this($"Entity {type.Name} = {id} not exist")
        {
        }

        protected EntityNotExistException(string message) : base(message)
        {
        }
    }
}