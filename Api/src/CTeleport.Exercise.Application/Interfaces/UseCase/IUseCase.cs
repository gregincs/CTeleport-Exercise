namespace CTeleport.Exercise.Application.Interfaces.UseCase
{
    public interface IUseCase<TRequest, TResponse>
    {
        public Task<TResponse> ExecuteAsync(TRequest request);
    }
}
