namespace CTeleport.Exercise.Application.Interfaces;

public interface IRequestValidator<TRequest>
{
    IEnumerable<string> ValidateRequest(TRequest request);
}
