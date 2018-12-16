using System;

namespace TransactionQuery.CSharp.Models
{
    public class TransactionQueryResponse
    {
        public Response Response = new Response();
        public DateTime StartDate = new DateTime();
        public DateTime EndDate = new DateTime();
        public string LastFour = string.Empty;
    }
}