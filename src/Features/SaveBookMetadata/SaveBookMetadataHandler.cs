using GoodReadScripts.Application.Abstractions;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Features.SaveBookMetadata;

public class SaveBookMetadataHandler(IFileSystem fileSystem)
    : IHandler<SaveBookMetadataRequest, SaveBookMetadataResponse>
{
    public Task<Result<SaveBookMetadataResponse>> HandleAsync(SaveBookMetadataRequest request, CancellationToken cancellationToken)
    {
        // Pseudo implementation
        /*
         * 1. Ensure the target directory exists using IFileSystem
         * 2. Create or overwrite the metadata file with the provided book metadata
         * 3. Return a successful SaveBookMetadataResponse
         */

        var path = string.IsNullOrWhiteSpace(request.FolderName)
            ? request.Directory.FullName
            : Path.Combine(request.Directory.FullName, request.FolderName);

        var ensureDirResult = fileSystem.EnsureDirectoryExists(path);

        return ensureDirResult.Match<Task<Result<SaveBookMetadataResponse>>>(async _ =>
        {
            var title = RemoveInvalidCharacters(request.BookMetadata.Title);
            var filePath = Path.Combine(path, $"{title}.md");
            var content = request.BookMetadata.ToMarkdown();
            var result = await fileSystem.SaveFileAsync(filePath, content, cancellationToken);

            return result.Match<Result<SaveBookMetadataResponse>>(
                _ => new SaveBookMetadataResponse(filePath),
                error => error);
        }, error => Task.FromResult<Result<SaveBookMetadataResponse>>(error));
    }

    private static string RemoveInvalidCharacters(string title)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var filteredTitle = title.Where(c => !invalidChars.Contains(c)).ToArray();
        return new string(filteredTitle);
    }
}