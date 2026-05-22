using MediatR;

namespace ProjectTaskManagement.Domain.Common;

public interface ICommand<out TResponse> : IRequest<TResponse>;
