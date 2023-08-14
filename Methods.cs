using HS_BG_Api.Models;
using System.Data;

namespace HS_BG_Api
{
    public class Methods
    {
        public static List<Users> GetUsers(string server)
        {
            string sqlQuery = $@"SELECT *
                                 FROM hsbg_{server}";
            var res = SQLRequest.PostgreSQL(sqlQuery);
            var resList = (from DataRow a in res.Rows
                           select new Users
                           {
                               Name = (string)a["name"],
                               Rating = (int)a["rating"],
                               Rank = (int)a["rank"]
                           }).OrderBy(a => a.Rank).ToList();
            return resList;
        }
        public static List<UsersRankChanges> GetUsersChanges(string server, List<int> ids, DateTime dateStart, DateTime? dateEnd, string Type)
        {
            string sqlQuery = $@"SELECT name,
                                        rank
                                 FROM hsbg_{server}
                                 WHERE rank IN ({string.Join(",", ids)})";
            var res = SQLRequest.PostgreSQL(sqlQuery);
            var resList = (from DataRow a in res.Rows
                           select new
                           {
                               Name = (string)a["name"],
                               Rank = (int)a["rank"]
                           }).ToList();
            sqlQuery = $@"SELECT *
                          FROM hsbg_{server}_changes
                          WHERE name IN ('{string.Join("','", resList.Select(a => a.Name))}') AND
                                date >= '{dateStart}' AND
                                date <= '{dateEnd}'";
            res = SQLRequest.PostgreSQL(sqlQuery);
            var changesList = (from DataRow a in res.Rows
                               select new
                               {
                                   Name = (string)a["name"],
                                   Rating = (int)a["rating"],
                                   Date = (DateTime)a["date"]
                               }).ToList().GroupBy(a => new { a.Name, a.Date.Year, a.Date.Month, a.Date.Day, a.Date.Hour }).Select(a => new
                               {
                                   Name = a.Key.Name,
                                   Rating = a.First().Rating,
                                   Date = new DateTime(a.Key.Year, a.Key.Month, a.Key.Day, a.Key.Hour, 0, 0)
                               });
            if (Type == "day")
            {
                changesList = changesList.Where(a => a.Date.Hour == 0).ToList();
            }
            var result = (from a in resList
                          select new UsersRankChanges
                          {
                              Name = a.Name,
                              Rank = a.Rank,
                              rankChanges = new List<RankChanges>()
                          }).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                foreach (var item in changesList.Where(a => a.Name == result[i].Name))
                {
                    result[i].rankChanges.Add(new RankChanges()
                    {
                        Rating = item.Rating,
                        Date = item.Date
                    });
                }
                result[i].Name = result[i].Name.Replace(" (1)", "");
            }
            return result.OrderBy(a => a.Rank).ToList();
        }
    }
}
