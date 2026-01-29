using System.Reflection;
using Item.Application.Configuration.Commands;

namespace Item.Infrastructure.Configuration;

internal static class Assemblies
{
   public static readonly Assembly Application = typeof(IItemModule).Assembly;
}
