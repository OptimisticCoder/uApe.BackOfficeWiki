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

Copyright &copy; 2013 uApe Ecommerce Limited. All rights reserved.