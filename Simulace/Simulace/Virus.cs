using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulace {
    public class Virus {
        /// <summary>
        /// inkubacni doba ve dnech
        /// </summary>
        public int incubationPeriod;
        /// <summary>
        /// sance na umrti kazdy den v prvni polovine nemoci
        /// </summary>
        public float deathRateStart;
        /// <summary>
        /// sance kazdy den v druhe polovine
        /// </summary>
        public float deathRateEnd;
        /// <summary>
        /// jak dlouho nemoc trva?
        /// </summary>
        public int illnessTime;
        /// <summary>
        /// potrebny kontakt s nakazenym aby se nakazil dalsi
        /// </summary>
        public int TRNeed;
        /// <summary>
        /// maximalni kontakt s nakazenym (100% se nakazi)
        /// </summary>
        public int TRTop;
        /// <summary>
        /// sance na nakazeni; nakazi se? = (Rand(TRTop - kontakty) &lt; TR)
        /// </summary>
        public int TR;
        /// <summary>
        /// sance na ziskani plne imunity na virus
        /// </summary>
        public float longImunityChance;
        /// <summary>
        /// jmeno viru
        /// </summary>
        public string name;

        public Virus(int ip, float drs, float dre, int it, 
            int trn, int trt, int tr, float lic, string n) {
            incubationPeriod = ip;
            deathRateStart = drs;
            deathRateEnd = dre;
            illnessTime = it;
            TRNeed = trn;
            TRTop = trt;
            TR = tr;
            longImunityChance = lic;
            name = n;
        }

        public Virus Clone() {
            return new Virus(incubationPeriod, deathRateStart, deathRateEnd,
                illnessTime, TRNeed, TRTop, TR, 
                longImunityChance,
                name);
        }
    }
}
