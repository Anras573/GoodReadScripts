// See https://aka.ms/new-console-template for more information

using System.CommandLine;

using GoodReadScripts.Application;
using GoodReadScripts.Domain;
using GoodReadScripts.Features.FetchBookListMetadata;
using GoodReadScripts.Features.FetchBookMetadata;
using GoodReadScripts.Features.SaveBookMetadata;
using GoodReadScripts.Infrastructure;
using GoodReadScripts.Utils;

var httpClient = new HttpClient();
var goodReadClient = new GoodReadClient(httpClient);
var htmlContentParser = new HtmlContentParser();
var fileSystem = new FileSystem();

Argument<string> urlArgument = new("url")
{
    Description = "The Goodreads book URL to fetch metadata from."
};

Option<DirectoryInfo> directoryOption = new("--directory", "-d")
{
    Description = "The output directory to save the fetched metadata files.",
    Required = true
};

Option<string> folderOption = new("--folder", "-f")
{
    Description = "The name of the folder to save the fetched metadata files in the output directory.",
    Required = true
};

Command fetchBookMetadataCommand = new("fetch-book", "Fetch book metadata from a Goodreads URL")
{
    Arguments = { urlArgument },
    Options = { directoryOption, folderOption }
};

Command fetchBookListCommand = new("fetch-list", "Fetch book metadata from a Goodreads list URL")
{
    Arguments = { urlArgument },
    Options = { directoryOption, folderOption }
};

fetchBookMetadataCommand.SetAction(HandleFetchBookMetadataCommand);
fetchBookListCommand.SetAction(HandleFetchBookListMetadataCommand);

RootCommand rootCommand = new("GoodReadScripts CLI")
{
    fetchBookMetadataCommand,
    fetchBookListCommand
};

return await rootCommand.Parse(args).InvokeAsync();

async Task<int> HandleFetchBookMetadataCommand(ParseResult parseResult, CancellationToken cancellationToken)
{
    var parametersResult = ParseParameters(parseResult);
    return await parametersResult.Match<Task<int>>(async parameters =>
    {
        var request = new FetchBookMetadataRequest(parameters.Url);
        var handler = new FetchBookMetadataHandler(goodReadClient, htmlContentParser);

        var result = await handler.HandleAsync(request, cancellationToken);

        return await result.Match<Task<int>>(async response =>
        {
            var metadata = response.BookMetadata;
            
            var saveRequest = new SaveBookMetadataRequest(
                metadata,
                parameters.Directory,
                parameters.FolderName
            );
            var saveHandler = new SaveBookMetadataHandler(fileSystem);
            var saveResult = await saveHandler.HandleAsync(saveRequest, cancellationToken);
            return await saveResult.Match<Task<int>>(saveResponse =>
            {
                Console.WriteLine($"Metadata saved to: {saveResponse.FilePath}");
                return Task.FromResult(0);
            }, HandleError);
        }, HandleError);
    }, HandleError);
}

async Task<int> HandleFetchBookListMetadataCommand(ParseResult parseResult, CancellationToken cancellationToken)
{
    var parametersResult = ParseParameters(parseResult);

    return await parametersResult.Match<Task<int>>(async parameters =>
    {
        var request = new FetchBookListMetadataRequest(parameters.Url);
        var handler = new FetchBookListMetadataHandler(goodReadClient, htmlContentParser);
        
        var result = await handler.HandleAsync(request, cancellationToken);
        
        return await result.Match<Task<int>>(async response =>
        {
            var saveHandler = new SaveBookMetadataHandler(fileSystem);
            Error? error = null;
            foreach (var url in response.BookUrls)
            {
                var fetchRequest = new FetchBookMetadataRequest(url);
                var fetchHandler = new FetchBookMetadataHandler(goodReadClient, htmlContentParser);
                var fetchResult = await fetchHandler.HandleAsync(fetchRequest, cancellationToken);
                
                await fetchResult.SwitchAsync(async fetchResponse =>
                {
                    var metadata = fetchResponse.BookMetadata;
                    var saveRequest = new SaveBookMetadataRequest(
                        metadata,
                        parameters.Directory,
                        parameters.FolderName
                    );
                    var saveResult = await saveHandler.HandleAsync(saveRequest, cancellationToken);
                    saveResult.Switch(saveResponse =>
                    {
                        Console.WriteLine($"Metadata saved to: {saveResponse.FilePath}");
                    }, failure => error = failure);
                }, failure => Task.FromResult(error = failure));
                
                if (error is not null) return await HandleError(error);
            }

            return 0;
        }, HandleError);
    }, HandleError);
}

Result<Parameters> ParseParameters(ParseResult parseResult)
{
    var urlValue = parseResult.GetRequiredValue(urlArgument);
    var urlResult = GoodReadUrl.Create(urlValue);
    
    return urlResult.Match<Result<Parameters>>(url =>
    {
        var directory = parseResult.GetRequiredValue(directoryOption);
        var folderName = parseResult.GetValue(folderOption);
        return new Parameters(url, directory, folderName);
    }, error => error);
}

Task<int> HandleError(Error error)
{
    Console.Error.WriteLine(error);
    return Task.FromResult(1);
}

record Parameters(GoodReadUrl Url, DirectoryInfo Directory, string? FolderName);