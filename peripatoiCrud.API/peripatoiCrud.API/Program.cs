using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using peripatoiCrud.API.Data;
using peripatoiCrud.API.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//εδω χρησιμοποιουμε dependency injection, περνοντας το dbcontext και υστερα παρεχουμε το connection string
builder.Services.AddDbContext<PeripatoiDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("PeripatoiConnectionString")));

builder.Services.AddDbContext<PeripatoiAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PeripatoiAuthConnectionString")));

//εδω συσχετιζουμε το interface με την υλοποιηση, η χρηση του repository pattern μας προσφερει επισης και την ελευθερια να αλλαξουμε εντελως την υλοποιηση εαν θελησουμε
// για παραδειγμα στην περιπτωση μας εχουμε sql server αλλα θα μπορουσαμε να ειχαμε inMemory repository απλα αλλαζοντας την υλοποιηση παρακατω, και τιποοτα αλλο
builder.Services.AddScoped<IPerioxhRepository, PerioxhRepository>();
builder.Services.AddScoped<IPeripatosRepository, PeripatosRepository>();
builder.Services.AddScoped<ITokenRepositroy, TokenRepository>();

//εδω πρεπει να κναουμε inject και τα identities
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("PeripatoiProvider")
    .AddEntityFrameworkStores<PeripatoiAuthDbContext>()
    .AddDefaultTokenProviders();

//και εδω τα identity options (για το password μονο)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 16;
    options.Password.RequiredUniqueChars = 6;
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //οριζουμε οτι θελουμε να γινει authentication πριν το build
    .AddJwtBearer(epiloges => // εδω κανουμε το configuration του jwtbearer με καποιες παραμετρους πχ εαν θελουμε να ελεγξουμε τον οποιονδηποτε καλεσε το JWT, εαν εχει ληξει το token κτλπ
    epiloges.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // το key εδω μετατρεπεται απο string σε byte array και χρησιμοποιειται ως signing key
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
