using Microsoft.EntityFrameworkCore;
using peripatoiCrud.API.Data;
using peripatoiCrud.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//εδω χρησιμοποιουμε dependency injection, περνοντας το dbcontext και υστερα παρεχουμε το connection string
builder.Services.AddDbContext<PeripatoiDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("PeripatoiConnectionString")));

//εδω συσχετιζουμε το interface με την υλοποιηση, η χρηση του repository pattern μας προσφερει επισης και την ελευθερια να αλλαξουμε εντελως την υλοποιηση εαν θελησουμε
// για παραδειγμα στην περιπτωση μας εχουμε sql server αλλα θα μπορουσαμε να ειχαμε inMemory repository απλα αλλαζοντας την υλοποιηση παρακατω, και τιποοτα αλλο
builder.Services.AddScoped<IPerioxhRepository, PerioxhRepository>(); 

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
