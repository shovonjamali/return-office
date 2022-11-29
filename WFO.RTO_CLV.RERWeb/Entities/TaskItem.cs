using Microsoft.SharePoint.Client;

namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class TaskItem
    {
        public TaskItem() { }

        public TaskItem(int itemId, string state, string instanceType, FieldUrlValue link, string approver, string title, string role, string action)
        {
            Title = title;
            InstanceType = instanceType;
            PrimaryId = itemId;
            State = state;
            Link = link;
            Approver = approver;
            Role = role;
            Action = action;
        }

        public string Title { get; set; }
        public string InstanceType { get; set; }
        public int PrimaryId { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
        public FieldUrlValue Link { get; set; }
        public string Approver { get; set; }
        public string Action { get; set; }
    }
}