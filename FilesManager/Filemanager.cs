using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Shared;


namespace Manager;

public static class FileManager
{

    public static async Task CopyOrMoveFileFromSourceFileAsync(SampleData data)
    {
        string parent = "", child = "", parentFull = ""; string message = "Finished................";
        List<string> musicsToExport = new();
        string[] arrayOfListElementFromFile = Array.Empty<string>();

        var sw = new Stopwatch();

        string FilePath = Path.Combine(data.PathSource, data.FilePath);

        if (!Directory.Exists(data.VideoPath) || !File.Exists(FilePath)) 
            throw new FileNotFoundException() ?? throw new DirectoryNotFoundException();

        var itemForHeader = new Dictionary<int, string>();
        int counterMusic = 0, count = 1;

        try { arrayOfListElementFromFile = File.ReadAllLines(FilePath) ; }  // Ok var
        catch (Exception) { throw; }

        itemForHeader = GetItemHeaderHashFromFile(arrayOfListElementFromFile); // ok
        int number = itemForHeader.Count; // ok

        var musicFullPath = GetFullPath(data.VideoPath, arrayOfListElementFromFile);
        //this.LogsDataFromOriginAfterMove(musicFullPath, ItemHeader,  FilePath);

        // foreach (var item in musicFullPath){
        //     Console.WriteLine("music matching item vkey {0} ----- value {1}",item.Key, item.Value);
        // }

        for (int i = 0; i < number - 1; i++)
        {
            if (count < number)
            {
                KeyValuePair<int, string> headers = itemForHeader.ElementAt(i);

                for (int j = 0; j < musicFullPath.Count; j++)
                {
                    counterMusic = headers.Value.Where(x => x == '#').Count();
                    KeyValuePair<int, string> musicElement = musicFullPath.ElementAt(j);

                    if (musicElement.Key >= headers.Key && musicElement.Key <= itemForHeader.ElementAt(count).Key)
                    {
                        int value = musicElement.Key;
                        var unique = musicFullPath.Where(x => x.Key == value);

                        if (counterMusic == 3)
                        {
                            parent = headers.Value;
                            parent = parent.Replace('#', ' ').Trim();
                            parentFull = Path.Combine(data.VideoPath, parent);
                            Console.WriteLine(counterMusic);
                            /*try { if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                            catch (Exception) { throw; }*/
/*
                            data.FileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));
                            Console.WriteLine("copy of ................. {0}", musicElement.Value);*/

                            musicsToExport.Add($"{headers.Value}");
                            musicsToExport.Add($"{Path.GetFileName(musicElement.Value)}");
                            /*try
                            {
                                sw.Start();
                                await Task.Delay(2000);
                                //if(data.Action =="copy") File.Copy(musicElement.Value, data.FileDestination,false); 
                                //if(data.Action =="move") File.Copy(musicElement.Value, data.FileDestination,false);   

                                Console.WriteLine("copying  to--------------{0}  in {1} 's Elapsed time",data.FileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                sw.Stop();
                                sw.Reset();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }*/
                        }

                        if (counterMusic == 4)
                        {
                            //System.Console.WriteLine(counter.Value[i]);
                            child = headers.Value;
                            child = child.Replace('#', ' ').Trim();
                            parentFull = Path.Combine(data.VideoPath, parent, child);
                            //Console.WriteLine(parentFull);
                            //System.Console.WriteLine("child ---------------{0}",parentFull);
                            /*try { if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                            catch (Exception) { throw; }*/
/*
                            data.FileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));
                            Console.WriteLine(headers.Value);
                            Console.WriteLine("copy of--------------{0}", musicElement.Value);*/

                            // musicsToExport.Add($"{headers.Value}");
                            // musicsToExport.Add($"{Path.GetFileName(musicElement.Value)}");
                            /*try
                            {
                                sw.Start();
                                
                                //if(data.Action =="copy") File.Copy(musicElement.Value, data.FileDestination,false);
                                //if(data.Action =="move") File.Copy(musicElement.Value, data.FileDestination,false);
                                //
                                await Task.Delay(2000);
                                Console.WriteLine("copying  to--------------{0} in {1} 's Elapsed time", data.FileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                sw.Stop();
                                sw.Restart();

                            }
                            catch (Exception e) { Console.WriteLine(e); };*/
                        }

                    }
                }

            }
            count++;

        }
        // foreach (var item in musicsToExport)
        // {
        //     Console.WriteLine("item {0}", item);
        // }
        //var message = await LogsDataFromOriginAfterCopy(musicsToExport, data);
        await Task.Delay(1000);
        Console.WriteLine(message);
    }

    //Get List item Matching from logs
    public static async Task<string> LogsDataFromOriginAfterCopy(List<string> musicsToExport,  SampleData data)
    {
        string message;

        if (musicsToExport == null || data.LogFileDestination == null || data.LogPathDestination == null) 
            throw new ArgumentNullException("data is null");

        try { if (!Directory.Exists(data.LogPathDestination)) Directory.CreateDirectory(data.LogPathDestination); }
        catch (DirectoryNotFoundException e) { Console.WriteLine(e.ToString()); }

        string currentLogDirectory = Path.GetFullPath(data.LogPathDestination);
        var logFile = Path.Combine(currentLogDirectory, data.LogFileDestination);

        // logFile = Path.ChangeExtension(logFile, DateTime.Now.ToString("dd-MM-yyyy")+".txt");

        try
        {
    
            if (File.Exists(logFile))
            {
                var logFileListToCheckIfExisted = File.ReadAllLines(logFile).ToList();

                if(logFileListToCheckIfExisted is not null && !logFileListToCheckIfExisted.Any(data => musicsToExport.Contains(data, StringComparer.OrdinalIgnoreCase))){
                    using StreamWriter sw = File.AppendText(logFile);
                    sw.WriteLine("");
                    sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                    foreach (var item in musicsToExport)
                    {
                        sw.WriteLine(item);
                    }
                    sw.WriteLine("");
                }
                
            }
            else
                File.WriteAllLines(logFile, musicsToExport);

            await Task.Delay(100);
            message = "---------------------------- Texts added to Log File successfully";
            //File.Copy(OldMusicFile, fileDestination);
        }
        catch (Exception) { throw; }

        return message;
    }

    public static async Task ExportEmbeedFileFromFolderAsync(SampleData data){
                
        if(data is null || data.PathSource is null || data.FileDestination is null || data.PathDestination is null) throw new ArgumentNullException(nameof(data));  
        
        var (filecombined, fileItems) = await GetFilesWithInFoldersAndSubFolders(data); 

        try
        {
            if (File.Exists(filecombined))
            {
                using StreamWriter sw = File.AppendText(filecombined);
                sw.WriteLine("");
                sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                foreach (var item in fileItems)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine("");
            }
            else{
                using StreamWriter sw = new(filecombined);
                sw.WriteLine("");
                sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                foreach (var item in fileItems)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine("");
            }
            await Task.Delay(1000);
            System.Console.WriteLine("---------------------Texts added to Log File successfully");
            //File.Copy(OldMusicFile, fileDestination);
        }
        catch (Exception) { throw; }
    }

    public static async Task<(string, Dictionary<string, List<string>>)> GetFilesWithInFoldersAndSubFolders(SampleData data)
    {
        if(data is null || data.PathSource is null || data.FileDestination is null || data.PathDestination is null) throw new ArgumentNullException(nameof(data));  

        string filecombined ="";
        DirectoryInfo[] directories; var items = new List<string>();
        Dictionary<string, List<string>> fileItems = new();

        try
        {
            var directory = new DirectoryInfo(data.PathSource);
            directories = directory.GetDirectories("*", SearchOption.AllDirectories);
            //root= Directory.GetDirectories(mainPath,"*", searchOption: SearchOption.AllDirectories);
            if (!Directory.Exists(data.PathDestination)) Directory.CreateDirectory(data.PathDestination);

            filecombined = Path.Combine(data.PathDestination, data.FileDestination);

            foreach (var item in directories)
            {
                //System.Console.WriteLine(item.FullName);
                fileItems.TryAdd(item.FullName, items);
                items = new();
            }
        }
        catch (UnauthorizedAccessException e) {System.Console.WriteLine(e.ToString());  }
        catch (DirectoryNotFoundException e) { System.Console.WriteLine(e.ToString()); }
        
        await Task.Delay(100);
        return (filecombined, fileItems);
    }

    /// <summary>
    /// Get a list of item in a specific folder when items match each other
    /// </summary>
    /// <param name="videoDirectory"></param>
    /// <param name="arrayOfFullPath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Dictionary<int, string> GetFullPath(string videoDirectory, string[] arrayOfListElementFromFile)
    {
        if (string.IsNullOrEmpty(videoDirectory)) throw new ArgumentNullException();
        Dictionary<int, string> musicFullPath = new();

        if (Directory.Exists(videoDirectory))
        {
            var clips = Directory.GetFiles(videoDirectory);
            for (int h = 0; h < arrayOfListElementFromFile.Length; h++)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    string extension = Path.GetFileNameWithoutExtension(clips[i]);

                    if (arrayOfListElementFromFile[h].Contains(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        musicFullPath.Add(h, Path.GetFullPath(clips[i]));
                    }
                }
            }

        }
        return musicFullPath;
    }

    /*Get only headers from input file*/
    private static Dictionary<int, string> GetItemHeaderHashFromFile(string[] arrayFilePath)
    {
        Dictionary<int, string> itemHeader = new();

        if (arrayFilePath is null || arrayFilePath.Length == 0) throw new ArgumentNullException("Array is null");

        for (int i = 0; i < arrayFilePath.Length; i++)
        {
            if (arrayFilePath[i].StartsWith('#')) itemHeader.Add(i, arrayFilePath[i]);
        }

        return itemHeader;
    }


}