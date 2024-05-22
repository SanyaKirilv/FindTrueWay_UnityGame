using System;

namespace FTW.Data
{
    [Serializable]
    public class Level
    {
        public int Index;
        public string State;
        public int Stars;
        public int Time;

        public void Complete(int time, int stars)
        {
            State = "Completed";
            Time = time;
            Stars = stars;
        }

        public void Open()
        {
            if (State == "Completed")
            {
                return;
            }
            State = "Active";
        }
    }
}
