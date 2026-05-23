using ProjectTaskManagement.Application.Common;

namespace ProjectTaskManagement.Application.Common.Exceptions;

public class ForbiddenException(string? message = null)
    : Exception(message ?? ApplicationMessages.ForbiddenPermission);
