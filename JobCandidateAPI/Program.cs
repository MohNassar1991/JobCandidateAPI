using FluentValidation;
using FluentValidation.AspNetCore;
using JobCandidateAPI.Helper;
using JobCandidateAPI.Interfaces;
using JobCandidateAPI.IRepositories;
using JobCandidateAPI.Repositories;
using JobCandidateAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<CsvCandidateService>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddValidatorsFromAssemblyContaining<CandidateValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();