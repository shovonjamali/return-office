namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class AppUsers
    {
        public Approver Manager { get; set; }
        public Approver Employee { get; set; }
        public Approver EMT2 { get; set; }
        public Approver EMT1 { get; set; }
        public Approver EMT { get; set; }
        public Approver WFOAdmin { get; set; }
        public Approver ExceptionCommittee { get; set; }
        public Approver HR { get; set; }
        public Approver BackupSubmitter { get; set; }
        public Approver DelegateApprover { get; set; }
        public Approver DashboardViewers { get; set; }
        public Approver ServiceEmail { get; set; }
    }
}