using GoodReadScripts.Application.Abstractions;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Infrastructure;

public class FileSystem : IFileSystem
{
    public Result<Success> EnsureDirectoryExists(string path)
    {
        try
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        catch (Exception ex)
        {
            return new Error(ex.Message);
        }

        return new Success();
    }

    public async Task<Result<Success>> SaveFileAsync(string path, string content, CancellationToken cancellationToken)
    {
        try
        {
            await File.WriteAllTextAsync(path, content, cancellationToken);
        }
        catch (Exception ex)
        {
            return new Error(ex.Message);
        }

        return new Success();
    }
}