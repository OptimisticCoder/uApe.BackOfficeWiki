using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uApe.BackOfficeWiki
{
    public class WikiPage : WikiData
    {
        public String Name { get; set; }
        public String MarkDown { get; set; }

        public Boolean GetFirst()
        {
            var page = getFirst();
            if (page != null)
            {
                Name = page.Attribute("name").Value;
                MarkDown = page.Value;

                return true;
            }
            else
                return false;
        }

        public Boolean GetPage(String name)
        {
            var page = getPage(name);
            if (page != null)
            {
                Name = page.Attribute("name").Value;
                MarkDown = page.Value;

                return true;
            }
            else
                return false;
        }

        public void Save(String newName)
        {
            save(Name, MarkDown, newName);
        }

        public void Delete()
        {
            deletePage(Name);
        }

        public Boolean MakeNew(String category)
        {
            makeNewPage(Name, MarkDown, category);
            return true;
        }
    }
}