using System.Net.Http.Json;
using Godot;
using HttpClient = System.Net.Http.HttpClient;
using System.Threading.Tasks;
namespace Game.core;
using Newtonsoft.Json;
public static class Modules
{
    public static bool IsActionJustPressed()
    {
        return Input.IsActionJustPressed("ui_up") || Input.IsActionJustPressed("ui_down") || Input.IsActionJustPressed("ui_left") || Input.IsActionJustPressed("ui_right");
    }
    public static bool IsActionPressed()
    {
        return Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") || Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right");
    }
    public static bool IsActionJustReleased()
    {
        return Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") || Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right");
    }


    private static readonly HttpClient httpClient = new HttpClient();
    public static async Task<T> FetchDataFromPokeApi<T>(string url)
    {
        try
        {
            var responce = await httpClient.GetAsync(url);

            if (!responce.IsSuccessStatusCode)
            {
                Logger.Error($"Api Error: failed fetching {url} -> {responce.StatusCode}");
                return default;
            }
            var json = await responce.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (System.Exception ex)
        {
            Logger.Error($"Api Error: failed fetching {url} -> {ex.Message}");
            return default;
        }
    }
}
