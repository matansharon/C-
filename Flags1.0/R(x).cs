using System;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Timers;

namespace Flags1
{
    /*
     *This is the R(X) service class. 
     * i decided to create for each service an own class.
     * The R(x) class has a wacher for listen for the source dictionary
     * the R(x) class has 2 special instance variabels:
     * 1.watcher of type Watcher. a class that i'v created. both T(x) and R(x) use this class.
     * 2.Timer that run an interval till the work is done.
     * 
     * 
     */
    public class R_x_
    {
        //gifyrlpgwccoynvx
        //instance variabels
        private Watcher _watcher;
        //_aTimer of type Timer is for the interval to run and listen to the source foler
        private Timer _aTimer = new Timer();
        private Random rand = new Random();
        private string _SOURCE_DICTIONARY_PATH;
        private string _DESTINATION_DICTIONARY_PATH;
        //variable for the destination log file
        private string _log_path;

        /*
         * R(x) default constructor
         * the constructor start with initial all the pathes that require 
         * for the R(x) will run correctly.
         * then initilize the _watcher of type Watcher.
         * and check if the destination log file is already exsits.
         */
        public R_x_()
        {
            //the destination loction to listen to
            this.SetUpPath();
            //initilaize the Watcher
            this._watcher = new Watcher(this._DESTINATION_DICTIONARY_PATH,this._log_path);
            //clear the last log file
            File.WriteAllText(this._log_path, String.Empty);
            
            //validation to check if the log file already exists
            if (File.Exists(this._log_path))
            {
                this.LogIt("file already exsist");
            }
            else
            {
                File.CreateText(this._log_path);
            }
            

        }
        private void LogIt(string s)
        {
            File.AppendAllText(this._log_path, "At " + DateTime.Now.ToString() + " " + s + "\n");
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
                this._log_path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()) + "/Destination_log.txt";
            }
            catch (Exception ex)
            {
                this.LogIt(ex.ToString());

            }
            //in this stage the path will berak into parts.
            //and will be rebuild again
            string[] splited_path = temp_path.Split('/');
            string res = "";
            for (int i = 0; i < splited_path.Length - 2; i++)
            {
                res += splited_path[i] + '/';
            }
            this._SOURCE_DICTIONARY_PATH = res + "Source/";
            res += "Destination/";

            this._DESTINATION_DICTIONARY_PATH=res;
        }
        /*
         * function GetLogPath return the path of the destination log file.
         * @return a string that represnt the path.
         */
        public string GetLogPath()
        {
            return this._log_path;
        }
        /*
         * StartTimer function will start to timer interval and listen to the destination folder.
         * 
         */
        public void StartTimer()
        {
            //TODO:change here to 5000-10000
            this._aTimer.Interval = rand.Next(1000, 2000);

            // Hook up the Elapsed event for the timer. 
            this._aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)

            this._aTimer.AutoReset = true;
            // Start the timer
            this._aTimer.Enabled = true;





        }
        /*
         * function OnTimerEvent is called every interval and delete the files in the source folder.
         * the function will get all the files in the source folder and delete the one by one
         * the program only search for text files.
         * 
         */
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //this line return all the files with an end of "txt" and create an array.
            //i could use a foreach loop to delete them all together but u wanted the program to be slow
            //so you can see the process.
            string[] files = Directory.GetFiles(this._SOURCE_DICTIONARY_PATH, "*.txt");
            if (files.Length == 0)
            {
                this.StopTimer();
            }
            try
            {

                File.Delete(Path.Combine(this._SOURCE_DICTIONARY_PATH, files[0]));
                //TODO: uncomment this
                //this.SendMessage();
            }
            catch (IOException ex)
            {
                this.LogIt("something went wrong on R(x) OnTimeEvent function");
            }

        }
        /*
         * StopTimer is the function that is killing the R(x) process in other words.
         * the function stop the interval.
         * 
         */
        private void StopTimer()
        {
            this.LogIt("timer has stopped");

            Console.WriteLine("R(x) has done");
            this._aTimer.Enabled = false;

        }
        /*
         * SendMessage is the function that responsable for sending the email massage for the user.
         * the function is using SMTP protocol and gmail api.
         * since the lat May Google blocked from third party apps with low security protocol
         * to use the gmail api.
         * Hence i'v needed to produce a special password for this service
         */
        private void SendMessage()
        {
            //gmail protocol for smtp use
            string smtpAddress = "smtp.gmail.com";
            //port number
            int portNumber = 587;
            bool enableSSL = true;
            // third party app password for the connection password
            string password = "gifyrlpgwccoynvx"; //Sender Password  
            string emailFromAddress = "matca2952@gmail.com"; //Sender Email Address  
            string emailToAddress = "matca2955@gmail.com"; //Receiver Email Address  
            string subject = "Hello";
            string body = "Hello, This is Email sending test using gmail.";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                    try
                    {
                        smtp.Send(mail);
                        this.LogIt("email was sent");
                    }
                    catch (Exception e)
                    {
                        this.LogIt("email did not sent");
                        this.LogIt(e.ToString());
                    }

                }
            }



        }
        public void ClearDestinationFolder()
        {
            string[] files = Directory.GetFiles(this._DESTINATION_DICTIONARY_PATH, "*");
            foreach(string f in files)
            {
                try
                {

                    File.Delete(Path.Combine(this._DESTINATION_DICTIONARY_PATH, f));
                    
                }
                catch (IOException ex)
                {
                    this.LogIt("something went wrong on ClearDestinationFolder function");
                }
            }
            
        }
    }
}


