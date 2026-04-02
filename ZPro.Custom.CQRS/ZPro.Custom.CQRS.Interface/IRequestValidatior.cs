namespace ZPro.Custom.CQRS.Interface;

public interface IRequestValidatior<in TRequest>
{
    Task<IReadOnlyCollection<string>> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}