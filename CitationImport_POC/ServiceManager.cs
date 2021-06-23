using System;
using System.Timers;
using System.Configuration;
using NLog;
using System.Linq;
using CitationImport_POC.BLL;

namespace CitationImport_POC
{
    public class ServiceManager
    {
        private readonly Timer _timer;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ServiceManager()
        {
            logger.Info("Service manager start");

            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("connectionStrings");

            //Converts TimerInterval from app.config into minutes. Timer elapses every (TimerInterval) minutes.
            Double timerInterval = (Convert.ToDouble(ConfigurationManager.AppSettings["TimerInterval"]) * 1000 * 60);

            _timer = new Timer(timerInterval) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            logger.Info("Service manager end");
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //Runs service during the hours contained in the RunTimes string
            String runTimes = ConfigurationManager.AppSettings["RunTimes"];
            String[] runTimesArray = runTimes.Split('|');
            DateTime currentDate = DateTime.Now;
            String currentHour = currentDate.Hour.ToString();


            if (runTimesArray.Contains(currentHour))
            {
                TaskManager tm = new TaskManager();
                tm.Run();
            }

        }

        public void Start()
        {
            String runTimes = ConfigurationManager.AppSettings["RunTimes"];
            String timerInterval = ConfigurationManager.AppSettings["TimerInterval"];
            String devEnvironment = ConfigurationManager.AppSettings["DevEnvironment"];
            String connectionString = ConfigurationManager.ConnectionStrings["OdyClerkInternal"].ConnectionString;

            logger.Debug("Start service");
            logger.Info("Checking Runtimes [" + runTimes + "]");
            logger.Info("Timer Interval [" + timerInterval + "]");
            logger.Info("Dev Environment [" + devEnvironment + "]");
            logger.Info("Connection String [" + connectionString + "]");

            _timer.Start();
        }

        public void Stop()
        {
            logger.Debug("Stop service");
            _timer.Stop();
        }
    }
}
