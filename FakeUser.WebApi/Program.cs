using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FakeUser.Model;
using FakeUser.WebApi.DAL;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var app = builder.Build();
app.UseHttpsRedirection();

const string baseUrl = "/api/v0/users";

app.MapGet(baseUrl, () =>
{
    var db = new UserContext(connectionString);
    return SlowStreamAsync(db.Users);
});


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