<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Recombee_setup"
    Theme="Default" ValidateRequest="false" EnableEventValidation="false"  Codebehind="Recombee_setup.aspx.cs"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>
<asp:Content ID="cntHeader" runat="server" ContentPlaceHolderID="plcBeforeContent">
    <style>
      .recombee .wrapper{
          margin-bottom:20px;
      }  

      .recombee .wrapper.history {
          margin-top:50px;
      }
    </style>
</asp:Content>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="recombee">
        <h2>Recombee</h2>
        <h4>Recombee database structure</h4>
        <div class="wrapper">
            <div class="wrapper">
                <p>To initialize Recombee database structure press following button</p>
                <cms:LocalizedButton runat="server" ID="btnIntDbStructure" OnClick="InitDatabaseStructure_Click" Text="Initialize database" CssClass="btn btn-primary" />
            </div>
            <div class="wrapper">
                <p>To reset Recombee database including the structure press following button</p>
                <cms:LocalizedButton runat="server" ID="btnResetDatabase" OnClick="ResetDatabase_Click" Text="Reset database" ButtonStyle="Default" />
            </div>
        </div>
        <div class="wrapper history" runat="server" id="divHistory">
            <h4>Generate history data</h4>
            <p>You can generate sample history data for example articlse views, cart additions, purchases to Recombee by clicking "Generate history data" </p>
            <cms:LocalizedButton runat="server" ID="btnInitDatabase" OnClick="InitHistory_Click" Text="Generate history data" CssClass="btn-primary" />
        </div>
    </div>  
</asp:Content>

