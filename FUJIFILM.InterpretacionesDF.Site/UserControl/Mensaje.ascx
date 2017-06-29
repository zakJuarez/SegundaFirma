<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mensaje.ascx.cs" Inherits="FUJIFILM.InterpretacionesDF.Site.UserControl.Mensaje" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>

<asp:HiddenField runat="server" ID="hidModal" />
<style type="text/css">
    ._ModalPopup
    {
        background-color: White;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        z-index: 4000 !important;
    }

    .modalBackgroundMessage
{
    z-index: 888888 !important;
	background-color:Gray;
	filter:alpha(opacity=70);
	opacity:0.7;
}

    ._Mensaje
    {
        width: auto;
        height: auto;
        padding: 20px;
        margin-left: auto;
        margin-right: auto;
        margin-top: auto;
        z-index: 999999 !important;
    }
</style>
<ajaxToolkit:ModalPopupExtender ID="mpeConfirmacion" runat="server" TargetControlID="hidModal"
    PopupControlID="pnlPopup_ClientesNuevos" CancelControlID="btnCerrarConfirmacion"
    BackgroundCssClass="modalBackgroundMessage" DropShadow="false" Drag="false" />
<asp:Panel ID="pnlPopup_ClientesNuevos" runat="server" Style="display:none" class="_Mensaje" Width="100%" Height="100%">
    <asp:Panel ID="pnlConfirmacion" runat="server">
        <center>
            <div class="panel panel-default" style="width:40%; height:40%; vertical-align:middle">
                <div class="panel-heading" style="vertical-align:middle">
                    Mensaje del Sistema
                </div>
                <div class="panel-body" style="vertical-align:middle">
                    <div class="row">
                        <div class="col-md-12">
                            <table width="100%" style="height: 50%; ">
                                <tr style="margin-left: 15px; margin-right: 15px;">
                                    </tr>
                                 <tr>
                                    <td>
                                    </td>
                                     <tr>
                                         <td>
                                             <asp:UpdatePanel ID="upaeMensaje" runat="server">
                                                 <ContentTemplate>
                                                     <asp:Panel ID="Panel1" runat="server" GroupingText="Mensaje: ">
                                                         <table>
                                                             <tr>
                                                                 <td align="center">
                                                                     <asp:Image ID="imgMensaje" runat="server" Height="60%" ImageUrl="~/Images/ic_action_bulb.png" Width="60%" />
                                                                 </td>
                                                                 <td align="left" style="vertical-align:middle;">
                                                                     <div style="overflow:auto; width:100%; height:200px; vertical-align:middle; text-align:center;">
                                                                         <asp:Label ID="lblMensaje" runat="server" Font-Size="Small" ForeColor="WindowFrame" Text="">
                                                                    </asp:Label>
                                                                     </div>
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                     </asp:Panel>
                                                 </ContentTemplate>
                                             </asp:UpdatePanel>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td>&nbsp; </td>
                                     </tr>
                                     <tr>
                                         <td align="center">
                                             <asp:Button ID="btnCerrarConfirmacion" runat="server" CausesValidation="false" CssClass="btn btn-default" style="background-color: #428bca; border-color: #01992E; color:white;" Text="Cerrar" />
                                         </td>
                                     </tr>
                                     <tr>
                                         <td>&nbsp; </td>
                                     </tr>
                                     </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </center>
    </asp:Panel>
</asp:Panel>
