﻿using System;
using System.Windows.Forms;

using Human_Resources_Department.classes;
using Human_Resources_Department.classes.helplers;
using Human_Resources_Department.classes.employees;
using Human_Resources_Department.classes.employees.db;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;

namespace Human_Resources_Department.forms
{
    public partial class FormSalary : Form
    {
        private const string TEXT_SEARCH = "Швидкий пошук по Прізвищу";

        private const int I_ID     = 0;
        private const int I_FNAME  = 1;
        private const int I_LNAME  = 2;
        private const int I_SALARY = 3;
        private const int I_NDFL   = 4;
        private const int I_VZ     = 5;
        private const int I_ESV    = 6;
        private const int I_CLEAR  = 7;

        public FormSalary()
        {
            InitializeComponent();

            this.Text = Config.PROJECT_NAME + " - Зарплата";

            LVHelpler.Normillize(listView1);
            PanelHelpler.ActivateToggle(panel1);

            SetColumns();
            FillData();
        }

        private void SetColumns()
        {
            listView1.Columns.Add("#");
            listView1.Columns.Add("Ім'я");
            listView1.Columns.Add("Прізвище");
            listView1.Columns.Add("Зарплата");
            listView1.Columns.Add("НДФЛ");
            listView1.Columns.Add("ВЗ");
            listView1.Columns.Add("ЄСВ");
            listView1.Columns.Add("Чиста Зарплата");
        }

        private void FillData()
        {
            try
            {
                var data = new EmployeesModel(Config.currentFolder + "\\" + EmployeesModel.nameFile)
                    .GetAllData(true);

                foreach (var one in data)
                {
                    var salary = new SalaryHelpler(one.Salary);

                    listView1.Items.Add( new ListViewItem( new[] {
                        LVHelpler.T(one.Id), LVHelpler.T(one.FName), LVHelpler.T(one.LName), LVHelpler.T(one.Salary),
                        salary.GetNDFL().ToString(), salary.GetVZ().ToString(),
                        salary.GetESV().ToString(), salary.GetClearSalary().ToString()
                    }));
                }
            }
            catch
            {
                Close();
            }
        }

        private double GetSalary()
        {
            if ( ! IsSelected() )
                return 0;
            
            if ( Double.TryParse(GetSelectedItem(3).ToString(), out double salary) )
                return salary;

            return 0;
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( ! IsSelected() )
                return;

            pictureBox1.Image = Employees.GetImageUrl( GetSelectedID() );
            textBox1.Text = GetSelectedItem(I_FNAME).ToString();
            textBox5.Text = GetSelectedItem(I_LNAME).ToString();
            textBox2.Text = GetSelectedItem(I_SALARY).ToString();
            textBox3.Text = GetSelectedItem(I_NDFL).ToString();
            textBox4.Text = GetSelectedItem(I_VZ).ToString();
            textBox6.Text = GetSelectedItem(I_ESV).ToString();
            textBox7.Text = GetSelectedItem(I_CLEAR).ToString();
        }

        private bool IsSelected()
        {
            return listView1.SelectedItems.Count != 0;
        }

        private object GetSelectedItem(int cell)
        {
            if ( ! IsSelected() )
                return 0;

            return listView1.SelectedItems[0].SubItems[cell].Text;
        }

        private int GetSelectedID()
        {
            if ( ! IsSelected() )
                return 0;

            return Convert.ToInt32( listView1.SelectedItems[0].SubItems[I_ID].Text );
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Image Files(*.jpg, *.png) | *.jpg; *.png"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(panel1.Width);
                int height = Convert.ToInt32(panel1.Height);

                Bitmap bmp = new Bitmap(width, height);

                panel1.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
            }
        }
    }
}