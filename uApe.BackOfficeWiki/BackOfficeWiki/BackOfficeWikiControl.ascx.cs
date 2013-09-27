using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using umbraco.uicontrols;
using umbraco;
using MarkdownSharp;

namespace uApe.BackOfficeWiki
{
    public enum DisplayMode
    {
        Display,
        Edit,
        New,
        CategoryEditor
    }

    public partial class BackOfficeWikiControl : System.Web.UI.UserControl
    {
        public const String IconEditPath = "/plugins/backofficewiki/images/edit.png";
        public const String IconEditTitle = "Edit Page";

        public const String IconBackPath = "/plugins/backofficewiki/images/back.png";
        public const String IconBackTitle = "Back";

        public const String IconSavePath = "/plugins/backofficewiki/images/save.png";
        public const String IconSaveTitle = "Save Page";

        public const String IconDeletePath = "/plugins/backofficewiki/images/delete.png";
        public const String IconDeleteTitle = "Delete Page";

        public const String IconSplitterPath = "/images/editor/separator.gif";

        public const String IconNewPagePath = "/plugins/backofficewiki/images/new.png";
        public const String IconNewPageTitle = "New Page";

        public const String IconCategoriesPath = "/plugins/backofficewiki/images/cats.png";
        public const String IconCategoriesTitle = "Category Editor";

        MenuImageButton btnEditAndBack = null;
        MenuImageButton btnSave = null;
        MenuImageButton btnDelete = null;
        Image splitter = null;
        MenuImageButton btnNewPage = null;
        MenuImageButton btnCategoryEditor = null;

        protected override void OnInit(EventArgs e)
        {
            TabPage tab = (TabPage)Parent.Parent.Parent;

            btnEditAndBack = tab.Menu.NewImageButton();
            btnEditAndBack.ImageUrl = GlobalSettings.Path + IconEditPath;
            btnEditAndBack.Command += new CommandEventHandler(btn_Command);
            btnEditAndBack.ToolTip = IconEditTitle;
            btnEditAndBack.CommandName = "edit";

            btnSave = tab.Menu.NewImageButton();
            btnSave.ImageUrl = GlobalSettings.Path + IconSavePath;
            btnSave.Command += new CommandEventHandler(btn_Command);
            btnSave.ToolTip = IconSaveTitle;
            btnSave.CommandName = "save";
            btnSave.Visible = false;

            btnDelete = tab.Menu.NewImageButton();
            btnDelete.ImageUrl = GlobalSettings.Path + IconDeletePath;
            btnDelete.Command += new CommandEventHandler(btn_Command);
            btnDelete.ToolTip = IconDeleteTitle;
            btnDelete.CommandName = "deletepage";
            btnDelete.OnClientClick = "return deletePage()";
            btnDelete.Visible = false;

            splitter = new Image();
            splitter.ImageUrl = GlobalSettings.Path + IconSplitterPath;
            splitter.Visible = false;
            tab.Menu.InsertNewControl(splitter);

            btnNewPage = tab.Menu.NewImageButton();
            btnNewPage.ImageUrl = GlobalSettings.Path + IconNewPagePath;
            btnNewPage.Command += new CommandEventHandler(btn_Command);
            btnNewPage.ToolTip = IconNewPageTitle;
            btnNewPage.CommandName = "newpage";
            btnNewPage.Visible = false;

            btnCategoryEditor = tab.Menu.NewImageButton();
            btnCategoryEditor.ImageUrl = GlobalSettings.Path + IconCategoriesPath;
            btnCategoryEditor.Command += new CommandEventHandler(btn_Command);
            btnCategoryEditor.ToolTip = IconCategoriesTitle;
            btnCategoryEditor.CommandName = "cats";
            btnCategoryEditor.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlError.Visible = false;
            Page.Form.Attributes.Add("autocomplete", "off");

            if (!Page.IsPostBack && !Page.IsCallback)
            {
                ltPageName.Text = String.Empty;
                showMode(DisplayMode.Display);
            }
        }

        protected void btn_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "edit":
                    showMode(DisplayMode.Edit);
                    break;
                case "close":
                    if (hdnMode.Value != "Edit")
                        showMode(DisplayMode.Edit);
                    else
                        showMode(DisplayMode.Display);
                    break;
                case "save":
                    if (hdnMode.Value == "Edit")
                    {
                        if (String.IsNullOrEmpty(txtPageName.Text))
                        {
                            ltError.Text = "The page must have a name.";
                            pnlError.Visible = true;
                            return;
                        }

                        WikiPage page = new WikiPage();
                        page.Name = ltPageName.Text;
                        page.MarkDown = txtMarkDown.Text;
                        page.Save(txtPageName.Text);
                        ltPageName.Text = txtPageName.Text;

                        showMode(DisplayMode.Display);
                    }
                    else if (hdnMode.Value == "New Page")
                    {
                        if (String.IsNullOrEmpty(ddlNewCategory.SelectedValue))
                        {
                            ltError.Text = "The page must belong to a category.";
                            pnlError.Visible = true;
                            return;
                        }
                        if (String.IsNullOrEmpty(txtNewPageTitle.Text))
                        {
                            ltError.Text = "The page must have a name.";
                            pnlError.Visible = true;
                            return;
                        }

                        WikiPage page = new WikiPage();
                        page.Name = txtNewPageTitle.Text;
                        page.MarkDown = txtNewMarkDown.Text;
                        page.MakeNew(ddlNewCategory.SelectedValue);
                        ltPageName.Text = txtNewPageTitle.Text;

                        showMode(DisplayMode.Display);
                    }
                    break;
                case "newpage":
                    showMode(DisplayMode.New);
                    bindCategories();
                    break;
                case "cats":
                    showMode(DisplayMode.CategoryEditor);
                    break;
                case "link":
                    ltPageName.Text = e.CommandArgument.ToString();
                    showMode(DisplayMode.Display);
                    break;
                case "createcat":
                    if (String.IsNullOrEmpty(txtNewCatName.Text))
                    {
                        ltError.Text = "The new category must have a name.";
                        pnlError.Visible = true;
                        return;
                    }

                    var newCat = new WikiPageCategory();
                    newCat.Name = txtNewCatName.Text;
                    newCat.MakeNew();

                    showMode(DisplayMode.CategoryEditor);
                    break;
                case "deletecat":
                    if (String.IsNullOrEmpty(ddlDelCatName.SelectedValue))
                    {
                        ltError.Text = "You must select a category to delete.";
                        pnlError.Visible = true;
                        return;
                    }

                    var delCat = new WikiPageCategory();
                    delCat.Name = ddlDelCatName.SelectedValue;
                    delCat.Delete();

                    showMode(DisplayMode.CategoryEditor);
                    break;
                case "savecatorder":
                    String[] catOrder = hdnDisplayOrder.Value.Split(new String[] { "|,|" }, StringSplitOptions.RemoveEmptyEntries);

                    var reorderCat = new WikiPageCategory();
                    reorderCat.ReOrder(catOrder);

                    showMode(DisplayMode.CategoryEditor);
                    break;
                case "deletepage":
                    var delPage = new WikiPage();
                    delPage.Name = hdnPageName.Value;
                    delPage.Delete();

                    showMode(DisplayMode.CategoryEditor);
                    break;
            }
        }

        protected void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var rptPages = (Repeater)e.Item.FindControl("rptPages");
            rptPages.DataSource = DataBinder.Eval(e.Item.DataItem, "Pages");
            rptPages.DataBind();
        }

        protected void rptCatEditorList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var rptPagesEditor = (Repeater)e.Item.FindControl("rptPagesEditor");
            rptPagesEditor.DataSource = DataBinder.Eval(e.Item.DataItem, "Pages");
            rptPagesEditor.DataBind();
        }

        protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ltListStart = (Literal)e.Item.FindControl("ltListStart");
            if (DataBinder.Eval(e.Item.DataItem, "Name").ToString() == ltPageName.Text)
                ltListStart.Text = "<li class=\"active\">";
            else
                ltListStart.Text = "<li>";

            var ltListEnd = (Literal)e.Item.FindControl("ltListEnd");
            ltListEnd.Text = "</li>";

            var btnPageNav = (LinkButton)e.Item.FindControl("btnPageNav");
            btnPageNav.Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString();
            btnPageNav.CommandArgument = DataBinder.Eval(e.Item.DataItem, "Name").ToString();
        }

        protected void rptPagesEditor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var ltPageName = (Literal)e.Item.FindControl("ltPageName");
            ltPageName.Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString();
        }

        private void showMode(DisplayMode mode)
        {
            pnlEdit.Visible = false;
            pnlNew.Visible = false;
            pnlCategoryEditor.Visible = false;
            pnlDisplay.Visible = false;
            txtNewCatName.Text = String.Empty;

            umbraco.BusinessLogic.User currUser = umbraco.BusinessLogic.User.GetCurrent();

            switch (mode)
            {
                case DisplayMode.Display:
                    pnlDisplay.Visible = true;

                    btnEditAndBack.ImageUrl = GlobalSettings.Path + IconEditPath;
                    btnEditAndBack.ToolTip = IconEditTitle;
                    btnEditAndBack.CommandName = "edit";

                    var editPerms = BackOfficeWikiConfig.GetRolesFor(BackOfficePermissionType.EditPage);
                    if (editPerms.Contains(currUser.UserType.Name))
                        btnEditAndBack.Visible = true;
                    else
                        btnEditAndBack.Visible = false;

                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    splitter.Visible = false;
                    btnNewPage.Visible = false;
                    btnCategoryEditor.Visible = false;

                    hdnMode.Value = "Display";

                    loadData(ltPageName.Text);
                    break;
                case DisplayMode.Edit:

                    pnlEdit.Visible = true;

                    btnEditAndBack.ImageUrl = GlobalSettings.Path + IconBackPath;
                    btnEditAndBack.ToolTip = IconBackTitle;
                    btnEditAndBack.CommandName = "close";
                    btnEditAndBack.Visible = true;

                    var savePerms = BackOfficeWikiConfig.GetRolesFor(BackOfficePermissionType.SavePage);
                    if (savePerms.Contains(currUser.UserType.Name))
                        btnSave.Visible = true;
                    else
                        btnSave.Visible = false;

                    var delPerms = BackOfficeWikiConfig.GetRolesFor(BackOfficePermissionType.DeletePage);
                    if (delPerms.Contains(currUser.UserType.Name))
                        btnDelete.Visible = true;
                    else
                        btnDelete.Visible = false;

                    var newPagePerms = BackOfficeWikiConfig.GetRolesFor(BackOfficePermissionType.NewPage);
                    if (newPagePerms.Contains(currUser.UserType.Name))
                        btnNewPage.Visible = true;
                    else
                        btnNewPage.Visible = false;

                    var catEditorPerms = BackOfficeWikiConfig.GetRolesFor(BackOfficePermissionType.CategoryEditor);
                    if (catEditorPerms.Contains(currUser.UserType.Name))
                        btnCategoryEditor.Visible = true;
                    else
                        btnCategoryEditor.Visible = false;

                    if (btnNewPage.Visible || btnCategoryEditor.Visible)
                        splitter.Visible = true;
                    else
                        splitter.Visible = false;

                    hdnMode.Value = "Edit";

                    txtMarkDown.Text = loadRaw(ltPageName.Text);
                    txtPageName.Text = ltPageName.Text;
                    hdnPageName.Value = ltPageName.Text;
                    break;
                case DisplayMode.New:
                    pnlNew.Visible = true;

                    btnSave.Visible = true;
                    btnDelete.Visible = false;
                    splitter.Visible = false;
                    btnNewPage.Visible = false;
                    btnCategoryEditor.Visible = false;

                    hdnMode.Value = "New Page";

                    break;
                case DisplayMode.CategoryEditor:
                    pnlCategoryEditor.Visible = true;

                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    splitter.Visible = false;
                    btnNewPage.Visible = false;
                    btnCategoryEditor.Visible = false;

                    hdnMode.Value = "Category Editor";

                    var createPerms = BackOfficeWikiConfig.GetRolesFor(BackOfficePermissionType.CreateCategory);
                    if (createPerms.Contains(currUser.UserType.Name))
                        pnlCreateCategory.Visible = true;
                    else
                        pnlCreateCategory.Visible = false;

                    var deletePerms = BackOfficeWikiConfig.GetRolesFor(BackOfficePermissionType.DeleteCategory);
                    if (deletePerms.Contains(currUser.UserType.Name))
                        pnlDeleteCategory.Visible = true;
                    else
                        pnlDeleteCategory.Visible = false;

                    bindCategories();

                    break;
            }
        }

        private void loadData(String pageName)
        {
            Markdown markDown = new Markdown();
            String pageMarkDown = loadRaw(pageName);
            if (!String.IsNullOrEmpty(pageMarkDown))
                pageMarkDown = markDown.Transform(pageMarkDown);

            phContent.Controls.Add(ParseControl(pageMarkDown));

            var cats = new WikiPageCategory();
            bindCategories();

            cats.LoadByPage(ltPageName.Text);
            ltCategory.Text = cats.Name;
        }

        private String loadRaw(String pageName)
        {
            WikiPage page = new WikiPage();
            if (page.GetPage(pageName))
                ltPageName.Text = page.Name;

            return page.MarkDown;
        }

        private void bindCategories()
        {
            var cats = new WikiPageCategory();
            var data = cats.Load();

            rptCategories.DataSource = data;
            rptCategories.DataBind();

            rptCatEditorList.DataSource = data;
            rptCatEditorList.DataBind();

            ddlNewCategory.Items.Clear();
            ddlNewCategory.Items.Add(new ListItem("[ select ]", ""));
            ddlNewCategory.AppendDataBoundItems = true;

            ddlNewCategory.DataSource = data;
            ddlNewCategory.DataValueField = "Name";
            ddlNewCategory.DataTextField = "Name";
            ddlNewCategory.DataBind();

            ddlDelCatName.Items.Clear();
            ddlDelCatName.Items.Add(new ListItem("[ select ]", ""));
            ddlDelCatName.AppendDataBoundItems = true;

            ddlDelCatName.DataSource = data;
            ddlDelCatName.DataValueField = "Name";
            ddlDelCatName.DataTextField = "Name";
            ddlDelCatName.DataBind();
        }
    }
}