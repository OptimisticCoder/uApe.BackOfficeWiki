using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.Xml.Linq;
using System.Configuration;

namespace uApe.BackOfficeWiki
{
    public class WikiData
    {
        protected void save(String pageName, String markDown, String newName)
        {
            var xml = XDocument.Load(getXmlPath());

            using (XmlWriter writer = XmlWriter.Create(getXmlPath()))
            {
                // Root XML element
                writer.WriteStartDocument();
                writer.WriteWhitespace("\n");
                writer.WriteStartElement("Wiki");
                writer.WriteWhitespace("\n    ");

                // Loop through existing categories and pages, writing them to the new file.
                foreach (var c in xml.Descendants("Category"))
                {
                    writer.WriteStartElement("Category");
                    writer.WriteAttributeString("name", c.Attribute("name").Value);
                    foreach (var p in c.Descendants("Page"))
                    {
                        writer.WriteWhitespace("\n        ");
                        writer.WriteStartElement("Page");
                        if (p.Attribute("name").Value == pageName)
                        {
                            writer.WriteAttributeString("name", newName);
                            writer.WriteValue(markDown);
                        }
                        else
                        {
                            writer.WriteAttributeString("name", p.Attribute("name").Value);
                            writer.WriteValue(p.Value);
                        }
                        writer.WriteEndElement();
                        writer.WriteWhitespace("\n    ");
                    }
                    writer.WriteEndElement();
                }

                writer.WriteWhitespace("\n    ");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
                writer.WriteEndDocument();
            }
        }

        protected void makeNewPage(String pageName, String markDown, String category)
        {
            var xml = XDocument.Load(getXmlPath());

            using (XmlWriter writer = XmlWriter.Create(getXmlPath()))
            {
                // Root XML element
                writer.WriteStartDocument();
                writer.WriteWhitespace("\n");
                writer.WriteStartElement("Wiki");
                writer.WriteWhitespace("\n    ");

                // Loop through existing categories and pages, writing them to the new file.
                foreach (var c in xml.Descendants("Category"))
                {
                    writer.WriteStartElement("Category");
                    writer.WriteAttributeString("name", c.Attribute("name").Value);
                    foreach (var p in c.Descendants("Page"))
                    {
                        writer.WriteWhitespace("\n        ");
                        writer.WriteStartElement("Page");
                        writer.WriteAttributeString("name", p.Attribute("name").Value);
                        writer.WriteValue(p.Value);
                        writer.WriteEndElement();
                        writer.WriteWhitespace("\n    ");
                    }
                    if (c.Attribute("name").Value == category)
                    {
                        writer.WriteWhitespace("\n        ");
                        writer.WriteStartElement("Page");
                        writer.WriteAttributeString("name", pageName);
                        writer.WriteValue(markDown);
                        writer.WriteEndElement();
                        writer.WriteWhitespace("\n    ");
                    }
                    writer.WriteEndElement();
                }

                writer.WriteWhitespace("\n    ");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
                writer.WriteEndDocument();
            }
        }

        protected void makeNewCategory(String category)
        {
            var xml = XDocument.Load(getXmlPath());

            using (XmlWriter writer = XmlWriter.Create(getXmlPath()))
            {
                // Root XML element
                writer.WriteStartDocument();
                writer.WriteWhitespace("\n");
                writer.WriteStartElement("Wiki");
                writer.WriteWhitespace("\n    ");

                // Loop through existing categories and pages, writing them to the new file.
                foreach (var c in xml.Descendants("Category"))
                {
                    writer.WriteStartElement("Category");
                    writer.WriteAttributeString("name", c.Attribute("name").Value);
                    foreach (var p in c.Descendants("Page"))
                    {
                        writer.WriteWhitespace("\n        ");
                        writer.WriteStartElement("Page");
                        writer.WriteAttributeString("name", p.Attribute("name").Value);
                        writer.WriteValue(p.Value);
                        writer.WriteEndElement();
                        writer.WriteWhitespace("\n    ");
                    }
                    writer.WriteEndElement();
                }
                writer.WriteStartElement("Category");
                writer.WriteAttributeString("name", category);
                writer.WriteEndElement();

                writer.WriteWhitespace("\n    ");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
                writer.WriteEndDocument();
            }
        }

        protected void deleteCategory(String category)
        {
            var xml = XDocument.Load(getXmlPath());

            using (XmlWriter writer = XmlWriter.Create(getXmlPath()))
            {
                // Root XML element
                writer.WriteStartDocument();
                writer.WriteWhitespace("\n");
                writer.WriteStartElement("Wiki");
                writer.WriteWhitespace("\n    ");

                // Loop through existing categories and pages, writing them to the new file.
                foreach (var c in xml.Descendants("Category"))
                {
                    if (c.Attribute("name").Value != category)
                    {
                        writer.WriteStartElement("Category");
                        writer.WriteAttributeString("name", c.Attribute("name").Value);
                        foreach (var p in c.Descendants("Page"))
                        {
                            writer.WriteWhitespace("\n        ");
                            writer.WriteStartElement("Page");
                            writer.WriteAttributeString("name", p.Attribute("name").Value);
                            writer.WriteValue(p.Value);
                            writer.WriteEndElement();
                            writer.WriteWhitespace("\n    ");
                        }
                        writer.WriteEndElement();
                    }
                }

                writer.WriteWhitespace("\n    ");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
                writer.WriteEndDocument();
            }
        }

        protected void reOrderCategory(String[] categoryNames)
        {
            var xml = XDocument.Load(getXmlPath());

            using (XmlWriter writer = XmlWriter.Create(getXmlPath()))
            {
                // Root XML element
                writer.WriteStartDocument();
                writer.WriteWhitespace("\n");
                writer.WriteStartElement("Wiki");
                writer.WriteWhitespace("\n    ");

                // Loop through existing categories and pages, writing them to the new file.
                foreach (var currName in categoryNames)
                {
                    var c = getCategoryByName(xml, currName);
                    writer.WriteStartElement("Category");
                    writer.WriteAttributeString("name", c.Attribute("name").Value);
                    foreach (var p in c.Descendants("Page"))
                    {
                        writer.WriteWhitespace("\n        ");
                        writer.WriteStartElement("Page");
                        writer.WriteAttributeString("name", p.Attribute("name").Value);
                        writer.WriteValue(p.Value);
                        writer.WriteEndElement();
                        writer.WriteWhitespace("\n    ");
                    }
                    writer.WriteEndElement();
                }

                writer.WriteWhitespace("\n    ");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
                writer.WriteEndDocument();
            }
        }

        protected void deletePage(String pageName)
        {
            var xml = XDocument.Load(getXmlPath());

            using (XmlWriter writer = XmlWriter.Create(getXmlPath()))
            {
                // Root XML element
                writer.WriteStartDocument();
                writer.WriteWhitespace("\n");
                writer.WriteStartElement("Wiki");
                writer.WriteWhitespace("\n    ");

                // Loop through existing categories and pages, writing them to the new file.
                foreach (var c in xml.Descendants("Category"))
                {
                        writer.WriteStartElement("Category");
                        writer.WriteAttributeString("name", c.Attribute("name").Value);
                        foreach (var p in c.Descendants("Page"))
                        {
                            if (p.Attribute("name").Value != pageName)
                            {
                                writer.WriteWhitespace("\n        ");
                                writer.WriteStartElement("Page");
                                writer.WriteAttributeString("name", p.Attribute("name").Value);
                                writer.WriteValue(p.Value);
                                writer.WriteEndElement();
                                writer.WriteWhitespace("\n    ");
                            }
                        }
                        writer.WriteEndElement();
                }

                writer.WriteWhitespace("\n    ");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
                writer.WriteEndDocument();
            }
        }

        protected XElement[] getCategories()
        {
            var xml = XDocument.Load(getXmlPath());
            return (from a in xml.Descendants("Category")
                    select a).ToArray();
        }

        protected XElement getCategoryByName(XDocument xml, String categoryName)
        {
            return (from a in xml.Descendants("Category")
                    where a.Attribute("name").Value == categoryName
                    select a).FirstOrDefault();
        }

        protected XElement getCategoryByPage(String pageName)
        {
            var xml = XDocument.Load(getXmlPath());
            return (from a in xml.Descendants("Page")
                    where a.Attribute("name").Value == pageName
                    select a.Parent).FirstOrDefault();
        }

        protected XElement[] getPages(XElement src)
        {
            return (from a in src.Descendants("Page")
                    select a).ToArray();
        }

        protected XElement getFirst()
        {
            var xml = XDocument.Load(getXmlPath());
            return (from a in xml.Descendants("Page")
                    select a).FirstOrDefault();
        }

        protected XElement getPage(String name)
        {
            if (String.IsNullOrEmpty(name))
                return getFirst();
            else
            {
                var xml = XDocument.Load(getXmlPath());
                return (from a in xml.Descendants("Page")
                        where a.Attribute("name").Value == name
                        select a).FirstOrDefault();
            }
        }

        private String getXmlPath()
        {
            BackOfficeWikiConfig config =
                    (BackOfficeWikiConfig)System.Configuration.ConfigurationManager.GetSection("backOfficeWiki");

            String dataPath = config.XmlDataPath.Path;
            if (dataPath.StartsWith("~/"))
                dataPath = HttpContext.Current.Server.MapPath(dataPath);

            return dataPath;
        }
    }
}