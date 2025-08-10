using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace UserManagement.Services.Tests;
public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;
    public TestAsyncQueryProvider(IQueryProvider inner) { _inner = inner; }

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
        new TestAsyncEnumerable<TElement>(expression);

    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
    public object? Execute(Expression expression) => _inner.Execute<object?>(expression);
    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) =>
        new TestAsyncEnumerable<TResult>(expression);

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) =>
        Task.FromResult(Execute<TResult>(expression));
    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => _inner.Execute<TResult>(expression);
}
