using System;
using System.Collections.Generic;
using System.Configuration;
using NLog;

namespace CitationImport_POC.BLL
{
    public class TaskManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            int fileLogID = Convert.ToInt32(ConfigurationManager.AppSettings["FileLogID"]);
            int vendorAgencyID = Convert.ToInt32(ConfigurationManager.AppSettings["VendorAgencyID"]);
            string localPath = ConfigurationManager.AppSettings["LocalPath"];

            logger.Debug("FileLogID [" + fileLogID + "]");
            logger.Debug("VendorAgencyID [" + vendorAgencyID + "]");
            logger.Debug("LocalPath [" + localPath + "]");

            OdyClerkInternalEntities db = new OdyClerkInternalEntities();

            logger.Debug("Begin TrafficCitationImport_ImportCitationsToOdyssey SP");
            db.TrafficCitationImport_ImportCitationsToOdyssey(fileLogID, vendorAgencyID, localPath);
            logger.Debug("End TrafficCitationImport_ImportCitationsToOdyssey SP");
        }

    }
}
