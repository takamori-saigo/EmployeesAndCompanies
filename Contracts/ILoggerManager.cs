namespace Contracts;

public interface ILoggerManager
{
    void LogInfo(string message);
    void LogWarn(String message);
    void LogDebug(String message);
    void LogError(String message);
}