using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;

namespace uApe.BackOfficeWiki
{
    public class WikiPageCategory : WikiData
    {
        public String Name { get; set; }
        public List<WikiPage> Pages { get; set; }

        public void MakeNew()
        {
            makeNewCategory(this.Name);
        }

        public void Delete()
        {
            deleteCategory(this.Name);
        }

        public void ReOrder(dynamic categories)
        {
            reOrderCategory(categories);
        }

        public Boolean LoadByPage(String pageName)
        {
            var c = getCategoryByPage(pageName);     
            if(c == null)
                return false;

            Name = c.Attribute("name").Value;
            return true;
        }

        public IEnumerable<WikiPageCategory> Load()
        {
            List<WikiPageCategory> results = new List<WikiPageCategory>();
            var categories = getCategories();
            foreach (var c in categories)
            {
                results.Add(new WikiPageCategory
                {
                    Name = c.Attribute("name").Value,
                    Pages = (from p in getPages(c)
                             select new WikiPage
                             {
                                 Name = p.Attribute("name").Value,
                                 MarkDown = p.Value
                             }).ToList()
                });
            }
            return results;
        }
    }
}