using AirView.DBLayer.AirView.DAL;
using AirView.DBLayer.AirView.Entities;
using SWI.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirView.DBLayer.AirView.BLL
{
    public class AL_AlertBL
    {
        private AL_AlertDL AL = new AL_AlertDL();

        public AL_GetAlertSubscription IsSubscribed(string Filter, int UserId, int ParentEntityId, int ChildEntityId, string Alert)
        {
            DataTable dataTableModel = AL.Get(Filter, UserId, ParentEntityId, ChildEntityId, Alert);

            List<AL_GetAlertSubscription> ListModel = dataTableModel.ToList<AL_GetAlertSubscription>();
            if (ListModel.Count != 0)
            {
                return ListModel[0];
            }
            else
            {
                AL_GetAlertSubscription model = new AL_GetAlertSubscription();
                model.IsSubscribed = false;
                return model;
            }

        }


        public List<AL_GetAlertConfiguration> Configuration(string Filter, AL_GetAlertConfiguration Configuration)
        {
            DataTable dataTableModel = AL.Get(Filter, Configuration);

            List<AL_GetAlertConfiguration> ListModel = dataTableModel.ToList<AL_GetAlertConfiguration>();
            return ListModel;

        }

        public List<AL_GetNotification> GetNotification(string Filter, AL_GetNotification Info)
        {
            DataTable dataTableModel = AL.Get(Filter, Info);

            List<AL_GetNotification> ListModel = dataTableModel.ToList<AL_GetNotification>();
            return ListModel;

        }

        public List<AL_GetNotification> SendNotification(string Filter, AL_SetNotification Info)
        {
            DataTable dataTableModel = AL.Set(Filter, Info);
            List<AL_GetNotification> ListModel = dataTableModel.ToList<AL_GetNotification>();
            return ListModel;
        }

        public int InsertAlert(string Filter, int EntityId, int AlertSender, int ParentEntityId, int ChildEntityId, string Alert, int StatusId)
        {
            return AL.Insert(Filter, EntityId, AlertSender, ParentEntityId, ChildEntityId, Alert, StatusId);
        }

        public int Subscribe(string Filter, string KeyCode, int EntityId, string ConfigId, int UserId)
        {
            return AL.Subscribe(Filter, KeyCode, EntityId, ConfigId, UserId);
        }

        public int Subscription(string Filter, string KeyCode, List<AL_AlertSubscriptionUser> SubscriptionList)
        {

            foreach(AL_AlertSubscriptionUser model in SubscriptionList)
            {
                AL.Subscription(Filter, KeyCode, model);
            }

            return 1;
        }

        public int UpdateConfiguration(string Filter, List<AL_GetAlertConfiguration> ConfigurationList)
        {
          
            foreach (AL_GetAlertConfiguration model in ConfigurationList)
            {
                AL.UpdateConfiguration(Filter, model);
            }

            return 1;
        }

        public int UpdateNotification(string Filter, List<AL_GetNotification> ConfigurationList)
        {

            foreach (AL_GetNotification model in ConfigurationList)
            {
                AL.UpdateNotification(Filter, model);
            }

            return 1;
        }
    }
}
