using System;
using System.IO;
namespace Flags1
{
    /*
     * this is th Watcher class
     * i'v decided to create a class for the watcher that listen to the folder
     * beacause i wanted to improve the abillities of the listening and the events that occur.
     */
    public class Watcher
    {
        //instance variables
        //this is the watcher from microsoft api
        private FileSystemWatcher _sw;
        //_path is the path for the folder that the watcher will listen to
        private string _path;
        // log_path 
        private string _log_path;


        /*
         * Watcher constructor
         * @param folder_path for the path of the folder to listen to. Source/Destinition.
         * @param log_path for the path of the log file to document the events that occur
         * in the constructor i'v created an init function just to make the constructor more readble.
         * 
         */
        public Watcher(string folder_path,string log_path)
        {
            _sw = new FileSystemWatcher(@"" + folder_path);
            this._path = folder_path;
            this._log_path = log_path;
            Console.WriteLine(this._path);
            //the init function to make the constructor more readble.
            this.Init();
        }
        /*
         * GetWatcher return the watcher.
         * this function is mostly for the debugging stage.
         * @return FileSystemWatcher.
         */
        public FileSystemWatcher GetWatcher()
        {
            return this._sw;
        }
        /*
         * GetPath is the function that return the path of the folder that the watcher is listen to.
         * @return string represent the path of the listenning folder.
         */
        public string GetPath()
        {
            return this._path;
        }
        /*
         * SetWatcher is to edit the wathcer
         * creted for the debugging stage.
         */
        public void SetWatcher(FileSystemWatcher s)
        {
            this._sw = s;
        }
        /*
         * SetPath function for editing the path of the watcher is listen to
         */
        public void SetPath(string p)
        {
            this._sw.Path = p;
            this._path = p;
        }
        /*
         * Init function is a helper function that innitiate the watcher
         * all of the code is copied from the micrisoft api.
         * the function will create 5 diffrent function of 5 diffrent types of events the watcher will listen to
         * the watcher is listen to all types of files.
         * this could be change easly at the Filter property
         */
        private void Init()
        {
            _sw.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            _sw.Changed += OnChanged;
            _sw.Created += OnCreated;
            _sw.Deleted += OnDeleted;
            _sw.Renamed += OnRenamed;
            

            _sw.Filter = "*";
            _sw.IncludeSubdirectories = true;
            _sw.EnableRaisingEvents = true;
        }
        /*
         * OnChange function will log any change of that occur in the folder.
         */
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;

            }
            this.LogIt("change was made");
            
        }
        /*
         * OnCreted will log when a new file is created.
         */
        private void OnCreated(object sender, FileSystemEventArgs e)
        {

            this.LogIt("a file was created");
        }
        /*
         * OnDeleted will log when a new file is deleted.
         */
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {

            this.LogIt("a file was deleted");
        }

        /*
         * OnRnamed will log when a new file is renamed.
         */
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            this.LogIt("a file was renamed");

        }

        /*
         * PrintException will log when an exception will occur.
         */
        private void PrintException(Exception ex)
        {
            this.LogIt(ex.ToString());
            
        }
        /*
         * LogIt is just a function that take a string and record it to the correct log file.
         */
        private void LogIt(string s)
        {
            string s1 = "At " + DateTime.Now.ToString()+" " +s+"\n";
            File.AppendAllText(this._log_path, s1);
        }
        


    }
}

