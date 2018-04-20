using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HDH
{
    public partial class Form1 : Form
    {
        List<GetProcesses> windows = new List<GetProcesses>();
        List<ProcessItem> processItems = new List<ProcessItem>();
        Process[] process;
        public Form1()
        {
            InitializeComponent();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getPrecess();
            
            timer1.Enabled = true;
            timer1.Interval = 3000;
            int ff = 0;
            List<string> FileNames = new List<string>();
            foreach (var wnd in GetProcesses.GetAllWindows())
            {
                GetProcesses.GetWindowThreadProcessId(wnd.handle, out ff);
                FileNames.Add(Process.GetProcessById(ff).MainModule.ModuleName);
            }
        }
        private void getPrecess()
        {
            process = Process.GetProcesses();
            processItems.Clear();
            foreach (var item in process)
            {
                processItems.Add(new ProcessItem(item));
            }
            gcProcess.DataSource = processItems;
            windows = GetProcesses.GetAllWindows();
            gcRun.DataSource = windows;
        }
        public ProcessItem findById(long id)
        {

            foreach(var p in processItems)
            {
                if (p.ID == id)
                {
                    return p;
                }
            }
            return null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            getPrecess();
            gcProcess.RefreshDataSource();
            gcRun.RefreshDataSource();
        }

        private void gvProcess_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            //    e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gvProcess_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //if (e.RowHandle != gvProcess.FocusedRowHandle && (e.RowHandle % 2 == 0))
            //    e.Appearance.BackColor = Color.LightGreen; 
        }
        private void gvRun_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gvRun_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle != gvProcess.FocusedRowHandle && (e.RowHandle % 2 == 0))
                e.Appearance.BackColor = Color.LightGreen; 
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                long id = long.Parse(gvProcess.GetRowCellValue(gvProcess.FocusedRowHandle, "ID").ToString());
                ProcessItem process = findById(id);
                if (process != null)
                {
                    if (process.kill())
                    {
                        MessageBox.Show("OK");
                    }
                    else
                    {
                        MessageBox.Show("Khong thuc hien duoc thao tac nay!");
                    }
                    getPrecess();
                    gcProcess.RefreshDataSource();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
