using AutoMapper;
using EventsAPI.Mapping;
using EventsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => { }, typeof(EventProfile));
builder.Services.AddControllers();
builder.Services.AddScoped<IEventService, EventService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

app.Run();