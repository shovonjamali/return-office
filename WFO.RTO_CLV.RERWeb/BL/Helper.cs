using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using WFO.RTO_CLV.RERWeb.AppServices;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;

namespace WFO.RTO_CLV.RERWeb.BL
{
    public class Helper
    {
        protected General GeneralService { get; set; }

        public Helper()
        {
            GeneralService = new General();
        }

        private readonly List<ApprovalFlow> _rtoApprovalFlow = new List<ApprovalFlow>()
        {
            { new ApprovalFlow() { RolebasedAction = Constants.Role.MANAGER + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.EMPLOYEE, NextStatus = Constants.Status.SUBMITTED, NextInternalStatus = Constants.InternalStatus.SUBMITTED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.MANAGER + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.NA, NextRole = Constants.Role.HR, NextStatus = Constants.Status.EMT1_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT1_APPROVED, CommentsColumn = Constants.FlowConstant.NA} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.EMT2, NextStatus = Constants.Status.EMPLOYEE_ACKNOWLEDGED, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.EMT2, NextRole = Constants.Role.EMT1, NextStatus = Constants.Status.EMT2_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT2_APPROVED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.SECOND, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_ACKNOWLEDGED_HR, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED_HR, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.EXCEPTION, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_ACKNOWLEDGED_EXCEPTION, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED_EXCEPTION, CommentsColumn = Constants.FlowConstant.NA} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.DECLINED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_DECLINED, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_DECLINED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.DECLINED + Constants.FlowConstant.SECOND, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_DECLINED_HR, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_DECLINED_HR, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.DECLINED + Constants.FlowConstant.EXCEPTION, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_DECLINED_EXCEPTION, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_DECLINED_EXCEPTION, CommentsColumn = Constants.FlowConstant.NA} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT2 + Constants.ApproverAction.APPROVED, NextRole = Constants.Role.EMT1, NextStatus = Constants.Status.EMT2_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT2_APPROVED, CommentsColumn = Constants.RequestColumns.EMT2_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT2 + Constants.ApproverAction.REJECTED, NextRole = Constants.Role.WFO_ADMIN, NextStatus = Constants.Status.EMT2_REJECTED, NextInternalStatus = Constants.InternalStatus.EMT2_REJECTED, CommentsColumn = Constants.RequestColumns.EMT2_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT2 + Constants.ApproverAction.NMI, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMT2_NMI, NextInternalStatus = Constants.InternalStatus.EMT2_NMI, CommentsColumn = Constants.RequestColumns.EMT2_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT1 + Constants.ApproverAction.APPROVED, NextRole = Constants.Role.HR, NextStatus = Constants.Status.EMT1_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT1_APPROVED, CommentsColumn = Constants.RequestColumns.EMT1_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT1 + Constants.ApproverAction.REJECTED, NextRole = Constants.Role.WFO_ADMIN, NextStatus = Constants.Status.EMT1_REJECTED, NextInternalStatus = Constants.InternalStatus.EMT1_REJECTED, CommentsColumn = Constants.RequestColumns.EMT1_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT1 + Constants.ApproverAction.NMI, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMT1_NMI, NextInternalStatus = Constants.InternalStatus.EMT1_NMI, CommentsColumn = Constants.RequestColumns.EMT1_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.HR + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.EMPLOYEE, NextStatus = Constants.Status.HR_SUBMITTED, NextInternalStatus = Constants.InternalStatus.HR_SUBMITTED, CommentsColumn = Constants.RequestColumns.HR_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.HR + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.EXCEPTION, NextRole = Constants.Role.EMPLOYEE, NextStatus = Constants.Status.HR_SUBMITTED_EXCEPTION, NextInternalStatus = Constants.InternalStatus.HR_SUBMITTED_EXCEPTION, CommentsColumn = Constants.RequestColumns.HR_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.WFO_ADMIN + Constants.ApproverAction.FORWARDED, NextRole = Constants.Role.EXCEPTION_COMMITTEE, NextStatus = Constants.Status.SENT_EXCEPTION, NextInternalStatus = Constants.InternalStatus.SENT_EXCEPTION, CommentsColumn = Constants.RequestColumns.WFO_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.WFO_ADMIN + Constants.ApproverAction.NO_ACTION, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.NO_ACTION, NextInternalStatus = Constants.InternalStatus.NO_ACTION, CommentsColumn = Constants.RequestColumns.WFO_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EXCEPTION_COMMITTEE + Constants.ApproverAction.APPROVED, NextRole = Constants.Role.HR, NextStatus = Constants.Status.EXCEPTION_APPROVED, NextInternalStatus = Constants.InternalStatus.EXCEPTION_APPROVED, CommentsColumn = Constants.RequestColumns.EXCEPTION_COMMITTEE_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EXCEPTION_COMMITTEE + Constants.ApproverAction.REJECTED, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EXCEPTION_REJECTED, NextInternalStatus = Constants.InternalStatus.EXCEPTION_REJECTED, CommentsColumn = Constants.RequestColumns.EXCEPTION_COMMITTEE_COMMENTS} }
        };

        private readonly List<ApprovalFlow> _clvApprovalFLow = new List<ApprovalFlow>()
        {
            { new ApprovalFlow() { RolebasedAction = Constants.Role.MANAGER + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.EMPLOYEE, NextStatus = Constants.Status.SUBMITTED, NextInternalStatus = Constants.InternalStatus.SUBMITTED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.MANAGER + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.NA, NextRole = Constants.Role.HR, NextStatus = Constants.Status.EMT1_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT1_APPROVED, CommentsColumn = Constants.FlowConstant.NA} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.EMT2, NextStatus = Constants.Status.EMPLOYEE_ACKNOWLEDGED, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.EMT2, NextRole = Constants.Role.EMT1, NextStatus = Constants.Status.EMT2_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT2_APPROVED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.SECOND, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_ACKNOWLEDGED_HR, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED_HR, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.ACKNOWLEDGED + Constants.FlowConstant.EXCEPTION, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_ACKNOWLEDGED_EXCEPTION, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED_EXCEPTION, CommentsColumn = Constants.FlowConstant.NA} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.DECLINED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_DECLINED, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_DECLINED, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.DECLINED + Constants.FlowConstant.SECOND, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_DECLINED_HR, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_DECLINED_HR, CommentsColumn = Constants.FlowConstant.NA} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMPLOYEE + Constants.ApproverAction.DECLINED + Constants.FlowConstant.EXCEPTION, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMPLOYEE_DECLINED_EXCEPTION, NextInternalStatus = Constants.InternalStatus.EMPLOYEE_DECLINED_EXCEPTION, CommentsColumn = Constants.FlowConstant.NA} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT2 + Constants.ApproverAction.APPROVED, NextRole = Constants.Role.EMT1, NextStatus = Constants.Status.EMT2_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT2_APPROVED, CommentsColumn = Constants.RequestColumns.EMT2_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT2 + Constants.ApproverAction.REJECTED, NextRole = Constants.Role.WFO_ADMIN, NextStatus = Constants.Status.EMT2_REJECTED, NextInternalStatus = Constants.InternalStatus.EMT2_REJECTED, CommentsColumn = Constants.RequestColumns.EMT2_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT2 + Constants.ApproverAction.NMI, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMT2_NMI, NextInternalStatus = Constants.InternalStatus.EMT2_NMI, CommentsColumn = Constants.RequestColumns.EMT2_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT1 + Constants.ApproverAction.APPROVED, NextRole = Constants.Role.HR, NextStatus = Constants.Status.EMT1_APPROVED, NextInternalStatus = Constants.InternalStatus.EMT1_APPROVED, CommentsColumn = Constants.RequestColumns.EMT1_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT1 + Constants.ApproverAction.REJECTED, NextRole = Constants.Role.WFO_ADMIN, NextStatus = Constants.Status.EMT1_REJECTED, NextInternalStatus = Constants.InternalStatus.EMT1_REJECTED, CommentsColumn = Constants.RequestColumns.EMT1_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EMT1 + Constants.ApproverAction.NMI, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EMT1_NMI, NextInternalStatus = Constants.InternalStatus.EMT1_NMI, CommentsColumn = Constants.RequestColumns.EMT1_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.HR + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.REGULAR, NextRole = Constants.Role.EMPLOYEE, NextStatus = Constants.Status.HR_SUBMITTED, NextInternalStatus = Constants.InternalStatus.HR_SUBMITTED, CommentsColumn = Constants.RequestColumns.HR_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.HR + Constants.ApproverAction.SUBMITTED + Constants.FlowConstant.EXCEPTION, NextRole = Constants.Role.EMPLOYEE, NextStatus = Constants.Status.HR_SUBMITTED_EXCEPTION, NextInternalStatus = Constants.InternalStatus.HR_SUBMITTED_EXCEPTION, CommentsColumn = Constants.RequestColumns.HR_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.WFO_ADMIN + Constants.ApproverAction.FORWARDED, NextRole = Constants.Role.EXCEPTION_COMMITTEE, NextStatus = Constants.Status.SENT_EXCEPTION, NextInternalStatus = Constants.InternalStatus.SENT_EXCEPTION, CommentsColumn = Constants.RequestColumns.WFO_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.WFO_ADMIN + Constants.ApproverAction.NO_ACTION, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.NO_ACTION, NextInternalStatus = Constants.InternalStatus.NO_ACTION, CommentsColumn = Constants.RequestColumns.WFO_COMMENTS} },

            { new ApprovalFlow() { RolebasedAction = Constants.Role.EXCEPTION_COMMITTEE + Constants.ApproverAction.APPROVED, NextRole = Constants.Role.HR, NextStatus = Constants.Status.EXCEPTION_APPROVED, NextInternalStatus = Constants.InternalStatus.EXCEPTION_APPROVED, CommentsColumn = Constants.RequestColumns.EXCEPTION_COMMITTEE_COMMENTS} },
            { new ApprovalFlow() { RolebasedAction = Constants.Role.EXCEPTION_COMMITTEE + Constants.ApproverAction.REJECTED, NextRole = Constants.Role.NO_ROLE, NextStatus = Constants.Status.EXCEPTION_REJECTED, NextInternalStatus = Constants.InternalStatus.EXCEPTION_REJECTED, CommentsColumn = Constants.RequestColumns.EXCEPTION_COMMITTEE_COMMENTS} }
        };

        internal string DefineFlowKey(ApprovalProcessItems approval_process)
        {
            string role = Convert.ToString(approval_process.TaskItem[Constants.TaskColumns.ROLE]);
            string action = Convert.ToString(approval_process.TaskItem[Constants.TaskColumns.ACTION]);

            string current_status = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.INTERNAL_STATUS]);
            int manager_count = Convert.ToInt16(approval_process.RequestItem[Constants.RequestColumns.MANAGER_COUNT]);

            if (role == Constants.Role.MANAGER && action == Constants.ApproverAction.SUBMITTED)
            {
                return ManageManagerSubmits(role, action, manager_count);
            }

            else if (role == Constants.Role.EMPLOYEE && action == Constants.ApproverAction.ACKNOWLEDGED)
            {
                return ManageEmployeeAcknowledges(role, action, current_status, manager_count);
            }

            else if (role == Constants.Role.EMPLOYEE && action == Constants.ApproverAction.DECLINED)
            {
                return ManageEmployeeDeclines(role, action, current_status);
            }

            else if (role == Constants.Role.HR && action == Constants.ApproverAction.SUBMITTED)
            {
                return ManageHRSubmits(role, action, current_status);
            }

            return role + action;
        }

        private string ManageManagerSubmits(string role, string action, int manager_count)
        {
            if (manager_count < Constants.FlowLevelIdentifier.CONSTANT)
            {
                // Manger is EMT1, EMT, or CEO. Request goes to HR
                return role + action + Constants.FlowConstant.NA;
            }

            return role + action + Constants.FlowConstant.REGULAR;
        }

        private string ManageEmployeeAcknowledges(string role, string action, string current_status, int manager_count)
        {
            if (manager_count == Constants.FlowLevelIdentifier.CONSTANT && current_status != Constants.InternalStatus.HR_SUBMITTED && current_status != Constants.InternalStatus.HR_SUBMITTED_EXCEPTION)
            {
                // Manager is EMT2, Employee Acknowledging for first time
                return role + action + Constants.FlowConstant.EMT2;
            }
            else if (current_status == Constants.InternalStatus.HR_SUBMITTED)
            {
                // Employee Acknowledging second time
                return role + action + Constants.FlowConstant.SECOND;
            }
            else if (current_status == Constants.InternalStatus.HR_SUBMITTED_EXCEPTION)
            {
                // Employee Acknowledging second time for Exception flow
                return role + action + Constants.FlowConstant.EXCEPTION;
            }
            else
            {
                // Regular flow. Employee Acknowledging for first time
                return role + action + Constants.FlowConstant.REGULAR;
            }
        }

        private string ManageEmployeeDeclines(string role, string action, string current_status)
        {
            if (current_status == Constants.InternalStatus.HR_SUBMITTED)
            {
                // Employee Declines after HR Submits for Regular flow
                return role + action + Constants.FlowConstant.SECOND;
            }
            else if (current_status == Constants.InternalStatus.HR_SUBMITTED_EXCEPTION)
            {
                // Employee Declines after HR Submits for Exception flow
                return role + action + Constants.FlowConstant.EXCEPTION;
            }
            else
            {
                // Regular flow. Employee Declines
                return role + action + Constants.FlowConstant.REGULAR;
            }
        }

        private string ManageHRSubmits(string role, string action, string current_status)
        {
            if (current_status == Constants.InternalStatus.EXCEPTION_APPROVED)
            {
                // HR submis for Exception flow
                return role + action + Constants.FlowConstant.EXCEPTION;
            }
            else
            {
                // HR submis for Regular flow
                return role + action + Constants.FlowConstant.REGULAR;
            }
        }

        internal ApprovalFlow GetRTOApprovalFlow(string role_based_action)
        {
            return _rtoApprovalFlow.Where(rto => rto.RolebasedAction == role_based_action).FirstOrDefault();
        }

        internal ApprovalFlow GetCLVApprovalFlow(string role_based_action)
        {
            return _clvApprovalFLow.Where(vcl => vcl.RolebasedAction == role_based_action).FirstOrDefault();
        }

        internal TaskItem GetTaskItemData(ApprovalProcessItems approval_process, FieldUrlValue url)
        {
            var task_item = new TaskItem();

            task_item.Title = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.TITLE]);
            task_item.InstanceType = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.REQUEST_TYPE]);
            task_item.PrimaryId = Convert.ToInt32(approval_process.RequestItem.Id);
            task_item.Role = approval_process.ApprovalFlow.NextRole;
            task_item.State = Constants.ApprovalState.PENDING;
            task_item.Link = url;
            task_item.Approver = approval_process.ApprovalFlow.Approver.ApproverString;
            task_item.Action = Constants.ApproverAction.PENDING;

            return task_item;
        }

        internal RequestItem GetRequestItemData(ApprovalProcessItems approval_process)
        {
            var request_item = new RequestItem();

            request_item.Status = approval_process.ApprovalFlow.NextStatus;
            request_item.InternalStatus = approval_process.ApprovalFlow.NextInternalStatus;
            request_item.ApproverText = Convert.ToString(approval_process.TaskItem[Constants.TaskColumns.COMMENTS]);

            return request_item;
        }

        internal Approver GetEmployee(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.RequestColumns.EMPLOYEE, approval_process.RequestItem, Constants.ApproverType.PERSON);

        internal Approver GetManager(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.RequestColumns.MANAGER, approval_process.RequestItem, Constants.ApproverType.PERSON);

        internal Approver GetBackupSubmitter(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.RequestColumns.BACKUP_SUBMITTER, approval_process.RequestItem, Constants.ApproverType.PERSON);

        internal Approver GetEMT2(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.RequestColumns.EMT2, approval_process.RequestItem, Constants.ApproverType.EMAIL);

        internal Approver GetEMT1(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.RequestColumns.EMT1, approval_process.RequestItem, Constants.ApproverType.EMAIL);

        internal Approver GetEMT(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.RequestColumns.EMT, approval_process.RequestItem, Constants.ApproverType.EMAIL);

        internal Approver GetWFOAdmin(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.ConfigColumns.WFO, approval_process.ConfigItem, Constants.ApproverType.GROUP);

        internal Approver GetExceptionCommittee(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.ConfigColumns.EXCEPTION_COMMITTEE, approval_process.ConfigItem, Constants.ApproverType.GROUP);

        internal Approver GetBackupApprovers(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.ConfigColumns.BACKUP_APPROVERS, approval_process.ConfigItem, Constants.ApproverType.GROUP);

        internal Approver GetDashboardViewers(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.ConfigColumns.DASHBOARD_VIEWERS, approval_process.ConfigItem, Constants.ApproverType.GROUP);

        internal Approver GetServiceEmail(ClientContext context, ApprovalProcessItems approval_process) => GeneralService.GetApprover(context, Constants.ConfigColumns.SERVICE_EMAIL_BOX, approval_process.ConfigItem, Constants.ApproverType.EMAIL);

        internal Approver GetHR(ClientContext context, ApprovalProcessItems approval_process)
        {
            string approver_column = GetHRByRegion(approval_process);
            return GeneralService.GetApprover(context, approver_column, approval_process.ConfigItem, Constants.ApproverType.GROUP);
        }

        internal string GetHRByRegion(ApprovalProcessItems approval_process)
        {
            string region = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.REGION]);
            string approver_column = string.Empty;

            switch (region)
            {
                case Constants.Regions.NA:
                    approver_column = Constants.ConfigColumns.HR_NA;
                    break;
                case Constants.Regions.INDIA:
                    approver_column = Constants.ConfigColumns.HR_INDIA;
                    break;
                case Constants.Regions.EUROPE:
                    approver_column = Constants.ConfigColumns.HR_EUROPE;
                    break;
                case Constants.Regions.ASIA_PAC:
                    approver_column = Constants.ConfigColumns.HR_ASIAPAC;
                    break;
                case Constants.Regions.JAPAN:
                    approver_column = Constants.ConfigColumns.HR_JAPAN;
                    break;
                default:
                    approver_column = Constants.ConfigColumns.HR_GLOBAL;
                    break;
            }

            return approver_column;
        }
    }
}