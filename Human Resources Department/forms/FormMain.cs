﻿using System;
using System.Windows.Forms;

using Human_Resources_Department.classes;
using Human_Resources_Department.classes.db;
using Human_Resources_Department.classes.employees;
using Human_Resources_Department.classes.employees.main;

namespace Human_Resources_Department.forms
{
    public partial class FormMain : Form
    {
        private const string TEXT_SEARCH = "Швидкий пошук по Прізвищу";

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // If the project is not selected - finish the application.
            if ( ! SelectProject() )
                Application.ExitThread();

            WindowState = FormWindowState.Maximized;
            AcceptButton = button4;
        }

        /// <summary>
        /// Quick search bar. Focus set.
        /// </summary>
        private void FindField_Enter(object sender, EventArgs e)
        {
            if ( findField.Text.Equals(TEXT_SEARCH) )
                findField.Text = string.Empty;
        }

        /// <summary>
        /// Quick search bar. Focus lost.
        /// </summary>
        private void FindField_Leave(object sender, EventArgs e)
        {
            if ( string.IsNullOrWhiteSpace(findField.Text) )
                findField.Text = TEXT_SEARCH;
        }

        /// <summary>
        /// Event when the selected employee is changed.
        /// </summary>
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If the focus is lost, delete the panel.
            if ( ! EmployeesLV.IsSelected() )
            {
                EmployeesPanel.ClearAllData();
                button7.Enabled = false;
                button7.Text = "Оберіть працівника";
                return;
            }

            FillPanelEmployee();
        }
        
        /// <summary>
        /// Quick search button.
        /// </summary>
        private void Button4_Click(object sender, EventArgs e)
        {
            // If empty or the string is constant TEXT_SEARCH => return.
            if ( string.IsNullOrEmpty(findField.Text) || findField.Text.Equals(TEXT_SEARCH) )
                return;

            EmployeesLV.FindCellAndSetFocus(findField.Text, EmployeesLV.I_LNAME, true);
        }
        
        private void FormChooseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();

            // Close and clear all data.
            EmployeesLV.ClearAllData();
            EmployeesPanel.ClearAllData();

            EmployeesLV.Close();
            EmployeesPanel.Close();
            
            Database.CloseConnection();

            button7.Enabled = false;
            button7.Text = "Оберіть працівника";

            if ( ! SelectProject() )
                Application.ExitThread();

            Show();
        }

        private void FormInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using ( FormEmployee f = new FormEmployee() )
            {
                f.ShowDialog();
            }
        }

        private void FormSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFind f = new FormFind(true);
            f.Show(this);
        }

        private void FormFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFind f = new FormFind(false);
            f.Show(this);
        }

        private void SalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenOtherFormWithImage();

            using ( FormSalary f = new FormSalary() )
            {
                f.ShowDialog();
            }
            
            OpenOtherFormWithImage(false);
        }

        private void EmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( ! EmployeesLV.IsSelected() )
            {
                MessageBox.Show("Оберіть працівника");
                return;
            }

            OpenOtherFormWithImage();

            using ( FormEmployee f = new FormEmployee(EmployeesLV.GetSelectedID()) )
            {
                f.ShowDialog();
            }

            OpenOtherFormWithImage(false);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using ( BoxAbout b = new BoxAbout() )
            {
                b.ShowDialog();
            }
        }

        private void JobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using ( FormJobs f = new FormJobs() )
            {
                f.ShowDialog();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Project selection. Setting up a new environment.
        /// </summary>
        private bool SelectProject()
        {
            using ( FormChoose f = new FormChoose() )
            {
                f.ShowDialog();

                if ( f.IsOpen() )
                {
                    try
                    {
                        Database.SetConnection(Config.currentFolder + "\\" + Database.FILE_NAME);

                        EmployeesLV.SetListBox(listView1);
                        EmployeesLV.SetNameColumns();
                        EmployeesLV.GetAllData(!checkBox1.Checked);

                        EmployeesPanel.SetPanel(panelEmployee);
                        EmployeesPanel.Enabled();

                        Text = Config.PROJECT_NAME + " - " + f.GetNameFolder();
                    }
                    catch
                    {
                        MessageBox.Show("Файл пошкоджений");
                        return false;
                    }

                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Fill the panel with data from dataGridView.
        /// </summary>
        private void FillPanelEmployee()
        {
            if (!Employees.isOpen)
                EmployeesPanel.AddInfo(pictureBox1,     Employees.GetImage(EmployeesLV.GetSelectedID()));

            EmployeesPanel.AddInfo(fieldFName,      EmployeesLV.GetSelectedCell(EmployeesLV.I_FNAME));
            EmployeesPanel.AddInfo(fieldLName,      EmployeesLV.GetSelectedCell(EmployeesLV.I_LNAME));
            EmployeesPanel.AddInfo(fieldMName,      EmployeesLV.GetSelectedCell(EmployeesLV.I_MNAME));
            EmployeesPanel.AddInfo(fieldJob,        EmployeesLV.GetSelectedCell(EmployeesLV.I_JOB_ID));
            EmployeesPanel.AddInfo(fieldTelWork,    EmployeesLV.GetSelectedCell(EmployeesLV.I_TEL_WORK));
            EmployeesPanel.AddInfo(fieldEmail,      EmployeesLV.GetSelectedCell(EmployeesLV.I_EMAIL));
            EmployeesPanel.AddInfo(fieldEmployment, EmployeesLV.GetSelectedCell(EmployeesLV.I_EMPLOYMENT_DATE));
            EmployeesPanel.AddInfo(fieldUpdateAt,   EmployeesLV.GetSelectedCell(EmployeesLV.I_UPDATE_AT));

            button7.Enabled = true;
            button7.Text = "Відкрити картку #" + EmployeesLV.GetSelectedID();
        }
        
        /// <summary>
        /// Reset filters.
        /// </summary>
        private void Button6_Click(object sender, EventArgs e)
        {
            EmployeesPanel.ClearAllData();
            EmployeesLV.ClearAllData();
            EmployeesLV.SetNameColumns();
            EmployeesLV.GetAllData(!checkBox1.Checked);
        }

        private void OpenOtherFormWithImage(bool isOpen = true)
        {
            if (isOpen)
            {
                pictureBox1.Image = null;
                Employees.CloseImage();
                Employees.isOpen = true;
                return;
            }

            Employees.isOpen = false;
            EmployeesLV.UpdateSelected();
        }
    }
}
