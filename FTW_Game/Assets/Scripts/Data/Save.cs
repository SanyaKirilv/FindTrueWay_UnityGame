using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTW.Data
{
    [Serializable]
    public class Save
    {
        public Player Player;
        public Settings Settings;
        public Resources Resources;
        public Time ExitTime;
        public List<Game> Games;
    }
}
