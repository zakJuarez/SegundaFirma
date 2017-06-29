<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmInterpretacion.aspx.cs" Inherits="FUJIFILM.InterpretacionesDF.Site.frmInterpretacion" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function Redirecciona(strRuta, urlSyn, version, idCarpeta, AccNum, InstSyn, pathSyn) {
            if (version == 1) //browser
            {
                var sID = Math.round(Math.random() * 10000000000);
                var winX = screen.availWidth;
                var winY = screen.availHeight;
                sID = "E" + sID;
                var win = window.open(strRuta, sID,
                                                "menubar=yes,toolbar=yes,location=yes,directories=yes,status=yes,resizable=yes" +
                                                ",scrollbars=yes,top=0,left=0,screenX=0,screenY=0,Width=" +
                                                winX + ",Height=" + winY);
            }
            if(version == 2)//versiones anteriores
            {
                var oSynapseAPI = new ActiveXObject('Synapse.SynapseAPI');
                var carpetaAccNum = pathSyn.concat(AccNum); 
                oSynapseAPI.OpenSynapseExplorer(InstSyn, urlSyn, carpetaAccNum);

            }

            if(version == 3)//API
            {
                var oSynapseAPI = new ActiveXObject("Synapse.SynapseAPI");
                var sStudyPath = oSynapseAPI.MakePath(idCarpeta, AccNum);
                oSynapseAPI.OpenSynapseExplorer(InstSyn, urlSyn, sStudyPath);
            }
        }

        function escribeArchivo(strLines, _url, urlSyn, version, idCarpeta, AccNum, InstSyn, pathSyn) {
            try {
    
                var fso = new ActiveXObject("Scripting.FileSystemObject");
                varFileObject = fso.OpenTextFile('C:\\Dictaphone\\study.xml', 2, true, 0);
                varFileObject.writeLine('<?xml version="1.0" encoding="iso-8859-1" ?>' + strLines);
                //varFileObject.write(strLines);
                varFileObject.close();
                //ChangeFileName()
                Redirecciona(_url, urlSyn, version, idCarpeta, AccNum, InstSyn, pathSyn);
            }
            catch (err) {
                alert(err);
            }
        }

        function ChangeFileName() {
            try
            {
                var fso, f,a;
                fso = new ActiveXObject("Scripting.FileSystemObject");
                fileBool = fso.FileExists('C:\\Dictaphone\\study.xml');
                if (fileBool)
                {
                    a = fso.GetFile('C:\\Dictaphone\\study.xml');
                    var sID = Math.round(Math.random() * 10000000000);
                    a.name = sID + "study_anterior.dat";
                }
                f = fso.GetFile('C:\\Dictaphone\\study.dat');
                f.name = "study.xml"
            }
            catch(err)
            {
                throw err;
            }
        }
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "tab1success";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        function Actualiza()
        {
            document.getElementById('<%= btnBus2.ClientID %>').click();
        }

        $(function () {
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var target = $(e.target).attr("href")
                Actualiza();
            });
        })
    </script>
    <style type="text/css" >
        
    </style>
    <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick"></asp:Timer>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label Text="" runat="server" ID="lblUsuario"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <br />
    <div>
        <div class="container">
            <div class="row">
                <div class="messagealert" id="alert_container">
                </div>
		        <div class="col-lg-12 col-md-12 col-sm-12">
                    <div class="panel with-nav-tabs">
                        <div class="panel-heading" id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li class="active">
                                    <a href="#tab1success" data-toggle="tab" >Con Solicitud de Verificación</a>
                                </li>
                                <li>
                                    <a  data-toggle="tab" href="#tab2success">Con Solicitud de Revisión 
                                        <span class="badge bg-green">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label Text="" runat="server" ID="lblNumPendientes"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div class="panel-body">
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="tab1success">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="panel panel-info">
                                                <div class="panel-heading text-center">
                                                    Búsqueda
                                                </div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-2 col-md-2 col-sm-2 text-center">
                                                            <asp:Label runat="server" Text="Número de Estudio"></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusNumEst"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2 text-center">
                                                            <asp:Label runat="server" Text="Nombre del Paciente"></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusqNomPac"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2  text-center">
                                                            <asp:Label runat="server" Text="ID del Paciente" ></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusqPaciente"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2  text-center">
                                                            <asp:Label runat="server" Text="Modalidad" ID="lblModalidad" ></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusModalidad"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2  text-center">
                                                            <asp:Label runat="server" Text="Usuarios" ID="lblUsuarios" Visible="false"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlBusUsuarios" CssClass="form-control" Visible="false"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2 text-center">
                                                            <asp:Button ID="btnNusqueda" runat="server" OnClick="btnNusqueda_Click" Text="Buscar" CssClass="btn btn-primary" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" >
                                            <asp:UpdatePanel runat="server" ID="updPanelResult">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" ID="pnlResult">
                                                        <asp:GridView ID="grvBusqueda" runat="server" AllowPaging="true" CssClass="table table-bordered table-hover"
                                                        PageSize="5" AutoGenerateColumns="false" OnRowDataBound="grvBusqueda_RowDataBound"
                                                        OnPageIndexChanging="grvBusqueda_PageIndexChanging"
                                                        OnRowCommand="grvBusqueda_RowCommand"
                                                        EmptyDataText="No hay resultado bajo el criterio de búsqueda.">
                                                            <Columns>
                                                                <asp:TemplateField ControlStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="InternalPatientID"  HeaderText="Paciente ID" ReadOnly="true" />
                                                                <asp:BoundField DataField="AccessionNumber" HeaderText="Núm. de Estudio" ReadOnly="true" />
                                                                <asp:BoundField DataField="FullName" HeaderText="Nombre del Paciente" ReadOnly="true"/>
                                                                <asp:BoundField DataField="DOB" HeaderText="Fecha Nac." ReadOnly="true" />
                                                                <asp:BoundField DataField="Gender" HeaderText="Genero" ReadOnly="true" />
                                                                <asp:BoundField DataField="ProcedureDescription" HeaderText="Procedimiento" ReadOnly="true" />
                                                                <asp:BoundField DataField="usuarioDicto" HeaderText="Usuario" ReadOnly="true"/>
                                                                <asp:BoundField DataField="Modality" HeaderText="Modalidad" ReadOnly="true"/>
                                                                <asp:BoundField DataField="AdmissionTimeDate" HeaderText="Fecha Estudio" ReadOnly="true" />
                                                                <asp:BoundField DataField="Priority" HeaderText="Prioridad" ReadOnly="true" />
                                                                <asp:TemplateField ControlStyle-Width="5%" HeaderText="Solicita Revisión">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="btnSolRev" runat="server" ImageUrl="~/Images/transparente.png" Height="20px" Width="20px" ></asp:Image>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Verificar">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:LinkButton ID="btnVisualizar"  CommandName="viewDoc" CommandArgument='<%# Bind("AccessionNumber") %>' runat="server">
                                                                                <asp:Image ID="ImageVisializa" runat="server" ImageUrl="~/Images/ic_action_edit.png" Height="35px" Width="35px" ToolTip=""/>
                                                                            </asp:LinkButton>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerTemplate>
                                                                <asp:Label ID="lblTemplate" runat="server" Text="Muestra Filas: " CssClass="Label" />
                                                                <asp:DropDownList ID="ddlBandeja" runat="server" AutoPostBack="true" CausesValidation="false"
                                                                    Enabled="true" OnSelectedIndexChanged="ddlBandeja_SelectedIndexChanged">
                                                                        <asp:ListItem Value="5" />
                                                                        <asp:ListItem Value="10" />
                                                                        <asp:ListItem Value="15" />
                                                                        <asp:ListItem Value="20" />
                                                                        <asp:ListItem Value="25" />
                                                                </asp:DropDownList>
                                                                &nbsp;Página
                                                                <asp:TextBox ID="txtBandeja" runat="server" AutoPostBack="true" OnTextChanged="txtBandeja_TextChanged"
                                                                    Width="40" MaxLength="10" />
                                                                de
                                                                <asp:Label ID="lblBandejaTotal" runat="server" />
                                                                &nbsp;
                                                                <asp:Button ID="btnBandeja_I" runat="server" CommandName="Page" CausesValidation="false"
                                                                    ToolTip="Página Anterior" CommandArgument="Prev" CssClass="previous" />
                                                                <asp:Button ID="btnBandeja_II" runat="server" CommandName="Page" CausesValidation="false"
                                                                    ToolTip="Página Siguiente" CommandArgument="Next" CssClass="next" />
                                                            </PagerTemplate>
                                                            <HeaderStyle CssClass="headerstyle" />
                                                            <FooterStyle CssClass="text-center" />
                                                            <PagerStyle CssClass="text-center" />
                                                    </asp:GridView>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane" id="tab2success">
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="panel panel-yellow">
                                                <div class="panel-heading text-center">
                                                    Búsqueda
                                                </div>
                                                <div class="panel-body">
                                                    <div class="row">
                                                        <div class="col-lg-2 col-md-2 col-sm-2 text-center">
                                                            <asp:Label runat="server" Text="Número de Estudio"></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusNumEst2"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2 text-center">
                                                            <asp:Label runat="server" Text="Nombre del Paciente"></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusNomPac2"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2  text-center">
                                                            <asp:Label runat="server" Text="ID del Paciente" ></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusIDPac2"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2  text-center">
                                                            <asp:Label runat="server" Text="Modalidad" ID="lblModalidad2" ></asp:Label>
                                                            <asp:TextBox runat="server" CssClass="form-control" Text="" ID="txtBusModalidad2"  Width="100%"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2  text-center">
                                                            <asp:Label runat="server" Text="Usuarios" ID="lblUsuarios2" Visible="false"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlBusUsuario2" CssClass="form-control" Visible="false"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-2 col-md-2 col-sm-2 text-center">
                                                            <asp:Button ID="btnBus2" runat="server" OnClick="btnBus2_Click" Text="Buscar" CssClass="btn btn-yellow" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <asp:UpdatePanel runat="server" ID="updGrid2">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" ID="pnlGrid2">
                                                        <asp:GridView ID="grvBusqueda2" runat="server" AllowPaging="true" CssClass="table table-bordered table-hoverYellow"
                                                        PageSize="5"
                                                        AutoGenerateColumns="false" OnRowDataBound="grvBusqueda2_RowDataBound"
                                                        OnPageIndexChanging="grvBusqueda2_PageIndexChanging"
                                                        OnRowCommand="grvBusqueda2_RowCommand"
                                                        EmptyDataText="No hay resultado bajo el criterio de búsqueda.">
                                                            <Columns>
                                                                <asp:TemplateField ControlStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItemIndex")) + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="InternalPatientID"  HeaderText="Paciente ID" ReadOnly="true"/>
                                                                <asp:BoundField DataField="AccessionNumber" HeaderText="Núm. de Estudio" ReadOnly="true"/>
                                                                <asp:BoundField DataField="FullName" HeaderText="Nombre del Paciente" ReadOnly="true" />
                                                                <asp:BoundField DataField="DOB" HeaderText="Fecha Nac." ReadOnly="true" />
                                                                <asp:BoundField DataField="Gender" HeaderText="Genero" ReadOnly="true" />
                                                                <asp:BoundField DataField="ProcedureDescription" HeaderText="Procedimiento" ReadOnly="true" />
                                                                <asp:BoundField DataField="usuarioDicto" HeaderText="Usuario" ReadOnly="true" />
                                                                <asp:BoundField DataField="Modality" HeaderText="Modalidad" ReadOnly="true"/>
                                                                <asp:BoundField DataField="AdmissionTimeDate" HeaderText="Fecha Estudio" ReadOnly="true" />
                                                                <asp:BoundField DataField="Priority" HeaderText="Prioridad" ReadOnly="true" />
                                                                <asp:TemplateField ControlStyle-Width="5%" HeaderText="Solicita Revisión">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="btnSolRev" runat="server" ImageUrl="~/Images/transparente.png" Height="20px" Width="20px" ></asp:Image>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Revisar">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:LinkButton ID="btnVisualizar"  CommandName="viewDoc" CommandArgument='<%# Bind("AccessionNumber") %>' runat="server">
                                                                                <asp:Image ID="ImageVisializa" runat="server" ImageUrl="~/Images/ic_action_edit.png" Height="35px" Width="35px" ToolTip=""/>
                                                                            </asp:LinkButton>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerTemplate>
                                                                <asp:Label ID="lblTemplate" runat="server" Text="Muestra Filas: " CssClass="Label" />
                                                                <asp:DropDownList ID="ddlBandeja2" runat="server" AutoPostBack="true" CausesValidation="false"
                                                                    Enabled="true" OnSelectedIndexChanged="ddlBandeja2_SelectedIndexChanged">
                                                                        <asp:ListItem Value="5" />
                                                                        <asp:ListItem Value="10" />
                                                                        <asp:ListItem Value="15" />
                                                                        <asp:ListItem Value="20" />
                                                                        <asp:ListItem Value="25" />
                                                                </asp:DropDownList>
                                                                &nbsp;Página
                                                                <asp:TextBox ID="txtBandeja2" runat="server" AutoPostBack="true" OnTextChanged="txtBandeja2_TextChanged"
                                                                    Width="40" MaxLength="10" />
                                                                de
                                                                <asp:Label ID="lblBandejaTotal2" runat="server" />
                                                                &nbsp;
                                                                <asp:Button ID="btnBandeja_I" runat="server" CommandName="Page" CausesValidation="false"
                                                                    ToolTip="Página Anterior" CommandArgument="Prev" CssClass="previous" />
                                                                <asp:Button ID="btnBandeja_II" runat="server" CommandName="Page" CausesValidation="false"
                                                                    ToolTip="Página Siguiente" CommandArgument="Next" CssClass="next" />
                                                            </PagerTemplate>
                                                            <HeaderStyle CssClass="headerstyleYellow" />
                                                            <FooterStyle CssClass="text-center" />
                                                            <PagerStyle CssClass="text-center" />
                                                    </asp:GridView>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
	        </div>
        </div>
    </div>
    <asp:HiddenField ID="TabName" runat="server" />


    <script type="text/javascript">
        function ShowMessage(message, messagetype) {
            var cssclass;
            switch (messagetype) {
                case 'Success':
                    cssclass = 'alert-success'
                    break;
                case 'Error':
                    cssclass = 'alert-danger'
                    break;
                case 'Warning':
                    cssclass = 'alert-warning'
                    break;
                default:
                    cssclass = 'alert-info'
            }
            $('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
            $("#alert_div").fadeTo(2000, 500).slideUp(500, function () {
                $("#alert_div").slideUp(500);
            });
        }
    </script>
</asp:Content>
