/* Employee Database with UI interface
 * 
 * Authoer: Jonathan Le
 * 
 * Date: Feb. 27, 2025
 * 
 * Purpose: creating a data visualization of a list of employee's, which includes their
 * employeeIDs, first names, and last names. The user will be allowed to sort by employeeID
 * or by last name. The time it takes in ticks to perform each sorting algorithm will be counted
 * and displayed in a textbox within the application.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics; 
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;

namespace Employee_Display_and_Sorting
{
    public partial class Form1 : Form
    {
        List<Employee> providedData = new List<Employee>();
        List<Employee> fileData = new List<Employee>();
        Stopwatch sw = new Stopwatch();

        //Pre-made data
        int[] employeeIdList = { 28, 53, 12, 18, 8, 2, 19, 57, 62, 34, 23, 14, 48, 35, 55, 22, 26, 15, 7, 9, 32, 43, 41, 51 };
        string[] firstNameList = {"Emily", "Michael", "Olivia", "Daniel", "Sophia", "Ethan", "Ava", "Benjamin", "Isabella",
                "Jacob", "Mia", " William", "Emma", "Alexander", "Charlotte", "James", "Amelia", "Logan", "Harper",
                " Elijah", " Grace", " Noah", " Lily", "Lucas" };
        string[] lastNameList = { "Johnson", "Smith", "Williams", "Brown", "Jones", "Davis", "Miller", "Wilson", "Moore",
                "Taylor", "Anderson", "Thomas", "Jackson", "Moore", "Harris", "Martin", "Thompson", "Garcia", "Martinez",
                "Robinson", "Smith", "Rodriguez", "Lewis", "Clark"};

        private struct Employee
        {
            public int _employeeId;
            public string _firstName;
            public string _lastName;
        }
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < employeeIdList.Length; i++)
            {
                //adding employees to the providedData list
                providedData.Add(CreateEmployee(employeeIdList[i], firstNameList[i], lastNameList[i]));
               
            }
        }
        /////////////////////////////////// UNSORTED BUTTON-CLICK EVENT HANDLER /////////////////////////////////////////
        private void Unsorted_Btn_Click(object sender, EventArgs e)
        {
            if (sender == UI_DisplayUnsort_Btn)
            {
                UI_Unsorted_Dgv.Rows.Clear();
                //provided data option
                if (UI_Provided_Rbtn.Checked)
                {
                    List<Employee> providedDataCopy = new List<Employee>(providedData);
                    for (int i = 0; i < providedDataCopy.Count; i++)
                    {
                        DisplayRow(providedDataCopy, i, UI_Unsorted_Dgv);
                    }
                }
                //file data option
                if (UI_FileData_Rbtn.Checked)
                {
                    List<Employee> fileDataCopy = new List<Employee>(fileData);
                    for (int i = 0; i < fileDataCopy.Count; i++)
                    {
                        DisplayRow(fileDataCopy, i, UI_Unsorted_Dgv);
                    }
                }
            }
            if (sender == UI_ClearUnsorted_Btn)
            {
                UI_Unsorted_Dgv.Rows.Clear();
            }
        }
        ///////////////////////////////////// SORTED BUTTON-CLICK EVENT HANDLER /////////////////////////////////////////
        private void Sorted_Btn_Click(object sender, EventArgs e)
        {
            UI_Sorted_Dgv.Rows.Clear();
            if (sender == UI_EmployeeIdSort_Btn)
            {
                //provided data option
                if (UI_Provided_Rbtn.Checked)
                {
                    List<Employee> providedDataCopy = new List<Employee>(providedData);//clone of 'providedData' list
                    sw.Start();
                    SelectionSort(providedDataCopy);
                    for (int i = 0; i < providedDataCopy.Count; i++)
                    {
                        DisplayRow(providedDataCopy, i, UI_Sorted_Dgv);
                    }
                }
                //file data option
                if (UI_FileData_Rbtn.Checked)
                {
                    List<Employee> fileDataCopy = new List<Employee>(fileData);//clone of 'fileData' list
                    sw.Start();
                    SelectionSort(fileDataCopy);
                    for (int i = 0; i < fileDataCopy.Count; i++)
                    {
                        DisplayRow(fileDataCopy, i, UI_Sorted_Dgv);

                    }
                }
                sw.Stop();
                UI_Tick_Tbx.Text = sw.ElapsedTicks.ToString();
                sw.Reset();
            }
            if (sender == UI_LastNameSort_Btn)
            {
                //provided data option
                if (UI_Provided_Rbtn.Checked)
                {
                    List<Employee> providedDataCopy = new List<Employee>(providedData);//clone of 'providedData' list
                    sw.Start();
                    InsertionSort(providedDataCopy);
                    for (int i = 0; i < providedDataCopy.Count; i++)
                    {
                        DisplayRow(providedDataCopy, i, UI_Sorted_Dgv);
                    }
                }
                //file data option
                if (UI_FileData_Rbtn.Checked)
                {
                    List<Employee> fileDataCopy = new List<Employee>(fileData);//clone of 'fileData' list
                    sw.Start();
                    InsertionSort(fileDataCopy);
                    for (int i = 0; i < fileDataCopy.Count; i++)
                    {
                        DisplayRow(fileDataCopy, i, UI_Sorted_Dgv);
                    }
                }
                sw.Stop();
                UI_Tick_Tbx.Text = sw.ElapsedTicks.ToString();
                sw.Reset();
            }
            if (sender == UI_ClearSorted_Btn)
            {
                UI_Sorted_Dgv.Rows.Clear();
            }
        }
        /////////////////////////////////////////////// OFD EVENT HANDLER ///////////////////////////////////////////////
        private void UI_OFD_Btn_Click(object sender, EventArgs e)
        {
            if (sender == UI_OFD_Btn)
            {
                if (UI_Data_OFD.ShowDialog() == DialogResult.OK)
                {
                    fileData.Clear();
                    string fname = UI_Data_OFD.FileName;
                    string[] intermediate = File.ReadAllLines(fname);

                    int store = 0;
                    for (int i = 0; i < intermediate.Length; i++)
                    {
                        //separating each item within each element of the separated lines
                        string[] intermediate2 = intermediate[i].Split(',');
                        int.TryParse(intermediate2[0], out store);//parsing the [0] because it represents employeeIDs in the dataset
                        fileData.Add(CreateEmployee(store, intermediate2[1].Trim(), intermediate2[2].Trim()));//[1] - first name; [2] - last name
                    }
                }
                else
                {
                    //error message for user
                    MessageBox.Show("File selection was cancelled. \rPlease choose a text file.");
                }
            }
        }
        /////////////////////////////// RADIO BUTTON CHECK-CHANGED EVENT HANDLER ////////////////////////////////////////
        private void DataSource_CheckChanged(object sender, EventArgs e)
        {
            if(UI_FileData_Rbtn.Checked)
            {
                //enables file inputs from user
                UI_OFD_Btn.Enabled = true;
                UI_Unsorted_Dgv.AllowDrop = true;
            }
            if (UI_Provided_Rbtn.Checked)
            {
                //disables file inputs from user
                UI_OFD_Btn.Enabled = false;
                UI_Unsorted_Dgv.AllowDrop = false;
            }
        }
        /////////////////////////////////////////////// DRAG AND DROP ///////////////////////////////////////////////////
        private void DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void DragDrop(object sender, DragEventArgs e)
        {
            fileData.Clear();
            string fname = ((string[])e.Data.GetData(DataFormats.FileDrop)).First();
            string[] intermediate = File.ReadAllLines(fname);

            int store = 0;
            for (int i = 0; i < intermediate.Length; i++)
            {
                //separating each item within each element of the separated lines
                string[] intermediate2 = intermediate[i].Split(',');
                int.TryParse(intermediate2[0], out store);//parsing the [0] because it represents employeeIDs in the dataset
                fileData.Add(CreateEmployee(store, intermediate2[1].Trim(), intermediate2[2].Trim()));//[1] - first name; [2] - last name
            }
        }
        //////////////////////////////////////////////////// METHODS ////////////////////////////////////////////////////
        
        /// <summary>
        /// DisplayRow() will use struct objects within a struct array to display each employee's info
        /// in a row of the data grid view
        /// </summary>
        /// <param name="List"></param>
        /// <param name="i"></param>
        /// <param name="dgv"></param>
        private void DisplayRow(List<Employee> List, int i, DataGridView dgv)
        {
            DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();

            row.Cells[0].Value = List[i]._employeeId;
            row.Cells[1].Value = List[i]._firstName;
            row.Cells[2].Value = List[i]._lastName;
            dgv.Rows.Add(row);
        }

        /// <summary>
        /// CreateEmployee() takes values for an employee and returns an employee struct object
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private Employee CreateEmployee(int employeeId, string firstName, string lastName)
        {
            Employee employee = new Employee();
            employee._employeeId = employeeId;
            employee._firstName = firstName;
            employee._lastName = lastName;
            return employee;
        }
        /// <summary>
        /// SelectionSort() takes a list parameter and sorts the elements by ascending int order
        /// </summary>
        /// <param name="list"></param>
        private void SelectionSort(List<Employee> list)
        {
            int n = list.Count;//we keep a variable n for the number of elements in the list
            for (int pass = 0; pass < n - 1; pass++)//n-1 passes
            {
                //in each pass, we set max_posn to 0 and we scan the unsorted list to find
                //the next max value
                int max_posn = 0;
                int last_posn = n - 1 - pass;//last_posn is the index of the last position of unsorted list

                //iterate over the unsorted part of the list
                for (int j = 1; j <= last_posn; j++)
                {
                    if (list[j]._employeeId > list[max_posn]._employeeId)//we have found position j having a greater value than the value at max_posn
                    {
                        max_posn = j;
                    }
                }
                //when we leave the for loop we swap the structs at max_posn with that at last_posn
                Swap(list, max_posn, last_posn);
            }
        }
        /// <summary>
        /// Method swaps position 1 and 2 in the list of structs
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="posn1"></param>
        /// <param name="posn2"></param>
        private void Swap(List<Employee> list1, int posn1, int posn2)
        {
            Employee temp = list1[posn1];
            list1[posn1] = list1[posn2];
            list1[posn2] = temp;
        }
        /// <summary>
        /// InsertionSort() adpated for strings using lexicographical comparison
        /// </summary>
        /// <param name="list2"></param>
        private void InsertionSort(List<Employee> list)
        {
            int n = list.Count;

            //we have n-1 passes, we start with p=1 (p is pass number)
            for (int p = 1; p < n; p++)
            {
                Employee temp = list[p];//in each pass pick the element at position p
                int j = p - 1;//have a variable j initialied to p-1 (for iterating backwards)

                //list2[j].CompareTo(temp) > 0 implies that list2[j] is lexicographically greater than temp
                while ((j >= 0) && (list[j]._lastName.CompareTo(temp._lastName) > 0))
                {
                    list[j + 1] = list[j];//each element bigger than temp is moved 1 position to right
                    j = j - 1;
                }
                //when leaving the while loop, we have already gone one position too far for temp
                list[j + 1] = temp;
            }
        }
    }
}
