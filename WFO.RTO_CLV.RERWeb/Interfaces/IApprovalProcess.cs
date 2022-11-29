using Microsoft.SharePoint.Client;
using WFO.RTO_CLV.RERWeb.Entities;

namespace WFO.RTO_CLV.RERWeb.Interfaces
{
    interface IApprovalProcess
    {
        ApprovalProcessItems GetApprovalProcess(ClientContext context, ListItem task_item);

        int CreateTask(ClientContext context, ApprovalProcessItems approval_process);
        void UpdateMainRequest(ClientContext context, ApprovalProcessItems approval_process);
        void SetPermission(ClientContext context, ApprovalProcessItems approval_process);
        void SendEmail(ClientContext context, ApprovalProcessItems approval_process);

        void RevertActions(ClientContext context, ApprovalProcessItems approval_process);
    }
}
