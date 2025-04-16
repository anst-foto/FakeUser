using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FakeUser.Model;
using FakeUser.WebApi.DAL;
using Microsoft.AspNetCore.Http;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

var endPoint = app.MapGroup("api/v0/users").WithOpenApi();
endPoint.MapGet("/", async (UserContext db) =>
    {
        var users = await db.Users.ToListAsync();
        return users.Count == 0
            ? Results.NotFound()
            : Results.Ok(SlowStreamAsync(users));
    })
    .Produces<IAsyncEnumerable<User>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

endPoint.MapPost("/", async (UserContext db, User user) =>
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();

        return Results.Created($"/api/v0/users/{user.Id}", user);
    })
    .Produces<User>(StatusCodes.Status201Created);

endPoint.MapPut("/{id:int}", async (UserContext db, int id, User user) =>
    {
        var userToUpdate = await db.Users.SingleOrDefaultAsync(u => u.Id == id);

        if (userToUpdate == null) return Results.NotFound();

        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;
        await db.SaveChangesAsync();

        return Results.Ok(userToUpdate);
    }).Produces<User>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

endPoint.MapDelete("/{id:int}", async (UserContext db, int id) =>
{
    var userToDelete = await db.Users.SingleOrDefaultAsync(u => u.Id == id);

    if (userToDelete == null) return Results.NotFound();

    db.Users.Remove(userToDelete);
    await db.SaveChangesAsync();

    return Results.Ok();
}).Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

await app.RunAsync();
return;

async IAsyncEnumerable<User> SlowStreamAsync(IEnumerable<User> users)
{
    foreach (var user in users)
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        yield return user;
    }
}
