using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using WFO.RTO_CLV.RERWeb.Configuration;
using System.ServiceModel;
using System.Diagnostics;

namespace WFO.RTO_CLV.RERWeb.Services
{
    public class AppEventReceiver : IRemoteEventService
    {
        /// <summary>
        /// Handles app events that occur after the app is installed or upgraded, or when app is being uninstalled.
        /// </summary>
        /// <param name="properties">Holds information about the app event.</param>
        /// <returns>Holds information returned from the app event.</returns>
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            switch (properties.EventType)
            {
                case SPRemoteEventType.AppInstalled:
                    HandleAppInstalled(properties);
                    break;
                case SPRemoteEventType.AppUninstalling:
                    HandleAppUninstalling(properties);
                    break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// This method is a required placeholder, but is not used by app events.
        /// </summary>
        /// <param name="properties">Unused.</param>
        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            Trace.TraceInformation("Fired ProcessOneWayEvent");

            //if (properties.EventType == SPRemoteEventType.ItemAdded)
            //{
            //    HandleItemAdded(properties);
            //}

            if (properties.EventType == SPRemoteEventType.ItemUpdated)
            {
                HandleItemUpdated(properties);
            }
        }

        /// <summary>
        /// Handles when an app is installed.  Activates a feature in the
        /// host web.  The feature is not required.  
        /// Next, if the Jobs list is
        /// not present, creates it.  Finally it attaches a remote event
        /// receiver to the list.  
        /// </summary>
        /// <param name="properties"></param>        
        private void HandleAppInstalled(SPRemoteEventProperties properties)
        {
            using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, false))
            {
                if (clientContext != null)
                {
                    //OfficeDevPnP.Core.PnPClientContext _PnPClientContext = OfficeDevPnP.Core.PnPClientContext.ConvertFrom(clientContext);
                    new EventReceiverManager().AssociateRemoteEventsToHostWeb(clientContext, EventReceiverType.ItemUpdated, EventReceiverSynchronization.Asynchronous, Constants.EventReceiverNames.ITEM_UPDATED_RECEIVER_NAME);
                    //new EventReceiverManager().AssociateRemoteEventsToHostWeb(clientContext, EventReceiverType.ItemAdded, EventReceiverSynchronization.Asynchronous, Constants.EventReceiverNames.ITEM_ADDED_RECEIVER_NAME);
                }
            }
        }

        /// <summary>
        /// Removes the remote event receiver from the list and 
        /// adds a new item to the list.
        /// </summary>
        /// <param name="properties"></param>
        private void HandleAppUninstalling(SPRemoteEventProperties properties)
        {
            using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, false))
            {
                if (clientContext != null)
                {
                    //OfficeDevPnP.Core.PnPClientContext _PnPClientContext = OfficeDevPnP.Core.PnPClientContext.ConvertFrom(clientContext);
                    new EventReceiverManager().RemoveEventReceiversFromHostWeb(clientContext, Constants.EventReceiverNames.ITEM_UPDATED_RECEIVER_NAME);
                    //new EventReceiverManager().RemoveEventReceiversFromHostWeb(clientContext, Constants.EventReceiverNames.ITEM_ADDED_RECEIVER_NAME);
                }
            }
        }

        private void HandleItemUpdated(SPRemoteEventProperties properties)
        {
            SharePointContextToken contextToken = null;
            Uri sharepointUrl = new Uri(properties.ItemEventProperties.WebUrl);
            string appOnlyAccessToken = string.Empty;

            try
            {
                contextToken = TokenHelper.ReadAndValidateContextToken(properties.ContextToken, OperationContext.Current.IncomingMessageHeaders.To.Host);
                appOnlyAccessToken = TokenHelper.GetAppOnlyAccessToken(contextToken.TargetPrincipalName, sharepointUrl.Authority, contextToken.Realm).AccessToken;
            }
            catch (Exception ex)
            {
                //Log something
            }

            using (ClientContext clientContext = TokenHelper.GetClientContextWithAccessToken(sharepointUrl.ToString(), appOnlyAccessToken))
            {
                if (clientContext != null)
                {
                    //OfficeDevPnP.Core.PnPClientContext _PnPClientContext = OfficeDevPnP.Core.PnPClientContext.ConvertFrom(clientContext);
                    Trace.TraceInformation($"Start HandleItemUpdated Process for Item: {properties.ItemEventProperties.ListItemId}");

                    new EventReceiverManager().ItemUpdatedEventHandler(clientContext, properties.ItemEventProperties.ListId, properties.ItemEventProperties.ListTitle, properties.ItemEventProperties.ListItemId);
                }
            }
        }
    }
}
