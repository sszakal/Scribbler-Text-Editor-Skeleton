using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Operations.Search
{
    public partial class ReplaceDialog : Form
    {
        public ReplaceDialog()
        {
            InitializeComponent();
        }

        internal string SearchTerm
        {
            get
            {
                return textBoxFind.Text;
            }
        }

        internal string ReplaceTerm
        {
            get
            {
                return textBoxReplace.Text;
            }
        }
    }
}
