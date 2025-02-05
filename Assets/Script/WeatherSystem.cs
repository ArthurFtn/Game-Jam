using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public enum WeatherType { Normal, Snowstorm, Rain, Heatwave }
    public WeatherType currentWeather;
    
    public delegate void OnWeatherChange(WeatherType newWeather);
    public static event OnWeatherChange WeatherChanged;

    // ✅ Make ChangeWeather() public so other scripts (like EnemySpawner) can access it
    public void ChangeWeather()
    {
        currentWeather = (WeatherType)Random.Range(0, System.Enum.GetValues(typeof(WeatherType)).Length);
        Debug.Log($"🌦️ New Weather: {currentWeather}");
        WeatherChanged?.Invoke(currentWeather);
    }
}
