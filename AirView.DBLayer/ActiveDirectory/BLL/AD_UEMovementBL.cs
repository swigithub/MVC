using SWI.Libraries.AD.DAL;
using SWI.Libraries.AD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.AD.BLL
{
    /*----MoB!----*/

    public class AD_UEMovementBL
    {

       private AD_UEMovementDL uemd = new AD_UEMovementDL();

        public bool Manage(string filter, AD_UEMovement uem) {
            return uemd.Manage(filter,uem.UEMovementId,uem.UEId,uem.FromUserId,uem.UserId,uem.UEStatusId);
        }
    }
}
