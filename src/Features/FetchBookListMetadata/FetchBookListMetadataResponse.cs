using GoodReadScripts.Domain;

namespace GoodReadScripts.Features.FetchBookListMetadata;

public record FetchBookListMetadataResponse(List<GoodReadUrl> BookUrls);