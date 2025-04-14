using System.Net.Http.Json;
using FakeUser.Model;

var http = new HttpClient();
var url = "http://localhost:5257/api/v0/users";

var users = http.GetFromJsonAsAsyncEnumerable<User>(url);
await foreach (var user in users)
{
    Console.WriteLine(user);
}