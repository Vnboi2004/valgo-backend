using VAlgo.Modules.Submissions;

var builder = WebApplication.CreateBuilder(args);

// Core framework
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

// Modules
builder.Services.AddSubmissionsModule(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();


// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
