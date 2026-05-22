using MediatR;
using ProjectTaskManagement.Application.Features.Auth.Dtos;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    public Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException("Implement register identity logic in this handler.");
}
