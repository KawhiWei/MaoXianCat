namespace MaoXianCat.Files;

public class MoveFile:IMoveFile
{
    
    public  virtual  void OnCreateFile(object sender, FileSystemEventArgs e)
    {
        Task.Delay(TimeSpan.FromSeconds(60));
        
        var fileTarget ="";
        Console.WriteLine("新增:" + e.ChangeType + ";" + e.FullPath + ";" + e.Name);
    }  
    public  virtual void OnChangeFile(object sender, FileSystemEventArgs e)
    {
        Task.Delay(TimeSpan.FromSeconds(60));
        Console.WriteLine("新增:" + e.ChangeType + ";" + e.FullPath + ";" + e.Name);
    }  
}