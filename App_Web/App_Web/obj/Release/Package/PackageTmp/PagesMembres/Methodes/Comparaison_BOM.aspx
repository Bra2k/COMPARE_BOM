<%@ Page Title="Comparaison de BOM" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Comparaison_BOM.aspx.vb" Inherits="App_Web.Comparaison_BOM" Theme="Skin_CHRT_ECO"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="font-family: Verdana; font-size: xx-large; font-weight: 900; font-variant: normal; color: #002F60">
    Comparaison de BOM&nbsp;&nbsp;&nbsp;
    <asp:Label ID="Label_LOAD" runat="server" Text="Chargement en cours ..." Visible="False"></asp:Label>
</div>
    <asp:Image ID="Image_EOL_LOAD_1" runat="server" ImageUrl="~/App_Themes/PIC_CHAR/eolienneanimee.gif" Visible="False" />
<asp:Image ID="Image_EOL_LOAD_2" runat="server" ImageUrl="~/App_Themes/PIC_CHAR/eolienneanimee.gif" Visible="False" />
<asp:Image ID="Image_EOL_LOAD_3" runat="server" ImageUrl="~/App_Themes/PIC_CHAR/eolienneanimee.gif" Visible="False" />
    <br />
    <asp:MultiView ID="MultiView_COMP_BOM" runat="server" ActiveViewIndex="0">
        <asp:View ID="View_SEL_BOM" runat="server">
            Sélectionner le nom du client :<br />
            <asp:DropDownList ID="DropDownList_Client" runat="server" AutoPostBack="True" CausesValidation="True" ValidateRequestMode="Enabled">
                <asp:ListItem Value="ELTA"></asp:ListItem>
                <asp:ListItem Value="ROLLS ROYCE"></asp:ListItem>
                <asp:ListItem Value="ZODIAC"></asp:ListItem>
                <asp:ListItem Value="CP DESOUTTER"></asp:ListItem>
                <asp:ListItem Value="SPHEREA"></asp:ListItem>
                <asp:ListItem Value="THALES OPTRONIQUE"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            Sélectionner l&#39;article :
            <asp:Label ID="Label_SEL_ARTI" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label>
            <br />
            <asp:DropDownList ID="DropDownList_Article" runat="server" AutoPostBack="True" CausesValidation="True">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <br />
            Sélectionner le fichier BOM à contrôler :<asp:FileUpload ID="FileUpload_BOM" runat="server" />
            <br />
            <asp:Button ID="Button_Importer" runat="server" Text="Importer" />
            <asp:SqlDataSource ID="SqlDataSource_PARA_BOM" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" InsertCommand="INSERT INTO TEST.[dbo].[Essai_PARA_BOM]
           ([NM_CLIE]
           ,[NM_PARA]
           ,[NM_PARA_VAL]
           ,[DT_MAJ_PARA])
     VALUES
           (@NM_CLIE
           ,@NM_PARA
           ,@NM_PARA_VAL
           ,GETDATE())" SelectCommand="SELECT     Essai_PARA_BOM_1.NM_CLIE, Essai_PARA_BOM_1.NM_PARA, Essai_PARA_BOM_1.NM_PARA_VAL
FROM         (SELECT     NM_CLIE, NM_PARA, MAX(DT_MAJ_PARA) AS DT_MAJ_PARA
                       FROM          TEST.dbo.Essai_PARA_BOM
                       GROUP BY NM_CLIE, NM_PARA
                       HAVING      (NM_CLIE = @NM_CLIE)) AS DERN_CONF INNER JOIN
                      TEST.dbo.Essai_PARA_BOM AS Essai_PARA_BOM_1 ON DERN_CONF.NM_PARA = Essai_PARA_BOM_1.NM_PARA AND 
                      DERN_CONF.NM_CLIE = Essai_PARA_BOM_1.NM_CLIE AND DERN_CONF.DT_MAJ_PARA = Essai_PARA_BOM_1.DT_MAJ_PARA">
                <InsertParameters>
                    <asp:Parameter Name="NM_CLIE" />
                    <asp:Parameter Name="NM_PARA" />
                    <asp:Parameter Name="NM_PARA_VAL" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList_Client" Name="NM_CLIE" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:View>
        <asp:View ID="View_ONGL" runat="server">

            Sélectionner l&#39;onglet du fichier :
            <asp:DropDownList ID="DropDownList_ONGL_BOM_CLIE" runat="server">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button_ONGL_BOM_CLIE" runat="server" Text="Valider" SkinID="BOUT_VALI" />

        </asp:View>
        <asp:View ID="View_SEL_DONN" runat="server">
            <br />
            Saisir la première ligne où commencent les données :&nbsp;<asp:TextBox ID="TextBox_PREM_LIGN_DONN" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
            <br />
            Saisir la dernière ligne où terminent les données :
            <asp:TextBox ID="TextBox_DERN_LIGN_DONN" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:CheckBox ID="CheckBox_COLO_FORM" runat="server" AutoPostBack="True" Checked="True" Text="Colonnes séparées" />
            <br />
            <asp:MultiView ID="MultiView_CONF_COLO" runat="server"  ActiveViewIndex="1">
                <asp:View ID="View_COLO_UNIQ" runat="server">
                    Saisir le caractère séparateur :
                    <asp:TextBox ID="TextBox_CARA_SEPA" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button_CARA_SEPA" runat="server" SkinID="BOUT_VALI" Text="Valider" />
                    <br />
                </asp:View>
                <asp:View ID="View_CONF_MULT" runat="server">
                    Saisir la colonne où se trouvent les codes articles client :
                    <asp:TextBox ID="TextBox_COLO_CD_ARTI_CLIE" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    Saisir la colonne où se trouvent les repères :
                    <asp:TextBox ID="TextBox_COLO_REPE" runat="server"></asp:TextBox>
                    &nbsp;<br />
                    <br />
                    Saisir la colonne où se trouvent les quantités :
                    <asp:TextBox ID="TextBox_COLO_QTE" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="Button_APER_PIVO" runat="server" Text="Afficher l'extraction du fichier BOM client" />
                    <br />
                    <br />
                    Aperçu du fichier BOM :
                    <div  style="vertical-align:top;  width: 1198px; height: 539px; overflow: auto;">
                        <asp:GridView ID="GridView_FICH_BOM_CLIE" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" SkinID="GV_AFF_DONN">
                          
                            <Columns>
                                <asp:CommandField SelectText="Sélectionner la dernière ligne" ShowSelectButton="True" />
                            </Columns>
                            
                        </asp:GridView>
                    </div>
                    <br />
                </asp:View>
            </asp:MultiView>
        </asp:View>
        <asp:View ID="View_CONF_CAS_PART" runat="server">
            <table style="width:100%;">
                <tr>
                    <td style="vertical-align:top" rowspan="2">
                        <asp:Button ID="Button_RAZ_PIVO" runat="server" Text="Réinitialiser l'aperçu" />
                        <br />
                        <asp:RadioButtonList ID="RadioButtonList_LIST_OPTI" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="REPE_MULT_COLO_REPE">Scinder les repères dans la colonne &quot;Repère&quot;</asp:ListItem>
                            <asp:ListItem Value="RESU_ANA_REPE_CARA_SPEC">Analyse de la colonne repère et correction semi-automatique</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <asp:MultiView ID="MultiView_CONF_CAS_PART" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View_VIDE" runat="server">
                            </asp:View>
                            <asp:View ID="View_RESU_ANA_REPE_CARA_SPEC" runat="server">
                                <asp:Label ID="Label_RESU_ANA_REPE_CARA_SPEC" runat="server"></asp:Label>
                                &nbsp;:&nbsp;
                                <asp:TextBox ID="TextBox_RESU_ANA_REPE_CARA_SPEC" runat="server" Width="390px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button_RESU_ANA_REPE_CARA_SPEC" runat="server" Text="Valider" />
                                <br />
                                <br />
                                <asp:RadioButtonList ID="RadioButtonList_GENE_RPR_RPC_CARA" runat="server">
                                    <asp:ListItem Selected="True" Value="Générer" Text="Générer les repères"></asp:ListItem>
                                    <asp:ListItem Value="Séparer" Text="Séparer les repères"></asp:ListItem>
                                </asp:RadioButtonList>
                            </asp:View>
                            <asp:View ID="View_SUPP_LETT_CD_ARTI_BOM_CLIE" runat="server">
                                Saisir le ou la suite de caractères à supprimer :
                                <asp:TextBox ID="TextBoxSUPP_LETT_CD_ARTI_BOM_CLIE" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="ButtonSUPP_LETT_CD_ARTI_BOM_CLIE" runat="server" SkinID="BOUT_VALI" Text="Valider" />
                            </asp:View>
                            <asp:View ID="View_SUPP_CRCR_RPR" runat="server">
                                Saisir le ou la suite de caractères à supprimer :
                                <asp:TextBox ID="TextBox_SUPP_CRCR_RPR" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button_SUPP_CRCR_RPR" runat="server" SkinID="BOUT_VALI" Text="Valider" />
                            </asp:View>
                            <asp:View ID="View_REPE_MULT_COLO_REPE" runat="server">
                                Saisir le caractère séparateur :
                                <asp:TextBox ID="TextBox_REPE_MULT_COLO_REPE" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="Button_REPE_MULT_COLO_REPE" runat="server" Text="Valider" SkinID="BOUT_VALI" />
                                <br />
                                <asp:RadioButtonList ID="RadioButtonList_QTE_PLUS_LIGN" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">Une quantité par ligne</asp:ListItem>
                                    <asp:ListItem>Quantité identique pour plusieurs lignes</asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                            </asp:View>
                        </asp:MultiView>
                    </td>
                    <td style="vertical-align:top">Aperçu du pivot :<asp:Button ID="Button_GENE_PIVO" runat="server" Text="Générer le pivot" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td> 
                        <div style="vertical-align:top;  width: 300px; height: 800px; overflow: auto;">
                        <asp:GridView ID="GridView_PIVO_BOM_CLIE" runat="server" SkinID="GV_AFF_DONN">
                        </asp:GridView>
                    </div>

                    </td>
                </tr>
                
            </table>
            <br />
            <br />
        </asp:View>
        <asp:View ID="View_RESU_COMP" runat="server">
            <table style="width:100%;">
                <tr>
                    <td style="vertical-align: top; ">Lignes du fichier BOM du client non-présentes dans SAP :<br />
                        <div style="vertical-align:top;  width: 400px; height: 800px; overflow: auto;">
                        <asp:GridView ID="GridView_DIFF_BOM_CLIE" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_DIFF_BOM_CLIE" SkinID="GV_AFF_DONN">
                            <Columns>
                                <asp:BoundField DataField="Code article Client" HeaderText="Code article Client" SortExpression="Code article Client" />
                                <asp:BoundField DataField="Repère Client" HeaderText="Repère Client" SortExpression="Repère Client" />
                                <asp:BoundField DataField="Quantité Client" HeaderText="Quantité Client" SortExpression="Quantité Client" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource_DIFF_BOM_CLIE" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT [CD_ARTI] AS [Code article Client], [NM_REPE] AS [Repère Client], [QT_REPE] AS [Quantité Client]
FROM [APP_WEB_ECO].[dbo].[DWH_CPRS_BOM_PIVO_CLIE]
WHERE NOT EXISTS (
SELECT [CD_ARTI_CLIE]
      ,[NM_REPE_CLIE]
      ,[QT_REPE_CLIE]    
  FROM [APP_WEB_ECO].[dbo].[V_CRPS_BOM_PIVO_EGAL]
  WHERE [DWH_CPRS_BOM_PIVO_CLIE].[CD_ARTI] = [V_CRPS_BOM_PIVO_EGAL].[CD_ARTI_CLIE] AND
  [DWH_CPRS_BOM_PIVO_CLIE].[NM_REPE] = [V_CRPS_BOM_PIVO_EGAL].[NM_REPE_CLIE] AND
   [DWH_CPRS_BOM_PIVO_CLIE].[QT_REPE] = [V_CRPS_BOM_PIVO_EGAL].[QT_REPE_CLIE]) AND [QT_REPE] &lt;&gt; '0'
   GROUP BY [CD_ARTI], [NM_REPE], [QT_REPE]
   	  ORDER BY [NM_REPE]"></asp:SqlDataSource>
                            </div>
                    </td>
                    <td style="vertical-align: top">Lignes de SAP non-présentes dans le fichier BOM du client:<br />
                        <div style="vertical-align:top;  width: 400px; height: 800px; overflow: auto;">
                        <asp:GridView ID="GridView_DIFF_BOM_SAP" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_DIFF_BOM_SAP" SkinID="GV_AFF_DONN">
                            <Columns>
                                <asp:BoundField DataField="Code article SAP" HeaderText="Code article SAP" SortExpression="Code article SAP" />
                                <asp:BoundField DataField="Repère SAP" HeaderText="Repère SAP" SortExpression="Repère SAP" />
                                <asp:BoundField DataField="Quantité SAP" HeaderText="Quantité SAP" SortExpression="Quantité SAP" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource_DIFF_BOM_SAP" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT [CD_ARTI] AS [Code article SAP], [NM_REPE] AS [Repère SAP], [QT_REPE] AS [Quantité SAP]
FROM [APP_WEB_ECO].[dbo].[DWH_CPRS_BOM_PIVO_SAP]
WHERE NOT EXISTS (SELECT [CD_ARTI_SAP]
      ,[NM_REPE_SAP]
      ,[QT_REPE_SAP]    
  FROM [APP_WEB_ECO].[dbo].[V_CRPS_BOM_PIVO_EGAL]
  WHERE [DWH_CPRS_BOM_PIVO_SAP].[CD_ARTI] = [V_CRPS_BOM_PIVO_EGAL].[CD_ARTI_SAP] AND
  [DWH_CPRS_BOM_PIVO_SAP].[NM_REPE] = [V_CRPS_BOM_PIVO_EGAL].[NM_REPE_SAP] AND
   [DWH_CPRS_BOM_PIVO_SAP].[QT_REPE] = [V_CRPS_BOM_PIVO_EGAL].[QT_REPE_SAP])
      GROUP BY [CD_ARTI], [NM_REPE], [QT_REPE]
	  ORDER BY [NM_REPE]"></asp:SqlDataSource>
                            </div>
                    </td>
                </tr>
                <tr>
                    <td colspan=2 align=center style="vertical-align: top; height: 300px; overflow: auto;">
                                               Lignes du fichier BOM du client présent dans SAP :
                        <asp:GridView ID="GridView_IDEN_BOM_SAP" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_IDEN_BOM_SAP" SkinID="GV_AFF_DONN">
                            <Columns>
                                <asp:BoundField DataField="Code article client" HeaderText="Code article client" SortExpression="Code article client" />
                                <asp:BoundField DataField="Repère client" HeaderText="Repère client" SortExpression="Repère client" />
                                <asp:BoundField DataField="Quantité client" HeaderText="Quantité client" SortExpression="Quantité client" />
                                <asp:BoundField DataField="Nombre de caractères différents dans les codes article" HeaderText="Nombre de caractères différents dans les codes article" SortExpression="Nombre de caractères différents dans les codes article" />
                                <asp:BoundField DataField="Code article SAP" HeaderText="Code article SAP" SortExpression="Code article SAP" />
                                <asp:BoundField DataField="Repère SAP" HeaderText="Repère SAP" SortExpression="Repère SAP" />
                                <asp:BoundField DataField="Quantité SAP" HeaderText="Quantité SAP" SortExpression="Quantité SAP" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource_IDEN_BOM_SAP" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT CD_ARTI_CLIE AS [Code article client], NM_REPE_CLIE AS [Repère client], QT_REPE_CLIE AS [Quantité client], MIN_LVS AS [Nombre de caractères différents dans les codes article], CD_ARTI_SAP AS [Code article SAP], NM_REPE_SAP AS [Repère SAP], QT_REPE_SAP AS [Quantité SAP]
  FROM [APP_WEB_ECO].[dbo].[V_CRPS_BOM_PIVO_EGAL]"></asp:SqlDataSource>
                    </td>
                    
                </tr>
            </table>
            
        </asp:View>
        
        <br />
        
    </asp:MultiView>
    </asp:Content>
