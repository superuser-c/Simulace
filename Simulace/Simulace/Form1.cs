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
    public partial class Form1 : Form {
        World world;

        public Form1() {
            InitializeComponent();
            SettingsForm settings = new SettingsForm();
            settings.ShowDialog();
            world = new World();
            Controls.Add(world);
            world.viruses = settings.v;
            world.timestep = settings.speed;
            world.walks = settings.walks;
            world.workRate = settings.workRate;
            world.Init(this);
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e) {
            world.UpdateWorld();
            if (world.rendMode < 2) {
                world.g.Clear(world.daytime < 450 || world.daytime > 1200 ?
                    Color.Green : Color.Lime);
                if (world.rendMode == 0 && world.daytime > 450 && world.daytime < 1200)
                    world.RenderCitizens();
                world.RenderInfo();
                world.SwapBuffers();
            }
        }
    }
}
