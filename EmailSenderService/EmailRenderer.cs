using EmailTemplates;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace EmailSenderService
{
    public class EmailRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : IDisposable
    {
        private readonly HtmlRenderer _htmlRenderer = new(serviceProvider, loggerFactory);

        public void Dispose() => _htmlRenderer.Dispose();

        public Task<string> RenderTemplateAsync<TTemplate>(IDictionary<string, object?> parameters, CancellationToken cancellationToken = default)
            where TTemplate : EmailTemplate
        {
            var parameterView = ParameterView.FromDictionary(parameters);

            return _htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var output = await _htmlRenderer.RenderComponentAsync<TTemplate>(parameterView);
                return output.ToHtmlString();
            });
        }
    }
}
