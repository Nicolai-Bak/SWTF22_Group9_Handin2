namespace SWTF22_Group9_Handin2_ClassLibrary;

public class LogToTxt : ILog
{
    static public string _filePath = "log.txt";
    public void LogDoorLocked(int id)
    {
        Log($"Door locked with id: {id}");
    }

    public void LogDoorUnlocked(int id)
    {
        Log($"Door unlocked with id: {id}");
    }

    // Taken from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-open-and-append-to-a-log-file 
    private void Log(string logMessage)
    {
        using (StreamWriter w = File.AppendText(_filePath))
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }
    }
}