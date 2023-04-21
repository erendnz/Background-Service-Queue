using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using BackgroundServiceQueue;
using BackgroundServiceQueue.Queues;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<QueueHostedService>();
builder.Services.AddLogging(i =>
{
    i.AddConsole();
    i.AddDebug();
});

builder.Services.AddSingleton(typeof(IBackgroundQueue<string>),typeof(BackgroundQueue));
//builder.Services.AddSingle ton(typeof(IBackgroundQueue<string>), typeof(BackgroundQueue));


builder.Services.AddHostedService<QueueHostedService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();


