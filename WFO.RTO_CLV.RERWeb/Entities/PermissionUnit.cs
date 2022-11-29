using Microsoft.SharePoint.Client;

namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class PermissionUnit
    {
        public string Role { get; set; }
        public RoleType RoleType { get; set; }
        public Principal UserGroup { get; set; }
    }
}