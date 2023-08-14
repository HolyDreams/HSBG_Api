using HS_BG_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HS_BG_Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Users>>> Get(string server = "eu")
        {
            if (server == string.Empty)
            {
                Logs.Log("[api/users] Не ввели сервер");
                return StatusCode(400, "Не ввели сервер");
            }
            else if (server != "eu" && server != "us")
            {
                Logs.Log("[api/users] Неправильный сервер");
                return StatusCode(404, "Неправильный сервер");
            }
            var _users = Methods.GetUsers(server);

            return Ok(_users);
        }
        [HttpGet]
        [Route("history")]
        public async Task<ActionResult<List<UsersRankChanges>>> Get(string ids, DateTime dateStart, DateTime? dateEnd = null, string? server = "eu", string? Type = "day")
        {
            if (server != "eu" && server != "us")
            {
                Logs.Log("[api/users/history] Неправильный сервер");
                return StatusCode(404, "Неправильный сервер");
            }
            else if (ids.Length == 0)
            {
                Logs.Log("[api/users/history] Нужно хотябы 1 ID для поиска");
                return StatusCode(404, "Нужно хотябы 1 ID для поиска");
            }
            else if (dateStart > dateEnd)
            {
                Logs.Log("[api/users/history] Дата старта больше даты конца!");
                return StatusCode(404, "Дата старта больше даты конца!");
            }
            else if (Type != "day" && Type != "hour")
            {
                Logs.Log("[api/users/history] Неправильный тип поиска");
                return StatusCode(404, "Неправильный тип поиска");
            }

            if (dateStart < new DateTime(2023, 8, 14))
                dateStart = new DateTime(2023, 8, 14);
            if (dateEnd > DateTime.Now || dateEnd == null)
                dateEnd = DateTime.Now;

            List<int> idsInt = new List<int>();
            foreach (var id in ids.Split(',').Where(a => int.TryParse(a, out int n)))
                idsInt.Add(int.Parse(id));

            if (idsInt.Count < 1)
            {
                Logs.Log("[api/users/history] Неверное значение ID");
                return StatusCode(404, "Неверное значение ID");
            }
            else if (idsInt.Any(a => a < 1 || a > 200))
            {
                Logs.Log("[api/users/history] Имеется ID за границами диапазона возможного поиска! Введите от 1 до 200");
                return StatusCode(404, "Имеется ID за границами диапазона возможного поиска! Введите от 1 до 200");
            }
            return Methods.GetUsersChanges(server, idsInt, dateStart, dateEnd, Type);
        }
    }
}
