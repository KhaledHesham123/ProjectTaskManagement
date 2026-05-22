using MediatR;
using ProjectTaskManagement.Application.Features.Auth.Dtos;

namespace ProjectTaskManagement.Application.Features.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken) =>
        throw new NotImplementedException("Implement login identity logic in this handler.");
}
