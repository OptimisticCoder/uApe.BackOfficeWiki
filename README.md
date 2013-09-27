![alt text](https://uape.co.uk/img/uape-wiki.jpg "uApe Ecommerce")

Welcome to BackOffice Wiki
==========================

This package was developed out of a requirement to allow back office users to share information and processes.

The package can also be useful for Umbraco developers to leave instructions for the back office content editors.
There are no additional SQL tables added. The Wiki only uses XML files.

Wiki page content is written in [Markdown](http://en.wikipedia.com/wiki/Markdown).

---

Dependencies
------------

Transformation from Markdown to Html is done using [MarkdownSharp](https://code.google.com/p/markdownsharp/).

Styles and Modal dialogs are from [Twitter Bootstrap](http://getbootstrap.com/).

Draggable sortable lists are using [jQuery Sortable](http://johnny.github.io/jquery-sortable/).

Everything required is included in the Umbraco package installation.

---

Installation
------------

The package includes a 'package action script' which executes when it is installed to configure 
itself automatically.

The package will automatically add ...

* A *backOfficeWiki* configuration section to the Umbraco web.config
* A *backOfficeWiki* section to the bottom of the Umbraco web.config
* A *tab* element to the Config/Dashboard.config file

If there are any problems with uninstallation, removing the above elements will return Umbraco
to it's previous state.

The package adds a new configuration file called Config/backofficewiki.config where permissions
can be set to restrict actions of back office users based on their 'user type'.

---

Screen Grabs
------------

    
    
#### Display Mode

Display mode is for showing the rendered Wiki content, the Edit button in the toolbar can be controlled
with permissions in the BackOfficeWiki.config file.

![alt text](https://uape.co.uk/img/wiki-display-mode.jpg "uApe BackOffice Wiki - Display Mode")

---
    
    
#### Edit Mode

Edit mode allows manipulation of Wiki pages using Markdown, as used by Github and Stack Overflow, and is
again restrictable using permissions set up in the config file.

![alt text](https://uape.co.uk/img/wiki-edit-mode.jpg "uApe BackOffice Wiki - Edit Mode")

---
    
    
#### Category Editor

The Category Editor allows re-ordering of categories by dragging, adding new categories and deleting old ones, 
functionality can be restricted again with permissions.

![alt text](https://uape.co.uk/img/wiki-category-editor.jpg "uApe BackOffice Wiki - Category Editor")

---

Copyright &copy; 2013 uApe Ecommerce Limited. All rights reserved.