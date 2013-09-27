<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BackOfficeWikiControl.ascx.cs" Inherits="uApe.BackOfficeWiki.BackOfficeWikiControl" %>
<%@ Register Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" TagPrefix="CD" %>

<CD:CssInclude ID="CssInclude1" runat="server" FilePath="~/umbraco/Plugins/BackOfficeWiki/css/bootstrap.min.css" />
<CD:CssInclude ID="CssInclude2" runat="server" FilePath="~/umbraco/Plugins/BackOfficeWiki/css/bootstrap-theme.min.css" />
<CD:CssInclude ID="CssInclude3" runat="server" FilePath="~/umbraco/Plugins/BackOfficeWiki/css/wiki-ui.min.css" />

<script type="text/javascript">
    var ddlDelCatName = '<%=ddlDelCatName.ClientID %>';
    var hdnDisplayOrder = '<%=hdnDisplayOrder.ClientID %>';
    var hdnPageName = '<%=hdnPageName.ClientID %>';
</script>

<asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-danger alert-dismissable">
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        <strong>Error!</strong>     
        <asp:Literal ID="ltError" runat="server"></asp:Literal>    
</asp:Panel>

<div class="wiki">   
    <asp:HiddenField ID="hdnMode" runat="server" />
    <asp:Panel ID="pnlDisplay" runat="server">
            <ol class="breadcrumb">
                <li><asp:Literal ID="ltCategory" runat="server"></asp:Literal></li>
                <li class="active"><asp:Literal ID="ltPageName" runat="server"></asp:Literal></li>
            </ol> 
            <div class="display col-xs-9">
                <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
            </div>
            <div class="col-xs-3 well">
                <ul class="nav nav-pills nav-stacked" role="navigation">
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
            </div>
    </asp:Panel>

    <asp:Panel ID="pnlEdit" runat="server" Visible="false" CssClass="edit">
        <ol class="breadcrumb">
            <li class="active">Edit Page</li>
        </ol>        
        <asp:Label ID="lbPageName" runat="server" Text="Page Name" AssociatedControlID="txtPageName"></asp:Label>
        <asp:HiddenField ID="hdnPageName" runat="server" />
        <asp:TextBox ID="txtPageName" runat="server"></asp:TextBox>  
        <asp:TextBox ID="txtMarkDown" runat="server" TextMode="MultiLine"></asp:TextBox>
    </asp:Panel>

    <asp:Panel ID="pnlNew" runat="server" Visible="false" CssClass="new">
        <ol class="breadcrumb">
            <li class="active">New Page</li>
        </ol>        
        <div>
            <asp:Label ID="lbNewCategory" runat="server" Text="Category" AssociatedControlID="ddlNewCategory"></asp:Label>
            <asp:DropDownList ID="ddlNewCategory" runat="server">
            </asp:DropDownList>
        </div>
        <div>
            <asp:Label ID="lbNewPageName" runat="server" Text="Page Name" AssociatedControlID="txtNewPageTitle"></asp:Label>
            <asp:TextBox ID="txtNewPageTitle" runat="server"></asp:TextBox>  
        </div>
        <asp:TextBox ID="txtNewMarkDown" runat="server" TextMode="MultiLine"></asp:TextBox>
    </asp:Panel>

    <asp:Panel ID="pnlCategoryEditor" runat="server" Visible="false" CssClass="cateditor">
        <ol class="breadcrumb">
            <li class="active">Category Editor</li>
        </ol>        
        <div class="display col-xs-9">
            <asp:Panel ID="pnlCreateCategory" runat="server">
                <h3>Create Category</h3>

                <asp:Label ID="lbNewCatName" runat="server" Text="Category Name" AssociatedControlID="txtNewCatName"></asp:Label>
                <asp:TextBox ID="txtNewCatName" runat="server"></asp:TextBox>

                <div class="btndiv">
                    <asp:Button ID="btnNewCategory" runat="server" Text="Create Category" CssClass="btn btn-success"
                        CommandName="createcat" OnCommand="btn_Command"  />
                </div>
                <hr />
            </asp:Panel>

            <asp:Panel ID="pnlDeleteCategory" runat="server">
                <h3>Delete Category</h3>

                <asp:Label ID="lbDelCatName" runat="server" Text="Category Name" AssociatedControlID="ddlDelCatName"></asp:Label>
                <asp:DropDownList ID="ddlDelCatName" runat="server">
                </asp:DropDownList>

                <div class="btndiv">
                    <button class="btn btn-danger" onclick="return deleteCategory()">Delete Category</button>
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
        <div class="col-xs-3 well">
            <ul id="categorylist" class="nav nav-pills nav-stacked">
                <asp:Repeater ID="rptCatEditorList" runat="server" OnItemDataBound="rptCatEditorList_ItemDataBound">
                    <ItemTemplate>
                        <li class="placeholder">
                            <div class="cathead">
                                <div>
                                    <span>
                                        <img src="Plugins/BackOfficeWiki/images/icon_cursor.png" alt="" class="drag-handle" />
                                    </span>
                                    <h5>
                                        <%# Eval("Name") %>
                                    </h5>
                                </div>
                            </div>
                            <ul class="nav nav-pills nav-stacked cateditorpages">
                                <asp:Repeater ID="rptPagesEditor" runat="server" OnItemDataBound="rptPagesEditor_ItemDataBound">
                                    <ItemTemplate>
                                    <li>
                                        <span><asp:Literal ID="ltPageName" runat="server"></asp:Literal></span>
                                    </li>
                                    </ItemTemplate>
                                </asp:Repeater>                    
                            </ul>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>

            <div class="saveorder">
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
<script type="text/javascript" src="Plugins/BackOfficeWiki/js/wiki.min.js"></script>
<script type="text/javascript" src="Plugins/BackOfficeWiki/js/jquery.sortable.js"></script>
