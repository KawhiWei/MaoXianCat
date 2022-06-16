using MaoXianCat.Cli;
using Microsoft.Extensions.Options;

namespace MaoXianCat.Files;

public class MoveFile:IMoveFile
{
    private readonly IOptionsMonitor<FilePorterOptions> _optionsAccessor;
    public MoveFile(IOptionsMonitor<FilePorterOptions> optionsMonitor)
    {
        _optionsAccessor = optionsMonitor;
    }
    
    public  virtual  void OnCreateFile(object sender, FileSystemEventArgs e)
    {
        Task.Delay(TimeSpan.FromSeconds(60));
        
        var fileTarget ="/Users/wangzewei/Documents/Code/download/mx-wc";
        Console.WriteLine("新增:" + e.ChangeType + ";" + e.FullPath + ";" + e.Name);
    }  
    public  virtual void OnChangeFile(object sender, FileSystemEventArgs e)
    {
        Task.Delay(TimeSpan.FromSeconds(60));
        Console.WriteLine("新增:" + e.ChangeType + ";" + e.FullPath + ";" + e.Name);
    }  
}