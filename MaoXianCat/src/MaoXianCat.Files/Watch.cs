namespace MaoXianCat.Files;

public class Watch
{
    private FileSystemWatcher _watcher;
    private  readonly  IMoveFile _moveFile;

    public Watch(IMoveFile moveFile)
    {
        _moveFile = moveFile;
    }

    public  void Watching()
    {
        var directory= Path.Combine("/Users/wangzewei/Documents/Code/download/mx-wc");

        if (Directory.Exists(directory))
        {
            this._watcher = new FileSystemWatcher(directory);
            _watcher.Path = directory;
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.DirectoryName;
            _watcher.IncludeSubdirectories = true;
            _watcher.Created += new FileSystemEventHandler(_moveFile.OnCreateFile);    
        }
        
    }
}