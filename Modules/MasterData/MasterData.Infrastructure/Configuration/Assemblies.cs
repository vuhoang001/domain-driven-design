using System.Reflection;
using MasterData.Application.Configuration.Commands;

namespace Item.Infrastructure.Configuration;

internal static class Assemblies
{
   public static readonly Assembly Application = typeof(IMasterDataModule).Assembly;
}
