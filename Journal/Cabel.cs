﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    internal class Cabel
    {
        public string mark;
        public string number;
        public string typeOfC;
        public string numOfPol;
        public List<(string direction, string file)> directionInfo = new List<(string, string)>();
        public string? errMsg;
        public bool errType = false;
        public bool errSize = false;

        public Cabel(string mark, string number, string typeOfC, string numOfPol, string directionFrom, string dwgFile)
        {
            this.mark = mark.Contains(' ') ? mark.Remove(' ') : mark;
            this.number = number.Contains(' ') ? number.Remove(' ') : number;
            this.typeOfC = typeOfC.Contains(' ') ? typeOfC.Remove(' ') : typeOfC;
            this.numOfPol = numOfPol.Contains(' ') ? numOfPol.Remove(' ') : numOfPol;
            directionInfo.Add((directionFrom, dwgFile));
        }
    }
}
