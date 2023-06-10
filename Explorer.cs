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
    public async Task MoveFileFromOneDirToAnotherAsync()
    {
        string parent = "", child ="", parentFull = ""; List<string> export = new List<string>();
        var sw = new Stopwatch();

        if (!Directory.Exists(VideoDirectory) || !File.Exists(FilePath)) throw new Exception();

        ItemHeader = new Dictionary<int, string>();
        int counterMusic = 0, count = 1;


        if (File.Exists(FilePath)) ArrayFilePath = File.ReadAllLines(FilePath); // List item file

        ItemHeader =  GetItemFromFile(ArrayFilePath);
        int number = ItemHeader.Count();

        var musicFullPath = this.GetFullPath(FilePath, VideoDirectory);
        //this.LogsDataFromOriginAfterMove(musicFullPath, ItemHeader,  FilePath);

        for (int i = 0; i < number - 1; i++)
        {
            if (count < number)
            {
                // System.Console.WriteLine($"{i} - {count}");
                KeyValuePair<int, string> headers = ItemHeader.ElementAt(i);
                for (int j = 0; j < musicFullPath.Count; j++)
                {
                    counterMusic = headers.Value.Where(x => x =='#').Count(); 
                    
                    KeyValuePair<int, string> musicElement = musicFullPath.ElementAt(j);
                    if ((musicElement.Key >= headers.Key && musicElement.Key <= ItemHeader.ElementAt(count).Key))
                    {
                        //System.Console.WriteLine(musicElement.Key);
                        int value = musicElement.Key;
                        var unique = musicFullPath.Where(x => x.Key == value);

                        if (counterMusic == 3)
                        {
                            parent = headers.Value;
                            parent = parent.Replace('#', ' ').Trim();

                            // parentFull = $"{VideoDirectory}\\{parent}";
                            parentFull = Path.Combine(VideoDirectory, parent);

                            if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull);

                            FileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));

                            System.Console.WriteLine(headers.Value);
                            System.Console.WriteLine("copy of ................. {0}", musicElement.Value);

                            export.Add($"{headers.Value}");
                            export.Add($"{Path.GetFileName(musicElement.Value)}");
                            try
                            {
                                sw.Start();
                                await Task.Delay(2000);
                                //File.Copy(musicElement.Value, FileDestination,false);   
                                System.Console.WriteLine("copying  to--------------{0}  in {1} 's Elapsed time",FileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                sw.Stop();
                                sw.Reset();
                                
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e);;
                            }
                            
                        }

                        if (counterMusic == 4)
                        {
                            //System.Console.WriteLine(counter.Value[i]);
                            child = headers.Value;
                            child = child.Replace('#',' ').Trim();
                            parentFull = Path.Combine(VideoDirectory, parent, child);
                            //System.Console.WriteLine("child ---------------{0}",parentFull);
                            if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull);

                            FileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));
                            System.Console.WriteLine(headers.Value);
                            System.Console.WriteLine("copy of--------------{0}", musicElement.Value);

                            export.Add($"{headers.Value}");
                            export.Add($"{Path.GetFileName(musicElement.Value)}");
                            try
                            {
                                sw.Start();
                                //File.Copy(musicElement.Value, FileDestination,false);  
                                await Task.Delay(2000);
                                System.Console.WriteLine("copying  to--------------{0} in {1} 's Elapsed time",FileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                sw.Stop();
                                sw.Restart();

                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine(e);
                            }
                        }

                    }
                }

            }
            count++;

        }
        var message = await this.LogsDataFromOriginAfterMove(export, FilePath);
        await Task.Delay(1000);
        System.Console.WriteLine(message);
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