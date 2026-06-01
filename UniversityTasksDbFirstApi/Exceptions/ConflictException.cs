namespace UniversityTasksDbFirstApi.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException()
    {
        
    }
}
