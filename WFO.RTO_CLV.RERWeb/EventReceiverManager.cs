using System;
using Microsoft.SharePoint.Client;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;
using WFO.RTO_CLV.RERWeb.AppServices;
using WFO.RTO_CLV.RERWeb.Interfaces;
using WFO.RTO_CLV.RERWeb.BL;

namespace WFO.RTO_CLV.RERWeb
{
    public class EventReceiverManager
    {
        protected FlowAid Pander { get; set; } = new FlowAid();
        protected General GeneralService { get; set; } = new General();

        internal void AssociateRemoteEventsToHostWeb(ClientContext clientContext, EventReceiverType eventReceiverType, EventReceiverSynchronization objEventReceiverSynchronization, string receiverName)
        {
            if (clientContext != null)
            {
                var taskList = clientContext.Web.Lists.GetByTitle(Constants.CommonLists.TASK_LIST);
                clientContext.Load(taskList, p => p.EventReceivers);
                clientContext.ExecuteQuery();

                var rer = taskList.EventReceivers.Where(e => e.ReceiverName == receiverName).FirstOrDefault();

                try
                {
                    Trace.WriteLine("Removing " + receiverName + " receiver at " + rer.ReceiverUrl);
                    // This will fail when deploying via F5, but works
                    // when deployed to production
                    rer.DeleteObject();
                    clientContext.ExecuteQuery();
                }
                catch (Exception oops)
                {
                    Trace.TraceError(oops.Message);
                    Trace.TraceError(oops.StackTrace);
                }

                EventReceiverDefinitionCreationInformation receiver = new EventReceiverDefinitionCreationInformation();
                receiver.EventType = eventReceiverType;

                // Get WCF URL where this message was handled
                OperationContext op = OperationContext.Current;
                Message msg = op.RequestContext.RequestMessage;
                receiver.ReceiverUrl = msg.Headers.To.ToString();

                receiver.ReceiverName = receiverName;
                receiver.Synchronization = objEventReceiverSynchronization;

                // Add the new event receiver to a list in the host web
                taskList.EventReceivers.Add(receiver);
                clientContext.ExecuteQuery();

                Trace.WriteLine("Added " + receiverName + " receiver at " + receiver.ReceiverUrl);
                Trace.WriteLine("Event Receiver AssociateRemoteEventsToHostWeb sucessfully.");
            }
        }

        internal void RemoveEventReceiversFromHostWeb(ClientContext clientContext, string receiverName)
        {
            var taskList = clientContext.Web.Lists.GetByTitle(Constants.CommonLists.TASK_LIST);
            clientContext.Load(taskList, p => p.EventReceivers);
            clientContext.ExecuteQuery();

            var rer = taskList.EventReceivers.Where(e => e.ReceiverName == receiverName).FirstOrDefault();

            try
            {
                Trace.WriteLine("Removing " + receiverName + " receiver at " + rer.ReceiverUrl);
                // This will fail when deploying via F5, but works
                // when deployed to production
                rer.DeleteObject();
                clientContext.ExecuteQuery();
            }
            catch (Exception oops)
            {
                Trace.TraceError(oops.Message);
                Trace.TraceError(oops.StackTrace);
            }
        }

        internal void ItemUpdatedEventHandler(ClientContext context, Guid listId, string listTitle, int listItemId)
        {
            // TestingRER(context, listId, listItemId);     // A SIMPLE CHECK TO SEE IF RER IS TRIGGERING OR NOT
            ItemUpdatedExecution(context, listId, listTitle, listItemId);
        }

        private void ItemUpdatedExecution(ClientContext context, Guid listId, string listTitle, int listItemId)
        {
            var approval_process = new ApprovalProcessItems();

            var task_item = GeneralService.GetItemById(context, listItemId, Constants.CommonLists.TASK_LIST);
            string application = Convert.ToString(task_item[Constants.TaskColumns.TYPE]);

            IApprovalProcess process = Pander.GetApplicationType(application);
            Log.TaskId = listItemId;

            try
            {
                Trace.TraceInformation($"{application} process started. Task item: {task_item.Id}\n====================================");

                // GETTING TASK, MAIN, EMAIL TEMPLATE ITEMS
                approval_process = process.GetApprovalProcess(context, task_item);

                // CHECKING IF ALL THE RELATED USERS ARE AVAILABLE
                var validated_users = Pander.ValidateUsers(approval_process);
                                
                if (!validated_users.Error)
                {
                    // CREATING TASK ITEM
                    approval_process.ApprovalFlow.NewTaskId = process.CreateTask(context, approval_process);

                    // UPDAING STATUS AND APPROVER COMMENTS OF MAIN ITEM
                    process.UpdateMainRequest(context, approval_process);

                    // SETTING PERMISSION FOR MAIN ITEM
                    process.SetPermission(context, approval_process);

                    // SENDING EMAIL TO BUSINESS USERS
                    process.SendEmail(context, approval_process);

                    Trace.TraceInformation("\nProcess completed\n==================");
                }
                else
                {
                    throw new Exception(validated_users.Message);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error: {ex.Message}\nStack trace: {ex.StackTrace.Trim()}");
                Log.LogEvent(context, ex, Constants.ErrorTypes.FATAL, approval_process);
                // IN CASE OF EXCEPTION WE MAY NEED TO REVERT BACK ALL THE PERFORMED ACTIONS.                
                process.RevertActions(context, approval_process);
            }
        }

        private void TestingRER(ClientContext context, Guid listId, int listItemId)
        {
            var list = context.Web.Lists.GetById(listId);
            ListItem taskItem = list.GetItemById(listItemId);
            context.Load(taskItem);
            context.ExecuteQuery();

            // TESTING RER
            ListItemCreationInformation listCreationInformation = new ListItemCreationInformation();
            ListItem listItem = list.AddItem(listCreationInformation);
            listItem[Constants.TaskColumns.TITLE] = $"RER started for processing item: {taskItem[Constants.TaskColumns.PRIMARY_ID]}";
            listItem.Update();
            context.ExecuteQuery();
        }
    }
}