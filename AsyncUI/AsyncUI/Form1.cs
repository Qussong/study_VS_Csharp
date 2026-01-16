using System.Diagnostics;

namespace AsyncUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnAsync_Click(object sender, EventArgs e)
        {
            Task task1 = TaskAsync1();
            Task task2 = TaskAsync2();

            await Task.WhenAll(task1, task2);

        }

        async Task TaskAsync1()
        {
            lbLog.Items.Add("TaskAsync1 Started");
            await Task.Delay(3000).ConfigureAwait(false);   // ContextSwitching »ç¿ëx
            lbLog.Items.Add("TaskAsync1 Finished");
        }

        async Task TaskAsync2()
        {
            lbLog.Items.Add("TaskAsync2 Started");
            await Task.Delay(1500);
            lbLog.Items.Add("TaskAsync2 Finished");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
