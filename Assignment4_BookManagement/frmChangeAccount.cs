using DataAccess;
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
    public partial class frmChangeAccount : Form
    {
        private Employee _employee;
        private EmployeeData _employeeData;
        public frmChangeAccount()
        {
            InitializeComponent();
        }
        public frmChangeAccount(Employee employee):this()
        {
            _employee = employee;
            _employeeData = new EmployeeData();
        }

        private void frmChangeAccount_Load(object sender, EventArgs e)
        {
            if (_employee == null)
            {
                MessageBox.Show("Something wrong happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                lblID.Text = _employee.EmpID;
                lblRole.Text = _employee.EmpRole.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_employee == null)
            {
                MessageBox.Show("Something wrong happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (_employeeData.ChangePassword(_employee.EmpID.Trim(),txtPassword.Text.Trim()))
                {
                    MessageBox.Show("Changed password successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Changed password failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.Close();
        }

        private void txtConfirmedPassword_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string errorMsg = String.Empty;
                if (!InputValidators.InputValidators.IsValidRequiredField("New Password", txtPassword.Text, ref errorMsg))
                {
                    e.Cancel = false;
                    // Sua
                    txtPassword.Focus();
                    errorProvider.SetError(txtPassword, errorMsg);
                }
                else
                {
                    if (!InputValidators.InputValidators.IsValidRequiredField("Confirm Password", txtConfirmedPassword.Text, ref errorMsg))
                    {
                        e.Cancel = false;
                        // Sua
                        txtConfirmedPassword.Focus();
                        errorProvider.SetError(txtConfirmedPassword, errorMsg);
                    }
                    else
                    {
                        // Sua
                        errorProvider.SetError(txtConfirmedPassword, null);
                        if (!InputValidators.InputValidators.IsValidRequiredField("New Password", txtPassword.Text, ref errorMsg))
                        {
                            e.Cancel = false;
                            // Sua
                            txtPassword.Focus();
                            errorProvider.SetError(txtPassword, errorMsg);
                        }
                        else
                        {
                            errorProvider.SetError(txtPassword, null);
                            if (!txtConfirmedPassword.Text.Trim().Equals(txtPassword.Text.Trim()))
                            {
                                e.Cancel = true;
                                txtConfirmedPassword.Focus();
                                errorProvider.SetError(txtConfirmedPassword, "The confirmed password must match the new password. ");
                            }
                            else
                            {
                                e.Cancel = false;
                                errorProvider.SetError(txtConfirmedPassword, null);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                e.Cancel = false;
                MessageBox.Show("Something wrong happened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = String.Empty;
            if (!InputValidators.InputValidators.IsValidRequiredField("New Password", txtPassword.Text, ref errorMsg))
            {
                e.Cancel = true;
                // Sua
                txtPassword.Focus();
                errorProvider.SetError(txtPassword, errorMsg);
            }
            else
            {
                if(txtPassword.Text.Length > 15)
                {
                    e.Cancel = true;
                    // Sua
                    txtPassword.Focus();
                    errorProvider.SetError(txtPassword, "Password exceed max length!");
                }
                else
                {
                    e.Cancel = false;
                    // Sua
                    errorProvider.SetError(txtPassword, null);
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
