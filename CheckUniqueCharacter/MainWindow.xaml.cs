using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace CheckUniqueCharacter
{
    public enum MaxPhase
    {
        Phase1 = 1,
        Phase2,
        Phase3,
        Phase4,
        Phase0
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Hashtable ht = new Hashtable();
        private Hashtable htStep = new Hashtable();
        private ArrayList uniqueList = new ArrayList();
        private int step = 0;
        private Stack stack = new Stack(4);


        public MainWindow()
        {
            InitializeComponent();

        }

        public bool InsertHashMap(Hashtable cht, string value)
        {
            if(cht.Count > 0)
            {
                if (cht.ContainsKey(value))
                {
                    int inc = Int32.Parse(cht[value].ToString());
                    cht[value] = ++inc;
                    return false;
                }else
                {
                    cht.Add(value, 1);
                    return true;
                }
            }else
            {
                cht.Add(value, 1);
                return true;
            }
        }

        private bool Reset()
        {
            ht.Clear();
            htStep.Clear();
            uniqueList.Clear();

            lvHashTable.Items.Clear();
            lvArrayList.Items.Clear();

            lblResult.Content = "";
            lblStep.Content = "";
            lblCValue.Content = "";

            button1.IsEnabled = true;

            return true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!Reset())
            {
                ht = null;
                uniqueList = null;

                System.GC.Collect();

                ht = new Hashtable();
                uniqueList = new ArrayList();
            }

            string mainValue = textBox.Text;
            for(int i = 0; i < mainValue.Length; i++)
            {
                if (InsertHashMap(ht,mainValue[i].ToString())){
                    uniqueList.Add(mainValue[i].ToString());
                }
                else
                {
                    if (uniqueList.Contains(mainValue[i].ToString()))
                    {
                        uniqueList.Remove(mainValue[i].ToString());
                    }
                }
            }

            lblResult.Content = uniqueList[0].ToString();
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (stack.Count == 0)
                NextPhase();
            else if (stack.Peek().Equals(MaxPhase.Phase0))
            {
                stack.Pop();
                step++;
                NextPhase();
            }
            if(step != textBox.Text.Length)
            {
                if (stack.Peek().Equals(MaxPhase.Phase1))
                {
                    //Update Step
                    lblStep.Content = step.ToString();
                    NextPhase();
                }
                else if (stack.Peek().Equals(MaxPhase.Phase2))
                {
                    //Update value
                    lblCValue.Content = textBox.Text[step];
                    NextPhase();
                }
                else if (stack.Peek().Equals(MaxPhase.Phase3))
                {
                    //Update HastTable
                    InsertHashMap(htStep, textBox.Text[step].ToString());
                    lvHashTable.Items.Add(textBox.Text[step]);
                    NextPhase();
                }
                else if (stack.Peek().Equals(MaxPhase.Phase4))
                {
                    //Update ArrayList
                    UpdateArrayLV(textBox.Text[step].ToString());
                    //Increment
                    NextPhase();
                }
            }else
            {
                lblResult.Content = lvArrayList.Items[0].ToString();
                button1.IsEnabled = false;
            }

            

        }

        public void NextPhase()
        {
            if(stack.Count < 1)
            {
                stack.Push(MaxPhase.Phase0);
                stack.Push(MaxPhase.Phase4);
                stack.Push(MaxPhase.Phase3);
                stack.Push(MaxPhase.Phase2);
                stack.Push(MaxPhase.Phase1);
            }
            else
            {
                stack.Pop();
            }
        }

        public void PrevPhase()
        {

            if (stack.Peek().Equals(MaxPhase.Phase4))
            {
                stack.Push(MaxPhase.Phase3);
                int val = Int32.Parse(htStep[textBox.Text[step].ToString()].ToString());

                if(val == 1)
                {
                    //Remove from HashTable
                    htStep.Remove(textBox.Text[step].ToString());
                    //Remove for LvHashTable
                    lvHashTable.Items.Remove(textBox.Text[step]);
                }
                    

            }
            else
            {
                if (stack.Peek().Equals(MaxPhase.Phase3))
                {
                    //lblCurrent Value
                    stack.Push(MaxPhase.Phase2);
                    int check = step - 1;
                    if (check < 0)
                        check = 0;
                    lblCValue.Content = textBox.Text[check].ToString();
                }
                else if (stack.Peek().Equals(MaxPhase.Phase2))
                {
                    //Displaying Steps
                    stack.Push(MaxPhase.Phase1);
                    lblStep.Content = step.ToString();
                }
                else if (stack.Peek().Equals(MaxPhase.Phase1))
                {
                    //Update old view

                }
                else if (stack.Peek().Equals(MaxPhase.Phase0))
                {
                    //Update ArrayList
                    //check if value is 1 the remove From 
                    stack.Push(MaxPhase.Phase4);
                    RemUpdateArrayLV(textBox.Text[step].ToString());
                }


            }
        }


        public void UpdateArrayLV(string val)
        {
            int isAdd = Int32.Parse(htStep[val].ToString());

            if (isAdd == 1)
            {
                lvArrayList.Items.Add(val);
            }else if (isAdd == 2)
            {
                lvArrayList.Items.Remove(val);
            }
        }

        public void RemUpdateArrayLV(string val)
        {
            int isAdd = Int32.Parse(htStep[val].ToString());

            if (isAdd == 1)
            {
                lvArrayList.Items.Remove(val);
            }
            else if (isAdd == 2)
            {
                lvArrayList.Items.Add(val);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //Reset
            Reset();
        }
    }
}
