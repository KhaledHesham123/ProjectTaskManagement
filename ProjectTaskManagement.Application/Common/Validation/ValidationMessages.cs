namespace ProjectTaskManagement.Application.Common.Validation;

public static class ValidationMessages
{
    public const string Required = "{PropertyName} is required.";
    public const string MaxLength = "{PropertyName} must not exceed {MaxLength} characters.";
    public const string MinLength = "{PropertyName} must be at least {MinLength} characters.";
    public const string InvalidEmail = "{PropertyName} must be a valid email address.";
    public const string InvalidGuid = "{PropertyName} must be a valid identifier.";
}
