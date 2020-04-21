namespace Simulace {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.VName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.incubation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deathrates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deathratee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.illTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trneed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trtop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LongImunityChance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(53, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "3";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VName,
            this.incubation,
            this.deathrates,
            this.deathratee,
            this.illTime,
            this.trneed,
            this.trtop,
            this.tr,
            this.LongImunityChance});
            this.dataGridView1.Location = new System.Drawing.Point(13, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(795, 388);
            this.dataGridView1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 443);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "speed";
            this.toolTip1.SetToolTip(this.label1, "Speed in minutes per tick");
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(159, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(88, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "enable walks";
            this.toolTip1.SetToolTip(this.checkBox1, "if enabled, non-working on work days and all on weekend, will go on walks");
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "work rate";
            this.toolTip1.SetToolTip(this.label2, "how many citizens will be working");
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(311, 13);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "0.98";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
            // 
            // VName
            // 
            this.VName.HeaderText = "Virus name";
            this.VName.Name = "VName";
            this.VName.ToolTipText = "name of virus; don\'t use duplicate names";
            // 
            // incubation
            // 
            this.incubation.HeaderText = "Incubation Period";
            this.incubation.MinimumWidth = 4;
            this.incubation.Name = "incubation";
            this.incubation.ToolTipText = "incubation period of virus (time before infected citizen will go to hospital and " +
    "deaths on infection)";
            this.incubation.Width = 70;
            // 
            // deathrates
            // 
            this.deathrates.HeaderText = "Death rate start";
            this.deathrates.MinimumWidth = 4;
            this.deathrates.Name = "deathrates";
            this.deathrates.ToolTipText = "death rate each day when infected, after incubation period, to half of infection " +
    "time";
            this.deathrates.Width = 70;
            // 
            // deathratee
            // 
            this.deathratee.HeaderText = "Death rate end";
            this.deathratee.MinimumWidth = 4;
            this.deathratee.Name = "deathratee";
            this.deathratee.ToolTipText = "death rate each day when infected, after incubation period, after half of infecti" +
    "on time";
            this.deathratee.Width = 70;
            // 
            // illTime
            // 
            this.illTime.HeaderText = "Duration";
            this.illTime.MinimumWidth = 3;
            this.illTime.Name = "illTime";
            this.illTime.ToolTipText = "how long the infection lasts, includes incubation period";
            this.illTime.Width = 50;
            // 
            // trneed
            // 
            this.trneed.HeaderText = "Transmission rate need";
            this.trneed.MinimumWidth = 3;
            this.trneed.Name = "trneed";
            this.trneed.ToolTipText = "how many contacts with infected one a citizen need to have a chance to be infecte" +
    "d";
            this.trneed.Width = 70;
            // 
            // trtop
            // 
            this.trtop.HeaderText = "Transmission rate top";
            this.trtop.MinimumWidth = 4;
            this.trtop.Name = "trtop";
            this.trtop.ToolTipText = "how many contacts with infected one a citizen need to have a 100% chance to be in" +
    "fected";
            this.trtop.Width = 70;
            // 
            // tr
            // 
            this.tr.HeaderText = "Transmission rate";
            this.tr.MinimumWidth = 4;
            this.tr.Name = "tr";
            this.tr.ToolTipText = "transmision rate: if citizen get infected is computed: RandomNumTo(TRtop - contac" +
    "ts) < TR";
            this.tr.Width = 70;
            // 
            // LongImunityChance
            // 
            this.LongImunityChance.HeaderText = "Long Imunity Chance";
            this.LongImunityChance.Name = "LongImunityChance";
            this.LongImunityChance.ToolTipText = "Chance to be completly imune to infection after recovering";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 475);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn VName;
        private System.Windows.Forms.DataGridViewTextBoxColumn incubation;
        private System.Windows.Forms.DataGridViewTextBoxColumn deathrates;
        private System.Windows.Forms.DataGridViewTextBoxColumn deathratee;
        private System.Windows.Forms.DataGridViewTextBoxColumn illTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn trneed;
        private System.Windows.Forms.DataGridViewTextBoxColumn trtop;
        private System.Windows.Forms.DataGridViewTextBoxColumn tr;
        private System.Windows.Forms.DataGridViewTextBoxColumn LongImunityChance;
    }
}