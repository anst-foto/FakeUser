using System;

namespace FakeUser.Desktop;

public static class Urls
{
    private static readonly Uri Base = new Uri("http://localhost:5257/api/v0/users");

    public static Uri UpdateUserUrl(int id) => new Uri($"{Base}/{id}", UriKind.Absolute);
    public static Uri GetUsersUrl() => new Uri($"{Base}", UriKind.Absolute);
    public static Uri AddUserUrl() => new Uri($"{Base}", UriKind.Absolute);
    public static Uri DeleteUserUrl(int id) => new Uri($"{Base}/{id}", UriKind.Absolute);
}
