using EmailSenderService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<EmailRenderer>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
