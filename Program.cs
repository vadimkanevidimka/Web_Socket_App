using Koshelekpy_Test.Application;
using Koshelekpy_Test.Domain.Entities;
using Koshelekpy_Test.Infrastracture.Repositories;
using Koshelekpy_Test.Infrastracture.UOW;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRepository<Message>, MessageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddMvc();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Message API V1");
});

//Add WebSockets to DI
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromDays(1)
};

app.UseWebSockets(webSocketOptions);

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{method}");

app.MapHub<MessageHub>("/ws");

app.Run();

