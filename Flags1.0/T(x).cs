using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace Flags1
{
    /*
     *This is the T(X) service class. 
     * i decided to create for each service an own class.
     * The T(x) class has a wacher for listen for the source dictionary
     * the T(x) class has 2 special instance variabels:
     * 1.watcher of type Watcher. a class that i'v created. both T(x) and R(x) use this class.
     * 2.Timer that run an interval till the work is done.
     * 
     * 
     */
    public class T_x_
    {
        //instance variabels
        private Watcher _watcher;
        //_aTimer of type Timer is for the interval to run and listen to the source foler
        private Timer _aTimer=new Timer();
        //cnt is just an int use to for the names of files to be created for testing
        private int cnt = 0;
        //variables to save the path for the source/destination dictionary's
        private  string _SOURCE_DICTIONARY_PATH;
        private string _DESTINATION_DICTIONARY_PATH;
        //variable for the source log file
        private string _log_path;
        private bool _finished_working = false;
        
        
        Random _rand = new Random();

        /*
         * T(x) default constructor
         * the constructor start with initial all the pathes that require 
         * for the T(x) will run correctly.
         * then initilize the _watcher of type Watcher.
         */
        public T_x_()
        {
               
            //SetUpPath path initiate all of the pathes not depending on the enviorment
            this.SetUpPath();
            //build a watcher variable and send to the watcher construcot a system loction to listen and a log path to document all the changes
            this._watcher = new Watcher(this._SOURCE_DICTIONARY_PATH,this._log_path);
            //clear the log file from last run
            File.WriteAllText(this._log_path, String.Empty);
            //validation to check if the log file is already exists
            if (File.Exists(this._log_path))
            {
                this.LogIt("file is already exists");
            }
            else
            {
                File.CreateText(this._log_path);
            }
            



        }

        /*
         * GetWatcher function is for the testing
         * @return Watcher varible.
         */
        public Watcher GetWatcher()
        {
            return this._watcher;
        }
        /*
         * function GetFinish is the function the determine when the T(x) finish his job
         * for creating files and copy the to the destinition folder.\
         * @return bool that determain that T(x) has done.
         * 
         */
        public bool GetFinish()
        {
            return this._finished_working;
        }
        //LogIt function is the function that write to the log file
        private void LogIt(string s)
        {
            File.AppendAllText(this._log_path,"At " + DateTime.Now.ToString() +" "+s+"\n");
        }


        /*
         * SetUpPath is a helper function to initiate all of the pathes the program need.
         * i'v build the function just to clear a little bit the constructor
         * 
        */
        private void SetUpPath()
        {
            
            string temp_path = "";
            //validation in case of system failre.
            try
            {
                temp_path = Directory.GetCurrentDirectory();
                this._log_path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + "/Source_log.txt";
            }
            catch(Exception ex)
            {
                this.LogIt(ex.ToString());
                
            }
            //split the path that return by  '/' and build it again without the last 2 dictinoray's
            string[] splited_path = temp_path.Split('/');
            

            string res = "";
            for (int i = 0; i < splited_path.Length - 2; i++)
            {
                res += splited_path[i] + '/';
            }
            //at R(x) ths order here is opposite.
            this._DESTINATION_DICTIONARY_PATH = res + "/Destination/";
            res += "Source/";

            this._SOURCE_DICTIONARY_PATH = res;
        }
        /*
         * getlogPath is a function the return the path of the source log file.
         * @return the path of the log file. in this case the source_log.txx 
         */
        public string GetLogPath()
        {
            return this._log_path;
        }
        /*
         * function that start the timer
         * to loop and wait for events to happen.
         * @return nothing
        */
        public void StartTimer()
        {
            this.LogIt("im in start up timer");
            //TODO:change here to 5000-10000
            //every inteval is between 5-10 seconds.
            this._aTimer.Interval = _rand.Next(1000, 2000);

            // Hook up the Elapsed event for the timer. 
            this._aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)

            this._aTimer.AutoReset = true ;
            // Start the timer
            this._aTimer.Enabled = true;
            
            
            

            
        }
        /*
         * function that will be called by the timer
         * the function will be create a file
         * just for testing i'v made the program to create 5 files
         * @return nothin
        */
        private  void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            this.CreateFile();
            
            cnt++;
            //after the program creted 5 files stoptimer function
            //will end the timer
            if (cnt > 5) { this.StopTimer(); }


        }
        /*
         * stoptimer function will end the listening of the timer.
         * in other words, this function will end the T(x) to run
         * return nothing
         */
        private void StopTimer()
        {
            this.LogIt("timer has stopped");

            this._aTimer.Enabled = false;
            string []files=this.GetFiles();
            this.CopyFiles(files);
            //Console.WriteLine("t(x)");
            this._finished_working = true;
            
            
            
            

        }
        /*
         * createFile function create the text files in the source folder
         * @reutrn nothing
         */
        private void CreateFile()
        {
            try
            {
                StreamWriter sw = File.CreateText(this._SOURCE_DICTIONARY_PATH + "/" + cnt + ".txt");
            }
            catch(Exception ex)
            {
                this.LogIt(ex.ToString());
            }
            
            

        }
        /*
         * getFiles function return as an array of string the files that is currently
         * in the source folder
         * 
         * 
         * @return array of strings
         */
        private string[] GetFiles()

        {
            //the result to be return
            string[] res = new string[0];
            try
            {
                string[] files = Directory.GetFiles(this._SOURCE_DICTIONARY_PATH, "*.txt");
                
                
                return files;
            }
            catch(IOException ex)
            {
                this.LogIt(ex.ToString());
                return res;
            }
            
        }/*
          * CopyFiles is the function to copy all the files from the source folder
          * to the destination flder
          * @ param []files is an array of strings that represnt the files in the source folder
          * @return nothing
          */
        private void CopyFiles(string []files)
        {
            //loop over the files to be copied. if a file already exists this will be logged
            foreach(var f in files)
            {
                string fileName = f.Substring(f.Length - 5, 5);
                string des = this._DESTINATION_DICTIONARY_PATH;
                
                try
                {
                    File.Copy(f, des + fileName);
                    
                }
                catch(IOException ex)
                {
                    this.LogIt("file is already exists");
                }
            }
        }
    }
}

