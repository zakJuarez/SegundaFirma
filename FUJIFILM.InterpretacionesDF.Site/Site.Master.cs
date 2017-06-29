using FUJI.InterDF.Site.InterpretacionService;
using System;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FUJIFILM.InterpretacionesDF.Site
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // El código siguiente ayuda a proteger frente a ataques XSRF
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Utilizar el token Anti-XSRF de la cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generar un nuevo token Anti-XSRF y guardarlo en la cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer token Anti-XSRF
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validar el token Anti-XSRF
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Error de validación del token Anti-XSRF.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string usuariolbl = "";
                Session["UserID"] = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
                //if (Session["UserID"].ToString() == "")
                    //Session["UserID"] = "AREYES";
                usuariolbl = Session["UserID"].ToString();
                if (Session["Usuario"] != null)
                {
                    opeUsuario _mdl = (opeUsuario)Session["Usuario"];
                    if(_mdl!=null)
                    {
                        if (_mdl.ouAdministrador == "S")
                            usuariolbl = usuariolbl + " - Modo: Administrador";
                    }
                }
                lblUser.Text = usuariolbl;
                escribirBitacora(Session["UserID"].ToString());
            }
            catch(Exception e2)
            {
                throw e2;
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            //Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        private void escribirBitacora(string msg)
        {
            const string saltoLinea = "\n";
            if (!Directory.Exists("C:\\temp\\"))
                Directory.CreateDirectory("C:\\temp\\");
            File.AppendAllText("C:\\temp\\BitacoraSegundaFirma.txt", DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToShortTimeString() + DateTime.Now.Second.ToString() + " " + msg + saltoLinea);
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Clear();
                Response.Redirect("/");
            }
            catch(Exception ecs)
            {
                throw ecs;
            }
        }
    }

}