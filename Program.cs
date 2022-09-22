using Microsoft.EntityFrameworkCore; // dotnet build keeps removing global use -- creating issue on github
var builder = WebApplication.CreateBuilder(args);

// Use InMemoryDb in application 
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors( options => options.AddDefaultPolicy(builder => {
    builder.WithOrigins(
        "http://localhost:5173/"
    );
}));
var app = builder.Build();



app.UseCors( builder => builder
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyHeader()
);
// HTTP EndPoint points
// default index method
app.MapGet("/", () => "TodoAPI");

// Grab all of the todo item.
// Asynchronously creates a List<T> from an IQueryable<out T> by enumerating it asynchronously.
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

// Returns completed todo items(checks the boolean value attribute IsComplete in Todo model)
app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

// Find specific todo item with id
app.MapGet("/todoitems/{id}", async (int id,TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
        ? Results.Ok(todo)
        : Results.NotFound());

// Post method adding todo items.
app.MapPost("/todoitems", async (Todo todo , TodoDb db) =>
{
    // inserts into the inMem Db.
    db.Todos.Add(todo);
    await db.SaveChangesAsync(); // saves all changes made in the context of the db.
});

// Endpoint used as a edit/complete task
app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) => 
{
    var todo = await db.Todos.FindAsync(id);
    if(todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id,TodoDb db) =>
{
    if(await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});


app.Run();
