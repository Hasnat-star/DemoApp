using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DemoApp
{
    public partial class Form1 : Form
    {
        private String ConnectionString = "Data Source=HASNAT-PC;Initial Catalog=DemoApp;Integrated Security=True;Connect Timeout=30;";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label10_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            String query = @"delete from student where StudentId = @StudentID;";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@StudentID", guna2TextBox1.Text);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record deleted successfully.");
                }
                else
                {
                    MessageBox.Show("No record found to delete.");
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            String query = @"select * from student where StudentId = @StudentID;";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@StudentID", guna2TextBox1.Text);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.Read())
                {
                    guna2TextBox2.Text = reader["FirstName"].ToString();
                    guna2TextBox3.Text = reader["LastName"].ToString();
                    guna2TextBox4.Text = reader["FatherName"].ToString();
                    if (reader["DateOfBirth"] != DBNull.Value)
                    {
                        guna2DateTimePicker1.Value = Convert.ToDateTime(reader["DateOfBirth"]);
                    }
                    String gender = reader["Gender"].ToString();

                    if (gender == "Male")
                    {
                        guna2RadioButton1.Checked = true;
                    }
                    else if (gender == "Female")
                    {
                        guna2RadioButton2.Checked = true;
                    }
                    else if (gender == "Other")
                    {
                        guna2RadioButton3.Checked = true;
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("No record found.");
                }
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            String query = @"merge into student as target using (select @StudentID as StudentID, @FirstName as FirstName, @LastName as LastName,@FatherName as FatherName, @DateOfBirth as DateOfBirth,@Gender as Gender)
        as source on target.StudentID = source.StudentID
        when matched then
        update set FirstName = source.FirstName, LastName = source.LastName, FatherName = source.FatherName, DateOfBirth = source.DateOfBirth,Gender = source.Gender
        when not matched then
        insert (StudentID,FirstName,LastName,FatherName,DateOFBirth,Gender)
        values(source.StudentID,source.FirstName,source.LastName,source.FatherName,source.DateOfBirth,source.Gender);";
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", guna2TextBox1.Text);
                    cmd.Parameters.AddWithValue("@FirstName", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@LastName", guna2TextBox3.Text);
                    cmd.Parameters.AddWithValue("@FatherName", guna2TextBox4.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", guna2DateTimePicker1.Value.Date);
                    String gender = "";
                    if (guna2RadioButton1.Checked == true)
                    {
                        gender = "Male";
                    }
                    else if (guna2RadioButton2.Checked == true)
                    {
                        gender = "Female";
                    }
                    else if (guna2RadioButton3.Checked == true)
                    {
                        gender = "Other";
                    }
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Inserted/Updated Successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ther error is " + ex.Message);
            }
        }
    }
}