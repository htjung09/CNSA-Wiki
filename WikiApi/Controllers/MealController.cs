using Microsoft.AspNetCore.Mvc;
using WikiApi.Services;

namespace WikiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealController : ControllerBase
    {
        private readonly MealService _mealService;
        private readonly IConfiguration _config;

        public MealController(MealService mealService, IConfiguration config)
        {
            _mealService = mealService;
            _config = config;
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> GetMeal(string date)
        {
            string apiKey = _config["Neis:ApiKey"];
            string office = _config["Neis:OfficeCode"];
            string school = _config["Neis:SchoolCode"];

            // 안전하게 MealService 통해 호출 (MealService가 key 넣어줌)
            string response = await _mealService.GetMealAsync(office, school, date, apiKey);

            // 콘솔 로그(디버깅)
            Console.WriteLine("===== 응답 데이터 =====");
            Console.WriteLine(response);

            // 클라이언트에 그대로 JSON으로 응답
            return Content(response, "application/json");
        }
    }
}