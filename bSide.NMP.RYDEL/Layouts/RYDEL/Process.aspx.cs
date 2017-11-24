using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace bSide.NMP.RYDEL.Layouts.RYDEL
{
    public partial class Process : UnsecuredLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Habilita acceso anónimo
        /// </summary>
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
    }
}
