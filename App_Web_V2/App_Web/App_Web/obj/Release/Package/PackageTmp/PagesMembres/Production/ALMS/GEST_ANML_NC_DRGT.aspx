<%@ Page Title="Gestion des anomalies / non-conformités ALMS" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="GEST_ANML_NC_DRGT.aspx.vb" Inherits="App_Web.GEST_ANML_NC_DRGT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <table style="width: 100%;">
        <tr>
            <td style="vertical-align: top">
                <div class="titre_page">
                    <h2>Gestion des anomalies / non-conformités ALMS</h2>
                    <p>
                        &nbsp;
                    </p>
                    
                </div>


                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading" role="tab" id="headingOne">
                            <h4 class="panel-title">
                                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Anomalies</a>
                            </h4>
                        </div>
                        <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne" runat="server">
                            <div class="panel-body row">
                                <asp:MultiView ID="MultiView_ANML" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="View_SAIS" runat="server">
                                        <table style="width: 100%; vertical-align: top;">
                                            <tr>
                                                <td style="width: 256px; vertical-align: top;">Numéro de série :</td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox_NU_SER_SPFE" runat="server" Width="319px" AutoPostBack="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 256px; vertical-align: top;">Séquence : </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="DropDownList_SEQU_FAIL" runat="server" Width="319px" Enabled="False" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 256px; vertical-align: top;">Phase :</td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="DropDownList_PHAS_FAIL" runat="server" Width="319px" Enabled="False" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 256px; vertical-align: top;">Sous-Phase : </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="DropDownList_SOUS_PHAS_FAIL" runat="server" Width="319px" Enabled="False" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 256px; vertical-align: top;">Type de Non-Conformité :</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_TYPE_NC" runat="server" Width="296px" Enabled="False" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:GridView ID="GridView_TYPE_NC" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Font-Size="X-Small" AutoGenerateColumns="False" DataSourceID="SqlDataSource_DCPO_ANML">
                                                        <AlternatingRowStyle BackColor="#DCDCDC" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Description de l'anomalie" HeaderText="Description de l'anomalie" SortExpression="Description de l'anomalie" />
                                                        </Columns>
                                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                        <HeaderStyle BackColor="#002F60" Font-Bold="True" ForeColor="#989000" />
                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                                    </asp:GridView>
                                                    <asp:SqlDataSource ID="SqlDataSource_DCPO_ANML" runat="server" ConnectionString="<%$ ConnectionStrings:ALMS_PROD_PRDConnectionString %>" SelectCommand="SELECT [NM_DCPO_NC] AS [Description de l'anomalie]FROM [DTM_REF_TYPE_NC] WHERE ([ID_TYPE_NC] = @ID_TYPE_NC)">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownList_TYPE_NC" Name="ID_TYPE_NC" PropertyName="SelectedValue" Type="Byte" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                    <br />
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button_AJOU_TYPE_NC" runat="server" Text="+" Enabled="False" EnableTheming="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 256px; vertical-align: top;">Type de Cause :</td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_TYPE_CAUSE" runat="server" Width="296px" Enabled="False" DataSourceID="SqlDataSource_LIST_TYPE_CAUS" DataTextField="NM_TYPE_CAUS" DataValueField="ID_TYPE_CAUS" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource_LIST_TYPE_CAUS" runat="server" ConnectionString="<%$ ConnectionStrings:ALMS_PROD_PRDConnectionString %>" SelectCommand="SELECT 'TDC_' + CONVERT(NVARCHAR,[ID_TYPE_CAUS]) AS NM_TYPE_CAUS
      ,[ID_TYPE_CAUS]
  FROM [dbo].[DTM_REF_TYPE_CAUS]
 FULL OUTER JOIN (SELECT '' AS NM_TYPE_CAUS) AS A ON A.NM_TYPE_CAUS = [dbo].[DTM_REF_TYPE_CAUS].[ID_TYPE_CAUS]
                            ORDER BY [ID_TYPE_DEFA]"></asp:SqlDataSource>
                                                    <br />
                                                    <asp:GridView ID="GridView_TYPE_CAUS" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Font-Size="X-Small" DataSourceID="SqlDataSource_DETA_CAUS" AutoGenerateColumns="False">
                                                        <AlternatingRowStyle BackColor="#DCDCDC" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Défaut" HeaderText="Défaut" SortExpression="Défaut" />
                                                            <asp:BoundField DataField="Organe incriminé" HeaderText="Organe incriminé" SortExpression="Organe incriminé" />
                                                            <asp:BoundField DataField="Résolution" HeaderText="Résolution" SortExpression="Résolution" />
                                                            <asp:BoundField DataField="Séquence de retour en production" HeaderText="Séquence de retour en production" ReadOnly="True" SortExpression="Séquence de retour en production" />
                                                            <asp:BoundField DataField="Phase de retour en production" HeaderText="Phase de retour en production" ReadOnly="True" SortExpression="Phase de retour en production" />
                                                        </Columns>
                                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                        <HeaderStyle BackColor="#002F60" Font-Bold="True" ForeColor="#989000" />
                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                                    </asp:GridView>
                                                    <asp:SqlDataSource ID="SqlDataSource_DETA_CAUS" runat="server" ConnectionString="<%$ ConnectionStrings:ALMS_PROD_PRDConnectionString %>" SelectCommand="SELECT NM_DSGT_DEFA AS Défaut, CD_ARTI_DFTE AS [Organe incriminé], NM_RSLT AS Résolution, NM_RFRC_SEQU AS [Séquence de retour en production], NM_PHAS_RETO AS [Phase de retour en production] FROM V_NC_LIST_INFO_TYPE_CAUS WHERE (ID_TYPE_CAUS = @ID_TYPE_CAUS) GROUP BY NM_DSGT_DEFA, CD_ARTI_DFTE, NM_RSLT, NM_RFRC_SEQU, NM_PHAS_RETO, ID_TYPE_CAUS">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownList_TYPE_CAUSE" Name="ID_TYPE_CAUS" PropertyName="SelectedValue" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                    <br />
                                                    <asp:Button ID="Button_VALI_ENTER2" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button_ADD_TYPE_CAUS" runat="server" Text="+" Enabled="False" />
                                                </td>
                                            </tr>
                                        </table>







                                    </asp:View>
                                    <asp:View ID="View_NOUV_TYPE_NC" runat="server">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="vertical-align: top">Description :</td>
                                                <td>
                                                    <asp:TextBox ID="TextBox_NOUV_NC_DCPO" runat="server" Height="44px" Width="491px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Button ID="Button_VALI_ENTER0" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                                    </asp:View>
                                    <asp:View ID="View_NOUV_TYPE_CAUS" runat="server">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="vertical-align: top">Type de défaut : </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList_TYPE_DEFA" runat="server" Height="16px" Width="433px" DataSourceID="SqlDataSource_LIST_TYPE_DEFA" DataTextField="NM_DSGT_DEFA" DataValueField="ID_TYPE_DEFA">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource_LIST_TYPE_DEFA" runat="server" ConnectionString="<%$ ConnectionStrings:ALMS_PROD_PRDConnectionString %>" SelectCommand="SELECT [ID_TYPE_DEFA], [NM_DSGT_DEFA] FROM [DTM_REF_TYPE_DEFA]"></asp:SqlDataSource>
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button_ADD_TYPE_DEFA" runat="server" Text="+" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top;">Organe incriminé (Code article ALMS) :</td>
                                                <td colspan="2" style="height: 82px">
                                                    <asp:TextBox ID="TextBox_CD_ARTI_DFTE" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">Résolution (analyse) :</td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox_RSLT" runat="server" Height="44px" Width="491px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">Nom de la séquence de retour en production :</td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="DropDownList_SEQU_RETO_PDTO" runat="server" Height="16px" Width="433px" DataSourceID="SqlDataSource_LIST_SEQU_RETO" DataTextField="NM_RFRC_SEQU" DataValueField="ID_RFRC_SEQU" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource_LIST_SEQU_RETO" runat="server" ConnectionString="<%$ ConnectionStrings:ALMS_PROD_PRDConnectionString %>" SelectCommand="SELECT DTM_SEQU_RFRC_LIST.ID_RFRC_SEQU, DTM_SEQU_RFRC_LIST.NM_RFRC_SEQU + '_V' + CONVERT (NVARCHAR, DTM_SEQU_RFRC_LIST.NU_VERS_SEQU) AS NM_RFRC_SEQU FROM DTM_SEQU_RFRC_LIST INNER JOIN (SELECT NM_RFRC_SEQU, MAX(NU_VERS_SEQU) AS MAX_NU_VERS_SEQU FROM DTM_SEQU_RFRC_LIST AS DTM_SEQU_RFRC_LIST_1 GROUP BY NM_RFRC_SEQU) AS DT_MAX_VERS_SEQU ON DTM_SEQU_RFRC_LIST.NM_RFRC_SEQU = DT_MAX_VERS_SEQU.NM_RFRC_SEQU AND DTM_SEQU_RFRC_LIST.NU_VERS_SEQU = DT_MAX_VERS_SEQU.MAX_NU_VERS_SEQU ORDER BY CONVERT (INTEGER, SUBSTRING(DTM_SEQU_RFRC_LIST.NM_RFRC_GAMM_ECO, CHARINDEX(':', DTM_SEQU_RFRC_LIST.NM_RFRC_GAMM_ECO) + 1, LEN(DTM_SEQU_RFRC_LIST.NM_RFRC_GAMM_ECO) - 1 - CHARINDEX(':', DTM_SEQU_RFRC_LIST.NM_RFRC_GAMM_ECO)))"></asp:SqlDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">Phase de retour en production :</td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="DropDownList_PHAS_RETO_PDTO" runat="server" Height="16px" Width="432px" DataSourceID="SqlDataSource_LIST_PHAS_RETO" DataTextField="NM_PHAS" DataValueField="NU_PHAS_FCGF">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource_LIST_PHAS_RETO" runat="server" ConnectionString="<%$ ConnectionStrings:ALMS_PROD_PRDConnectionString %>" SelectCommand="SELECT 'Phase n°' + CONVERT (nvarchar, NU_PHAS_FCGF) + ' : ' + NM_PHAS_FCGF AS NM_PHAS, NU_PHAS_FCGF FROM DTM_SEQU_RFRC_DETA WHERE (ID_RFRC_SEQU = @ID_RFRC_SEQU) GROUP BY NU_PHAS_FCGF, NM_PHAS_FCGF, ID_RFRC_SEQU">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="DropDownList_SEQU_RETO_PDTO" Name="ID_RFRC_SEQU" PropertyName="SelectedValue" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Button ID="Button_VALI_ENTER" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                                    </asp:View>
                                    <asp:View ID="View_CHAN_TCBL_UNIT" runat="server">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>Code article du sous-ensemble à changer : </td>
                                                <td style="width: 190px">
                                                    <asp:Label ID="Label_CD_ARTI_CHAN" runat="server" Text=""></asp:Label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>Numéro de série : </td>
                                                <td style="width: 190px">
                                                    <asp:TextBox ID="TextBoxNU_SER_CHAN" runat="server"></asp:TextBox>
                                                </td>

                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Button ID="Button_VALI_ENTER1" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                                    </asp:View>
                                </asp:MultiView>





                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading" role="tab" id="headingTwo">
                            <h4 class="panel-title">
                                <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">Dérogations, NQI ou n° de rapport de contrôle</a>
                            </h4>
                        </div>
                        <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo"  runat="server">
                            <div class="panel-body row">

                                <table style="width: 100%;">
                                    <tr>
                                        <td>Numéro de dérogation :
                        <asp:TextBox ID="TextBox_NU_DRGT" runat="server" AutoPostBack="True"></asp:TextBox>
                                            <br />
                                        </td>
                                        <td rowspan="4">Liste des numéros de série impactés :<asp:GridView ID="GridView_LIST_NU_SER_DRGT" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Size="X-Small" GridLines="Vertical">
                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <HeaderStyle BackColor="#002F60" Font-Bold="True" ForeColor="#989000" />

                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#000065" />
                                        </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Numéro de série :
                                    <asp:TextBox ID="TextBox_NU_SER_DRGT" runat="server" AutoPostBack="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Description :
                        <asp:TextBox ID="TextBox_NM_DCPO_DRGT" runat="server" Height="68px" Width="313px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="Button_VALI_ENTER3" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                                        </td>
                                    </tr>
                                </table>


                            </div>
                        </div>
                    </div>
                </div>




            </td>
            <td style="vertical-align: top">
                <table style="width: 100%;">
                    <tr>
                        <td>OF: 
                        </td>
                        <td>
                            <asp:Label ID="Label_NU_OF" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Code article Eolane :
                        </td>
                        <td>
                            <asp:Label ID="Label_CD_ARTI_ECO" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Désignation Eolane :
                        </td>
                        <td>
                            <asp:Label ID="Label_NM_DSGT_ECO" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Code article ALMS :
                        </td>
                        <td>
                            <asp:Label ID="Label_CD_ARTI_ALMS" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Numéro d&#39;incident :
                        </td>
                        <td>
                            <asp:Label ID="Label_NU_INCI" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Statut :
                        </td>
                        <td>
                            <asp:Label ID="Label_ID_STAT" runat="server" Text="Init"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>


        <tr>
            <td style="vertical-align: top">&nbsp;</td>
            <td style="vertical-align: top">&nbsp;</td>
        </tr>

    </table>
</asp:Content>
