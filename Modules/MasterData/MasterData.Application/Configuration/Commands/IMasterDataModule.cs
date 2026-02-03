using MasterData.Application.Configuration.Contracts;
using MasterData.Application.Contracts;

namespace MasterData.Application.Configuration.Commands;

public interface IMasterDataModule
{
   Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
   
   Task ExecuteCommandAsync(ICommand command);
   
   Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}