using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Table_manager
{
    public partial class Database_Table_Managament : Form
    {
        private SqlConnection sqlConnection = null;

        public Database_Table_Managament()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Db"].ConnectionString);
            await sqlConnection.OpenAsync();

            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.View = View.Details;
            listView1.Columns.Add("Id");
            listView1.Columns.Add("ClientName");
            listView1.Columns.Add("Age");
            listView1.Columns.Add("Birthday");

            await LoadClientsAsync();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        private async Task LoadClientsAsync() // SELECT
        {
            SqlDataReader data = null;
            SqlCommand LoadStudentsCommand = new SqlCommand("SELECT * FROM [Clients]", sqlConnection);

            try
            {
                data = await LoadStudentsCommand.ExecuteReaderAsync();

                while (await data.ReadAsync())
                {
                    var item = new ListViewItem(new string[]
                    {
                        Convert.ToString(data["Id"]),
                        Convert.ToString(data["ClientName"]),
                        Convert.ToString(data["Age"]),
                        Convert.ToString(data["BirthDay"])
                    });

                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (data != null && !data.IsClosed)
                {
                    data.Close();
                }
            }
        }

        private async Task DeleteClientsAsync(int id) // DELETE
        {
            SqlCommand DeleteCommand = new SqlCommand("DELETE FROM [Clients] WHERE [Id] = @Id", sqlConnection);
            DeleteCommand.Parameters.AddWithValue("Id", id);

            try
            {
                await DeleteCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void toolStripButton5_Click(object sender, EventArgs e) // Refresh button
        {
            listView1.Items.Clear();

            await LoadClientsAsync();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) // Insert button
        {
            InsertWindow insertWindow = new InsertWindow(sqlConnection);
            insertWindow.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) // Update button
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("You should select row you want to update", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ListViewItem lvItem = listView1.SelectedItems[0];

            UpdateWindow window = new UpdateWindow(sqlConnection, lvItem);
            window.Show();
        }

        private async void toolStripButton3_Click(object sender, EventArgs e) // Delete button
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("You should select row you want to delete", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int id = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
            var dialogResult = MessageBox.Show($"Are you sure you want to delete client {id}?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.Yes)
            {
                await DeleteClientsAsync(id);

                listView1.Items.Clear();
                await LoadClientsAsync();
            }
        }
    }
}
