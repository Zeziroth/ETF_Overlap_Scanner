using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace ETF_Overlap_Scanner
{

    public partial class Form1 : Form
    {
        List<ETF> etfs = new List<ETF>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ETF newETF = null;
            string isin = textBox1.Text;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    newETF = new IShares(isin);
                    break;

                case 1:
                    newETF = new Xtrackers(isin);
                    break;
            }
            etfs.Add(newETF);
            TreeNode node = new TreeNode($"{isin} ({newETF.GetIsinList().Count})");

            foreach (Holding holding in newETF.Holdings)
            {
                node.Nodes.Add($"{holding.ISIN} ({holding.Weight}%) ({holding.Name})");
            }

            treeView1.Nodes.Add(node);

            CompareETFs();
        }

        public void CompareETFs()
        {
            dynamic dupes = null;

            if (etfs.Count > 1)
            {
                dupes = etfs[0].Holdings.Where(h => etfs[1].Holdings.Count(h2 => h2.ISIN == h.ISIN) > 0);

                treeView2.Nodes.Clear();
                foreach (dynamic dupe in dupes)
                {
                    treeView2.Nodes.Add($"{dupe.ISIN} ({dupe.Weight}% + {etfs[1].Holdings.Where(h => h.ISIN == dupe.ISIN).First().Weight}%) ({dupe.Name})");
                }
            }

            label1.Text = $"In diesen ETFs sind {treeView2.Nodes.Count} Doppelungen enthalten.";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
