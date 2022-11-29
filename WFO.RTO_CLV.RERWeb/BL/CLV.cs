using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using WFO.RTO_CLV.RERWeb.AppServices;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;
using WFO.RTO_CLV.RERWeb.Interfaces;

namespace WFO.RTO_CLV.RERWeb.BL
{
    class CLV : IApprovalProcess
    {
        protected Helper Helper { get; set; }
        protected General GeneralService { get; set; }
        protected Email EmailService { get; set; }
        protected Permission PermissionService { get; set; }

        public CLV()
        {
            Helper = new Helper();
            GeneralService = new General();
            EmailService = new Email();
            PermissionService = new Permission();
        }

        public int CreateTask(ClientContext context, ApprovalProcessItems approval_process)
        {
            if (approval_process.ApprovalFlow.Approver != null)
            {
                FieldUrlValue url = GeneralService.GetTaskLink(context, Constants.CLVLists.REQUEST_LIST, approval_process);

                var task_item = Helper.GetTaskItemData(approval_process, url);

                return GeneralService.CreateTask(context, Constants.CommonLists.TASK_LIST, task_item);
            }

            return 0;
        }

        public void SendEmail(ClientContext context, ApprovalProcessItems approval_process)
        {
            for (int index = 0; index < approval_process.EmailItems.Count; index++)
            {
                var email_recipients = EmailService.GetEmailRecipients(approval_process, index);
                var email_content = EmailService.GetEmailContent(context, approval_process, index);

                var email_object = new EmailObject(email_recipients, email_content);
                EmailService.SendEmail(context, email_object);
            }
        }

        private AppUsers GetALLCLVUsers(ClientContext context, ApprovalProcessItems approval_process)
        {
            var clv_users = new AppUsers();

            clv_users.Employee = ResolveApprover(context, Constants.Role.EMPLOYEE, approval_process);
            clv_users.Manager = ResolveApprover(context, Constants.Role.MANAGER, approval_process);
            clv_users.EMT2 = ResolveApprover(context, Constants.Role.EMT2, approval_process);
            clv_users.EMT1 = ResolveApprover(context, Constants.Role.EMT1, approval_process);
            clv_users.EMT = ResolveApprover(context, Constants.Role.EMT, approval_process);

            clv_users.WFOAdmin = ResolveApprover(context, Constants.Role.WFO_ADMIN, approval_process);
            clv_users.ExceptionCommittee = ResolveApprover(context, Constants.Role.EXCEPTION_COMMITTEE, approval_process);
            clv_users.HR = ResolveApprover(context, Constants.Role.HR, approval_process);

            clv_users.BackupSubmitter = ResolveApprover(context, Constants.Role.BACKUP_SUBMITTER, approval_process);
            clv_users.DelegateApprover = ResolveApprover(context, Constants.Role.BACKUP_APPROVERS, approval_process);
            clv_users.DashboardViewers = ResolveApprover(context, Constants.Role.DASHBOARD_VIEWERS, approval_process);

            clv_users.ServiceEmail = ResolveApprover(context, Constants.Role.SERVICE_EMAIL, approval_process);

            return clv_users;
        }

        public void SetPermission(ClientContext context, ApprovalProcessItems approval_process)
        {
            PermissionService.BreakInheritance(context, approval_process);

            var permission_list = PermissionService.GetPermissionMatrix(context, approval_process);

            PermissionService.AssignUserRoleOnList(context: context, permission_unit_list: permission_list, list_item: approval_process.RequestItem);
        }

        public void UpdateMainRequest(ClientContext context, ApprovalProcessItems approval_process)
        {
            var request_item = Helper.GetRequestItemData(approval_process);

            GeneralService.UpdateMainRequest(context, request_item, approval_process);
        }

        public void RevertActions(ClientContext context, ApprovalProcessItems approval_process)
        {
            GeneralService.RevertActions(context, approval_process, Constants.CLVLists.REQUEST_LIST, Constants.CommonLists.TASK_LIST);
        }

        public ApprovalProcessItems GetApprovalProcess(ClientContext context, ListItem task_item)
        {
            ApprovalProcessItems approval_process = new ApprovalProcessItems();

            // SETTING TASK ITEM
            approval_process.TaskItem = task_item;

            // GETTING MAIN REQUEST ITEM
            int primary_id = Convert.ToInt32(approval_process.TaskItem[Constants.TaskColumns.PRIMARY_ID]);
            approval_process.RequestItem = GeneralService.GetItemById(context: context, item_id: primary_id, list_name: Constants.CLVLists.REQUEST_LIST);

            // GETTING CONFIGURATION ITEM 
            approval_process.ConfigItem = GeneralService.GetConfigItem(context: context, list_name: Constants.CommonLists.CONFIG_LIST);

            // GETTING APPROVAL FLOW
            approval_process.ApprovalFlow = GetApprovalFlow(context: context, approval_process: approval_process);

            // GETTING ALL CLV USERS
            approval_process.AppUsers = GetALLCLVUsers(context, approval_process);

            // GETTING EMAIL ITEM            
            approval_process.EmailItems = GeneralService.GetEmailTemplates(context: context, request_type: Constants.Applications.CLV, internal_status: approval_process.ApprovalFlow.NextInternalStatus, list_name: Constants.CommonLists.EMAIL_TEMPLATES_LIST);

            return approval_process;
        }

        /// <summary>
        /// Get approver flow for CLV type app
        /// Get approver for that flow
        /// Bind the approver to the flow and return approver flow
        /// </summary>
        /// <param name="context"></param>
        /// <param name="approval_process"></param>
        /// <returns></returns>
        public ApprovalFlow GetApprovalFlow(ClientContext context, ApprovalProcessItems approval_process)
        {
            string role_based_action = Helper.DefineFlowKey(approval_process);
            var approval_flow = Helper.GetCLVApprovalFlow(role_based_action);

            if (approval_flow.NextRole != Constants.Role.NO_ROLE)
            {
                var approver = ResolveApprover(context, approval_flow.NextRole, approval_process);
                approval_flow.Approver = approver;
            }

            return approval_flow;
        }

        private Approver ResolveApprover(ClientContext context, string role, ApprovalProcessItems approval_process)
        {
            var roleHandler = new Dictionary<string, Delegate>();
            roleHandler.Add(Constants.Role.EMPLOYEE, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetEmployee));
            roleHandler.Add(Constants.Role.EMT2, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetEMT2));
            roleHandler.Add(Constants.Role.EMT1, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetEMT1));
            roleHandler.Add(Constants.Role.EMT, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetEMT));
            roleHandler.Add(Constants.Role.WFO_ADMIN, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetWFOAdmin));
            roleHandler.Add(Constants.Role.EXCEPTION_COMMITTEE, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetExceptionCommittee));
            roleHandler.Add(Constants.Role.MANAGER, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetManager));
            roleHandler.Add(Constants.Role.HR, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetHR));
            roleHandler.Add(Constants.Role.BACKUP_SUBMITTER, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetBackupSubmitter));
            roleHandler.Add(Constants.Role.BACKUP_APPROVERS, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetBackupApprovers));
            roleHandler.Add(Constants.Role.DASHBOARD_VIEWERS, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetDashboardViewers));
            roleHandler.Add(Constants.Role.SERVICE_EMAIL, new Func<ClientContext, ApprovalProcessItems, Approver>(Helper.GetServiceEmail));

            var approver = roleHandler[role].DynamicInvoke(context, approval_process);
            
            return approver as Approver;
        }
    }
}