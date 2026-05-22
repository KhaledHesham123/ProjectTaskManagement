using System.Text.RegularExpressions;

namespace ProjectTaskManagement.Domain.ValueObjects;

public sealed partial record Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        email = email.Trim().ToLowerInvariant();

        if (!EmailRegex().IsMatch(email))
            throw new ArgumentException("Email format is invalid.", nameof(email));

        return new Email(email);
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
