using AsanPardakht.Core.Data;
using AsanPardakht.Core.Errors;
using AsanPardakht.Core.Resources;
using OneOf;

namespace AsanPardakht.Core.Command
{
    public sealed class DecoratorCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : IBaseCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandHandler<TCommand> _next;
        private readonly IResourceManager _resourceManager;

        public DecoratorCommandHandler(IResourceManager resourceManager, IUnitOfWork unitOfWork, ICommandHandler<TCommand> next)
        {
            _next = next;
            _unitOfWork = unitOfWork;
            _resourceManager = resourceManager;
        }

        public async Task<OneOf<bool, Error>> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            var handlerResult = await _next.ExecuteAsync(command, cancellationToken);

            return await handlerResult.Match<Task<OneOf<bool, Error>>>(async success =>
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }, async error => await Task.FromResult(new Error(_resourceManager[error.Message], error.Code)));
        }
    }
}