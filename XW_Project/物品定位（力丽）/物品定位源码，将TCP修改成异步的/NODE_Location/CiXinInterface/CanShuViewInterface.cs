using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CiXinLocation.CiXinInterface
{
    public interface CanShuViewInterface
    {

        /// <summary>
        /// listView删除Item操作
        /// </summary>
        /// <param name="sourLView"></param>
        /// <param name="deleItem"></param>
        void deleteListViewItem(ListView sourLView,ListViewItem deleItem);

        void textBoxChange(Control con,string text);

        void refash(RefashList reList,bool isRefesh);
    }

    public enum RefashList
    {
        CENGJILIST = 1,
        QUYULIST = 2,
        CANKAODIANLIST = 3,
        CARDLIST = 4,
    }
}
