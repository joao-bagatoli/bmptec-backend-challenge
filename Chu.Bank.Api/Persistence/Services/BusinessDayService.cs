using Chu.Bank.Api.Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Chu.Bank.Api.Persistence.Services;

public class BusinessDayService : IBusinessDayService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public BusinessDayService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<bool> IsBusinessDayAsync(DateTime date)
    {
        if (IsWeekend(date)) return false;
        if (await IsHolidayAsync(date)) return false;
        return true;
    }

    private static bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    private async Task<bool> IsHolidayAsync(DateTime date)
    {
        var year = date.Year;
        var holidays = await _cache.GetOrCreateAsync($"holidays-{year}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            var response = await _httpClient.GetAsync($"https://brasilapi.com.br/api/feriados/v1/{year}");
            return await response.Content.ReadFromJsonAsync<List<HolidayDto>>();
        });
        return holidays!.Any(h => h.Date == date.ToString("yyyy-MM-dd"));
    }

    private record HolidayDto(string Date, string Name, string Type);
}
