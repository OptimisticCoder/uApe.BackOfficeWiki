using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace uApe.BackOfficeWiki
{
    public enum BackOfficePermissionType
    {
        EditPage,
        SavePage,
        DeletePage,
        NewPage,
        CategoryEditor,
        CreateCategory,
        DeleteCategory
    }

    public class BackOfficeWikiConfig : ConfigurationSection
    {
        public static IEnumerable<String> GetRolesFor(BackOfficePermissionType permType)
        {
            BackOfficeWikiConfig config =
                    (BackOfficeWikiConfig)System.Configuration.ConfigurationManager.GetSection("backOfficeWiki");

            switch(permType)
            {
                case BackOfficePermissionType.EditPage:
                    return config.Permissions.EditPage.Roles.Split(',');
                case BackOfficePermissionType.SavePage:
                    return config.Permissions.SavePage.Roles.Split(',');
                case BackOfficePermissionType.DeletePage:
                    return config.Permissions.DeletePage.Roles.Split(',');
                case BackOfficePermissionType.NewPage:
                    return config.Permissions.NewPage.Roles.Split(',');
                case BackOfficePermissionType.CategoryEditor:
                    return config.Permissions.CategoryEditor.Roles.Split(',');
                case BackOfficePermissionType.CreateCategory:
                    return config.Permissions.CreateCategory.Roles.Split(',');
                case BackOfficePermissionType.DeleteCategory:
                    return config.Permissions.DeleteCategory.Roles.Split(',');
            }
            return null;
        }

        [ConfigurationProperty("XmlDataPath")]
        public DataPathElement XmlDataPath
        {
            get
            {
                return (DataPathElement)this["XmlDataPath"];
            }
            set
            { this["XmlDataPath"] = value; }
        }

        [ConfigurationProperty("Menu")]
        public IsEnabledElement Menu
        {
            get
            {
                return (IsEnabledElement)this["Menu"];
            }
            set
            { this["Menu"] = value; }
        }

        [ConfigurationProperty("Breadcrumbs")]
        public IsEnabledElement Breadcrumbs
        {
            get
            {
                return (IsEnabledElement)this["Breadcrumbs"];
            }
            set
            { this["Breadcrumbs"] = value; }
        }

        [ConfigurationProperty("Permissions")]
        public PermissionsElement Permissions
        {
            get
            {
                return (PermissionsElement)this["Permissions"];
            }
            set
            { this["Permissions"] = value; }
        }
    }

    public class DataPathElement : ConfigurationElement
    {
        [ConfigurationProperty("path", DefaultValue = "~/App_Data/backofficewiki.config", IsRequired = true)]
        [StringValidator(MinLength = 1)]
        public String Path
        {
            get
            {
                return (String)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }

    public class IsEnabledElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
        public Boolean Enabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }

    public class PermissionsElement : ConfigurationElement
    {
        [ConfigurationProperty("EditPage")]
        public PermissionRoleElement EditPage
        {
            get
            {
                return (PermissionRoleElement)this["EditPage"];
            }
            set
            {
                this["EditPage"] = value;
            }
        }

        [ConfigurationProperty("SavePage")]
        public PermissionRoleElement SavePage
        {
            get
            {
                return (PermissionRoleElement)this["SavePage"];
            }
            set
            {
                this["SavePage"] = value;
            }
        }

        [ConfigurationProperty("DeletePage")]
        public PermissionRoleElement DeletePage
        {
            get
            {
                return (PermissionRoleElement)this["DeletePage"];
            }
            set
            {
                this["DeletePage"] = value;
            }
        }

        [ConfigurationProperty("NewPage")]
        public PermissionRoleElement NewPage
        {
            get
            {
                return (PermissionRoleElement)this["NewPage"];
            }
            set
            {
                this["NewPage"] = value;
            }
        }

        [ConfigurationProperty("CategoryEditor")]
        public PermissionRoleElement CategoryEditor
        {
            get
            {
                return (PermissionRoleElement)this["CategoryEditor"];
            }
            set
            {
                this["CategoryEditor"] = value;
            }
        }

        [ConfigurationProperty("CreateCategory")]
        public PermissionRoleElement CreateCategory
        {
            get
            {
                return (PermissionRoleElement)this["CreateCategory"];
            }
            set
            {
                this["CreateCategory"] = value;
            }
        }

        [ConfigurationProperty("DeleteCategory")]
        public PermissionRoleElement DeleteCategory
        {
            get
            {
                return (PermissionRoleElement)this["DeleteCategory"];
            }
            set
            {
                this["DeleteCategory"] = value;
            }
        }
    }

    public class PermissionRoleElement : ConfigurationElement
    {
        [ConfigurationProperty("roles", DefaultValue = "Administrator", IsRequired = false)]
        public String Roles
        {
            get
            {
                return (String)this["roles"];
            }
            set
            {
                this["roles"] = value;
            }
        }
    }
}