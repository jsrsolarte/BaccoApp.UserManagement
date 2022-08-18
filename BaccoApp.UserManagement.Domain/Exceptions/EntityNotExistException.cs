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

        protected EntityNotExistException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}