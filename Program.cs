using hot_chocolate_demo.GraphQL;
using Microsoft.EntityFrameworkCore;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DogDataContext>(
  o => o.UseNpgsql(builder?.Configuration?.GetConnectionString("PostgresDb")) );
builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<DogDataContext>()
    .AddQueryType<Query>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapGraphQL();

app.Run();
