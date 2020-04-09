using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulace {
    public partial class SettingsForm : Form {
        public int speed = 3;
        public Virus[] v = new Virus[0];
        public bool walks;
        public float workRate;

        public SettingsForm() {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            Int32.TryParse(textBox1.Text, out speed);
            Single.TryParse(textBox2.Text, out workRate);
            walks = checkBox1.Checked;
            v = new Virus[dataGridView1.RowCount - 1];
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                if (Int32.TryParse((string)row.Cells[1].Value, out int ip))
                if (Single.TryParse((string)row.Cells[2].Value, out float drs))
                if (Single.TryParse((string)row.Cells[3].Value, out float dre))
                if (Int32.TryParse((string)row.Cells[4].Value, out int it))
                if (Int32.TryParse((string)row.Cells[5].Value, out int trn))
                if (Int32.TryParse((string)row.Cells[6].Value, out int trt))
                if (Int32.TryParse((string)row.Cells[7].Value, out int tr))
                if (Single.TryParse((string)row.Cells[8].Value, out float lic))
                    v[i] = new Virus(ip, drs, dre, it, trn, trt, tr,
                        lic, (string)row.Cells[0].Value);
                i++;
            }
        }
    }
}
