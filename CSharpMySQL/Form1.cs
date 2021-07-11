using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpMySQL
{
    public partial class Form1 : Form
    {
        System.Data.SqlClient.SqlConnection Con;
        System.Data.SqlClient.SqlDataAdapter da;
        DataSet ds;
        int RecordCount;
        int CurrentRow;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Con = new System.Data.SqlClient.SqlConnection();
            Con.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Mayas\\source\\repos\\CSharpMySQL\\CSharpMySQL\\Members.mdf;Integrated Security=True";
            Con.Open();
            String SqlStr = "SELECT * FROM Members";
            System.Data.SqlClient.SqlConnection SC = new System.Data.SqlClient.SqlConnection(Con.ConnectionString);
            da = new System.Data.SqlClient.SqlDataAdapter(SqlStr, Con);
            ds = new DataSet();
           da.Fill(ds, "Members");
           RecordCount = ds.Tables["Members"].Rows.Count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentRow = 0;
            ShowRecord(CurrentRow);
        }
        public void ShowRecord(int ThisRow)
        {
            textBox1.Text = ds.Tables["Members"].Rows[ThisRow].Field<int>("MemberID").ToString();
            textBox2.Text = ds.Tables["Members"].Rows[ThisRow].Field<String>("FirstName").ToString();
            textBox3.Text = ds.Tables["Members"].Rows[ThisRow].Field<String>("LastName").ToString();
            textBox4.Text = ds.Tables["Members"].Rows[ThisRow].Field<DateTime>("DOB").ToString();
            textBox5.Text = ds.Tables["Members"].Rows[ThisRow].Field<String>("Rank").ToString();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            CurrentRow++;
            if (CurrentRow > RecordCount - 1)
            {
                MessageBox.Show("End of file Encountered");
                CurrentRow--;
            }

            ShowRecord(CurrentRow);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CurrentRow--;
            if (CurrentRow < 0)
            {
                MessageBox.Show("Beginning of file Encountered");
                CurrentRow++;
            }
            ShowRecord(CurrentRow);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CurrentRow = Convert.ToInt16(RecordCount) - 1;
            ShowRecord(CurrentRow);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Data.DataRow[] FoundRows;
            String Strtofind;

            if (textBox1.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Please Enter member ID");
                return;
            }
            Strtofind = "MemberID =" + textBox1.Text;
            FoundRows = ds.Tables["Members"].Select(Strtofind);
            int Rowindex;

            if (FoundRows.Length == 0)
            {
                MessageBox.Show("Record Not Found");
            }
            else
            {
                Rowindex = ds.Tables["Members"].Rows.IndexOf(FoundRows[0]);
                CurrentRow = Rowindex;
                ShowRecord(CurrentRow);
            }
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ClearBoxes();
        }
        public void ClearBoxes()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Data.DataRow[] foundRows;
            String Strtofind;

            if (textBox1.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Please Enter a Member ID to find");
                return;
            }
            Strtofind = "MemberID =" + textBox1.Text;
            foundRows = ds.Tables["Members"].Select(Strtofind);
            if (foundRows.Length == 0)
            {
                //its a new record, we should be able to add 
                System.Data.DataRow NewRow = ds.Tables["Members"].NewRow();
                //Next line is needed so we can update the database 
                System.Data.SqlClient.SqlCommandBuilder Cb = new System.Data.SqlClient.SqlCommandBuilder(da);
                NewRow.SetField<int>("MemberID", Convert.ToInt32(textBox1.Text));
                NewRow.SetField<String>("FirstName", textBox2.Text);
                NewRow.SetField<String>("LastName", textBox3.Text);
                NewRow.SetField<DateTime>("DOB", Convert.ToDateTime (textBox4.Text));
                NewRow.SetField<String>("Rank", textBox5.Text);
                ds.Tables["Members"].Rows.Add(NewRow);
                //da.UpdateCommand = Cb.GetUpdateCommand();
                da.Update(ds, "Members");
                //da.AcceptChangesDuringUpdate = true;
                //ds.AcceptChanges(); 
                RecordCount = RecordCount + 1;
                System.Windows.Forms.MessageBox.Show("Record Added Succesfully");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Duplicate ID, try Again!!!");

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Data.DataRow[] foundRows;
            String Strtofind;
            int Rowindex;
            if (textBox1.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Please Enter a Member ID to find");
                return;
            }
            Strtofind = "MemberID =" + textBox1.Text;
            foundRows = ds.Tables["Members"].Select(Strtofind);
            System.Data.SqlClient.SqlCommandBuilder Cb = new System.Data.SqlClient.SqlCommandBuilder(da);
            if (foundRows.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Record Not Found, try again");
            }
            else
            {
                Rowindex = ds.Tables["Members"].Rows.IndexOf(foundRows[0]);
                ds.Tables["Members"].Rows[Rowindex].SetField<int>("MemberID", Convert.ToInt32(textBox1.Text));
                ds.Tables["Members"].Rows[Rowindex].SetField<String>("FirstName", textBox2.Text);
                ds.Tables["Members"].Rows[Rowindex].SetField<String>("LastName", textBox3.Text);
                ds.Tables["Members"].Rows[Rowindex].SetField<DateTime>("DOB", Convert.ToDateTime(textBox4.Text));
                ds.Tables["Members"].Rows[Rowindex].SetField<String>("Rank", textBox5.Text);
                System.Windows.Forms.MessageBox.Show("Modifications saved successfully!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Data.DataRow[] foundRows;
            String Strtofind;
            int Rowindex;
            if (textBox1.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Please Enter a Member ID to find");
                return;
            }
            Strtofind = "MemberID =" + textBox1.Text;
            foundRows = ds.Tables["Members"].Select(Strtofind);
            if (foundRows.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Record Not Found, try again");
            }
            else
            {
                int result;
                Rowindex = ds.Tables["Members"].Rows.IndexOf(foundRows[0]);
                result = Convert.ToInt32(System.Windows.Forms.MessageBox.Show("Are you Sure?", "Deleting Record", MessageBoxButtons.YesNo));
                if (result == 6)
                {
                  
                    ds.Tables["Members"].Rows[Rowindex].Delete();
                    ClearBoxes();
                    System.Windows.Forms.MessageBox.Show("record deleted Succesfully!!!");
                    RecordCount = RecordCount - 1;
                }
            }
        }
    }
}
