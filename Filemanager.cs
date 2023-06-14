using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
internal static class filemanager {

    public static async Task CopyFileFromSourceFileAsync(string videoDirectory, string filePath, string fileDestination)
    {
        string parent = "", child ="", parentFull = ""; 
        List<string> export = new List<string>();
        string[] arrayOfFilePath = new string[]{};

        var sw = new Stopwatch();

        if (!Directory.Exists(videoDirectory) || !File.Exists(filePath)) throw new Exception();

        var itemHeader = new Dictionary<int, string>();
        int counterMusic = 0, count = 1;

        try { arrayOfFilePath = File.ReadAllLines(filePath); }
        catch (System.Exception) { throw; }

        itemHeader =  GetItemFromFile(arrayOfFilePath.ToArray());
        int number = itemHeader.Count();

        var musicFullPath = GetFullPath(filePath, videoDirectory, arrayOfFilePath);
        //this.LogsDataFromOriginAfterMove(musicFullPath, ItemHeader,  FilePath);

        for (int i = 0; i < number - 1; i++)
        {
            if (count < number)
            {
                KeyValuePair<int, string> headers = itemHeader.ElementAt(i);

                for (int j = 0; j < musicFullPath.Count; j++)
                {
                    counterMusic = headers.Value.Where(x => x =='#').Count(); 
                    KeyValuePair<int, string> musicElement = musicFullPath.ElementAt(j);

                    if ((musicElement.Key >= headers.Key && musicElement.Key <= itemHeader.ElementAt(count).Key))
                    {
                        int value = musicElement.Key;
                        var unique = musicFullPath.Where(x => x.Key == value);

                        if (counterMusic == 3)
                        {
                            parent = headers.Value;
                            parent = parent.Replace('#', ' ').Trim();
                            parentFull = Path.Combine(videoDirectory, parent);
                            
                            try { if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                            catch (System.Exception) { throw; }

                            fileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));
                            System.Console.WriteLine("copy of ................. {0}", musicElement.Value);

                            export.Add($"{headers.Value}");
                            export.Add($"{Path.GetFileName(musicElement.Value)}");
                            try
                            {
                                sw.Start();
                                await Task.Delay(2000);
                                //File.Copy(musicElement.Value, FileDestination,false);   
                                System.Console.WriteLine("copying  to--------------{0}  in {1} 's Elapsed time",fileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
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
                            parentFull = Path.Combine(videoDirectory, parent, child);
                            //System.Console.WriteLine("child ---------------{0}",parentFull);
                            try { if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                            catch (System.Exception){ throw; }

                            fileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));
                            System.Console.WriteLine(headers.Value);
                            System.Console.WriteLine("copy of--------------{0}", musicElement.Value);

                            export.Add($"{headers.Value}");
                            export.Add($"{Path.GetFileName(musicElement.Value)}");
                            try
                            {
                                sw.Start();
                                //File.Copy(musicElement.Value, FileDestination,false);  
                                await Task.Delay(2000);
                                System.Console.WriteLine("copying  to--------------{0} in {1} 's Elapsed time",fileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
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
        var message = await LogsDataFromOriginAfterCopy(export, filePath, fileDestination);
        await Task.Delay(1000);
        System.Console.WriteLine(message);
    }
    
    //Get List item Matching from logs
    public static async Task<string> LogsDataFromOriginAfterCopy(List<string> musics, string logFolder,string fileDestination){
        
        if(musics == null  || logFolder == null || fileDestination ==  null) throw new ArgumentNullException();
        string message = "";
        string currentRootDirectory = Path.GetDirectoryName(logFolder); 
        string rootLogDirectory = Path.Combine(currentRootDirectory, "logs");
        
        try { if(!Directory.Exists(rootLogDirectory)) Directory.CreateDirectory(rootLogDirectory); }
        catch (System.Exception) { throw; }
        
        var logFile = Path.Combine(rootLogDirectory, Path.GetFileName(fileDestination));

        // logFile = Path.ChangeExtension(logFile, DateTime.Now.ToString("dd-MM-yyyy")+".txt");
        
        try
        { 
            
            if(File.Exists(logFile)) {
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine("");
                    sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                    foreach (var item in musics)
                    {
                        sw.WriteLine(item);
                    }
                    sw.WriteLine("");

                }
            }else
                File.WriteAllLines(logFile, musics);

            await Task.Delay(100);
            message = "*************Texts added to Log File successfully";
            //File.Copy(OldMusicFile, fileDestination);
        }
        catch (System.Exception){ throw; }
       
        return  message;
    }

    public static string ExportEmbedFiles(string mainPath, string destFile){
        string message = "";

        return message;
    }
    public static void GetFilesWithInFoldersAndSubFolders(string mainPath){
        
        if(string.IsNullOrEmpty(mainPath)) throw new ArgumentNullException();

        DirectoryInfo[] directories; var items = new List<string>();
        Dictionary<string, List<string>> fileItems = new System.Collections.Generic.Dictionary<string, List<string>>();
        
        try
        {
            var directory = new DirectoryInfo(mainPath);
            directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            //root= Directory.GetDirectories(mainPath,"*", searchOption: SearchOption.AllDirectories);
            foreach (var item in directories)
            {
                //System.Console.WriteLine(item.FullName);
                foreach (var files in item.EnumerateFiles())
                {
                    items?.Add(files.Name.Replace("🔥", string.Empty));
                }
                fileItems.TryAdd(item.FullName, items);    
                items = new();
            }
        }
        catch (UnauthorizedAccessException){}

    }

    /*Get a list of item in a specific folder when items match each other*/
    public static Dictionary<int, string> GetFullPath(string filePath, string videoDirectory, string[] arrayOfFullPath) {
        if(String.IsNullOrEmpty(videoDirectory) || String.IsNullOrEmpty(filePath)) throw new ArgumentNullException();
        Dictionary<int, string> musicFullPath = new();

        if(Directory.Exists(videoDirectory)){
            var clips = Directory.GetFiles(videoDirectory);   
            for (int h = 0; h < arrayOfFullPath.Length; h++){
                for (int i = 0; i < clips.Length; i++){
                        string extension = Path.GetFileNameWithoutExtension(clips[i]);
 
                        if(arrayOfFullPath[h].Contains(extension, StringComparison.OrdinalIgnoreCase)){
                            musicFullPath.Add(h, Path.GetFullPath(clips[i])); 
                        }
                } 
            }
            
        }
        return musicFullPath;
    }

    /*Get only headers from input file*/ 
    private static  Dictionary<int, string> GetItemFromFile(string[] arrayFilePath)
    {
        Dictionary<int, string> itemHeader = new();

        if(arrayFilePath is null || arrayFilePath.Length == 0) throw new ArgumentNullException("Array is null");
        
        for (int i = 0; i < arrayFilePath.Length; i++)
        {
            if (arrayFilePath[i].StartsWith('#')) itemHeader.Add(i, arrayFilePath[i]);
        }

        return itemHeader;
    }


}