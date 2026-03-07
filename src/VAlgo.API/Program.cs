using VAlgo.Modules.Contests;
using VAlgo.Modules.Identity;
using VAlgo.Modules.ProblemClassification;
using VAlgo.Modules.ProblemManagement;
using VAlgo.Modules.Submissions;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Core framework
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen();

// Modules

// Submissions
builder.Services.AddSubmissionsModule(builder.Configuration);

// Problem Management
builder.Services.AddProblemManagementModule(builder.Configuration);

// Problem Classification
builder.Services.AddProblemClassificationModule(builder.Configuration);

// Identity
builder.Services.AddIdentityModule(builder.Configuration);

// Contests
builder.Services.AddContestsModule(builder.Configuration);

// SharedKernel
builder.Services.AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>();
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();


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
