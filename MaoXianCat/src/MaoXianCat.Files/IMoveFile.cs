namespace MaoXianCat.Files;

public interface IMoveFile
{
    /// <summary>
    /// 新建文件或文件夹监听事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnCreateFile(object sender, FileSystemEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnChangeFile(object sender, FileSystemEventArgs e);
}