using Autofac;
using MasterData.Application.Configuration.Commands;
using MasterData.Application.Configuration.Contracts;
using MasterData.Application.Contracts;
using Item.Infrastructure.Configuration;
using Item.Infrastructure.Configuration.Processing;
using MediatR;

namespace Item.Infrastructure;

/// <summary>
/// Đây là cổng để API (ddd.API) tương tác với module Item.
/// API không gọi trực tiếp vào các service trong Item, mà gọi qua ItemModule.
/// Có 3 method:
///  + ExecuteCommandAsync(ICommand command): để thực thi các lệnh không trả về kết quả.
///  + ExecuteCommandAsync<TResult>(ICommand<TResult> command): để thực thi các l
///  + ExecuteQueryAsync<TResult>(IQuery<TResult> query): để thực thi các truy vấn và trả về kết quả.
///
/// Ý nghĩa:
///  + Vì đang sử dụng ddd + Modular Monolith, mỗi module là 1 "hộp đen" riêng biệt.
///  + API không biết chi tiết bên trong module Item, chỉ biết gửi Command/Query qua interface này.
///  + Điều này cho phép: tương lại tachsItem thành service riêng biệt, API không cần thay đổi.
/// </summary>
public class MasterDataModule : IMasterDataModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        await using var scope    = MasterDataCompositionRoot.BeginLifetimeScope();
        var             mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query);
    }
}