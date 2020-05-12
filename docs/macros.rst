
======
Macros
======

Macros are placeholders that can be used in all locations where StaxRip allows to customize a command line or script.


Interactive Macros
------------------

Interactive macros can be used in certain menus like the filter profiles menu.

.. csv-table::
    :header: "Name", "Description"
    :widths: auto

    "$browse_file$","Filepath returned from a file browser."
    "$enter_text$","Text entered in a input box."
    "$enter_text:prompt$","Text entered in a input box."
    "$select:param1;param2;...$","String selected from dropdown, to show a optional message the first parameter has to start with msg: and to give the items optional captions use caption|value. Example: $select:msg:hello;caption1|value1;caption2|value2$"


All Macros
----------

.. include:: generated/macro-table.rst
