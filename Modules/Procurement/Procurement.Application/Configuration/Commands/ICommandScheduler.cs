using Procurement.Application.Contracts;

namespace Procurement.Application.Configuration.Commands;

public interface ICommandScheduler
{
   Task EnqueueAsync(ICommand command);
   Task EnqueueAsync<T>(ICommand<T> command);
   
}