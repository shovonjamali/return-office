using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;

namespace WFO.RTO_CLV.RERWeb.AppServices
{
    public class General
    {
        internal ListItem GetItemById(ClientContext context, int item_id, string list_name)
        {
            var spList = context.Web.Lists.GetByTitle(list_name);
            ListItem item = spList.GetItemById(item_id);

            context.Load(item);
            context.Load(item, II => II.HasUniqueRoleAssignments);
            context.Load(item, II => II.RoleAssignments);
            context.ExecuteQuery();

            return item;
        }

        internal ListItemCollection GetEmailTemplates(ClientContext context, string internal_status, string request_type, string list_name)
        {
            List templateList = context.Web.Lists.GetByTitle(list_name);

            CamlQuery query = new CamlQuery();
            query.ViewXml =
                        @"<View>
                                <Query>
                                    <Where>
                                        <And>
                                            <Eq>
                                                <FieldRef Name='InternalStatus'/>
                                                <Value Type='Text'>" + internal_status + @"</Value>
                                            </Eq>
                                            <Eq>
                                                <FieldRef Name='RequestType'/>
                                                <Value Type='Text'>" + request_type + @"</Value>
                                            </Eq>
                                        </And>
                                    </Where>
                                </Query>
                            </View>";

            ListItemCollection items = templateList.GetItems(query);

            context.Load(items);
            context.ExecuteQuery();

            return items;
        }

        internal ListItem GetConfigItem(ClientContext context, string list_name)
        {
            List templateList = context.Web.Lists.GetByTitle(list_name);

            // CamlQuery query = new CamlQuery(); 
            ListItemCollection items = templateList.GetItems(new CamlQuery());

            context.Load(items);
            context.ExecuteQuery();

            return items[0];
        }

        internal Approver GetApprover(ClientContext context, string approver_column, ListItem item, string approver_type)
        {
            //User sp_user = null;
            FieldUserValue userFieldValue = new FieldUserValue();
            var approver = new Approver();

            if (approver_type == Constants.ApproverType.PERSON)
            {
                if (Convert.ToString(item[approver_column]) != "" && Convert.ToString(item[approver_column]) != null)
                {
                    userFieldValue = item[approver_column] as FieldUserValue;
                    approver = EnsureApprover(context, userFieldValue.Email);
                }
            }
            else if (approver_type == Constants.ApproverType.EMAIL)
            {
                // IF MANAGER EQ CEO, EMT, EMT-1 => EMT-1, EMT-2 WILL BE BLANK/EMPTY
                // IF MANAGER EQ EMT-2 => EMT-2 WILL BE BLANK/EMPTY
                approver = Convert.ToString(item[approver_column]) != "" ? EnsureApprover(context, Convert.ToString(item[approver_column])) : null;
            }
            else if (approver_type == Constants.ApproverType.ALIAS)
            {
                // do if alias
            }
            else    // if the field contains group
            {
                approver = GetApproversFromGroup(context, approver_column, item);
            }

            return approver;
        }

        private Approver GetApproversFromGroup(ClientContext context, string approver_column, ListItem item)
        {
            FieldUserValue userFieldValue = item[approver_column] as FieldUserValue;

            Group group = context.Web.SiteGroups.GetById(userFieldValue.LookupId);
            UserCollection userCollection = group.Users;

            context.Load(userCollection);
            context.ExecuteQuery();

            var approverIDList = new List<string>();
            var emails = new List<string>();

            foreach (User user in userCollection)
            {
                try
                {
                    User ensuredMember = context.Web.EnsureUser(user.LoginName);
                    context.Load(ensuredMember);
                    context.ExecuteQuery();

                    approverIDList.Add(ensuredMember.Id + ";#" + Convert.ToString(ensuredMember.LoginName));
                    emails.Add(ensuredMember.Email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\nStack trace: {ex.StackTrace.Trim()}");
                    Log.LogEvent(context, ex, Constants.ErrorTypes.EVALUATION);
                }
            }

            var approver = new Approver();
            approver.ApproverString = string.Join(";#", approverIDList);
            approver.ApproverEmail = string.Join(";", emails);
            approver.ApproverName = "";

            return approver;
        }

        private Approver EnsureApprover(ClientContext context, string login)
        {
            User sp_user = context.Web.EnsureUser(login);
            context.Load(sp_user);
            context.ExecuteQuery();

            var approver = new Approver();
            approver.ApproverString = Convert.ToString(sp_user.Id);     // FOR CREATING TASK
            approver.ApproverEmail = sp_user.Email;                     // FOR SENDING EMAIL
            approver.ApproverName = sp_user.Title;                      // FOR ADDRESSING IN EMAIL

            return approver;
        }

        internal int CreateTask(ClientContext context, string list_name, TaskItem item)
        {
            List list = context.Web.Lists.GetByTitle(list_name);
            ListItemCreationInformation listCreationInformation = new ListItemCreationInformation();
            ListItem listItem = list.AddItem(listCreationInformation);

            listItem[Constants.TaskColumns.PRIMARY_ID] = item.PrimaryId;
            listItem[Constants.TaskColumns.STATE] = item.State;
            listItem[Constants.TaskColumns.REQUEST_LINK] = item.Link;
            listItem[Constants.TaskColumns.APPROVER] = item.Approver;
            listItem[Constants.TaskColumns.TITLE] = item.Title;
            listItem[Constants.TaskColumns.ROLE] = item.Role;
            listItem[Constants.TaskColumns.TYPE] = item.InstanceType;
            listItem[Constants.TaskColumns.ACTION] = item.Action;

            listItem.Update();
            context.ExecuteQuery();

            return listItem.Id;
        }

        internal void UpdateMainRequest(ClientContext context, RequestItem request, ApprovalProcessItems approval_process)
        {
            var main_item = approval_process.RequestItem;

            main_item[Constants.RequestColumns.STATUS] = request.Status;
            main_item[Constants.RequestColumns.INTERNAL_STATUS] = request.InternalStatus;

            if (approval_process.ApprovalFlow.CommentsColumn != Constants.FlowConstant.NA)
            {
                string old_comments = Convert.ToString(main_item[approval_process.ApprovalFlow.CommentsColumn]);
                string approver_comments = Convert.ToString(approval_process.TaskItem[Constants.TaskColumns.COMMENTS]);

                if (approver_comments != null && approver_comments != "")
                {
                    string new_comments = $"{Convert.ToDateTime(approval_process.TaskItem[Constants.TaskColumns.DATE]).ToString("MM/dd/yyyy")}\n{Convert.ToString(approval_process.TaskItem[Constants.TaskColumns.ACTION])}: {approver_comments}";
                    string merged_comments = old_comments != "" ? old_comments + "\n\n" + new_comments : new_comments;
                    main_item[approval_process.ApprovalFlow.CommentsColumn] = merged_comments;
                }
            }

            main_item.Update();
            context.ExecuteQuery();
        }

        internal FieldUrlValue GetTaskLink(ClientContext context, string list_name, ApprovalProcessItems approval_process)
        {
            // To retrieve clientContext.Web.Url used in FieldUrlValue url
            context.Load(context.Web);
            context.ExecuteQuery();

            FieldUrlValue url = new FieldUrlValue();
            url.Url = context.Web.Url + "/Lists/" + list_name + "/DispForm.aspx?ID=" + Convert.ToInt32(approval_process.RequestItem.Id);      // TODO: need to update based on the approach
            url.Description = "Click Here";

            return url;
        }

        internal void RevertActions(ClientContext context, ApprovalProcessItems approval_process, string request_list, string task_list)
        {
            // TO-DO:
            // CHECK IF NEW TASK ID IS NOT 0
            // DELETE CREATED TASK
            // REVERT BACK PERMISSION CHANGES
            // REVERT MAIN REQUEST UPDATES

            Trace.TraceInformation("All actions are reverted");
        }
    }
}