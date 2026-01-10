namespace _41_thread_synchronization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // This is a big fat task that can be run on a different threat
            // Also it blocks the UI thread until it completes
            ShowMessage("First Message", 3000);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowMessage("Second Message", 5000);

        }

        private async Task ShowMessage(string message, int delay)
        {
            await Task.Delay(delay);

            lblMessage.Text = message;
        }
    }
}
