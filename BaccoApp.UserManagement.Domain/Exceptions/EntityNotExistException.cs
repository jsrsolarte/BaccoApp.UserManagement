namespace BaccoApp.UserManagement.Domain.Exceptions
{
    [Serializable]
    public class EntityNotExistException : AppException
    {
        public EntityNotExistException(Type type, object id) : this($"Entity {type.Name} with Id = {id} not exist")
        {
        }

        protected EntityNotExistException(string message) : base(message)
        {
        }
    }
}