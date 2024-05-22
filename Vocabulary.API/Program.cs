using System.Text.Json.Serialization;
using Vocabulary.Data;
using Vocabulary.Data.Repository;
using Vocabulary.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IWordMeaningRepository, WordMeaningRepository>();
builder.Services.AddScoped<IDataContextProvider, DataContextProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
