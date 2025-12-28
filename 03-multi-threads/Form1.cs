namespace _03_multi_threads
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
            //ShowMessage("First Message", 3000);

            // So, we can use Thread and assign this task to the thread to complete
            Thread thread = new Thread(() => ShowMessage("First Message", 3000));
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ShowMessage("Second Message", 5000);

            Thread thread = new Thread(() => ShowMessage("Second Message", 5000));
            thread.Start();
        }

        private void ShowMessage(string message, int delay)
        {
            Thread.Sleep(delay);
            //lblMessage.Text = message;

            // Assuming you are in a background thread and want to update lblMessage
            if (lblMessage.InvokeRequired)
            {
                // If we are on a different thread, use Invoke to marshal the call back to the UI thread
                //first things you learn is that you can't access UI elements outside of the UI thread
                lblMessage.Invoke((MethodInvoker)delegate
                {
                    lblMessage.Text = message;
                });
            }
            else
            {
                // Otherwise, we are already on the UI thread and can update directly
                lblMessage.Text = message;
            }
        }
    }
}
