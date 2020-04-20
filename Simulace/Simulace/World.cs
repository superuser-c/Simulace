using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulace {
    class World : PictureBox {
        private static Random random = new Random();
        public static readonly int worldSize = 100;
        public List<Citizen> population;
        private List<Point> workPlaces;
        private Bitmap[] buffers = new Bitmap[2];
        private int backBuffI = 0;
        public Graphics g;
        public uint daytime = 0;
        public uint day = 1;
        private int scale = 4;
        private int topOffset = 64;
        public Dictionary<string, List<int>> totalInfected = 
            new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> totalDead = 
            new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> totalImune = 
            new Dictionary<string, List<int>>();
        private Dictionary<string, int> infected = new Dictionary<string, int>();
        private Dictionary<string, int> dead = new Dictionary<string, int>();
        private Dictionary<string, int> imune = new Dictionary<string, int>();
        public Virus[] viruses;
        public int timestep;
        public bool walks;
        public List<int> ttDead = new List<int>();
        public int tDead = 0;
        public List<int> ttIf = new List<int>();
        public int tIf = 0;
        public List<int> ttIm = new List<int>();
        public int tIm = 0;
        public float workRate;
        public int rendMode = 0;

        public World() {
            Dock = DockStyle.Fill;
            Click += new EventHandler(SwitchCitRend);
        }

        private void SwitchCitRend(object s, EventArgs e) {
            rendMode = (rendMode + 1) % 3;
        }

        public void Init(Form f) {
            population = new List<Citizen>();
            workPlaces = new List<Point>();
            Dictionary<string, InfectionData> ifd =
                new Dictionary<string, InfectionData>();
            foreach (Virus v in viruses) {
                ifd.Add(v.name, new InfectionData() {
                    virus = v,
                    infectContact = 0,
                    infected = false,
                    infectTime = 0,
                    recovered = false
                });
                totalInfected.Add(v.name, new List<int>());
                totalDead.Add(v.name, new List<int>());
                totalImune.Add(v.name, new List<int>());
                infected.Add(v.name, 0);
                dead.Add(v.name, 0);
                imune.Add(v.name, 0);
            }
            int i;
            for (i = 0; i < 32; i++) { // 32 praci
                workPlaces.Add(new Point(random.Next(worldSize),
                    random.Next(worldSize)));
            }
            for (i = 0; i < 1000; i++) { // 1000 obyvatel
                population.Add(CreateCitizen(ifd));
                if (i < viruses.Length * 2) {
                    population[i].ifd[viruses[i / 2].name].Infect();
                    infected[viruses[i / 2].name]++;
                    tIf++;
                }
            }
            buffers[0] = new Bitmap(worldSize * scale + 800,
                worldSize * scale + topOffset);
            buffers[1] = new Bitmap(worldSize * scale + 800,
                worldSize * scale + topOffset);
            SwapBuffers();
            f.ClientSize = buffers[0].Size;
            foreach (Virus v in viruses) {
                totalInfected[v.name].Add(infected[v.name]);
                totalDead[v.name].Add(dead[v.name]);
                totalImune[v.name].Add(imune[v.name]);
            }
            ttDead.Add(tDead);
            ttIf.Add(tIf);
            ttIm.Add(tIm);
        }

        private Citizen CreateCitizen(Dictionary<string, InfectionData> ifd) {
            Dictionary<string, InfectionData> ifdc =
                new Dictionary<string, InfectionData>();
            foreach (var i in ifd) {
                ifdc.Add(i.Key, i.Value.Clone());
            }
            Point work = workPlaces[random.Next(workPlaces.Count)];
            Point home;
            do {
                home = new Point(work.X + random.Next(51) - 25,
                    work.Y + random.Next(51) - 25);
            } while (home.X < 0 || home.Y < 0 ||
                home.X >= worldSize || home.Y >= worldSize);
            return new Citizen(home, work, ifdc, workRate);
        }

        public void UpdateWorld() {
            daytime += (uint)timestep;
            if (daytime >= 1440) {
                daytime = 0;
                day++;
                foreach (Citizen c in population) {
                    TryInfect(c);
                }
                foreach (Virus v in viruses) {
                    totalInfected[v.name].Add(infected[v.name]);
                    totalDead[v.name].Add(dead[v.name]);
                    totalImune[v.name].Add(imune[v.name]);
                }
                ttDead.Add(tDead);
                ttIf.Add(tIf);
                ttIm.Add(tIm);
            }
            bool workday = !(day % 7 == 0 || day % 7 == 6);
            foreach (Citizen c in population) {
                if (!c.alive)
                    continue;
                c.Tick(daytime, workday, walks);
                if ((c.pos.X > 19 || c.pos.Y > 19 || c.pos.X < 12 || c.pos.Y < 12
                    || random.Next(50) == 0) && !c.FullImune())
                    Spread(c);
            }
        }

        private void Spread(Citizen c) {
            foreach (Citizen c2 in population) {
                if (c2.alive && !c.FullImune() && c.Distance(c2.pos) < 2) {
                    foreach (var inf in c2.Infections()) {
                        if (!c.ifd[inf.name].infected && 
                            !c.ifd[inf.name].longImunity)
                            c.ifd[inf.name].AddContact();
                    }
                }
            }
        }

        private void TryInfect(Citizen c) {
            if (!c.alive || c.FullImune())
                return;
            foreach (var i in c.ifd) {
                if (i.Value.infected) {
                    i.Value.infectTime++;
                    if (i.Value.infectTime > i.Value.virus.incubationPeriod) {
                        int inflen = i.Value.infectTime - 
                            i.Value.virus.incubationPeriod;
                        int fulliln = i.Value.virus.illnessTime -
                            i.Value.virus.incubationPeriod;
                        if (inflen < fulliln / 2) {
                            if (random.NextDouble() <
                                i.Value.virus.deathRateStart) {
                                tDead++;
                                dead[i.Value.virus.name]++;
                                tIf--;
                                foreach (Virus v in c.Infections()) {
                                    infected[v.name]--;
                                }
                                foreach (var id in c.ifd) {
                                    if (id.Value.longImunity)
                                        imune[id.Value.virus.name]--;
                                }
                                i.Value.infected = false;
                                c.alive = false;
                                return;
                            }
                        } else {
                            if (random.NextDouble() <
                                i.Value.virus.deathRateEnd) {
                                tDead++;
                                dead[i.Value.virus.name]++;
                                tIf--;
                                foreach (Virus v in c.Infections()) {
                                    infected[v.name]--;
                                }
                                foreach (var id in c.ifd) {
                                    if (id.Value.longImunity)
                                        imune[id.Value.virus.name]--;
                                }
                                i.Value.infected = false;
                                c.alive = false;
                                return;
                            }
                        }
                        if (i.Value.infectTime >= i.Value.virus.illnessTime) {
                            i.Value.recovered = true;
                            i.Value.infected = false;
                            i.Value.infectTime = 0;
                            i.Value.infectContact = 0;
                            infected[i.Value.virus.name]--;
                            if (!c.Infected())
                                tIf--;
                            if (random.NextDouble() <
                                i.Value.virus.longImunityChance) {
                                i.Value.longImunity = true;
                                imune[i.Value.virus.name]++;
                                if (c.FullImune())
                                    tIm++;
                            }
                        }
                    }
                }
                if (!i.Value.infected && 
                    i.Value.infectContact > i.Value.virus.TRNeed) {
                    if (i.Value.infectContact >= i.Value.virus.TRTop) {
                        if (!c.Infected())
                            tIf++;
                        i.Value.Infect();
                        infected[i.Value.virus.name]++;
                    } else if (random.Next(i.Value.virus.TRTop - 
                        i.Value.infectContact) < i.Value.virus.TR) {
                        if (!c.Infected())
                            tIf++;
                        i.Value.Infect();
                        infected[i.Value.virus.name]++;
                    }
                }
                i.Value.RemoveContacts();
            }
        }

        public void SwapBuffers() {
            if (g != null)
                g.Dispose();
            Image = buffers[backBuffI];
            backBuffI = backBuffI ^ 1;
            g = Graphics.FromImage(buffers[backBuffI]);
        }

        public void RenderInfo() {
            g.FillRectangle(Brushes.Silver, 0, 0, buffers[0].Width, topOffset);
            int x = scale * worldSize;
            g.FillRectangle(Brushes.Silver, x, 0, 
                buffers[0].Width - x, buffers[0].Height);
            g.DrawString("day " + day + " " + daytime / 60 + ":" + 
                (daytime % 60 < 10 ? "0" : "") + daytime % 60 + 
                "\ndead: " + tDead + "; infected: " + tIf + "; imune: " + tIm,
                new Font("Lucida", 16, FontStyle.Bold), Brushes.Black,
                new Point());
            if (ttIf.Count < 2) return;
            int y = 2, yPush = (buffers[0].Height - 2 - 
                                27 * viruses.Length) / viruses.Length;
            x += 2;
            foreach (Virus v in viruses) {
                g.DrawString(v.name + ":", new Font("Lucida", 16, FontStyle.Bold),
                    Brushes.Black, new Point(x, y));
                y += 25;
                RendGraph(v.name, x, y, 396, yPush);
                y += 2 + yPush;
            }
            float gl = x + 400;
            float gt = 2;
            float gw = 396;
            float gh = buffers[0].Height - 4;
            g.FillRectangle(Brushes.White, gl, gt, gw, gh);
            PointF[] pts = new PointF[ttIf.Count + 1];
            for (int i = 0; i < ttIf.Count; i++) {
                pts[i] = new PointF(gl + i * gw / (ttIf.Count - 1),
                    gt + gh - ttIf[i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt + gh);
            g.FillPolygon(Brushes.Red, pts);

            pts = new PointF[ttDead.Count + 1];
            for (int i = 0; i < ttDead.Count; i++) {
                pts[i] = new PointF(gl + i * gw / (ttDead.Count - 1),
                    gt + ttDead[i] * gh / population.Count +
                    ttIm[i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt);
            g.FillPolygon(Brushes.Black, pts);

            pts = new PointF[ttIm.Count + 1];
            for (int i = 0; i < ttIm.Count; i++) {
                pts[i] = new PointF(gl + i * gw / (ttIm.Count - 1),
                    gt + ttIm[i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt);
            g.FillPolygon(Brushes.Purple, pts);
        }

        private void RendGraph(string vNam, float gl,  float gt, 
            float gw = 200, float gh = 60) {
            g.FillRectangle(Brushes.White, gl, gt, gw, gh);

            PointF[] pts = new PointF[ttIf.Count + 1];
            for (int i = 0; i < ttIf.Count; i++) {
                pts[i] = new PointF(gl + i * gw / (ttIf.Count - 1),
                    gt + gh - ttIf[i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt + gh);
            g.FillPolygon(Brushes.LightSalmon, pts);

            pts = new PointF[totalInfected[vNam].Count + 1];
            for (int i = 0; i < totalInfected[vNam].Count; i++) {
                pts[i] = new PointF(gl + i * gw / (totalInfected[vNam].Count - 1),
                    gt + gh - totalInfected[vNam][i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt + gh);
            g.FillPolygon(Brushes.Red, pts);
            
            pts = new PointF[ttDead.Count + 1];
            for (int i = 0; i < ttDead.Count; i++) {
                pts[i] = new PointF(gl + i * gw / (ttDead.Count - 1),
                    gt + ttDead[i] * gh / population.Count +
                    totalImune[vNam][i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt);
            g.FillPolygon(Brushes.Gray, pts);
            
            pts = new PointF[totalDead[vNam].Count + 1];
            for (int i = 0; i < totalDead[vNam].Count; i++) {
                pts[i] = new PointF(gl + i * gw / (totalDead[vNam].Count - 1),
                    gt + totalDead[vNam][i] * gh / population.Count +
                    totalImune[vNam][i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt);
            g.FillPolygon(Brushes.Black, pts);

            pts = new PointF[totalImune[vNam].Count + 1];
            for (int i = 0; i < totalImune[vNam].Count; i++) {
                pts[i] = new PointF(gl + i * gw / (totalImune[vNam].Count - 1),
                    gt + totalImune[vNam][i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt);
            g.FillPolygon(Brushes.Purple, pts);

            pts = new PointF[ttIm.Count + 1];
            for (int i = 0; i < ttIm.Count; i++) {
                pts[i] = new PointF(gl + i * gw / (ttIm.Count - 1),
                    gt + ttIm[i] * gh / population.Count);
            }
            pts[pts.Length - 1] = new PointF(gl + gw, gt);
            g.FillPolygon(Brushes.Violet, pts);
        }

        public void RenderCitizens() {
            foreach (Point w in workPlaces) {
                g.FillEllipse(Brushes.LightGray,
                    w.X * scale, w.Y * scale + topOffset, scale, scale);
            }
            foreach (Citizen c in population) {
                g.FillEllipse(c.alive
                    ? (c.Infected() 
                        ? (c.InfectedWithSymptoms()
                            ? Brushes.Red
                            : Brushes.Orange)
                        : (c.FullImune()
                            ? Brushes.Purple
                            : Brushes.White)) 
                    : Brushes.Black,
                    c.pos.X * scale, c.pos.Y * scale + topOffset, scale, scale);
            }
        }
    }
}
