namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class RequestItem
    {
        public RequestItem() { }

        public RequestItem(string internalStatus, string status, string approverText)
        {
            InternalStatus = internalStatus;
            Status = status;
            ApproverText = approverText;
        }

        public string Status { get; set; }
        public string InternalStatus { get; set; }
        public string ApproverText { get; set; }
    }
}