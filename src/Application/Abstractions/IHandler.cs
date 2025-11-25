using GoodReadScripts.Utils;

namespace GoodReadScripts.Application.Abstractions;

public interface IHandler<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}