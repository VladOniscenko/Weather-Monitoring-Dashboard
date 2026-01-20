var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
// In a real app, you would do: builder.Services.AddInfrastructure(builder.Configuration);
// and: builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS for Next.js
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs",
        policy => policy.WithOrigins("http://localhost:3000") // Your Frontend URL
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// 2. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowNextJs"); // Activate CORS

app.UseAuthorization();

app.MapControllers();

app.Run();