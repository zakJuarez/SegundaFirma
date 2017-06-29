using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FUJIFILM.InterpretacionesDF.Site.UserControl
{
    public partial class Mensaje : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
            }
        }

        /// <summary>
        /// Metodo que selecciona el tipo de mensaje a mostrar al usuario, ademas
        /// de que se inicializa el mensaje a mostrar
        /// </summary>
        /// <param name="mensaje">Establece el mensaje</param>
        /// <param name="sintTipo">1-Correcto,2-Error,3-Alerta,Default-Alerta</param>
        public void setMensaje(string mensaje, Int16 sintTipo /*1-Correcto,2-Error,3-Alerta*/)
        {
            try
            {
                this.lblMensaje.Text = mensaje;
                switch (sintTipo)
                {
                    case 1:
                        imgMensaje.ImageUrl = "~/Images/ic_action_foursquare.png";
                        break;
                    case 2:
                        imgMensaje.ImageUrl = "~/Images/ic_action_cancel.png";
                        break;
                    case 3:
                        imgMensaje.ImageUrl = "~/Images/ic_action_bulb.png";
                        break;
                    default:
                        imgMensaje.ImageUrl = "~/Images/ic_action_bulb.png";
                        break;
                }
                mpeConfirmacion.Show();
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
            }
        }
    }
}