using HTSA;

var builder = WebApplication.CreateBuilder(args);

// Configure services using the Startup class
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline using the Startup class
startup.Configure(app, app.Environment);

app.Run();
