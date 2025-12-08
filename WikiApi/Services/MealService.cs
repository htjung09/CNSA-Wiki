using System.Net.Http;
using System.Threading.Tasks;

namespace WikiApi.Services
{
    public class MealService
    {
        private readonly HttpClient _http;

        public MealService(HttpClient http)
        {
            _http = http;
            // 안전을 위해 User-Agent 추가 (일부 서버가 필요로 함)
            _http.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (compatible)");
        }

        public async Task<string> GetMealAsync(string officeCode, string schoolCode, string date, string apiKey)
        {
            string url =
                $"https://open.neis.go.kr/hub/mealServiceDietInfo" +
                $"?KEY={Uri.EscapeDataString(apiKey)}&Type=json" +
                $"&ATPT_OFCDC_SC_CODE={Uri.EscapeDataString(officeCode)}" +
                $"&SD_SCHUL_CODE={Uri.EscapeDataString(schoolCode)}" +
                $"&MLSV_YMD={Uri.EscapeDataString(date)}";

            // GetAsync로 Response 확인 후 내용 반환
            using var res = await _http.GetAsync(url);
            var body = await res.Content.ReadAsStringAsync();

            // 상태코드나 컨텐츠타입이 JSON이 아니면 body를 그대로 반환 (프론트에서 검사)
            return body;
        }
    }
}