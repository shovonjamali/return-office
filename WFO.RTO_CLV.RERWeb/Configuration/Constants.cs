namespace WFO.RTO_CLV.RERWeb.Configuration
{
    public static class Constants
    {
        public static class EventReceiverNames
        {
            public const string ITEM_ADDED_RECEIVER_NAME = "ItemAddedEvent";
            public const string ITEM_UPDATED_RECEIVER_NAME = "ItemUpdatedEvent";
        }

        #region LISTS
        public static class CommonLists
        {
            public const string TASK_LIST = "ApprovalTask";
            public const string CONFIG_LIST = "RTOCLVConfiguration";
            public const string EVENT_LOG_LIST = "EventLog";
            public const string EMAIL_TEMPLATES_LIST = "EmailTemplates";
        }

        public static class RTOLists
        {
            public const string REQUEST_LIST = "CustomerVisitRequest";
        }

        public static class CLVLists
        {
            public const string REQUEST_LIST = "CustomerVisitRequest";
        }
        #endregion

        #region APPLICATION STATUS
        public static class Status
        {
            public const string SUBMITTED = "Employee's Review";

            public const string EMPLOYEE_ACKNOWLEDGED = "EMT-2 Review";
            public const string EMPLOYEE_DECLINED = "Employee Declined";

            public const string EMT2_APPROVED = "EMT-1 Review";
            public const string EMT2_REJECTED = "WFO Admin Review";
            public const string EMT2_NMI = "EMT-2 Needs More Info";

            public const string EMT1_APPROVED = "HR Review";
            public const string EMT1_REJECTED = "WFO Admin Review";
            public const string EMT1_NMI = "EMT-1 Needs More Info";

            public const string HR_SUBMITTED = "Employee's Review";
            public const string EMPLOYEE_ACKNOWLEDGED_HR = "Approved";
            public const string EMPLOYEE_DECLINED_HR = "Employee Declined";

            public const string NO_ACTION = "Rejected";
            public const string SENT_EXCEPTION = "Exception Committee Review";

            public const string EXCEPTION_APPROVED = "HR Review";
            public const string EXCEPTION_REJECTED = "Rejected";

            public const string HR_SUBMITTED_EXCEPTION = "Employee's Review";
            public const string EMPLOYEE_ACKNOWLEDGED_EXCEPTION = "Approved";
            public const string EMPLOYEE_DECLINED_EXCEPTION = "Employee Declined";
        }

        public static class InternalStatus
        {
            public const string SUBMITTED = "EmpReview";

            public const string EMPLOYEE_ACKNOWLEDGED = "EMT2Review";
            public const string EMPLOYEE_DECLINED = "EmpDeclined";

            public const string EMT2_APPROVED = "EMT1Review";
            public const string EMT2_REJECTED = "WFOAdminEMT2";
            public const string EMT2_NMI = "EMT2NMI";

            public const string EMT1_APPROVED = "HRReview";
            public const string EMT1_REJECTED = "WFOAdminEMT1";
            public const string EMT1_NMI = "EMT1NMI";

            public const string HR_SUBMITTED = "EmpReviewSecond";
            public const string EMPLOYEE_ACKNOWLEDGED_HR = "Approved";
            public const string EMPLOYEE_DECLINED_HR = "EmpDeclinedSecond";

            public const string NO_ACTION = "NoAction";
            public const string SENT_EXCEPTION = "ExceptionReview";

            public const string EXCEPTION_APPROVED = "HRReviewException";
            public const string EXCEPTION_REJECTED = "ExceptionRejected";

            public const string HR_SUBMITTED_EXCEPTION = "EmpReviewException";
            public const string EMPLOYEE_ACKNOWLEDGED_EXCEPTION = "ExceptionApproved";
            public const string EMPLOYEE_DECLINED_EXCEPTION = "EmpDeclinedException";
        }
        #endregion

        #region OTHER CONSTANTS
        public static class Applications
        {
            public const string RTO = "RTO";
            public const string CLV = "CLV";
            public const string RTO_LABEL = "Return to Office";
            public const string CLV_LABEL = "Customer Visit";
        }

        public static class FlowConstant
        {
            public static string REGULAR = "Regular";
            public static string EMT2 = "EMT2";
            public static string NA = "NA";
            public static string HR = "HR";
            public static string SECOND = "Second";
            public static string EXCEPTION = "Exception";
        }

        public static class FlowLevelIdentifier
        {
            public static int CONSTANT = 4;
        }

        public static class ApproverType
        {
            public const string PERSON = "Person";
            public const string GROUP = "Group";
            public const string EMAIL = "Email";
            public const string ALIAS = "Alias";
        }
        #endregion

        #region Regions
        public static class Regions
        {
            public const string GLOBAL = "Global";
            public const string NA = "North America";
            public const string INDIA = "India";
            public const string EUROPE = "Europe";
            public const string ASIA_PAC = "Asia Pacific";
            public const string JAPAN = "Japan";
        }
        #endregion

        #region ROLE
        public static class Role
        {
            public static string MANAGER = "Manager";
            public static string EMT = "EMT";
            public static string EMT2 = "EMT-2";
            public static string EMT1 = "EMT-1";
            public static string WFO_ADMIN = "WFO Admin";
            public static string EXCEPTION_COMMITTEE = "Exception Committee";
            public static string EMPLOYEE = "Employee";
            public static string NO_ROLE = "N_R";
            public static string HR = "HR";
            public static string DASHBOARD_VIEWERS = "Dashboard Viewers";
            public static string BACKUP_APPROVERS = "Backup Approvers";
            public static string BACKUP_SUBMITTERS = "Backup Submitters";
            public static string BACKUP_SUBMITTER = "Backup Submitter";
            public static string SERVICE_EMAIL = "Service Email";
        }
        #endregion

        #region APPROVER ACTIONS
        public static class ApproverAction
        {
            public static string SUBMITTED = "Submitted";
            public static string APPROVED = "Approved";
            public static string REJECTED = "Rejected";
            public static string ACKNOWLEDGED = "Acknowledged";
            public static string DECLINED = "Declined";
            public static string NMI = "Need more information";
            public static string NO_ACTION = "No action needed";
            public static string FORWARDED = "Forwarded";
            public static string PENDING = "Pending";
        }

        public static class ApprovalState
        {
            public static string COMPLETED = "Completed";
            public static string PENDING = "Pending";
        }
        #endregion

        #region LIST COLUMNS
        public static class TaskColumns
        {
            public static string TITLE = "Title";
            public static string TYPE = "InstanceType";
            public static string PRIMARY_ID = "PrimaryId";
            public static string ROLE = "Role";
            public static string ACTION = "ApprovalStatus";
            public static string DATE = "ApprovalDate";
            public static string COMMENTS = "ApproverComments";
            public static string REQUEST_LINK = "RequestLink";
            public static string STATE = "State";
            public static string APPROVER = "Approver";
        }

        public static class RequestColumns
        {
            public static string TITLE = "Title";
            public static string MANAGER_COUNT = "ManagerCount";
            public static string REQUEST_TYPE = "RequestType";

            public static string EMPLOYEE = "EmployeeName";
            public static string MANAGER = "Submitter";

            public static string EMT1 = "ExecutiveCommittee1";
            public static string EMT2 = "ExecutiveCommittee2";
            public static string EMT = "EMT";

            public static string STATUS = "Status";
            public static string INTERNAL_STATUS = "InternlStatus";

            public static string EMT1_COMMENTS = "EMT1Comments";
            public static string EMT2_COMMENTS = "EMT2Comments";
            public static string WFO_COMMENTS = "WFOComments";
            public static string HR_COMMENTS = "HRComments";
            public static string EXCEPTION_COMMITTEE_COMMENTS = "ExceptionComments";

            public static string REGION = "Region";
            public static string BACKUP_SUBMITTER = "BackUpSubmitter";
        }

        public static class ConfigColumns
        {
            public static string WFO = "WFOAdmin";
            public static string HR = "HR";
            public static string EXCEPTION_COMMITTEE = "ExceptionCommittee";

            public static string HR_GLOBAL = "HRGlobal";
            public static string HR_NA = "HRNA";
            public static string HR_INDIA = "HRIndia";
            public static string HR_EUROPE = "HREurope";
            public static string HR_ASIAPAC = "HRAsiapac";
            public static string HR_JAPAN = "HRJapan";

            public static string DASHBOARD_VIEWERS = "DashboardViewers";
            public static string BACKUP_APPROVERS = "BackUpApprovers";
            public static string BACKUP_SUBMITTERS = "BackUpSubmitters";

            public static string SERVICE_EMAIL_BOX = "EmailServiceAccount";
        }

        public static class EmailColumns
        {
            public static string TO = "To";
            public static string CC = "CC";
            public static string SUBJECT = "Subject";
            public static string BODY = "Body";
            public static string SET_IMPORTANCE = "SetImportance";
        }

        public static class LogColumns
        {
            public static string TITLE = "Title";
            public static string MESSAGE = "Message";
            public static string EVENT_NAME = "EventName";
            public static string ERROR_TYPE = "ErrorType";
            public static string REQUEST_ID = "RequestID";
        }
        #endregion

        #region Email Placeholders
        public static class EmailPlaceHolders
        {
            public static string REQUEST_TYPE = "{RequestType}";
            public static string EMPLOYEE_NAME = "{EmployeeName}";
            public static string REQUEST_LINK = "{RequestLink}";
            public static string MANAGER_NAME = "{ManagerName}";
            public static string EMT2_NAME = "{EMT2Name}";
            public static string EMT1_NAME = "{EMT1Name}";
            public static string EMT1_COMMENTS = "{EMT1NMIComments}";
            public static string EMT2_COMMENTS = "{EMT2NMIComments}";
        }
        #endregion

        #region Error 
        public static class ErrorTypes
        {
            public static string FATAL = "Fatal error";
            public static string EVALUATION = "Evaluation error";
            public static string SYSTEM = "System error";
            public static string WARNING = "Warning";
        }
        #endregion
    }
}