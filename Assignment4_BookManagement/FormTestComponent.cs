using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment4_BookManagement
{
    public partial class FormTestComponent : Form
    {
        public FormTestComponent()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormTestComponent_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            label1.Text = "Hello";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

            DataTable table = new DataTable("table");
            DataColumn dcFirstName = new DataColumn(
                "FirstName", Type.GetType("System.String"));
            table.Columns.Add(dcFirstName);
            // Run a function to create a DataTable with one column.
            DataRow row;

            // Create a new DataRow.
            row = table.NewRow();
            // Detached row.
            label1.Text += ("New Row " + row.RowState);

            table.Rows.Add(row);
            // New row.
            label1.Text += ("AddRow " + row.RowState);

            table.AcceptChanges();
            // Unchanged row.
            label1.Text += ("AcceptChanges " + row.RowState);

            row["FirstName"] = "Scott";
            // Modified row.
            label1.Text += ("Modified " + row.RowState);

            row.Delete();
            // Deleted row.
            label1.Text += ("Deleted " + row.RowState);
        }
    }
}
