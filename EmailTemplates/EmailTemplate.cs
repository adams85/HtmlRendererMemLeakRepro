using Microsoft.AspNetCore.Components;

namespace EmailTemplates;

public abstract class EmailTemplate : ComponentBase
{
    [Parameter] public required string Title { get; init; }
    [Parameter] public required string PreviewText { get; init; }

    // See: https://stackoverflow.com/questions/67170227/blazor-conditional-html-comments
    public static MarkupString Preserve(string value) => new(value);
}
