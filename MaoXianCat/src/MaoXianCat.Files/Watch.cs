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
        this._watcher = new FileSystemWatcher();
        _watcher.Path = "";
        _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.DirectoryName;
        _watcher.IncludeSubdirectories = true;
        _watcher.Created += new FileSystemEventHandler(_moveFile.OnCreateFile);
        _watcher.Changed += new FileSystemEventHandler(_moveFile.OnChangeFile);
    }
}