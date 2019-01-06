using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        PerformanceCounter CpuUse;
        PerformanceCounter MemUse;

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CpuUse = new PerformanceCounter();
            CpuUse.CategoryName = "Processor";
            CpuUse.CounterName = "% Processor Time";
            CpuUse.InstanceName = "_Total";
            float cpuuse = CpuUse.NextValue();

            MemUse = new PerformanceCounter("Memory", "Available MBytes");
            float a = MemUse.NextValue();
            chart1.Series["Series1"].Points.AddXY(0, cpuuse);
            chart2.Series["Series1"].Points.AddXY(0, a);       

        }

      
    }
}
