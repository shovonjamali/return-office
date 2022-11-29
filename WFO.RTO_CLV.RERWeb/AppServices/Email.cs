using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using System;
using System.Collections.Generic;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;

namespace WFO.RTO_CLV.RERWeb.AppServices
{
    public class Email
    {
        internal void SendEmail(ClientContext context, EmailObject email_object)
        {
            EmailProperties emailProperties = new EmailProperties();

            emailProperties.Subject = email_object.Content.Subject;
            emailProperties.Body = email_object.Content.Body;

            emailProperties.To = GetEmailAddressList(email_object.Recipients.TOAddress);
            emailProperties.CC = GetEmailAddressList(email_object.Recipients.CCAddress);

            emailProperties.AdditionalHeaders = GetEmailHeaders(email_object);

            Utility.SendEmail(context, emailProperties);
            context.ExecuteQuery();
        }

        private Dictionary<string, string> GetEmailHeaders(EmailObject email_object)
        {
            if (email_object.Content.SetImportance)
            {
                return new Dictionary<string, string>()
                {
                    { "Content-Type", "text/plain" },
                    { "fAppendHtmlTag", "true" },
                    { "fHtmlEncode", "true" },
                    { "X-Priority", "1 (Highest)" },
                    { "X-MSMail-Priority", "High" },
                    { "Importance", "High" }
                };
            }

            return new Dictionary<string, string>()
            {
                { "Content-Type", "text/plain" },
                { "fAppendHtmlTag", "true" },
                { "fHtmlEncode", "true" }
            };
        }

        private List<string> GetEmailAddressList(string emailAddress)
        {
            var emailAddressesList = new List<string>();

            string[] splitEmails = emailAddress.Split(';');
            foreach (string email in splitEmails)
            {
                if (email != string.Empty)
                {
                    emailAddressesList.Add(email);
                }
            }

            return emailAddressesList;
        }

        internal EmailAddress GetEmailRecipients(ApprovalProcessItems approval_process, int index)
        {
            ListItem email_item = approval_process.EmailItems[index];
            var app_users = approval_process.AppUsers;

            string to = Convert.ToString(email_item[Constants.EmailColumns.TO]);
            string cc = Convert.ToString(email_item[Constants.EmailColumns.CC]);

            var email_address = new EmailAddress();
            email_address.TOAddress = ResolveEmailAddress(app_users, to);
            email_address.CCAddress = ResolveEmailAddress(app_users, cc);

            return email_address;
        }

        private string ResolveEmailAddress(AppUsers app_users, string address)
        {
            string[] address_array = address.Split(';');

            var emails = string.Empty;

            foreach (var item in address_array)
            {
                if (item.Trim() == Constants.Role.MANAGER)
                {
                    emails += app_users.Manager?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.EMPLOYEE)
                {
                    emails += app_users.Employee?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.EMT1)
                {
                    emails += app_users.EMT1?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.EMT2)
                {
                    emails += app_users.EMT2?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.WFO_ADMIN)
                {
                    emails += app_users.WFOAdmin?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.EXCEPTION_COMMITTEE)
                {
                    emails += app_users.ExceptionCommittee?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.HR)
                {
                    emails += app_users.HR?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.BACKUP_SUBMITTER)
                {
                    emails += app_users.BackupSubmitter?.ApproverEmail + ";";
                }
                else if (item.Trim() == Constants.Role.SERVICE_EMAIL)
                {
                    emails += app_users.ServiceEmail?.ApproverEmail + ";";
                }
                else
                {
                    emails += "";
                }
            }

            return emails;  // emails.Remove(emails.Length - 1, 1);
        }

        internal EmailContent GetEmailContent(ClientContext context, ApprovalProcessItems approval_process, int index)
        {
            var email_content = new EmailContent();
            var app_users = approval_process.AppUsers;

            string subject = Convert.ToString(approval_process.EmailItems[index][Constants.EmailColumns.SUBJECT]);
            string body = Convert.ToString(approval_process.EmailItems[index][Constants.EmailColumns.BODY]);

            bool set_importance = Convert.ToBoolean(approval_process.EmailItems[index][Constants.EmailColumns.SET_IMPORTANCE]);

            string request_type = Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.REQUEST_TYPE]) == Constants.Applications.RTO ? Constants.Applications.RTO_LABEL : Constants.Applications.CLV_LABEL;

            subject = subject.Replace(Constants.EmailPlaceHolders.EMPLOYEE_NAME, app_users.Employee.ApproverName);

            subject = subject.Replace(Constants.EmailPlaceHolders.REQUEST_TYPE, request_type);

            body = body.Replace(Constants.EmailPlaceHolders.EMPLOYEE_NAME, app_users.Employee.ApproverName);
            body = body.Replace(Constants.EmailPlaceHolders.REQUEST_TYPE, request_type);

            string request_link = GetDisplayPageUrl(Convert.ToString(approval_process.RequestItem.Id), Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.TITLE]), context, Constants.RTOLists.REQUEST_LIST);
            body = body.Replace(Constants.EmailPlaceHolders.REQUEST_LINK, request_link);

            body = body.Replace(Constants.EmailPlaceHolders.MANAGER_NAME, app_users.Manager.ApproverName);
            body = body.Replace(Constants.EmailPlaceHolders.EMT2_NAME, app_users.EMT2?.ApproverName);
            body = body.Replace(Constants.EmailPlaceHolders.EMT1_NAME, app_users.EMT1?.ApproverName);

            // NEED MORE INFORMATION COMMENTS
            body = body.Replace(Constants.EmailPlaceHolders.EMT1_COMMENTS, Convert.ToString(approval_process.TaskItem[Constants.TaskColumns.COMMENTS]));
            body = body.Replace(Constants.EmailPlaceHolders.EMT2_COMMENTS, Convert.ToString(approval_process.TaskItem[Constants.TaskColumns.COMMENTS]));

            email_content.Subject = subject;
            email_content.Body = body;
            email_content.SetImportance = set_importance;

            return email_content;
        }

        private string GetDisplayPageUrl(string id, string title, ClientContext context, string list)
        {
            return string.Format("<a href='{0}/Lists/{3}/DispForm.aspx?ID={1}'>{2}</a>", context.Url, id, title, list);
        }
    }
}