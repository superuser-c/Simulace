using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Simulace {
    class InfectionData {
        public Virus virus; // virus - ubsahuje statistiky viru
        public bool infected; // je nakazeny?
        public int infectContact; // jak dlouho stykal se s nakazenym?
        public int infectTime; // jak dlouho je nakazeny?
        public bool recovered; // uzdravil se?
        public bool longImunity;

        public void Infect() {
            infected = true;
        }

        public void AddContact() {
            infectContact++;
        }

        public void RemoveContacts() {
            infectContact /= 50;
        }

        public InfectionData Clone() {
            return new InfectionData() {
                virus = virus.Clone(),
                infected = infected,
                infectContact = infectContact,
                infectTime = infectTime,
                recovered = recovered,
                longImunity = longImunity
            };
        }
    }

    // obsahuje chovani obyvatel
    // obyvatele chodi do prace a o vikendu na prochazky
    class Citizen {
        private static Random random = new Random();
        
        public bool alive;
        public Dictionary<string, InfectionData> ifd;
        public Point pos; // pozice
        private Point home; // bydliste
        private Point work; // prace
        private Point favouritePlace; // mista kam chodi na prochazky
        // cas -> 10 = 0:10, 20 = 0:20, 400 = 6:40
        private uint wakeTime; // kdy vstava
        private uint sleepTime; // kdy usina

        public Citizen(Point h, Point w, 
            Dictionary<string, InfectionData> ifd) {
            this.ifd = ifd;
            alive = true;
            home = h;
            work = w;
            pos = home;
            uint workdist = (uint)Distance(work) + 3;
            wakeTime = 510 - workdist;
            sleepTime = 990 + workdist;
            favouritePlace = new Point(home.X + random.Next(51) - 25,
                    home.Y + random.Next(51) - 25);
        }

        public List<Virus> Infections() {
            List<Virus> o = new List<Virus>();
            foreach (var i in ifd) {
                if (i.Value.infected)
                    o.Add(i.Value.virus);
            }
            return o;
        }

        public bool Infected() {
            foreach (var i in ifd) {
                if (i.Value.infected)
                    return true;
            }
            return false;
        }

        public bool InfectedWithSymptoms() {
            foreach (var i in ifd) {
                if (i.Value.infected && 
                    i.Value.infectTime - i.Value.virus.incubationPeriod >= 0)
                    return true;
            }
            return false;
        }

        public bool FullImune() {
            foreach (var i in ifd) {
                if (!i.Value.longImunity)
                    return false;
            }
            return true;
        }

        public int Distance(Point target) {
            return Math.Abs(pos.X - target.X) + Math.Abs(pos.Y - target.Y);
        }

        private void GoAt(Point target) {
            Point dir = new Point();
            if (pos.X < target.X)
                dir.X = 1;
            else if (pos.X > target.X)
                dir.X = -1;
            if (pos.Y < target.Y)
                dir.Y = 1;
            else if (pos.Y > target.Y)
                dir.Y = -1;
            if (dir.X != 0) {
                if (dir.Y != 0) {
                    if (random.Next(2) == 0)
                        pos.Y += dir.Y;
                    else
                        pos.X += dir.X;
                } else
                    pos.X += dir.X;
            } else if (dir.Y != 0) {
                pos.Y += dir.Y;
            }
        }

        private void DoWork() {
            if (Distance(work) < 3)
                Wander();
            else
                GoAt(work);
        }

        private void Wander() {
            int move = random.Next(4);
            switch (move) {
                case 0:
                    pos.X++;
                    break;
                case 1:
                    pos.X--;
                    break;
                case 2:
                    pos.Y++;
                    break;
                case 3:
                    pos.Y--;
                    break;
                default:
                    break;
            }
        }

        public void Tick(uint daytime, bool workday, bool walks) {
            if (alive) {
                if (InfectedWithSymptoms()) {
                    if (daytime < wakeTime || daytime > sleepTime)
                        return;
                    GoAt(new Point(random.Next(12, 20), random.Next(12, 20)));
                    return;
                }
                if (workday) {
                    if (daytime < wakeTime || daytime > sleepTime)
                        return; // spi, nic nedela
                    if (daytime < 510) // do 8:30 cesta do prace
                        GoAt(work);
                    else if (daytime < 990) // v praci do 16:30
                        DoWork();
                    else // od 16:30 cesta domu
                        GoAt(home);
                } else {
                    if (daytime < wakeTime || daytime > 1200) // spi od 20:00
                        return; // spi, nic nedela
                    if (daytime < 840) // do 14:00 odpocinek
                        return;
                    else if (daytime < 1020 && walks) { // do 17:00 prochazeni
                        if (random.Next(2) == 0)
                            Wander();
                        else
                            GoAt(favouritePlace);
                    } else // vrati se z prochazky (do 20:00)
                        GoAt(home);
                }
            }
        }
    }
}
