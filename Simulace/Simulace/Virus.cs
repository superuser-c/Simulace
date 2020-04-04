using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulace {
    public class Virus {
        public int incubationPeriod;
        public float deathRateStart, deathRateEnd;
        // start - sance na umrti kazdy den v prvni polovine nemoci
        // end - sance kazdy den v druhe polovine
        public int illnessTime;
        public int TRNeed, TRTop, TR;
        // TR - prenosnost
        // TRNeed - potrebny kontakt s nakazenym
        // TRTop - maximalni kontaks s nakazenym (100% se nakazi)
        public float longImunityChance;
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
