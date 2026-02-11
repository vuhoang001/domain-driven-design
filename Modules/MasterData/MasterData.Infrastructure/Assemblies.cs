using System.Reflection;
using MasterData.Application.Configuration.Command;

namespace MasterData.Infrastructure
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(IMasterDataModule).Assembly;
        public static readonly Assembly Infrastructure = typeof(MasterDataModule).Assembly;
    }
}