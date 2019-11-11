using System;
using System.Linq;
using System.Text;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        public string convertTime(string aTime)
        {
            //"24:00:00" is an invalid representation for TimeSpan class, but valid for Berlin Clock. 
            //Converting its value to the representation TimeSpan class would accept.
            if (aTime == "24:00:00")
                aTime = "1.00:00:00";

            var time = TimeSpan.Parse(aTime);

            return getBerlinClockRepresentation((int)time.TotalHours, time.Minutes, time.Seconds); //NOTE: TotalHours is used instead of Hours, in order to cover "24:00:00" case
        }

        private string getBerlinClockRepresentation(int hours, int minutes, int seconds)
        {
            var stringBuilder = new StringBuilder();

            //top row - yellow lamp
            stringBuilder.AppendLine(seconds % 2 == 0 ? "Y" : "O");

            //second row - 5 hour marks
            int full5Hours = hours / 5; 
            stringBuilder.Append(new string('R', full5Hours));
            stringBuilder.AppendLine(new string('O', 4 - full5Hours));

            //third row - single hours
            int remainderHours = hours % 5;
            stringBuilder.Append(new string('R', remainderHours));
            stringBuilder.AppendLine(new string('O', 4 - remainderHours));

            //forth row - 5 minute marks
            int quarters = minutes / 15;
            int full5Mins = (minutes % 15) / 5;
            stringBuilder.Append(string.Join("", Enumerable.Repeat("YYR", quarters)));
            stringBuilder.Append(new string('Y', full5Mins));
            stringBuilder.AppendLine(new string('O', 11 - 3 * quarters - full5Mins));

            //fifth row - single minutes
            int remainderMinutes = minutes % 5;
            stringBuilder.Append(new string('Y', remainderMinutes));
            stringBuilder.Append(new string('O', 4 - remainderMinutes));

            return stringBuilder.ToString();
        }
    }
}
