
using FUJI.InterDF.Site.InterpretacionService;
using FUJIFILM.InterpretacionesDF.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FUJIFILM.InterpretacionesDF.Site
{
    public partial class frmInterpretacion : System.Web.UI.Page
    {
        InterpretacionServiceClient InterpretacionDA = new InterpretacionServiceClient();
        public static List<vBuscaEnRevision> _lstCompleta = new List<vBuscaEnRevision>();
        const string saltoLinea = "\n";
        public static opeUsuario _user = new opeUsuario();
        public string URLTarget
        {
            get
            {
                return ConfigurationManager.AppSettings["URLTarget"].ToString();
            }

        }
        public enum MessageType { Success, Error, Info, Warning };

        public static string logUser = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["UserID"] = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf(@"\") + 1);
                    //if (Session["UserID"].ToString() == "")
                    //    Session["UserID"] = "AREYES";
                    lblUsuario.Text = Session["UserID"] == null ? logUser : Session["UserID"].ToString();
                    verificarUsuario();
                    cargaInterpretaciones();
                    cargaInterpretacionesRev();
                    cargaNumPendientes();
                }
                if (this.IsPostBack)
                {
                    TabName.Value = Request.Form[TabName.UniqueID];
                }
            }
            catch (Exception ePL)
            {
                escribirBitacora("Error al cargar la pagina: " + ePL.Message);
                ShowMessage("Existe un error al uniciar la página: " + ePL.Message, MessageType.Error, "alertPrio_container");
                //MensajeSis.setMensaje("Existe un error al uniciar la página: " + ePL.Message, 2);
            }
        }

        private void verificarUsuario()
        {
            try
            {
                if (lblUsuario.Text.ToString() != "")
                {
                    _user = InterpretacionDA.getUsuario(lblUsuario.Text.ToString());
                    Session["Usuario"] = _user;
                    if (_user != null && _user.ouAdministrador.ToUpper() == "S")
                    {
                        ddlBusUsuarios.Visible = true;
                        ddlBusUsuario2.Visible = true;
                        List<vBuscaEnRevision> _lstInter = new List<vBuscaEnRevision>();
                        List<vBuscaEnRevision> _lstInter2 = new List<vBuscaEnRevision>();
                        vBuscaEnRevision _mdl = obtenerBusqueda();
                        vBuscaEnRevision _mdl2 = obtenerBusqueda2();
                        _lstCompleta = InterpretacionDA.getInterpretacionList();
                        _lstInter = _lstCompleta.Where(x => x.oeInterpretacionAprobada == null && x.bkStatus == false && x.AccessionNumber.ToUpper().Contains(_mdl.AccessionNumber.ToUpper()) && x.InternalPatientID.ToUpper().Contains(_mdl.InternalPatientID.ToUpper()) && x.FullName.ToUpper().Contains(_mdl.FullName.ToUpper())).OrderBy(o => o.fechayhora).ToList();
                        _lstInter2 = _lstCompleta.Where(x => x.oeInterpretacionAprobada == false && x.AccessionNumber.ToUpper().Contains(_mdl2.AccessionNumber.ToUpper()) && x.InternalPatientID.ToUpper().Contains(_mdl2.InternalPatientID.ToUpper()) && x.FullName.ToUpper().Contains(_mdl2.FullName.ToUpper())).OrderBy(o => o.fechayhora).ToList();
                        cargarUsuarioscmb(_lstInter, _lstInter2);
                    }
                    else
                    {
                        ddlBusUsuarios.Visible = false;
                        ddlBusUsuario2.Visible = false;
                    }
                }
            }
            catch(Exception evU)
            {
                escribirBitacora("No fue posible verificar el usuario: " + evU.Message);
            }
        }

        protected void btnNusqueda_Click(object sender, EventArgs e)
        {
            try
            {
                cargaInterpretaciones();
                //cargaInterpretacionesRev();
                cargaNumPendientes();
            }
            catch (Exception eBusqueda)
            {
                //MensajeSis.setMensaje("Existe  un error al realizar la búsqueda: " + eBusqueda.Message, 2);
                ShowMessage("Existe un error: " + eBusqueda.Message, MessageType.Error, "alertPrio_container");
            }
        }

        protected void btnBus2_Click(object sender, EventArgs e)
        {
            try
            {
                //cargaInterpretaciones();
                cargaInterpretacionesRev();
                cargaNumPendientes();
            }
            catch (Exception eBusqueda)
            {
                //MensajeSis.setMensaje("Existe  un error al realizar la búsqueda: " + eBusqueda.Message, 2);
                ShowMessage("Existe un error: " + eBusqueda.Message, MessageType.Error, "alertPrio_container");
            }
        }

        protected void grvBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Pager)
                {
                    Label lblTotalNumDePaginas = (Label)e.Row.FindControl("lblBandejaTotal");
                    lblTotalNumDePaginas.Text = grvBusqueda.PageCount.ToString();

                    TextBox txtIrAlaPagina = (TextBox)e.Row.FindControl("txtBandeja");
                    txtIrAlaPagina.Text = (grvBusqueda.PageIndex + 1).ToString();

                    DropDownList ddlTamPagina = (DropDownList)e.Row.FindControl("ddlBandeja");
                    ddlTamPagina.SelectedValue = grvBusqueda.PageSize.ToString();
                }

                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }
            }
            catch (Exception egrdb)
            {
                throw new Exception(egrdb.Message);
            }
        }

        protected void grvBusqueda2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Pager)
                {
                    Label lblTotalNumDePaginas = (Label)e.Row.FindControl("lblBandejaTotal2");
                    lblTotalNumDePaginas.Text = grvBusqueda2.PageCount.ToString();

                    TextBox txtIrAlaPagina = (TextBox)e.Row.FindControl("txtBandeja2");
                    txtIrAlaPagina.Text = (grvBusqueda2.PageIndex + 1).ToString();

                    DropDownList ddlTamPagina = (DropDownList)e.Row.FindControl("ddlBandeja2");
                    ddlTamPagina.SelectedValue = grvBusqueda2.PageSize.ToString();
                }

                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }
            }
            catch (Exception egrdb)
            {
                throw new Exception(egrdb.Message);
            }
        }

        protected void grvBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (e.NewPageIndex >= 0)
                {
                    this.grvBusqueda.PageIndex = e.NewPageIndex;
                    cargaInterpretaciones();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Existe un error: " + ex.Message, MessageType.Error, "alertPrio_container");
                //MensajeSis.setMensaje(aex.Messge, 3);
            }
        }

        protected void grvBusqueda2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (e.NewPageIndex >= 0)
                {
                    this.grvBusqueda2.PageIndex = e.NewPageIndex;
                    cargaInterpretacionesRev();
                }
            }
            catch (Exception ex)
            {
                //MensajeSis.setMensaje(ex.Message, 3);
                ShowMessage("Existe un error: " + ex.Message, MessageType.Error, "alertPrio_container");
            }
        }

        protected void grvBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "viewDoc":
                        //string lofUser = System.Environment.MachineName;
                        string logUser1 = Session["UserID"] == null ? logUser : Session["UserID"].ToString();
                        bool _prodece = InterpretacionDA.getValidacionProcedeBloqueo(e.CommandArgument.ToString(), logUser1);
                        if (_prodece)
                        {
                            vBuscaEnRevision _mdl = _lstCompleta.Where(x => x.AccessionNumber == e.CommandArgument.ToString()).First();
                            string _url = URLTarget + _mdl.AccessionNumber + "&data2=" + logUser1;
                            generaArchivo(_mdl, 1, _url);
                            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Cerrar", "javascript:Redirecciona('" + URLTarget + _mdl.AccessionNumber + "&data2=" + logUser + "');", true);
                            cargaInterpretaciones();
                            cargaInterpretacionesRev();
                        }
                        else
                        {
                            //MensajeSis.setMensaje("El estudio se encuentra bloqueado por otro usuario.", 3);
                            ShowMessage("El estudio se encuentra bloqueado por otro usuario", MessageType.Error, "alertPrio_container");
                        }
                        break;

                }
            }
            catch (Exception egrRc)
            {
                throw new Exception(egrRc.Message);
            }
        }

        protected void grvBusqueda2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "viewDoc":
                        string logUser1 = lblUsuario.Text.ToString();
                        cargaInterpretacionesRev();
                        List<vBuscaEnRevision> _lstIn = _lstCompleta.Where(x => x.AccessionNumber == e.CommandArgument.ToString()).ToList();
                        if (_lstIn.Count > 0)
                        {
                            vBuscaEnRevision _mdl = _lstCompleta.Where(x => x.AccessionNumber == e.CommandArgument.ToString()).First();
                            if (_mdl != null)
                            {
                                string url = URLTarget + _mdl.AccessionNumber + "&data2=" + logUser1;
                                generaArchivo(_mdl, 2, url);
                            }
                            else
                            {
                                //MensajeSis.setMensaje("El estudio ya fue finalizado.", 3);
                                ShowMessage("El estudio ya fue finalizado.", MessageType.Info, "alertPrio_container");
                            }
                        }
                        else
                        {
                            ShowMessage("El estudio ya fue finalizado.", MessageType.Info, "alertPrio_container");
                            //MensajeSis.setMensaje("El estudio ya fue finalizado.", 3);
                        }
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Cerrar", "javascript:Redirecciona('" + URLTarget + _mdl.AccessionNumber + "&data2=" + logUser + "');", true);
                        cargaInterpretaciones();
                        cargaInterpretacionesRev();
                        break;
                }
            }
            catch (Exception egrRc)
            {
                throw new Exception(egrRc.Message);
            }
        }

        protected void ddlBandeja_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList dropDownList = (DropDownList)sender;
                if (int.Parse(dropDownList.SelectedValue) != 0)
                {
                    this.grvBusqueda.AllowPaging = true;
                    this.grvBusqueda.PageSize = int.Parse(dropDownList.SelectedValue);
                }
                else
                    this.grvBusqueda.AllowPaging = false;
                this.cargaInterpretaciones();
            }
            catch (Exception eddS)
            {
                ShowMessage("Existe un error al consultar la bandeja de resultados. " + eddS.Message, MessageType.Error, "alertPrio_container");
                //MensajeSis.setMensaje("Existe un error al consultar la bandeja de resultados. " + eddS.Message, 2);
            }
        }

        protected void ddlBandeja2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList dropDownList = (DropDownList)sender;
                if (int.Parse(dropDownList.SelectedValue) != 0)
                {
                    this.grvBusqueda2.AllowPaging = true;
                    this.grvBusqueda2.PageSize = int.Parse(dropDownList.SelectedValue);
                }
                else
                    this.grvBusqueda2.AllowPaging = false;
                this.cargaInterpretacionesRev();
            }
            catch (Exception eddS)
            {
                ShowMessage("Existe un error al consultar la bandeja de resultados. " + eddS.Message, MessageType.Error, "alertPrio_container");
                //MensajeSis.setMensaje("Existe un error al consultar la bandeja de resultados. " + eddS.Message, 2);
            }
        }

        protected void txtBandeja_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBandejaAvaluosGoToPage = (TextBox)sender;
                int numeroPagina;
                if (int.TryParse(txtBandejaAvaluosGoToPage.Text.Trim(), out numeroPagina))
                    this.grvBusqueda.PageIndex = numeroPagina - 1;
                else
                    this.grvBusqueda.PageIndex = 0;
                this.cargaInterpretaciones();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error, "alertPrio_container");
//                MensajeSis.setMensaje(ex.Message, 3);
            }
        }

        protected void txtBandeja2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBandejaAvaluosGoToPage = (TextBox)sender;
                int numeroPagina;
                if (int.TryParse(txtBandejaAvaluosGoToPage.Text.Trim(), out numeroPagina))
                    this.grvBusqueda2.PageIndex = numeroPagina - 1;
                else
                    this.grvBusqueda2.PageIndex = 0;
                this.cargaInterpretacionesRev();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error, "alertPrio_container");
                //MensajeSis.setMensaje(ex.Message, 3);
            }
        }

        #region metodos
        public void cargaInterpretaciones()
        {
            try
            {
                List<vBuscaEnRevision> _lstInter = new List<vBuscaEnRevision>();
                List<vBuscaEnRevision> _lstInterAux = new List<vBuscaEnRevision>();
                vBuscaEnRevision _mdl = obtenerBusqueda();
                _lstCompleta = InterpretacionDA.getInterpretacionList();
                grvBusqueda.DataSource = null;
                grvBusqueda.DataBind();
                if (_user != null && _user.ouAdministrador == "S")
                {
                    if (_lstCompleta != null)
                        _lstInter = _lstCompleta.Where(x => x.oeInterpretacionAprobada == null && x.Modality.ToUpper().Contains(txtBusModalidad.Text.ToUpper()) && x.bkStatus == false && x.AccessionNumber.ToUpper().Contains(_mdl.AccessionNumber.ToUpper()) && x.InternalPatientID.ToUpper().Contains(_mdl.InternalPatientID.ToUpper()) && x.FullName.ToUpper().Contains(_mdl.FullName.ToUpper())).OrderBy(o => o.fechayhora).ToList();
                    if (ddlBusUsuarios.SelectedValue != "0" && ddlBusUsuarios.SelectedValue != "")
                        _lstInter = _lstInter.Where(x => x.usuarioDicto.ToUpper() == ddlBusUsuarios.SelectedItem.ToString().ToUpper()).ToList();
                    if (_user.oeModalidad != null && _user.oeModalidad != null)
                    {
                        string[] array = _user.oeModalidad.ToUpper().Split(',');
                        List<string> list = new List<string>();
                        if (array.Count() > 0)
                        {
                            list.AddRange(array);
                            foreach (vBuscaEnRevision _mdlFilter in _lstInter)
                            {
                                foreach (string _str in list)
                                {
                                    if (_mdlFilter.Modality.ToUpper().Trim() == _str.ToUpper().Trim())
                                    {
                                        _lstInterAux.Add(_mdlFilter);
                                    }
                                }
                            }
                            _lstInter = _lstInterAux;
                        }
                    }
                    ddlBusUsuarios.Visible = true;
                    lblUsuarios.Visible = true;
                    ddlBusUsuario2.Visible = true;
                    lblUsuarios2.Visible = true;
                }
                else
                {
                    if (_user != null)
                    {
                        string logUser1 = lblUsuario.Text.ToString();
                        if (_lstCompleta != null)
                            _lstInter = _lstCompleta.Where(x => x.usuarioDicto.ToUpper() != logUser1.ToUpper() && x.Modality.ToUpper().Contains(txtBusModalidad.Text.ToUpper()) && x.oeInterpretacionAprobada == null && x.bkStatus == false && x.AccessionNumber.ToUpper().Contains(_mdl.AccessionNumber.ToUpper()) && x.InternalPatientID.ToUpper().Contains(_mdl.InternalPatientID.ToUpper()) && x.FullName.ToUpper().Contains(_mdl.FullName.ToUpper())).OrderBy(o => o.fechayhora).ToList();
                        if (_user.oeModalidad != null && _user.oeModalidad != null)
                        {
                            string[] array = _user.oeModalidad.ToUpper().Split(',');
                            List<string> list = new List<string>();
                            if (array.Count() > 0)
                            {
                                list.AddRange(array);
                                foreach (vBuscaEnRevision _mdlFilter in _lstInter)
                                {
                                    foreach (string _str in list)
                                    {
                                        if (_mdlFilter.Modality.ToUpper().Trim() == _str.ToUpper().Trim())
                                        {
                                            _lstInterAux.Add(_mdlFilter);
                                        }
                                    }
                                }
                                _lstInter = _lstInterAux;
                            }
                        }
                    }
                    ddlBusUsuarios.Visible = false;
                    lblUsuarios.Visible = false;
                    ddlBusUsuario2.Visible = false;
                    lblUsuarios2.Visible = false;

                }
                grvBusqueda.DataSource = _lstInter;
                grvBusqueda.DataBind();
            }
            catch (Exception ecI)
            {
                escribirBitacora("CargaInterpretaciones: " + ecI.Message);
            }
        }

        private void cargarUsuarioscmb(List<vBuscaEnRevision> _lstInter, List<vBuscaEnRevision> _lstInter2)
        {
            try
            {
                List<clsModeloCatalogo> _lstComboUsu = new List<clsModeloCatalogo>();
                int i = 0;
                foreach(vBuscaEnRevision _mdl in _lstInter)
                {
                    if (_lstComboUsu != null && _lstComboUsu.Count > 0)
                    {
                        if(!_lstComboUsu.Exists(x => x.vchDescripcion == _mdl.usuarioDicto.ToUpper()))
                        {
                            i++;
                            clsModeloCatalogo _cat = new clsModeloCatalogo();
                            _cat.vchValue = i.ToString();
                            _cat.vchDescripcion = _mdl.usuarioDicto.ToUpper();
                            _lstComboUsu.Add(_cat);
                        }
                    }
                    else
                    {
                        i++;
                        clsModeloCatalogo _cat = new clsModeloCatalogo();
                        _cat.vchValue = i.ToString();
                        _cat.vchDescripcion = _mdl.usuarioDicto.ToUpper();
                        _lstComboUsu.Add(_cat);
                    }
                }
                ddlBusUsuarios.DataSource = _lstComboUsu;
                ddlBusUsuarios.DataTextField = "vchDescripcion";
                ddlBusUsuarios.DataValueField = "vchValue";
                ddlBusUsuarios.DataBind();
                ddlBusUsuarios.Items.Insert(0, new ListItem("Todos...", "0"));
                List<clsModeloCatalogo> _lstComboUsu2 = new List<clsModeloCatalogo>();
                i = 0;
                foreach (vBuscaEnRevision _mdl in _lstInter2)
                {
                    if (_lstComboUsu2 != null && _lstComboUsu2.Count > 0)
                    {
                        if (!_lstComboUsu2.Exists(x => x.vchDescripcion == _mdl.usuarioDicto.ToUpper()))
                        {
                            i++;
                            clsModeloCatalogo _cat = new clsModeloCatalogo();
                            _cat.vchValue = i.ToString();
                            _cat.vchDescripcion = _mdl.usuarioDicto.ToUpper();
                            _lstComboUsu2.Add(_cat);
                        }
                    }
                    else
                    {
                        i++;
                        clsModeloCatalogo _cat = new clsModeloCatalogo();
                        _cat.vchValue = i.ToString();
                        _cat.vchDescripcion = _mdl.usuarioDicto.ToUpper();
                        _lstComboUsu2.Add(_cat);
                    }
                }
                ddlBusUsuario2.DataSource = _lstComboUsu2;
                ddlBusUsuario2.DataTextField = "vchDescripcion";
                ddlBusUsuario2.DataValueField = "vchValue";
                ddlBusUsuario2.DataBind();
                ddlBusUsuario2.Items.Insert(0, new ListItem("Todos...", "0"));
            }
            catch(Exception ecUC)
            {
                throw ecUC;
            }
        }

        private vBuscaEnRevision obtenerBusqueda()
        {
            vBuscaEnRevision _mdlBus = new vBuscaEnRevision();
            _mdlBus.AccessionNumber = txtBusNumEst.Text;
            _mdlBus.InternalPatientID = txtBusqPaciente.Text;
            _mdlBus.FullName = txtBusqNomPac.Text;
            return _mdlBus;
        }

        private vBuscaEnRevision obtenerBusqueda2()
        {
            vBuscaEnRevision _mdlBus = new vBuscaEnRevision();
            _mdlBus.AccessionNumber = txtBusNumEst2.Text;
            _mdlBus.InternalPatientID = txtBusNomPac2.Text;
            _mdlBus.FullName = txtBusIDPac2.Text;
            return _mdlBus;
        }

        public void cargaInterpretacionesRev()
        {
            try
            {
                vBuscaEnRevision _mdl = obtenerBusqueda2();
                List<vBuscaEnRevision> _lstInter = new List<vBuscaEnRevision>();
                List<vBuscaEnRevision> _lstInterAux = new List<vBuscaEnRevision>();
                _lstCompleta = InterpretacionDA.getInterpretacionList();
                grvBusqueda2.DataSource = null;
                grvBusqueda2.DataBind();
                if (_user != null && _user.ouAdministrador == "S")
                {
                    if (_lstCompleta != null)
                        _lstInter = _lstCompleta.Where(x => x.oeInterpretacionAprobada == false && x.Modality.ToUpper().Contains(txtBusModalidad2.Text.ToUpper()) && x.AccessionNumber.ToUpper().Contains(_mdl.AccessionNumber.ToUpper()) && x.InternalPatientID.ToUpper().Contains(_mdl.InternalPatientID.ToUpper()) && x.FullName.ToUpper().Contains(_mdl.FullName.ToUpper())).OrderBy(o => o.fechayhora).ToList();
                    if (ddlBusUsuario2.SelectedValue != "0" && ddlBusUsuario2.SelectedValue != "")
                        _lstInter = _lstInter.Where(x => x.usuarioDicto.ToUpper() == ddlBusUsuario2.SelectedItem.ToString().ToUpper()).ToList();
                    if (_user.oeModalidad != null && _user.oeModalidad != null)
                    {
                        string[] array = _user.oeModalidad.ToUpper().Split(',');
                        List<string> list = new List<string>();
                        if (array.Count() > 0)
                        {
                            list.AddRange(array);
                            foreach (vBuscaEnRevision _mdlFilter in _lstInter)
                            {
                                foreach (string _str in list)
                                {
                                    if (_mdlFilter.Modality.ToUpper().Trim() == _str.ToUpper().Trim())
                                    {
                                        _lstInterAux.Add(_mdlFilter);
                                    }
                                }
                            }
                            _lstInter = _lstInterAux;
                        }
                    }
                }
                else
                {
                    if (_user != null)
                    {
                        string logUser1 = lblUsuario.Text.ToString();
                        if (_lstCompleta != null)
                            _lstInter = _lstCompleta.Where(x => x.usuarioDicto.ToUpper() == logUser1.ToUpper() && x.Modality.ToUpper().Contains(txtBusModalidad2.Text.ToUpper()) && x.oeInterpretacionAprobada == false && x.AccessionNumber.ToUpper().Contains(_mdl.AccessionNumber.ToUpper()) && x.InternalPatientID.ToUpper().Contains(_mdl.InternalPatientID.ToUpper()) && x.FullName.ToUpper().Contains(_mdl.FullName.ToUpper())).OrderBy(o => o.fechayhora).ToList();
                        if (_user.oeModalidad != null && _user.oeModalidad != null)
                        {
                            string[] array = _user.oeModalidad.ToUpper().Split(',');
                            List<string> list = new List<string>();
                            if (array.Count() > 0)
                            {
                                list.AddRange(array);
                                foreach (vBuscaEnRevision _mdlFilter in _lstInter)
                                {
                                    foreach (string _str in list)
                                    {
                                        if (_mdlFilter.Modality.ToUpper().Trim() == _str.ToUpper().Trim())
                                        {
                                            _lstInterAux.Add(_mdlFilter);
                                        }
                                    }
                                }
                                _lstInter = _lstInterAux;
                            }
                        }
                    }
                }
                grvBusqueda2.DataSource = _lstInter;
                grvBusqueda2.DataBind();
            }
            catch (Exception ecI)
            {
                escribirBitacora("CargaInterpretacionesRev: " + ecI.Message);
            }
        }

        private void generaArchivo(vBuscaEnRevision _mdl, int Revision, string _url)
        {
            try
            {
                String sbA = crearArchivo(_mdl, Revision);
                // Write the string to a file.
                //StreamWriter file = new StreamWriter("c:\\test.txt");
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Cerrar", "javascript:escribeArchivo('" + sbA + "', '" + _url + "');", true);
                //file.WriteLine(sbA);
                //file.Close();
                ShowMessage("El proceso terminó correctamente", MessageType.Success, "alertPrio_container"); //MensajeSis.setMensaje("El proceso terminó correctamente.", 1);
            }
            catch (Exception eGA)
            {
                ShowMessage("Existe un error al generar el proceso: " + eGA.Message, MessageType.Error, "alertPrio_container");
                //MensajeSis.setMensaje("Existe un error al generar el proceso: " + eGA, 2);
            }
        }

        private String crearArchivo(vBuscaEnRevision _mdl, int _Revision)
        {
            String sb = "";
            try
            {
                string logUser1 = Session["UserID"] == null ? logUser : Session["UserID"].ToString();
                sb += "   <DictationData>";
                sb += "       <Studies>";
                sb += "           <StudyInfo>";
                sb += "                   <modouso>RIS</modouso>";
                sb += "                   <urlris></urlris>";
                sb += "                   <AccessionNumber>" + _mdl.AccessionNumber + "</AccessionNumber>";
                sb += "                   <InternalPatientID>" + _mdl.InternalPatientID + "</InternalPatientID>";
                sb += "                   <ExternalPatientID>" + _mdl.ExternalPatientID + "</ExternalPatientID>";
                sb += "                   <FullName>" + _mdl.FullName + "</FullName>";
                sb += "                   <FirstName>" + _mdl.FirstName + "</FirstName>";
                sb += "                   <LastName>" + _mdl.LastName + "</LastName>";
                sb += "                   <DOB>" + _mdl.DOB + "</DOB>";
                sb += "                   <Gender>" + _mdl.Gender + "</Gender>";
                sb += "                   <Pregnancy></Pregnancy>";
                sb += "                   <LastMenstruationDate></LastMenstruationDate>";
                sb += "                   <StudyId></StudyId>";
                sb += "                   <RISOderId></RISOderId>";
                sb += "                   <ProcedureCode>"+ _mdl.ProcedureCode + "</ProcedureCode>";
                sb += "                   <ProcedureDescription>" + _mdl.ProcedureDescription + "</ProcedureDescription>";
                sb += "                   <Modality>" + _mdl.Modality + "</Modality>";
                sb += "                   <ReasonForExam>" + _mdl.ReasonForExam + "</ReasonForExam>";
                sb += "                   <StudyStatusCode>" + _mdl.StudyStatusCode + "</StudyStatusCode>";
                sb += "                   <StudyImageCount>" + _mdl.StudyImageCount + "</StudyImageCount>";
                sb += "                   <StudyTimeStamp>" + _mdl.StudyTimeStamp + "</StudyTimeStamp>";
                sb += "                   <AdmissionTimeDate>" + _mdl.AdmissionTimeDate + "</AdmissionTimeDate>";
                sb += "                   <VisitClass>" + _mdl.VisitClass + "</VisitClass>";
                sb += "                   <VisitNumber>" + _mdl.VisitNumber + "</VisitNumber>";
                sb += "                   <Priority>" + _mdl.Priority + "</Priority>";
                sb += "                   <SiteCode>" + _mdl.SiteCode + "</SiteCode>";
                sb += "                   <PrimaryLocId>" + _mdl.PrimaryLocId + "</PrimaryLocId>";
                sb += "                   <PrimaryLocation>" + _mdl.PrimaryLocation + "</PrimaryLocation>";
                sb += "                   <CurrentLocId>" + _mdl.CurrentLocId + "</CurrentLocId>";
                sb += "                   <CurrentLocation>" + _mdl.CurrentLocation + "</CurrentLocation>";
                sb += "                   <StudySiteName>" + _mdl.StudySiteName + "</StudySiteName>";
                sb += "                   <RequestingPhysicianId>" + _mdl.RequestingPhysicianId + "</RequestingPhysicianId>";
                sb += "                   <RequestingPhysician>" + _mdl.RequestingPhysician + "</RequestingPhysician>";
                sb += "                   <ReferringPhysicianId>" + _mdl.ReferringPhysicianId + "</ReferringPhysicianId>";
                sb += "                   <ReferringPhysician>" + _mdl.ReferringPhysician + "</ReferringPhysician>";
                sb += "                   <AttendingPhysicianId>" + _mdl.AttendingPhysicianId + "</AttendingPhysicianId>";
                sb += "                   <AttendingPhysician>" + _mdl.AttendingPhysician + "</AttendingPhysician>";
                sb += "               </StudyInfo>";
                sb += "           </Studies>";
                sb += "       <UserInfo>";
                sb += "           <SynapseUserName>" + _mdl.usuarioDictoNombre + " " + _mdl.usuarioDictoApellidoPaterno + " " + _mdl.usuarioDictoApellidoMaterno + "</SynapseUserName>";
                sb += "           <SynapseUserId>" + logUser1 + "</SynapseUserId>";
                sb += "           <UserId>" + logUser1 + "</UserId>";
                if (_Revision == 1)
                    sb += "		<esRevision>SI</esRevision>";
                if (_Revision == 2)
                    sb += "		<SegundaRevision>SI</SegundaRevision> ";
                sb += "       </UserInfo>";
                sb += "   </DictationData>";
            }
            catch (Exception ecA)
            {
                throw ecA;
            }
            return sb;
        }
        #endregion metodos       

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                verificarUsuario();
                cargaInterpretaciones();
                cargaNumPendientes();
                cargaInterpretacionesRev();
            }
            catch (Exception eTM)
            {
                throw eTM;
            }
        }

        private void cargaNumPendientes()
        {
            try
            {
                List<vBuscaEnRevision> _lsTimer = InterpretacionDA.getInterpretacionList();
                string logUser1 = lblUsuario.Text.ToString();
                int pendientes = 0;
                //escribirBitacora("** cargaNumPendientes -- Consulta: " + logUser1);
                if (_lsTimer != null)
                    if(_user!= null)
                        if(_user.ouAdministrador == "S")
                        {
                            pendientes = _lsTimer.Where(x => x.oeInterpretacionAprobada == false).ToList().Count();
                        }
                        else
                        {
                            pendientes = _lsTimer.Where(x => x.usuarioDicto.ToUpper() == logUser1.ToUpper() && x.oeInterpretacionAprobada == false).ToList().Count();
                        }
                    
                lblNumPendientes.Text = pendientes.ToString();
                //escribirBitacora("-- lblNumPendientes: " + pendientes);
            }
            catch (Exception ecP)
            {
                escribirBitacora("CargaInterpretaciones: " + ecP.Message);
            }
        }

        private void escribirBitacora(string msg)
        {
            try
            {
                if (!Directory.Exists("C:\\temp\\"))
                    Directory.CreateDirectory("C:\\temp\\");
                File.AppendAllText("C:\\temp\\BitacoraSegundaFirma.txt", DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToShortTimeString() + DateTime.Now.Second.ToString() + " " + msg + saltoLinea);
            }
            catch(Exception e)
            {

            }
        }

        protected void ShowMessage(string Message, MessageType type, String container)
        {
            try
            {
                Message = Message.Replace("'", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "','" + container + "');", true);
            }
            catch (Exception eSM)
            {
                throw eSM;
            }
        }
    }
}