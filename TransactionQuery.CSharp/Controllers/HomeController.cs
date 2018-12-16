using System;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using TransactionQuery.CSharp.Infrastructure;
using TransactionQuery.CSharp.Models;

namespace TransactionQuery.CSharp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(DateTime? startDate, DateTime? endDate, string lastFour)
        {
            var theStartDate = startDate ?? DateTime.Now;
            var theEndDate = endDate ?? DateTime.Now;

            var txnQueryResponse = SendTransactionQuery(theStartDate, theEndDate);
            txnQueryResponse.StartDate = theStartDate;
            txnQueryResponse.EndDate = theEndDate;
            txnQueryResponse.LastFour = lastFour;

            if (!String.IsNullOrEmpty(lastFour))
            {
                var test = "xxxx-xxxx-xxxx-" + lastFour;
                txnQueryResponse.Response.ReportingData.Items.RemoveAll(x => x.CardNumberMasked != test);
            }

            return View(txnQueryResponse);
        }

        public ActionResult Details(string transactionID)
        {
            var txnQueryResponse = SendTransactionQuery(transactionID);

            return View(txnQueryResponse.Response.ReportingData.Items[0]);
        }

        private string buildTransactionQuery(ConfigurationData configurationData, DateTime startDate, DateTime endDate)
        {                           
            XNamespace express = "https://reporting.elementexpress.com";

            XDocument doc = new XDocument(new XElement(express + "TransactionQuery",
                                               new XElement(express + "Credentials",
                                                   new XElement(express + "AccountID", configurationData.AccountId),
                                                   new XElement(express + "AccountToken", configurationData.AccountToken),
                                                   new XElement(express + "AcceptorID", configurationData.AcceptorId)
                                                            ),
                                                new XElement(express + "Application",
                                                    new XElement(express + "ApplicationID", configurationData.ApplicationId),
                                                    new XElement(express + "ApplicationVersion", configurationData.ApplicationVersion),
                                                    new XElement(express + "ApplicationName", configurationData.ApplicationName)
                                                            ),
                                                new XElement(express + "Parameters",
                                                    new XElement(express + "TransactionDateTimeBegin", startDate.ToString("yyyy-MM-dd")),
                                                    new XElement(express + "TransactionDateTimeEnd", endDate.ToString("yyyy-MM-dd"))
                                                            )
                                                       )
                                         );
            return doc.ToString();
        }

        private string buildTransactionQuery(ConfigurationData configurationData, string transactionID)
        {
            XNamespace express = "https://reporting.elementexpress.com";

            XDocument doc = new XDocument(new XElement(express + "TransactionQuery",
                                               new XElement(express + "Credentials",
                                                   new XElement(express + "AccountID", configurationData.AccountId),
                                                   new XElement(express + "AccountToken", configurationData.AccountToken),
                                                   new XElement(express + "AcceptorID", configurationData.AcceptorId)
                                                            ),
                                                new XElement(express + "Application",
                                                    new XElement(express + "ApplicationID", configurationData.ApplicationId),
                                                    new XElement(express + "ApplicationVersion", configurationData.ApplicationVersion),
                                                    new XElement(express + "ApplicationName", configurationData.ApplicationName)
                                                            ),
                                                new XElement(express + "Parameters",
                                                    String.IsNullOrEmpty(transactionID) ? null : new XElement(express + "TransactionID", transactionID)
                                                            )
                                                       )
                                         );
            return doc.ToString();
        }

        private TransactionQueryResponse SendTransactionQuery(DateTime startDate, DateTime endDate)
        {
            var httpSender = new HttpSender();
            var response = string.Empty;
            var configurationData = new ConfigurationData();
            var request = buildTransactionQuery(configurationData, startDate, endDate);

            response = httpSender.Send(request, configurationData.ExpressReportingXMLEndpoint, string.Empty);

            //load into XmlDocument for parsing
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            return ParseResponse(xmlDoc);
        }

        private TransactionQueryResponse SendTransactionQuery(string transactionID)
        {
            var httpSender = new HttpSender();
            var response = string.Empty;
            var configurationData = new ConfigurationData();
            var request = buildTransactionQuery(configurationData, transactionID);

            response = httpSender.Send(request, configurationData.ExpressReportingXMLEndpoint, string.Empty);

            //load into XmlDocument for parsing
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response);
            return ParseResponse(xmlDoc);
        }

        private TransactionQueryResponse ParseResponse(XmlDocument xmlDoc)
        {
            var transactionQueryResponse = new TransactionQueryResponse();
            transactionQueryResponse.Response = new Models.Response();


            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("ns", "https://reporting.elementexpress.com");

            var tempNode = xmlDoc.SelectSingleNode("//ns:TransactionQueryResponse/ns:Response", namespaceManager);
            transactionQueryResponse.Response.ExpressResponseCode = tempNode.SelectSingleNode("ns:ExpressResponseCode", namespaceManager)?.InnerText;
            transactionQueryResponse.Response.ExpressResponseMessage = tempNode.SelectSingleNode("ns:ExpressResponseMessage", namespaceManager)?.InnerText;
            transactionQueryResponse.Response.ExpressTransactionDate = tempNode.SelectSingleNode("ns:ExpressTransactionDate", namespaceManager)?.InnerText;
            transactionQueryResponse.Response.ExpressTransactionTime = tempNode.SelectSingleNode("ns:ExpressTransactionTime", namespaceManager)?.InnerText;
            transactionQueryResponse.Response.ExpressTransactionTimezone = tempNode.SelectSingleNode("ns:ExpressTransactionTimezone", namespaceManager)?.InnerText;
            transactionQueryResponse.Response.ReportingID = tempNode.SelectSingleNode("ns:ReportingID", namespaceManager)?.InnerText;

            var xnList = xmlDoc.SelectNodes("//ns:TransactionQueryResponse/ns:Response/ns:ReportingData/ns:Items/ns:Item", namespaceManager);
            foreach (XmlNode xn in xnList)
            {
                var item = new Item();
                item.TransactionID = xn["TransactionID"]?.InnerText;
                item.AcceptorID = xn["AcceptorID"]?.InnerText;
                item.AccountID = xn["AccountID"]?.InnerText;
                item.TerminalID = xn["TerminalID"]?.InnerText;
                item.ApplicationID = xn["ApplicationID"]?.InnerText;
                item.ApprovalNumber = xn["ApprovalNumber"]?.InnerText;
                item.ExpirationMonth = xn["ExpirationMonth"]?.InnerText;
                item.ExpirationYear = xn["ExpirationYear"]?.InnerText;
                item.ExpressResponseCode = xn["ExpressResponseCode"]?.InnerText;
                item.ExpressResponseMessage = xn["ExpressResponseMessage"]?.InnerText;
                item.HostResponseMessage = xn["HostResponseMessage"]?.InnerText;
                item.ReferenceNumber = xn["ReferenceNumber"]?.InnerText;
                item.TicketNumber = xn["TicketNumber"]?.InnerText;
                item.TrackingID = xn["TrackingID"]?.InnerText;
                item.TransactionAmount = xn["TransactionAmount"]?.InnerText;
                item.TransactionStatus = xn["TransactionStatus"]?.InnerText;
                item.TransactionStatusCode = xn["TransactionStatusCode"]?.InnerText;
                item.TransactionType = xn["TransactionType"]?.InnerText;
                item.CardNumberMasked = xn["CardNumberMasked"]?.InnerText;
                item.CardLogo = xn["CardLogo"]?.InnerText;
                item.CardType = xn["CardType"]?.InnerText;
                item.TrackDataPresent = xn["TrackDataPresent"]?.InnerText;
                item.ExpressTransactionDate = xn["ExpressTransactionDate"]?.InnerText;
                item.ExpressTransactionTime = xn["ExpressTransactionTime"]?.InnerText;
                item.TimeStamp = xn["TimeStamp"]?.InnerText;

                transactionQueryResponse.Response.ReportingData.Items.Add(item);
            }

            return transactionQueryResponse;
        }

    }
}