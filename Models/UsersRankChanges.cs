namespace HS_BG_Api.Models
{
    public class UsersRankChanges
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public List<RankChanges> rankChanges { get; set; }
    }
    public class RankChanges
    {
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}