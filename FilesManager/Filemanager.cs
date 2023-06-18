using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Shared;

internal static class FileManager
{

    public static async Task CopyOrMoveFileFromSourceFileAsync(SampleData data)
    {
        string parent = "", child = "", parentFull = "";
        List<string> export = new();
        string[] arrayOfListElementFromFile = Array.Empty<string>();

        var sw = new Stopwatch();

        if (!Directory.Exists(data.VideoPath) || !File.Exists(data.FilePath)) throw new Exception();

        var itemForHeader = new Dictionary<int, string>();
        int counterMusic = 0, count = 1;

        try { arrayOfListElementFromFile = File.ReadAllLines(data.FilePath); }
        catch (System.Exception) { throw; }

        itemForHeader = GetItemHeaderHashFromFile(arrayOfListElementFromFile.ToArray());
        int number = itemForHeader.Count;

        var musicFullPath = GetFullPath(data.FilePath, data.VideoPath, arrayOfListElementFromFile);
        //this.LogsDataFromOriginAfterMove(musicFullPath, ItemHeader,  FilePath);

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

                            try { if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                            catch (Exception) { throw; }

                            data.FileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));
                            Console.WriteLine("copy of ................. {0}", musicElement.Value);

                            export.Add($"{headers.Value}");
                            export.Add($"{Path.GetFileName(musicElement.Value)}");
                            try
                            {
                                sw.Start();
                                await Task.Delay(2000);
                                //File.Copy(musicElement.Value, FileDestination,false);   
                                System.Console.WriteLine("copying  to--------------{0}  in {1} 's Elapsed time",data.FileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                sw.Stop();
                                sw.Reset();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }

                        if (counterMusic == 4)
                        {
                            //System.Console.WriteLine(counter.Value[i]);
                            child = headers.Value;
                            child = child.Replace('#', ' ').Trim();
                            parentFull = Path.Combine(data.VideoPath, parent, child);
                            //System.Console.WriteLine("child ---------------{0}",parentFull);
                            try { if (!Directory.Exists(parentFull)) Directory.CreateDirectory(parentFull); }
                            catch (System.Exception) { throw; }

                            data.FileDestination = Path.Combine(parentFull, Path.GetFileName(unique.First().Value));
                            System.Console.WriteLine(headers.Value);
                            System.Console.WriteLine("copy of--------------{0}", musicElement.Value);

                            export.Add($"{headers.Value}");
                            export.Add($"{Path.GetFileName(musicElement.Value)}");
                            try
                            {
                                sw.Start();
                                //File.Copy(musicElement.Value, FileDestination,false);  
                                await Task.Delay(2000);
                                System.Console.WriteLine("copying  to--------------{0} in {1} 's Elapsed time", data.FileDestination, sw.Elapsed.TotalSeconds.ToString("0:00"));
                                sw.Stop();
                                sw.Restart();

                            }
                            catch (Exception e)
                            {
                               Console.WriteLine(e);
                            }
                        }

                    }
                }

            }
            count++;

        }
        var message = await LogsDataFromOriginAfterCopy(export, data.FilePath, data.FileDestination);
        await Task.Delay(1000);
        System.Console.WriteLine(message);
    }

    //Get List item Matching from logs
    public static async Task<string> LogsDataFromOriginAfterCopy(List<string> musics, string logSourceFile, string folderDestination)
    {
        string message;

        if (musics == null || logSourceFile == null || folderDestination == null) throw new ArgumentNullException("data is null");

        try { if (!Directory.Exists(folderDestination)) Directory.CreateDirectory(folderDestination); }
        catch (System.Exception) { throw; }

        string currentLogDirectory = Path.GetFullPath(folderDestination);
        var logFile = Path.Combine(currentLogDirectory, logSourceFile);

        // logFile = Path.ChangeExtension(logFile, DateTime.Now.ToString("dd-MM-yyyy")+".txt");

        try
        {

            if (File.Exists(logFile))
            {
                using StreamWriter sw = File.AppendText(logFile);
                sw.WriteLine("");
                sw.WriteLine("----------------{0}-----------------", DateTime.Now.ToString("dd mm yyyy hh:mm:ss"));
                foreach (var item in musics)
                {
                    sw.WriteLine(item);
                }
                sw.WriteLine("");
            }
            else
                File.WriteAllLines(logFile, musics);

            await Task.Delay(100);
            message = "*************Texts added to Log File successfully";
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
    /// <param name="filePath"></param>
    /// <param name="videoDirectory"></param>
    /// <param name="arrayOfFullPath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Dictionary<int, string> GetFullPath(string filePath, string videoDirectory, string[] arrayOfFullPath)
    {
        if (String.IsNullOrEmpty(videoDirectory) || String.IsNullOrEmpty(filePath)) throw new ArgumentNullException();
        Dictionary<int, string> musicFullPath = new();

        if (Directory.Exists(videoDirectory))
        {
            var clips = Directory.GetFiles(videoDirectory);
            for (int h = 0; h < arrayOfFullPath.Length; h++)
            {
                for (int i = 0; i < clips.Length; i++)
                {
                    string extension = Path.GetFileNameWithoutExtension(clips[i]);

                    if (arrayOfFullPath[h].Contains(extension, StringComparison.OrdinalIgnoreCase))
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