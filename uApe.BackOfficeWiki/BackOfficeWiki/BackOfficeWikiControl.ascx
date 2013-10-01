<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BackOfficeWikiControl.ascx.cs" Inherits="uApe.BackOfficeWiki.BackOfficeWikiControl" %>
<%@ Register Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" TagPrefix="CD" %>

<CD:CssInclude ID="CssInclude1" runat="server" FilePath="~/umbraco/Plugins/BackOfficeWiki/css/bootstrap.min.css" />
<CD:CssInclude ID="CssInclude2" runat="server" FilePath="~/umbraco/Plugins/BackOfficeWiki/css/bootstrap-theme.min.css" />
<CD:CssInclude ID="CssInclude3" runat="server" FilePath="~/umbraco/Plugins/BackOfficeWiki/css/wiki-ui.min.css" />

<script type="text/javascript">
    var ddlDelCatName = '<%=ddlDelCatName.ClientID %>';
    var hdnDisplayOrder = '<%=hdnDisplayOrder.ClientID %>';
    var hdnPageName = '<%=hdnPageName.ClientID %>';
    var hdnMode = '<%=hdnMode.ClientID %>';
</script>

<asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissable">
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        <strong>Error!</strong>     
        <asp:Literal ID="ltError" runat="server"></asp:Literal>    
</asp:Panel>

<div class="wiki">   
    <asp:HiddenField ID="hdnMode" runat="server" />
    <asp:Panel ID="pnlDisplay" runat="server">
            <ol class="breadcrumb" id="displayBreadcrumbs" runat="server">
                <li><asp:Literal ID="ltCategory" runat="server"></asp:Literal></li>
                <li class="active"><asp:Literal ID="ltPageName" runat="server"></asp:Literal></li>
            </ol> 
            <asp:Panel ID="pnlDisplayContent" runat="server" CssClass="display displaystyle col-xs-9">
                <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
            </asp:Panel>
            <asp:Panel ID="pnlDisplayNav" runat="server" CssClass="col-xs-3 well">
                <ul class="nav nav-pills nav-stacked displaystyle" role="navigation">
                    <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
                        <ItemTemplate>
                            <li>
                                <h3>
                                    <%# Eval("Name") %>
                                </h3>
                                <ul class="nav nav-pills nav-stacked">
                                    <asp:Repeater ID="rptPages" runat="server" OnItemDataBound="rptPages_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltListStart" runat="server"></asp:Literal>
                                            <asp:LinkButton ID="btnPageNav" runat="server" CommandName="link"
                                                    OnCommand="btn_Command"></asp:LinkButton>
                                            <asp:Literal ID="ltListEnd" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:Repeater>                    
                                </ul>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlEdit" runat="server" Visible="false" CssClass="edit">
        <ol class="breadcrumb">
            <li>Edit Page</li>
            <li class="active"><asp:Literal ID="ltPageNameEdit" runat="server"></asp:Literal></li>
        </ol>
        <asp:HiddenField ID="hdnPageName" runat="server" />
        <div class="clearfix">
                <asp:Label ID="lbPageName" runat="server" Text="Page Name" 
                    AssociatedControlID="txtPageName" CssClass="col-xs-3 uape-label"></asp:Label>
            <asp:TextBox ID="txtPageName" runat="server" CssClass="col-xs-9 displaystyle"></asp:TextBox>          
        </div>        
        <asp:TextBox ID="txtMarkDown" runat="server" TextMode="MultiLine" CssClass="col-xs-12 displaystyle"></asp:TextBox>
    </asp:Panel>

    <asp:Panel ID="pnlNew" runat="server" Visible="false" CssClass="new">
        <ol class="breadcrumb">
            <li>Create a New Wiki Page</li>
        </ol>        
        <div class="clearfix">
            <asp:Label ID="lbNewCategory" runat="server" Text="Category" 
                AssociatedControlID="ddlNewCategory" CssClass="col-xs-3 uape-label"></asp:Label>
            <asp:DropDownList ID="ddlNewCategory" runat="server" CssClass="col-xs-9 displaystyle">
            </asp:DropDownList>
        </div>
        <div class="clearfix smallspace">
            <asp:Label ID="lbNewPageName" runat="server" Text="Page Name" 
                AssociatedControlID="txtNewPageTitle" CssClass="col-xs-3 uape-label"></asp:Label>
            <asp:TextBox ID="txtNewPageTitle" runat="server" CssClass="col-xs-9 displaystyle"></asp:TextBox>  
        </div>
        <asp:TextBox ID="txtNewMarkDown" runat="server" TextMode="MultiLine" CssClass="col-xs-12 displaystyle"></asp:TextBox>
    </asp:Panel>

    <asp:Panel ID="pnlCategoryEditor" runat="server" Visible="false" CssClass="cateditor">
        <ol class="breadcrumb">
            <li class="active">Category Editor</li>
        </ol>        
        <div class="col-xs-9">
            <asp:Panel ID="pnlCreateCategory" runat="server">
                <h3>Create Category</h3>

                <div class="clearfix">
                    <asp:Label ID="lbNewCatName" runat="server" Text="Category Name" 
                        AssociatedControlID="txtNewCatName" CssClass="col-xs-3 uape-label"></asp:Label>
                    <asp:TextBox ID="txtNewCatName" runat="server" CssClass="col-xs-8 displaystyle"></asp:TextBox>
                </div>

                <div class="clearfix space">
                    <div class="col-xs-11 right">
                        <asp:Button ID="btnNewCategory" runat="server" Text="Create Category" CssClass="btn btn-success"
                            CommandName="createcat" OnCommand="btn_Command"  />
                    </div>
                </div>
                <hr />
            </asp:Panel>

            <asp:Panel ID="pnlDeleteCategory" runat="server">
                <h3>Delete Category</h3>

                <div class="clearfix">
                    <asp:Label ID="lbDelCatName" runat="server" Text="Category Name"
                        AssociatedControlID="ddlDelCatName" CssClass="col-xs-3 uape-label"></asp:Label>
                    <asp:DropDownList ID="ddlDelCatName" runat="server" CssClass="col-xs-8 displaystyle">
                    </asp:DropDownList>
                </div>

                <div class="clearfix space">
                    <div class="col-xs-11 right">
                        <button class="btn btn-danger" onclick="return deleteCategory()">Delete Category</button>
                    </div>
                </div>
                <div class="modal fade" id="delCatModal" tabindex="-1" role="dialog" aria-labelledby="delCatModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title" id="delCatModalTitle">Delete Category</h4>
                            </div>
                            <div class="modal-body">
                              <strong>WARNING:</strong>
                              This will also permanently delete all child pages.
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnDeleteCategory" runat="server" Text="DELETE" CssClass="btn btn-danger"
                                CommandName="deletecat" OnCommand="btn_Command"  />
                            </div>
                        </div><!-- /.modal-content -->
                    </div><!-- /.modal-dialog -->
              </div><!-- /.modal -->                
            </asp:Panel>
        </div>
        <div class="col-xs-3 well whiteback">
                <h3>Display Order</h3>
                
                <ul class="categorylist" id="categorylist">
                <asp:Repeater ID="rptCatEditorList" runat="server" OnItemDataBound="rptCatEditorList_ItemDataBound">
                    <ItemTemplate>
                        <li>
                            <strong><%# Eval("Name") %></strong>
                            <ol class="pagelist">
                                <asp:Repeater ID="rptPagesEditor" runat="server" OnItemDataBound="rptPagesEditor_ItemDataBound">
                                    <ItemTemplate>
                                    <li>                                        
                                        <span><asp:Literal ID="ltPageName" runat="server"></asp:Literal></span>
                                    </li>
                                    </ItemTemplate>
                                </asp:Repeater>                    
                            </ol>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>

            <div class="center">
                <asp:Button ID="btnSaveOrder" runat="server" Text="Save Display Order" CssClass="btn btn-primary"
                     CommandName="savecatorder" OnCommand="btn_Command" OnClientClick="return saveCatOrder()"/>
            </div>
            <asp:HiddenField ID="hdnDisplayOrder" runat="server" />
        </div>
    </asp:Panel>
</div>

<div class="modal fade" id="delPageModal" tabindex="-1" role="dialog" aria-labelledby="delPageModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="delPageModalTitle">Delete Page</h4>
            </div>
            <div class="modal-body">
                <strong>WARNING:</strong>
                This will permanently delete the page, all content will be lost.
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            <asp:Button ID="btnDeletePage" runat="server" Text="DELETE" CssClass="btn btn-danger"
                CommandName="deletepage" OnCommand="btn_Command"  />
            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div><!-- /.modal -->                

<script type="text/javascript" src="Plugins/BackOfficeWiki/js/bootstrap.min.js"></script>
<script type="text/javascript" src="Plugins/BackOfficeWiki/js/jquery-sortable.min.js"></script>
<script type="text/javascript" src="Plugins/BackOfficeWiki/js/wiki.min.js"></script>
