using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WFO.RTO_CLV.RERWeb.BL;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;

namespace WFO.RTO_CLV.RERWeb.AppServices
{
    public class Permission
    {
        #region Permission Matrix List
        private readonly List<PermissionMatrix> _permissionMatrixList = new List<PermissionMatrix>()
        {
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.SUBMITTED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMPLOYEE_DECLINED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Reader}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMT2_APPROVED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMT2_REJECTED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMT2_NMI,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMT1_APPROVED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMT1_REJECTED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMT1_NMI,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.HR_SUBMITTED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED_HR,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Reader}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMPLOYEE_DECLINED_HR,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Reader}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.NO_ACTION,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Reader }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.SENT_EXCEPTION,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EXCEPTION_COMMITTEE, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EXCEPTION_APPROVED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },                        
                        new PermissionUnit() { Role = Constants.Role.EXCEPTION_COMMITTEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EXCEPTION_REJECTED,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EXCEPTION_COMMITTEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Reader }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.HR_SUBMITTED_EXCEPTION,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Contributor },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EXCEPTION_COMMITTEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Contributor}
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMPLOYEE_ACKNOWLEDGED_EXCEPTION,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EXCEPTION_COMMITTEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Reader }
                    }
                }
            },
            {
                new PermissionMatrix()
                {
                    State = Constants.InternalStatus.EMPLOYEE_DECLINED_EXCEPTION,
                    PermissionUnitList = new List<PermissionUnit>()
                    {
                        new PermissionUnit() { Role = Constants.Role.MANAGER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_SUBMITTER, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMPLOYEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT2, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT1, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EMT, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.EXCEPTION_COMMITTEE, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.HR, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.WFO_ADMIN, RoleType = RoleType.Administrator },
                        new PermissionUnit() { Role = Constants.Role.DASHBOARD_VIEWERS, RoleType = RoleType.Reader },
                        new PermissionUnit() { Role = Constants.Role.BACKUP_APPROVERS, RoleType = RoleType.Reader }
                    }
                }
            }
        };
        #endregion

        internal void BreakInheritance(ClientContext context, ApprovalProcessItems approval_process)
        {
            ListItem request = approval_process.RequestItem;

            Trace.TraceInformation($"Processing permission for item: {request.Id}");            

            if (!request.HasUniqueRoleAssignments)
            {
                request.BreakRoleInheritance(false, false);
                context.ExecuteQuery();
            }

            if (request.HasUniqueRoleAssignments)
            {
                foreach (var assignment in request.RoleAssignments)
                {
                    assignment.RoleDefinitionBindings.RemoveAll();
                    assignment.Update();
                    context.ExecuteQuery();
                }
            }
            
            Trace.TraceInformation($"Role permission deleted for all except System Account for item: {request.Id}");
        }

        internal List<PermissionUnit> GetPermissionMatrix(ClientContext context, ApprovalProcessItems approval_process)
        {
            var permission_matrix = _permissionMatrixList.Where(p => p.State == approval_process.ApprovalFlow.NextInternalStatus).FirstOrDefault();
            var permission_unit_list = new List<PermissionUnit>();

            foreach (var permission_unit in permission_matrix.PermissionUnitList)
            {
                Principal user_group = null;
                FieldUserValue user_value = null;

                string emt2_email = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT2]);
                string emt1_email = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT1]);
                string emt_email = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT]);

                if (permission_unit.Role == Constants.Role.MANAGER && approval_process.RequestItem[Constants.RequestColumns.MANAGER] != null)
                {
                    user_value = (FieldUserValue)approval_process.RequestItem[Constants.RequestColumns.MANAGER];
                    user_group = context.Web.SiteUsers.GetByEmail(user_value.Email);
                }
                else if (permission_unit.Role == Constants.Role.EMPLOYEE && approval_process.RequestItem[Constants.RequestColumns.EMPLOYEE] != null)
                {
                    user_value = (FieldUserValue)approval_process.RequestItem[Constants.RequestColumns.EMPLOYEE];
                    user_group = context.Web.SiteUsers.GetByEmail(user_value.Email);
                }
                else if (permission_unit.Role == Constants.Role.EMT2 && emt2_email != "" && emt2_email != null)
                {
                    user_group = context.Web.SiteUsers.GetByEmail(emt2_email);                    
                }
                else if (permission_unit.Role == Constants.Role.EMT1 && emt1_email != "" && emt1_email != null)
                {
                    user_group = context.Web.SiteUsers.GetByEmail(emt1_email);                    
                }
                else if (permission_unit.Role == Constants.Role.EMT && emt_email != "" && emt_email != null)
                {
                    user_group = context.Web.SiteUsers.GetByEmail(emt_email);
                }
                else if (permission_unit.Role == Constants.Role.HR && approval_process.ConfigItem[Constants.ConfigColumns.HR] != null)
                {
                    string approver_column = new Helper().GetHRByRegion(approval_process);
                    user_value = (FieldUserValue)approval_process.ConfigItem[approver_column];
                    user_group = context.Web.SiteGroups.GetByName(user_value.LookupValue);
                }
                else if (permission_unit.Role == Constants.Role.EXCEPTION_COMMITTEE && approval_process.ConfigItem[Constants.ConfigColumns.EXCEPTION_COMMITTEE] != null)
                {
                    user_value = (FieldUserValue)approval_process.ConfigItem[Constants.ConfigColumns.EXCEPTION_COMMITTEE];
                    user_group = context.Web.SiteGroups.GetByName(user_value.LookupValue);
                }
                else if (permission_unit.Role == Constants.Role.WFO_ADMIN && approval_process.ConfigItem[Constants.ConfigColumns.WFO] != null)
                {
                    user_value = (FieldUserValue)approval_process.ConfigItem[Constants.ConfigColumns.WFO];
                    user_group = context.Web.SiteGroups.GetByName(user_value.LookupValue);
                }
                else if (permission_unit.Role == Constants.Role.DASHBOARD_VIEWERS && approval_process.ConfigItem[Constants.ConfigColumns.DASHBOARD_VIEWERS] != null)
                {
                    user_value = (FieldUserValue)approval_process.ConfigItem[Constants.ConfigColumns.DASHBOARD_VIEWERS];
                    user_group = context.Web.SiteGroups.GetByName(user_value.LookupValue);
                }
                else if (permission_unit.Role == Constants.Role.BACKUP_APPROVERS && approval_process.ConfigItem[Constants.ConfigColumns.BACKUP_APPROVERS] != null)
                {
                    user_value = (FieldUserValue)approval_process.ConfigItem[Constants.ConfigColumns.BACKUP_APPROVERS];
                    user_group = context.Web.SiteGroups.GetByName(user_value.LookupValue);
                }
                else if (permission_unit.Role == Constants.Role.BACKUP_SUBMITTER && approval_process.RequestItem[Constants.RequestColumns.BACKUP_SUBMITTER] != null)
                {
                    user_value = (FieldUserValue)approval_process.RequestItem[Constants.RequestColumns.BACKUP_SUBMITTER];
                    user_group = context.Web.SiteUsers.GetByEmail(user_value.Email);
                }


                if (user_group != null)
                {
                    permission_unit_list.Add(new PermissionUnit() { UserGroup = user_group, RoleType = permission_unit.RoleType });
                }
            }

            return permission_unit_list;
        }

        internal void AssignUserRoleOnList(ClientContext context, ListItem list_item, List<PermissionUnit> permission_unit_list)
        {
            var roleDefBindCol = new RoleDefinitionBindingCollection(context);

            foreach (var permission in permission_unit_list)
            {
                roleDefBindCol.Add(context.Web.RoleDefinitions.GetByType(permission.RoleType));
                list_item.RoleAssignments.Add(permission.UserGroup, roleDefBindCol);

                list_item.Update();
                context.ExecuteQuery();
            }

            Trace.TraceInformation($"Permission set successfully for item: {list_item.Id}");
        }
    }
}