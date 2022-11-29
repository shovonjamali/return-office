using Microsoft.SharePoint.Client;

namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class ApprovalProcessItems
    {
        public ListItem TaskItem { get; set; }

        public ListItem RequestItem { get; set; }

        public ListItem ConfigItem { get; set; }

        public ListItemCollection EmailItems { get; set; }

        public ApprovalFlow ApprovalFlow { get; set; }

        public AppUsers AppUsers { get; set; }
    }
}