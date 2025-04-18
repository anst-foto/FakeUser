using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Reactive;
using System.Text.Json;
using System.Threading.Tasks;
using FakeUser.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FakeUser.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    [Reactive] public int? Id { get; set; }
    [Reactive] public string? LastName { get; set; }
    [Reactive] public string? FirstName { get; set; }

    public ObservableCollection<User> Users { get; } = [];
    [Reactive] public User? SelectedUser { get; set; }

    public ReactiveCommand<Unit, Unit> DeleteUserCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveUserCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadCommand { get; }

    public MainWindowViewModel()
    {
        this
            .WhenAnyValue(vm => vm.SelectedUser)
            .Subscribe(u =>
            {
                Id = u?.Id;
                LastName = u?.LastName;
                FirstName = u?.FirstName;
            });

        SaveUserCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (Id.HasValue)
            {
                await UpdateUserAsync();
            }
            else
            {
                await AddUserAsync();
            }
        });

        LoadCommand = ReactiveCommand.CreateFromTask(GetUsersAsync);
    }

    private async Task UpdateUserAsync()
    {
        var json = JsonSerializer.Serialize(SelectedUser);

        var url = Urls.UpdateUserUrl((int)SelectedUser?.Id!);
        using var response = await App.Http.PutAsJsonAsync(url, json);

        response.EnsureSuccessStatusCode();
    }

    private async Task AddUserAsync()
    {
        var json = JsonSerializer.Serialize(new User()
        {
            LastName = LastName!,
            FirstName = FirstName!
        });

        var url = Urls.AddUserUrl();
        using var response = await App.Http.PostAsJsonAsync(url, json);

        response.EnsureSuccessStatusCode();
    }

    private async Task DeleteUserAsync()
    {
        /*var url = Urls.DeleteUserUrl((int)SelectedUser?.Id!);
        await App.Http.DeleteFromJsonAsync(url);*/
    }

    private async Task GetUsersAsync()
    {
        Users.Clear();

        var url = Urls.GetUsersUrl();
        var users = App.Http.GetFromJsonAsAsyncEnumerable<User>(url);
        await foreach (var user in users)
        {
            Users.Add(user);
        }
    }
}
