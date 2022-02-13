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

            await LoadStudentsAsync();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        private async Task LoadStudentsAsync() //Select
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
    }
}
