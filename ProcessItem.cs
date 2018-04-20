using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDH
{
    public class ProcessItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MemorySize { get; set; }
        public Image IconProgess { get; set; }
        public DateTime StartTime { get; set; }
        private Process process;
        public ProcessItem(Process p)
        {
            process = p;
            this.ID = p.Id;
            this.Name = p.ProcessName;
            this.MemorySize =(p.WorkingSet64/1000000.0).ToString("0.#").Replace(".",",")+" MB";
            try
            {
                StartTime = p.StartTime;
                IconProgess = Icon.ExtractAssociatedIcon(p.MainModule.FileName).ToBitmap();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / 1024f;
        }
        private double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
        public bool kill()
        {
            try
            {
                process.Kill();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

    }
}
