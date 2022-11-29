using Microsoft.SharePoint.Client;
using System;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;

namespace WFO.RTO_CLV.RERWeb.AppServices
{
    public static class Log
    {
        public static int TaskId { get; set; }

        public static void LogEvent(ClientContext context, Exception ex, string error_type, ApprovalProcessItems approval_process = null)
        {
            List list = context.Web.Lists.GetByTitle(Constants.CommonLists.EVENT_LOG_LIST);
            ListItemCreationInformation listCreationInformation = new ListItemCreationInformation();
            ListItem item = list.AddItem(listCreationInformation);

            item[Constants.LogColumns.TITLE] = $"Error occured while processing Task Item: {TaskId}, on {DateTime.Now}";
            item[Constants.LogColumns.EVENT_NAME] = $"Error occured while processing Task Item: {TaskId}, on {DateTime.Now}";
            item[Constants.LogColumns.MESSAGE] = $"Message: {ex.Message}\nStack Trace: {ex.StackTrace.Trim()}";
            item[Constants.LogColumns.ERROR_TYPE] = error_type;
            item[Constants.LogColumns.REQUEST_ID] = Convert.ToString(approval_process?.RequestItem?.Id);

            item.Update();
            context.ExecuteQuery();
        }
    }
}