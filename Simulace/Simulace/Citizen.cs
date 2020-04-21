using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Simulace {
    /// <summary>
    /// data o jednotlive infekci
    /// </summary>
    class InfectionData {
        /// <summary>
        /// statistiky viru
        /// </summary>
        public Virus virus;
        /// <summary>
        /// je nakazeny?
        /// </summary>
        public bool infected;
        /// <summary>
        /// kolik mel kontaktu s nakazenym?
        /// </summary>
        public int infectContact;
        /// <summary>
        /// jak dlouho uz je nakazeny?
        /// </summary>
        public int infectTime;
        /// <summary>
        /// uzdravil se?
        /// </summary>
        [Obsolete("this field is deprecated")]
        public bool recovered;
        /// <summary>
        /// ma obcan imunitu proti nemoci?
        /// </summary>
        public bool longImunity;

        /// <summary>
        /// nakazi obcana
        /// </summary>
        public void Infect() {
            infected = true;
        }

        /// <summary>
        /// prida kontakt s nakazenym
        /// </summary>
        public void AddContact() {
            infectContact++;
        }

        /// <summary>
        /// odstrani vestinu kontaktu s nakazenymi
        /// </summary>
        public void RemoveContacts() {
            infectContact /= 50;
        }

        /// <summary>
        /// zkopiruje objekt do noveho
        /// </summary>
        /// <returns>kopie objektu</returns>
        public InfectionData Clone() {
            return new InfectionData() {
                virus = virus.Clone(),
                infected = infected,
                infectContact = infectContact,
                infectTime = infectTime,
                longImunity = longImunity
            };
        }
    }
    
    /// <summary>
    /// obsahuje chovani obyvatel;
    /// obyvatele chodi do prace a o vikendu na prochazky
    /// </summary>
    class Citizen {
        private static Random random = new Random();
        
        /// <summary>
        /// je nazivu?
        /// </summary>
        public bool alive;
        /// <summary>
        /// data  o infekcich
        /// </summary>
        public Dictionary<string, InfectionData> ifd;
        /// <summary>
        /// pozice
        /// </summary>
        public Point pos;
        /// <summary>
        /// pozice domu
        /// </summary>
        private Point home;
        /// <summary>
        /// pozice prace
        /// </summary>
        private Point work;
        /// <summary>
        /// pracuje nebo jen chodi na prochazky
        /// false chodi na prochazky kazdy den
        /// </summary>
        private bool works;
        /// <summary>
        /// oblibene prochazkove misto,
        /// priblizne tam chodi
        /// </summary>
        private Point favouritePlace;
        // cas -> 10 = 0:10, 20 = 0:20, 400 = 6:40
        /// <summary>
        /// kdy vstava?
        /// </summary>
        private uint wakeTime;
        /// <summary>
        /// kdy usina?
        /// </summary>
        private uint sleepTime;

        public Citizen(Point h, Point w, 
            Dictionary<string, InfectionData> ifd, float workRate) {
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
            works = random.NextDouble() < workRate;
        }

        /// <summary>
        /// vrati vsechny viry, kterimi je nakazen
        /// </summary>
        /// <returns>list viru</returns>
        public List<Virus> Infections() {
            List<Virus> o = new List<Virus>();
            foreach (var i in ifd) {
                if (i.Value.infected)
                    o.Add(i.Value.virus);
            }
            return o;
        }

        /// <summary>
        /// ma nejakou nemoc?
        /// </summary>
        public bool Infected() {
            foreach (var i in ifd) {
                if (i.Value.infected)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// je nakazeny, a uz to je po inkubacni dobe?
        /// </summary>
        public bool InfectedWithSymptoms() {
            foreach (var i in ifd) {
                if (i.Value.infected && 
                    i.Value.infectTime - i.Value.virus.incubationPeriod >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// je imuni proti vsemu?
        /// </summary>
        public bool FullImune() {
            foreach (var i in ifd) {
                if (!i.Value.longImunity)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// jak daleko je od mista?
        /// </summary>
        /// <param name="target">cil</param>
        /// <returns>vzdalenost od cile</returns>
        public int Distance(Point target) {
            return Math.Abs(pos.X - target.X) + Math.Abs(pos.Y - target.Y);
        }

        /// <summary>
        /// posune se o policko smerem k cili
        /// </summary>
        /// <param name="target">cil</param>
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

        /// <summary>
        /// pracuje
        /// </summary>
        private void DoWork() {
            if (Distance(work) < 3)
                Wander();
            else
                GoAt(work);
        }

        /// <summary>
        /// toulani, posune se o policko na nahodnou stranu
        /// </summary>
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

        /// <summary>
        /// tik
        /// </summary>
        /// <param name="daytime">jaky je cas?</param>
        /// <param name="workday">je pracovni den?</param>
        /// <param name="walks">jsou povolene prochazky?</param>
        public void Tick(uint daytime, bool workday, bool walks) {
            if (alive) {
                if (InfectedWithSymptoms()) {
                    if (daytime < wakeTime || daytime > sleepTime)
                        return;
                    GoAt(new Point(random.Next(12, 20), random.Next(12, 20)));
                    return;
                }
                if (works && workday) {
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
                    if (!walks)
                        return;
                    if (daytime < 1020) { // do 17:00 prochazeni
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
