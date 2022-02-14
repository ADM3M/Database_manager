using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Table_manager
{
    public partial class InsertWindow : Form
    {
        SqlConnection sqlConnection = null;

        public InsertWindow(SqlConnection sqlConnection)
        {
            InitializeComponent();
            this.sqlConnection = sqlConnection;
        }

        private async void button2_Click(object sender, EventArgs e) // INSERT
        {
            var InsertStudentCommand = new SqlCommand("INSERT INTO [Clients] (ClientName, Age, Birthday) VALUES(@Name, @Age, @Birthday)", sqlConnection);
            InsertStudentCommand.Parameters.AddWithValue("Name", textBox1.Text);
            InsertStudentCommand.Parameters.AddWithValue("Age", Convert.ToInt32(textBox2.Text));
            InsertStudentCommand.Parameters.AddWithValue("Birthday", Convert.ToDateTime(textBox3.Text));

            try
            {
                await InsertStudentCommand.ExecuteNonQueryAsync();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
