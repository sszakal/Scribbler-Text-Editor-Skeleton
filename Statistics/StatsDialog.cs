using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Operations.Statistics
{
    public partial class StatsDialog : Form
    {
        #region Constructors
        public StatsDialog()
        {
            InitializeComponent();
        }

        public StatsDialog(int count)
        {
            InitializeComponent();
            labelCount.Text = count.ToString();
        }
        #endregion
        #region Properties
        public bool ShowCharacterCount
        {
            set
            {
                labelTitle.Text = "Characters in document:";
            }
        }

        public bool ShowWordCount
        {
            set
            {
                labelTitle.Text = "Words in document:";
            }
        }
        #endregion

    }
}
