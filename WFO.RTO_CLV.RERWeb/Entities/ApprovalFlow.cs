namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class ApprovalFlow
    {
        public Approver Approver { get; set; } = null;
        public string RolebasedAction { get; set; }
        public string NextRole { get; set; }
        public string NextStatus { get; set; }
        public string NextInternalStatus { get; set; }
        public int NewTaskId { get; set; } = 0;
        public string CommentsColumn { get; set; }
    }
}