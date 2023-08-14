using Npgsql;
using System.Data;

namespace HS_BG_Api
{
    public class SQLRequest
    {
        internal static DataTable PostgreSQL(string sqlQuery)
        {
            try
            {
                string connect = $@"sqlConnection";
                NpgsqlConnection nc = new NpgsqlConnection(connect);
                nc.Open();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlQuery, nc);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                nc.Close();
                return dt;
            }
            catch (Exception ex)
            {
                Logs.Log(ex.Message);
                return null;
            }
        }
    }
}
