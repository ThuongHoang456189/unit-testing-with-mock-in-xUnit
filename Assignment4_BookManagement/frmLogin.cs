using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess;
using InputValidators;

namespace Assignment4_BookManagement
{
    public partial class frmLogin : Form
    {
        private EmployeeData _EmployeeData;
        public frmLogin()
        {
            _EmployeeData = new EmployeeData();
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Employee employee = _EmployeeData.Login(txtEmpID.Text.Trim());
                if (employee == null)
                {
                    MessageBox.Show("Login failed!", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (!txtEmpPassword.Text.Trim().Equals(employee.EmpPassword.Trim()))
                    {
                        MessageBox.Show("Login failed!", "Invalid Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (employee.EmpRole)
                        {
                            this.Hide();
                            new frmMaintailBook().ShowDialog();
                        }
                        else
                        {
                            this.Hide();
                            new frmChangeAccount(employee).ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong happened! ");
            }
        }

        private void txtEmpID_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = String.Empty;
            if (!InputValidators.InputValidators.IsValidRequiredField("Employee ID", txtEmpID.Text, ref errorMsg))
            {
                errorProvider.SetError(txtEmpID, errorMsg);
            }
            else
            {
                errorProvider.SetError(txtEmpID, null);
            }
        }

        private void txtEmpPassword_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = String.Empty;
            if (!InputValidators.InputValidators.IsValidRequiredField("Password", txtEmpPassword.Text, ref errorMsg))
            {
                // Sua
                errorProvider.SetError(txtEmpPassword, errorMsg);
            }
            else
            {
                // Sua
                errorProvider.SetError(txtEmpPassword, null);
            }
        }
    }
}
