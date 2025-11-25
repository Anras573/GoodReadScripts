using GoodReadScripts.Utils;

namespace GoodReadScripts.Application.Abstractions;

public interface IFileSystem
{
    Result<Success> EnsureDirectoryExists(string path);
    Task<Result<Success>> SaveFileAsync(string path, string content, CancellationToken cancellationToken);
}