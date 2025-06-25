namespace UsersMS.Core.Application
{
    public interface IService<TRequest, TResponse>
    {
        Task<TResponse> Execute(TRequest request);
    }
}