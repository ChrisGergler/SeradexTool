using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SeradexToolv2.ViewModels
{
    class LinqSystem
    {
        DataSet linqSet;
        DataTable linqTable;
        DataView linqView;

        public void initialization(DataSet set, DataTable table, DataView view)
        {
            linqSet = set;
            linqTable = table;
            linqView = view;
        }







    }
}
