using AsanPardakht.Core.Errors;
using OneOf;

namespace AsanPardakht.Core.Command
{
    public interface ICommandHandler { }
    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : IBaseCommand
    {
        Task<OneOf<bool, Error>> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default!);
    }
}