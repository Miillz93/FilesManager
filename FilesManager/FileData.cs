namespace filesmanager;

public class FileData
{
    public string FileSource { get; set;}
    public string FolderSource { get; set;}
    public string FileDestination { get; set; }
    public string FolderDestination { get; set; }
    public string LogFileSource { get; set; }
    public string logFolderSource { get; set; }
    public string LogFileDestination { get; set; }
    public string logFolderDestination { get; set; }
    public string[]? NameCombined { get; set; }
    public string[]? FolderCombinedSource {get; set;}


}
