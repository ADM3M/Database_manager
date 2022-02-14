using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Table_manager
{
    public partial class UpdateWindow : Form
    {
        private SqlConnection sqlConnection = null;
        private ListViewItem listViewItem;
        private bool IsDataChanged { get; set; } = false;

        public UpdateWindow(SqlConnection sqlConnection, ListViewItem listView)
        {
            InitializeComponent();
            this.sqlConnection = sqlConnection;
            this.listViewItem = listView;
        }

        private void UpdateWindow_Load(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(listViewItem.SubItems[1].Text);
            textBox2.Text = Convert.ToString(listViewItem.SubItems[2].Text);
            textBox3.Text = Convert.ToString(listViewItem.SubItems[3].Text);

            textBox1.TextChanged += (send, args) => IsDataChanged = true;
            textBox2.TextChanged += (send, args) => IsDataChanged = true;
            textBox3.TextChanged += (send, args) => IsDataChanged = true;
        }

        private async Task UpdateAsync() // UPDATE
        {
            if (!IsDataChanged)
            {
                return;
            }

            var UpdateCommand = new SqlCommand("UPDATE [Clients] SET [ClientName] = @Name, [Age] = @Age, [Birthday] = @Birthday WHERE [ID] = @Id", sqlConnection);
            UpdateCommand.Parameters.AddWithValue("Name", textBox1.Text);
            UpdateCommand.Parameters.AddWithValue("Age", Convert.ToInt32(textBox2.Text));
            UpdateCommand.Parameters.AddWithValue("Birthday", Convert.ToDateTime(textBox3.Text));
            UpdateCommand.Parameters.AddWithValue("Id", Convert.ToInt32(this.listViewItem.SubItems[0].Text));

            try
            {
                await UpdateCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await UpdateAsync();

            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
