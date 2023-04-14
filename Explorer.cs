using System.Linq;
using System.Text.RegularExpressions;
internal class Explorer {


    private string ? FilePath {get;set;}
    private string ? VideoDirectory {get;set;}
    private string ? FileDestination {get; set;}
    private string ? LogFile {get;set;}
    private string ? RootLogDirectory {get;set;}
    private string [] ArrayFilePath {get;set;}
    private string [] ArrayVideoDirectory {get; set;}
    private Dictionary<int, string> ItemHeader {get;set;}


    public Explorer(string rootfilePath, string rootVideoDirectory){    
        FilePath = rootfilePath;
        VideoDirectory = rootVideoDirectory;
    }

    /*Get a list of item in a specific folder when items match each other*/
    public Dictionary<int, string> GetFullPath(string filePath, string videoDirectory) {
        if(String.IsNullOrEmpty(videoDirectory) || String.IsNullOrEmpty(filePath)) throw new ArgumentNullException();
        Dictionary<int, string> musicFullPath = new Dictionary<int, string>();

        if(Directory.Exists(videoDirectory)){
            var clips = Directory.GetFiles(videoDirectory);   
            for (int h = 0; h < ArrayFilePath.Length; h++){
                for (int i = 0; i < clips.Length; i++){
                        string extension = Path.GetFileNameWithoutExtension(clips[i]);
                        //System.Console.WriteLine("array 1------------------{0}-----{1}", arrays[i], arrays[i].Contains(extension, StringComparison.OrdinalIgnoreCase));
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