using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
internal class Explorer {


    private string ? FilePath {get;set;}
    private string ? VideoDirectory {get;set;}
    private string ? FileDestination {get; set;}
    private string ? LogFile {get;set;}
    private string ? RootLogDirectory {get;set;}
    private string [] ArrayFilePath {get;set;}
    private string [] ArrayVideoDirectory {get; set;}
    private Dictionary<int, string> ItemHeader {get;set;}

    public Explorer(string filePath, string videoDirectory){
        FilePath = filePath;
        VideoDirectory = videoDirectory;
    }
    
    //Get List item Matching from logs
    public async Task<string> LogsDataFromOriginAfterMove(List<string> musics, string OldMusicFile){
        
        if(musics == null  || OldMusicFile ==  null) throw new ArgumentNullException();
        string message = "";
        string currentRootDirectory = Path.GetDirectoryName(OldMusicFile); 
        RootLogDirectory = Path.Combine(currentRootDirectory, "logs");
        
        if(!Directory.Exists(RootLogDirectory)) Directory.CreateDirectory(RootLogDirectory);
        LogFile = Path.Combine(RootLogDirectory, Path.GetFileName(OldMusicFile));
        
        LogFile = Path.ChangeExtension(LogFile, DateTime.Now.ToString("dd-MM-yyyy")+".txt");

        try
        { 
            File.WriteAllLines(LogFile, musics);

            await Task.Delay(1000);
            message = "*************Texts added to Log File successfully";
            //File.Copy(OldMusicFile, fileDestination);
        }
        catch (System.Exception)
        {
            throw;
        }
       
        return  message;
    }

    public void GetFilesWithInFoldersAndSubFolders(string? mainPath){
        
        mainPath = @"H:\"; string []root; DirectoryInfo[] directories; var items = new List<string>();
        
        try
        {
            var directorie = new DirectoryInfo(mainPath);
            var demo = directorie.GetDirectories("*", SearchOption.AllDirectories);
            //root= Directory.GetDirectories(mainPath,"*", searchOption: SearchOption.AllDirectories);

            foreach (var item in demo)
            {
                System.Console.WriteLine(item);
            }
        }
        catch (UnauthorizedAccessException){}

       System.Console.WriteLine("end script....");

    }

    /*Get a list of item in a specific folder when items match each other*/
    public Dictionary<int, string> GetFullPath(string filePath, string videoDirectory) {
        if(String.IsNullOrEmpty(videoDirectory) || String.IsNullOrEmpty(filePath)) throw new ArgumentNullException();
        Dictionary<int, string> musicFullPath = new();

        if(Directory.Exists(videoDirectory)){
            var clips = Directory.GetFiles(videoDirectory);   
            for (int h = 0; h < ArrayFilePath.Length; h++){
                for (int i = 0; i < clips.Length; i++){
                        string extension = Path.GetFileNameWithoutExtension(clips[i]);
 
                        if(ArrayFilePath[h].Contains(extension, StringComparison.OrdinalIgnoreCase)){
                            musicFullPath.Add(h, Path.GetFullPath(clips[i])); 
                        }
                } 
            }
            
        }
        return musicFullPath;
    }

    /*Get only headers from input file*/ 
    private Dictionary<int, string> GetItemFromFile(string [] ArrayFilePath)
    {
        if(ArrayFilePath is null || ArrayFilePath.Length == 0) throw new ArgumentNullException("Array is null");
        
        for (int i = 0; i < ArrayFilePath.Length; i++)
        {
            if (ArrayFilePath[i].StartsWith('#')) ItemHeader.Add(i, ArrayFilePath[i]);
        }

        return ItemHeader;
    }


}