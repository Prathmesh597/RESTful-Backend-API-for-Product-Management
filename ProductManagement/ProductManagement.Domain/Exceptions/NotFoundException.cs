namespace ProductManagement.Domain.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string entityName, int id)
            : base($"{entityName} with ID {id} was not found.", 404)
        {
        }
    }
}