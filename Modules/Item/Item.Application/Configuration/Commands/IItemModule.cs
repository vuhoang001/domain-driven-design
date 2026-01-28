using Item.Application.Configuration.Contracts;
using Item.Application.Contracts;

namespace Item.Application.Configuration.Commands;

public interface IItemModule
{
   Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
   
   Task ExecuteCommandAsync(ICommand command);
   
   Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}