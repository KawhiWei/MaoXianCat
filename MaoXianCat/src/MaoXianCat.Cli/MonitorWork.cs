using MaoXianCat.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MaoXianCat.Cli;

public class MonitorWork : BackgroundService
{
    private readonly ILogger<MonitorWork> _logger;
    private readonly IOptionsMonitor<FilePorterOptions> _optionsAccessor;
    private readonly IServiceProvider _serviceProvider;
    public MonitorWork(IOptionsMonitor<FilePorterOptions> optionsMonitor, ILogger<MonitorWork> logger,IServiceProvider serviceProvider)
    {
        _optionsAccessor = optionsMonitor;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        
        
        Task.Factory.StartNew(Watching);
        
        
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                var freshOptions = _optionsAccessor.CurrentValue;
                if (freshOptions.OptionsIsValid() == false)
                {
                    _logger.LogError("Options are not valid");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    continue;
                }

                FilePorterMonitor(freshOptions);

                await Task.Delay(freshOptions.LoopFileWatchInterval, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MonitorWork error");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    private void FilePorterMonitor(FilePorterOptions freshOptions)
    {
        if (Directory.Exists(freshOptions.MonitorDirectory) == false)
        {
            _logger.LogWarning("Monitor directory {FreshOptionsMonitorDirectory} does not exist",
                freshOptions.MonitorDirectory);
            return;
        }

        if (Directory.Exists(freshOptions.DestinationDirectory) == false)
        {
            Directory.CreateDirectory(freshOptions.DestinationDirectory);
        }

        var directories = Directory.GetDirectories(freshOptions.MonitorDirectory);
        foreach (var directory in directories)
        {
            // copy directory all files to destination directory
            CopyDirectory(directory, Path.Combine(freshOptions.DestinationDirectory, Path.GetFileName(directory)),
                true);
            Directory.Delete(directory, true);
        }

        var files = Directory.GetFiles(freshOptions.MonitorDirectory);
        foreach (var file in files)
        {
            File.Copy(file, Path.Combine(freshOptions.DestinationDirectory, Path.GetFileName(file)));
            File.Delete(file);
        }

        _logger.LogInformation(
            "MonitorWork executed at: {Time：yyyy-MM-dd HH:mm:ss}, move {Directories} directories and {Files} files",
            DateTimeOffset.Now, directories.Length, files.Length);
    }

    static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }


    private void Watching()
    {
        using (var rootScope=_serviceProvider.CreateScope())
        {
            var watch= rootScope.ServiceProvider.GetRequiredService<Watch>();
            watch.Watching();
            
            while (true)
            {
                Thread.Sleep(100000);
            }
            
        }
    }
}