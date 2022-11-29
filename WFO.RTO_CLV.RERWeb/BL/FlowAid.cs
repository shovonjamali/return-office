using System;
using System.Collections.Generic;
using WFO.RTO_CLV.RERWeb.Configuration;
using WFO.RTO_CLV.RERWeb.Entities;
using WFO.RTO_CLV.RERWeb.Interfaces;

namespace WFO.RTO_CLV.RERWeb.BL
{
    public class FlowAid
    {
        internal IApprovalProcess GetApplicationType(string application)
        {
            var appTypes = new Dictionary<string, IApprovalProcess>()
            {
                { Constants.Applications.RTO, new RTO() },
                { Constants.Applications.CLV, new CLV() }
            };

            return appTypes[application];
        }

        internal Validation ValidateUsers(ApprovalProcessItems approval_process)
        {
            var validation = new Validation();

            string message = string.Empty;
            bool error = false;

            if (Convert.ToInt16(approval_process.RequestItem[Constants.RequestColumns.MANAGER_COUNT]) > Constants.FlowLevelIdentifier.CONSTANT)
            {
                if (Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT1]) == "" || Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT1]) == null)
                {
                    message += Constants.Role.EMT1 + " not found";
                    error = true;
                }

                if (Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT2]) == "" || Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT2]) == null)
                {
                    message += Constants.Role.EMT2 + " not found";
                    error = true;
                }
            }

            if (Convert.ToInt16(approval_process.RequestItem[Constants.RequestColumns.MANAGER_COUNT]) == Constants.FlowLevelIdentifier.CONSTANT)
            {
                if (Convert.ToString(approval_process.RequestItem[Constants.RequestColumns.EMT1]) == "")
                {
                    message += Constants.Role.EMT1 + " not found";
                    error = true;
                }
            }

            if (approval_process.RequestItem[Constants.RequestColumns.EMPLOYEE] == null)
            {
                message += Constants.Role.EMPLOYEE + " not found";
                error = true;
            }

            if (approval_process.RequestItem[Constants.RequestColumns.MANAGER] == null)
            {
                message += Constants.Role.MANAGER + " not found";
                error = true;
            }

            if (approval_process.ConfigItem[Constants.ConfigColumns.WFO] == null)
            {
                message += Constants.Role.WFO_ADMIN + " not found";
                error = true;
            }

            if (approval_process.ConfigItem[Constants.ConfigColumns.EXCEPTION_COMMITTEE] == null)
            {
                message += Constants.Role.EXCEPTION_COMMITTEE + " not found";
                error = true;
            }

            if (approval_process.ConfigItem[Constants.ConfigColumns.HR] == null)
            {
                message += Constants.Role.HR + " not found";
                error = true;
            }

            validation.Error = error;
            validation.Message = message;

            return validation;
        }
    }
}