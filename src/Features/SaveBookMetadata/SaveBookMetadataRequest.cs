using GoodReadScripts.Domain;

namespace GoodReadScripts.Features.SaveBookMetadata;

public record SaveBookMetadataRequest(BookMetadata BookMetadata, DirectoryInfo Directory, string? FolderName);