using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

using umbraco.interfaces;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.BusinessLogic;

namespace uApe.BackOfficeWiki
{
    public class BackOfficeWikiAction : IPackageAction
    {
        public const String ConfigSectionName = "backOfficeWiki";

        public String Alias()
        {
            return "BackOfficeWikiAction";
        }

        public Boolean Execute(string packageName, XmlNode xmlData)
        {
            String webConfigPath = HttpContext.Current.Server.MapPath("~/web.config");
            String dashboardConfigPath = HttpContext.Current.Server.MapPath("~/Config/dashboard.config");

            XDocument xml = null;
            // Load the web.config file
            try
            {
                xml = XDocument.Load(webConfigPath);
            }
            catch (Exception)
            {
                return false;
            }

            // Get configSections from the web.config file
            var configSections = xml.Descendants("configSections")
                                 .FirstOrDefault();

            // Does the section exist?
            if (configSections.Descendants("section")
                              .Where(s => s.Attribute("name").Value == ConfigSectionName)
                              .Count() == 0)
            {
                // No, so add it.
                XElement section = new XElement("section");
                section.Add(new XAttribute("name", "backOfficeWiki"));
                section.Add(new XAttribute("type", "uApe.BackOfficeWiki.BackOfficeWikiConfig"));
                section.Add(new XAttribute("allowLocation", "true"));
                section.Add(new XAttribute("allowDefinition", "Everywhere"));
                configSections.Add(section);
            }

            // Get the backOfficeWiki section from the web.config file
            var confRoot = xml.Descendants("configuration")
                                 .FirstOrDefault();

            // Does the section exist?
            if (confRoot.Descendants(ConfigSectionName)
                        .Count() == 0)
            {
                // No, so add it.
                XElement wSection = new XElement(ConfigSectionName);
                wSection.Add(new XAttribute("configSource", @"Config\backofficewiki.config"));
                confRoot.Add(wSection);
            }
            xml.Save(webConfigPath);

            // --------------------------------------------------------
            // Load the dashboard.config file
            try
            {
                xml = XDocument.Load(dashboardConfigPath);
            }
            catch (Exception)
            {
                return false;
            }

            // Does the tab exist?
            if (xml.Descendants("tab")
                   .Where(t => t.Attribute("caption").Value == "Wiki")
                   .Count() == 0)
            {
                // No, so add it.
                XElement tab = new XElement("tab");
                tab.Add(new XAttribute("caption", "Wiki"));

                XElement control = new XElement("control");
                control.Add(new XAttribute("showOnce", "false"));
                control.Add(new XAttribute("addPanel", "true"));
                control.Add(new XAttribute("panelCaption", ""));
                control.Value = "/umbraco/plugins/BackOfficeWiki/BackOfficeWikiControl.ascx";
                tab.Add(control);

                xml.Descendants("section")
                   .Where(s => s.Attribute("alias").Value == "StartupDashboardSection")
                   .FirstOrDefault()
                   .AddFirst(tab);
                xml.Save(dashboardConfigPath);
            }

            return true;
        }

        public System.Xml.XmlNode SampleXml()
        {
            String sample = "<Action runat=\"install\" undo=\"true\" alias=\"BackOfficeWikiAction\" /></Action>";
            return helper.parseStringToXmlNode(sample);
        }

        public Boolean Undo(String packageName, XmlNode xmlData)
        {
            String webConfigPath = HttpContext.Current.Server.MapPath("~/web.config");
            String dashboardConfigPath = HttpContext.Current.Server.MapPath("~/Config/dashboard.config");

            XDocument xml = null;
            // Load the web.config file
            try
            {
                xml = XDocument.Load(webConfigPath);
            }
            catch (Exception)
            {
                return false;
            }

            // Get configSections from the web.config file
            var configSections = xml.Descendants("configSections")
                                 .FirstOrDefault();

            // Does the section exist?
            if (configSections.Descendants("section")
                              .Where(s => s.Attribute("name").Value == ConfigSectionName)
                              .Count() > 0)
            {
                // Yes, delete it
                configSections.Descendants("section")
                              .Where(s => s.Attribute("name").Value == ConfigSectionName)
                              .FirstOrDefault()
                              .Remove();
            }

            // Get the backOfficeWiki section from the web.config file
            var confRoot = xml.Descendants("configuration")
                                 .FirstOrDefault();

            // Does the section exist?
            if (confRoot.Descendants(ConfigSectionName)
                        .Count() > 0)
            {
                // Yes delete it.
                confRoot.Descendants(ConfigSectionName).FirstOrDefault().Remove();
            }
            xml.Save(webConfigPath);

            // --------------------------------------------------------
            // Load the dashboard.config file
            try
            {
                xml = XDocument.Load(dashboardConfigPath);
            }
            catch (Exception)
            {
                return false;
            }

            // Does the tab exist?
            if (xml.Descendants("tab")
                   .Where(t => t.Attribute("caption").Value == "Wiki")
                   .Count() > 0)
            {
                // Yes delete it.
                xml.Descendants("tab")
                   .Where(t => t.Attribute("caption").Value == "Wiki")
                   .FirstOrDefault()
                   .Remove();
                xml.Save(dashboardConfigPath);
            }

            return true;
        }
    }
}