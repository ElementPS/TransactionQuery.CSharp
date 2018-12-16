using System.Configuration;

namespace TransactionQuery.CSharp.Infrastructure
{
    public class ConfigurationData
    {
        public string AccountId = string.Empty;
        public string AccountToken = string.Empty;
        public string AcceptorId = string.Empty;
        public string ApplicationId = string.Empty;
        public string ApplicationVersion = string.Empty;
        public string ApplicationName = string.Empty;
        public string ExpressReportingXMLEndpoint = string.Empty;

        public ConfigurationData()
        {
            AccountId = ConfigurationManager.AppSettings["AccountID"];
            AccountToken = ConfigurationManager.AppSettings["AccountToken"];
            AcceptorId = ConfigurationManager.AppSettings["AcceptorID"];
            ApplicationId = ConfigurationManager.AppSettings["ApplicationID"];
            ApplicationVersion = ConfigurationManager.AppSettings["ApplicationVersion"];
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            ExpressReportingXMLEndpoint = ConfigurationManager.AppSettings["ExpressReportingXMLEndpoint"];
        }
    }
}