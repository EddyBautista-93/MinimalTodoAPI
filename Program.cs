using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Use InMemoryDb in application 
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
// Generates an HTML response to database related exceptions 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// HTTP EndPoint points

// default index method
app.MapGet("/", () => "Hello World");

// Grab all of the todo item
// app.MapGet("/todoitems", async (Todo db))
app.Run();
