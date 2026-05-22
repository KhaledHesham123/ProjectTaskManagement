namespace ProjectTaskManagement.Application.Common.Exceptions;

public class UnauthorizedException(string message = "Unauthorized access.")
    : Exception(message);
