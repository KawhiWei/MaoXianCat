namespace MaoXianCat.Cli;

public class FilePorterOptions
{
    public string MonitorDirectory { get; set; } = string.Empty;

    public string DestinationDirectory { get; set; } = string.Empty;
    
    public TimeSpan LoopFileWatchInterval { get; set; } = TimeSpan.FromMinutes(0.5);
    
    public bool OptionsIsValid()
    {
        return !string.IsNullOrEmpty(MonitorDirectory) && !string.IsNullOrEmpty(DestinationDirectory);
    }
}