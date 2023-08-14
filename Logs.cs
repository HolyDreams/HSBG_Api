namespace HS_BG_Api
{
    public class Logs
    {
        public static void Log(string textLog)
        {
            var dirPath = Path.Combine("logs", DateTime.Now.ToString("MM.yyг"));
            Directory.CreateDirectory(dirPath);
            var path = Path.Combine(dirPath, DateTime.Now.ToString("dd") + ".txt");
            var logMessage = DateTime.Now.ToString("[HH:mm:ss]") + " " + textLog;
            if (!File.Exists(path))
                File.WriteAllText(path, logMessage);
            else
            {
                var alreadyText = File.ReadAllLines(path).ToList();
                alreadyText.Add(logMessage);
                File.WriteAllLines(path, alreadyText);
            }

            Console.WriteLine(logMessage);
        }
    }
}