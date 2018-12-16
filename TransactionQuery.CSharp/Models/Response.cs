
namespace TransactionQuery.CSharp.Models
{
    public class Response
    {
        public string ExpressResponseCode = string.Empty;
        public string ExpressResponseMessage = string.Empty;
        public string ExpressTransactionDate = string.Empty;
        public string ExpressTransactionTime = string.Empty;
        public string ExpressTransactionTimezone = string.Empty;
        public ReportingData ReportingData = new ReportingData();
        public string ReportingID = string.Empty;
    }
}