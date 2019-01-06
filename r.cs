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
using System.Management;
using System.Management.Instrumentation;
using System.ServiceProcess;



namespace project
{
    public partial class Form1 : Form
    {
        private Thread cpuThread;
        private double[] cpuArray = new double[60];
        public Form1()
        {
            InitializeComponent();
        }
        List<int> bsProcess = new List<int>();
        List<String> tsProcess = new List<String>();
       // Process[] AllPro = Process.GetProcesses();
        PerformanceCounter CpuUse;
        PerformanceCounter MemUse;
        Process[] pro = Process.GetProcesses();
        private void getPerformanceCounters()
        {
            var cpuPerfCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");

            while (true)
            {
                cpuArray[cpuArray.Length - 1] = Math.Round(cpuPerfCounter.NextValue(), 0);

                Array.Copy(cpuArray, 1, cpuArray, 0, cpuArray.Length - 1);

                if (cpuChart.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate { UpdateCpuChart(); });
                }
                else
                {
                    //......
                }

                Thread.Sleep(200);
            }
        }

        private void UpdateCpuChart()
        {
            cpuChart.Series["Series1"].Points.Clear();

            for (int i = 0; i < cpuArray.Length - 1; ++i)
            {
                cpuChart.Series["Series1"].Points.AddY(cpuArray[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         //  CpuUse = new PerformanceCounter();
          //  CpuUse.CategoryName = "Processor";
          //  CpuUse.CounterName = "% Processor Time";
          // CpuUse.InstanceName = "1";
          //  float cpuuse = CpuUse.NextValue();

            MemUse = new PerformanceCounter("Memory", "Available MBytes");
            float a = MemUse.NextValue();
           // chart1.Series["Series1"].Points.AddXY(0, cpuuse) ;
            chart2.Series["Series1"].Points.AddXY(0, a);

          


           

        }
        private void tabPage1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            cpuThread = new Thread(new ThreadStart(this.getPerformanceCounters));
            cpuThread.IsBackground = true;
            cpuThread.Start();

            listView1.Items.Clear();

            PerformanceCounter total_cpu;
            total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            // float total = total_cpu.NextValue();

            foreach (Process process in pro)
            {
                float total = total_cpu.NextValue();
                float a = process.VirtualMemorySize64 / 1024;
                process.Refresh();
                // Stime[flag] = process.StartTime;

                // first wala he ye tu end task
                // id wagera sab hide huwa he from mouse

                ListViewItem item = new ListViewItem(process.ProcessName);
                item.SubItems.Add(process.Id.ToString());
                item.SubItems.Add(a.ToString());
                item.SubItems.Add(total.ToString());


                listView1.Items.Add(item);
                label8.Text = "(" + pro.Length.ToString() + ")";
                


            }

            ask();
            ManagementObjectSearcher DiskInfo = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");

            foreach (ManagementObject X in DiskInfo.Get())
            {
                ListViewItem item2 = new ListViewItem(X["Name"].ToString());

                item2.SubItems.Add(X["Name"].ToString());
                listView2.Items.Add(item2);

            }
            ManagementObjectSearcher DiskInfo2 = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            //ManagementObject moDisk in mosDisks.Get()
            foreach (ManagementObject X in DiskInfo2.Get())
            {
                // Add the HDD to the list (use the Model field as the item's caption)
                cpuname.Text = (X["Name"].ToString());
                cores.Text = (X["NumberOfCores"].ToString());
                logical.Text = (X["NumberOfLogicalProcessors"].ToString());
                cache2.Text = (X["L2CacheSize"].ToString());
                cache3.Text = (X["L3CacheSize"].ToString());
                speed.Text = Convert.ToString((0.001 * (UInt32)X.Properties["CurrentClockSpeed"].Value + " GHz"));
                sockets.Text = (X["NumberOfCores"].ToString());
            }
            ManagementObjectSearcher DiskInfo3 = new ManagementObjectSearcher("SELECT * FROM  Win32_OperatingSystem");
            foreach (ManagementObject X in DiskInfo3.Get())
            {

                label17.Text = (X["Caption"].ToString());

            }
            ManagementObjectSearcher DiskInfo4 = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
            foreach (ManagementObject X in DiskInfo4.Get())
            {

                label33.Text = (X["Speed"].ToString() + " MHz");
                label31.Text = (X["FormFactor"].ToString());
                label29.Text = (X["DeviceLocator"].ToString());

            }

            ManagementObjectSearcher DiskInfo5 = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemoryArray ");
            //ManagementObject moDisk in mosDisks.Get()
            foreach (ManagementObject X in DiskInfo5.Get())
            {
                label32.Text = (X["MemoryDevices"].ToString() + " of " + X["MemoryDevices"].ToString());
            }
            ManagementObjectSearcher DiskInfo6 = new ManagementObjectSearcher("SELECT * FROM Win32_SystemSlot ");
            //ManagementObject moDisk in mosDisks.Get()
            foreach (ManagementObject X in DiskInfo6.Get())
            {
                // Add the HDD to the list (use the Model field as the item's caption)
               label28.Text=(X["SlotDesignation"].ToString());

            }
           ServiceController[] services = ServiceController.GetServices();

  
  foreach (ServiceController service in services)
  {
      ListViewItem item4= new ListViewItem(service.ServiceName.ToString());
      item4.SubItems.Add(service.DisplayName.ToString());   
      item4.SubItems.Add(service.Status.ToString());
      listView3.Items.Add(item4);
  }
 
        }
        
        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        ListViewItem item = listView1.SelectedItems[0];
        listView1.SelectedItems[0].Remove();

       //  Process process = (Process)item.Tag;
     //  process.Kill();
          


        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }           

        private void timer2_Tick(object sender, EventArgs e)
        {
            float dcpu = performanceCounter1.NextValue();
            float dram=performanceCounter2.NextValue();
            float zerocpu = performanceCounter3.NextValue();
            float onecpu = performanceCounter4.NextValue();
            float twocpu = performanceCounter5.NextValue();
            float threecpu = performanceCounter6.NextValue();
            circularProgressBar6.Value = (int)threecpu;
            circularProgressBar6.Text = string.Format("{0:0.0}%", threecpu);
            circularProgressBar5.Value = (int)twocpu;
            circularProgressBar5.Text = string.Format("{0:0.0}%", twocpu);
            circularProgressBar2.Value = (int)dram;
           circularProgressBar2.Text = string.Format("{0:0.00}%", dram);
            circularProgressBar3.Value = (int)dcpu;
            circularProgressBar3.Text = string.Format("{0:0.00}%", dcpu);
            circularProgressBar4.Value = (int)zerocpu;
            circularProgressBar4.Text = string.Format("{0:0.00}%", zerocpu);
            circularProgressBar1.Value = (int)onecpu;
            circularProgressBar1.Text = string.Format("{0:0.00}%", onecpu);

           // ListViewItem item4 = new ListViewItem(dcpu);
           // listView3.Items.Add(item4);
            listBox2.Items.Add(dcpu);


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }
        private void ask()
        {
            bsProcess = new List<int>();
            tsProcess = new List<String>();

            listBox1.Items.Clear();
            bsProcess = new List<int>();
            foreach (Process process2 in pro)
            {
                if (!String.IsNullOrEmpty(process2.MainWindowTitle))
                {
                    ListViewItem item2 = new ListViewItem("Application : " + process2.MainWindowTitle.ToString());
                    listBox1.Items.Add(item2);
                    
                    tsProcess.Add(process2.ProcessName);
                    bsProcess.Add(process2.Id);
                   
                    label6.Text = "("+ process2.MainWindowTitle.Length.ToString()+")";

                }
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count > 0)
            {
                int Id = bsProcess[listBox1.SelectedIndex];
                Process.GetProcessById(Id).Kill();
                ask();
            }
        }
        }



    }
     
      
    
