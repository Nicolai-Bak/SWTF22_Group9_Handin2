using System.IO;
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
        if (File.Exists(LogToTxt._filePath))
        {
            string previousLog = ReadLog();
        
            _uut.LogDoorLocked(1);
        
            string newLog = ReadLog();
        
            Assert.That(newLog.Length, Is.GreaterThan(previousLog.Length));
        }
        else
        {
            _uut.LogDoorLocked(1);
            
            Assert.IsTrue(File.Exists(LogToTxt._filePath));
        }
    }
    
    [Test]
    public void SimulateLogDoorUnlocked_FileLengthIncreased()
    {
        if (File.Exists(LogToTxt._filePath))
        {
            string previousLog = ReadLog();
        
            _uut.LogDoorUnlocked(1);
        
            string newLog = ReadLog();
        
            Assert.That(newLog.Length, Is.GreaterThan(previousLog.Length));
        }
        else
        {
            _uut.LogDoorUnlocked(1);
            
            Assert.IsTrue(File.Exists(LogToTxt._filePath));
        }
    }

    private string ReadLog()
    {
        string log = "";
        
        using (StreamReader r = File.OpenText(LogToTxt._filePath))
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