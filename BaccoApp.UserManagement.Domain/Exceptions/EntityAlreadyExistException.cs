namespace BaccoApp.UserManagement.Domain.Exceptions
{
    [Serializable]
    public class EntityAlreadyExistException : AppException
    {
        public EntityAlreadyExistException(Type type, string property, string id) : this($"Entity {type.Name} with {property} = {id} already exist")
        {
        }

        protected EntityAlreadyExistException(string message) : base(message)
        {
        }
    }
}