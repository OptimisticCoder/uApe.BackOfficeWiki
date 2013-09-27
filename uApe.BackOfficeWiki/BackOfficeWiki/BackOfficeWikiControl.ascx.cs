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
    /// <summary>
    /// Enumeration to represent the current display mode of the Wiki.
    /// </summary>
    public enum DisplayMode
    {
        Display,
        Edit,
        New,
        CategoryEditor
    }

    /// <summary>
    /// The main user control class.
    /// </summary>
    public partial class BackOfficeWikiControl : System.Web.UI.UserControl
    {
        // Constants for consistent icons and text in the toolbar.
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

        // Private toolbar elements
        private MenuImageButton btnEditAndBack = null;
        private MenuImageButton btnSave = null;
        private MenuImageButton btnDelete = null;
        private Image splitter = null;
        private MenuImageButton btnNewPage = null;
        private MenuImageButton btnCategoryEditor = null;

        /// <summary>
        /// The first method to be executed in this class, responsible for initializing the toolbar.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            // Get a reference to the Wiki tab.
            TabPage tab = (TabPage)Parent.Parent.Parent;

            // Add the multi-purpose "Edit" and "Back" button.
            btnEditAndBack = tab.Menu.NewImageButton();
            btnEditAndBack.ImageUrl = GlobalSettings.Path + IconEditPath;
            btnEditAndBack.Command += new CommandEventHandler(btn_Command);
            btnEditAndBack.ToolTip = IconEditTitle;
            btnEditAndBack.CommandName = "edit";

            // Add and hide the "Save" button.
            btnSave = tab.Menu.NewImageButton();
            btnSave.ImageUrl = GlobalSettings.Path + IconSavePath;
            btnSave.Command += new CommandEventHandler(btn_Command);
            btnSave.ToolTip = IconSaveTitle;
            btnSave.CommandName = "save";
            btnSave.Visible = false;

            // Add and hide the "Delete" button.
            btnDelete = tab.Menu.NewImageButton();
            btnDelete.ImageUrl = GlobalSettings.Path + IconDeletePath;
            btnDelete.Command += new CommandEventHandler(btn_Command);
            btnDelete.ToolTip = IconDeleteTitle;
            btnDelete.CommandName = "deletepage";
            btnDelete.OnClientClick = "return deletePage()";
            btnDelete.Visible = false;

            // Add the splitter to seperate Save/Delete and New Page/Category Editor on Edit screen.
            splitter = new Image();
            splitter.ImageUrl = GlobalSettings.Path + IconSplitterPath;
            splitter.Visible = false;
            tab.Menu.InsertNewControl(splitter);

            // Add and hide the "New Page" button.
            btnNewPage = tab.Menu.NewImageButton();
            btnNewPage.ImageUrl = GlobalSettings.Path + IconNewPagePath;
            btnNewPage.Command += new CommandEventHandler(btn_Command);
            btnNewPage.ToolTip = IconNewPageTitle;
            btnNewPage.CommandName = "newpage";
            btnNewPage.Visible = false;

            // Add and hide the "Category Editor" button.
            btnCategoryEditor = tab.Menu.NewImageButton();
            btnCategoryEditor.ImageUrl = GlobalSettings.Path + IconCategoriesPath;
            btnCategoryEditor.Command += new CommandEventHandler(btn_Command);
            btnCategoryEditor.ToolTip = IconCategoriesTitle;
            btnCategoryEditor.CommandName = "cats";
            btnCategoryEditor.Visible = false;
        }

        /// <summary>
        /// Page load method fires after the Init method and before Button events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Hide the error panel, if it is visible.
            pnlError.Visible = false;

            // Add autocomplete attribute to the page's form, to keep the UI clean.
            Page.Form.Attributes.Add("autocomplete", "off");

            // If this is a postback, the button events handle data loading because they fire later.
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                // If not a postback, load the default Wiki page by specifying an empty page name.
                ltPageName.Text = String.Empty;
                showMode(DisplayMode.Display);
            }
        }

        /// <summary>
        /// The generic button "Command" event, fired by most UI buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Command(object sender, CommandEventArgs e)
        {
            // Work out which button fired the event.
            switch (e.CommandName)
            {
                case "edit":
                    // Edit was clicked from Display mode, so show Edit screen.
                    showMode(DisplayMode.Edit);
                    break;
                case "close":
                    if (hdnMode.Value != "Edit")
                        // Back was clicked from New Page, or Cat Editor mode, so show Edit screen.
                        showMode(DisplayMode.Edit);
                    else
                        // Back was clicked from Edit page, so show Display screen.
                        showMode(DisplayMode.Display);
                    break;
                case "save":
                    // One of the Save buttons was clicked.
                    if (hdnMode.Value == "Edit")
                    {
                        // We're in Edit mode, check the Wiki page has a name.
                        if (String.IsNullOrEmpty(txtPageName.Text))
                        {
                            ltError.Text = "The page must have a name.";
                            pnlError.Visible = true;
                            return;
                        }

                        // Save the page data to the XML file.
                        WikiPage page = new WikiPage();
                        page.Name = ltPageName.Text;
                        page.MarkDown = txtMarkDown.Text;
                        page.Save(txtPageName.Text);

                        // Set the current page name.
                        ltPageName.Text = txtPageName.Text;

                        // Load the Display screen.
                        showMode(DisplayMode.Display);
                    }
                    else if (hdnMode.Value == "New Page")
                    {
                        // We're in New Page mode, check the Wiki page has a name, and category.
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

                        // Save the page data to the XML file.
                        WikiPage page = new WikiPage();
                        page.Name = txtNewPageTitle.Text;
                        page.MarkDown = txtNewMarkDown.Text;
                        page.MakeNew(ddlNewCategory.SelectedValue);

                        // Set the current page name.
                        ltPageName.Text = txtNewPageTitle.Text;

                        // Load the Display screen.
                        showMode(DisplayMode.Display);
                    }
                    break;
                case "newpage":
                    // The New Page button was clicked, so load the screen.
                    showMode(DisplayMode.New);
                    bindCategories();
                    break;
                case "cats":
                    // The Category Editor button was clicked, so load the screen.
                    showMode(DisplayMode.CategoryEditor);
                    break;
                case "link":
                    // An internal Wiki link was clicked in Display mode, so load the new Wiki page.
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

                    // The page we were working on has gone, so reset to the default.
                    ltPageName.Text = String.Empty;

                    showMode(DisplayMode.Display);
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