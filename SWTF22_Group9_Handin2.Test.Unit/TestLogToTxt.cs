using System.IO;
using System.Linq;
using NUnit.Framework;
using SWTF22_Group9_Handin2_ClassLibrary;

namespace SWTF22_Group9_Handin2.Test.Unit;

[TestFixture]
public class TestLogToTxt
{
    private LogToTxt _uut;
    [SetUp]
    public void Setup()
    {
        _uut = new LogToTxt();
    }
    
    [Test]
    public void SimulateLogDoorLocked_FileLengthIncreased()
    {
        string previousLog = ReadLog();
        
        _uut.LogDoorLocked(1);
        
        string newLog = ReadLog();
        
        Assert.That(newLog.Length, Is.GreaterThan(previousLog.Length));
    }
    
    [Test]
    public void SimulateLogDoorUnlocked_FileLengthIncreased()
    {
        string previousLog = ReadLog();
        
        _uut.LogDoorUnlocked(1);
        
        string newLog = ReadLog();
        
        Assert.That(newLog.Length, Is.GreaterThan(previousLog.Length));
    }

    private string ReadLog()
    {
        string log = "";
        
        using (StreamReader r = File.OpenText("log.txt"))
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                log = log.Insert(log.Length, line);
            }
        }
        
        return log;
    }
}