using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMS.TPS.Common.Model.API;

namespace june19
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : UserControl
    {
        public List<string> vs1 = new List<string>();
        public List<string> vs2 = new List<string>();
        public StructureSet structureSet { get;set; }

        public UI(ScriptContext context)
        {
            InitializeComponent();            
            Patient patient = context.Patient;
            patient.BeginModifications();
            StructureSet structureSet1 = patient.StructureSets.First();
            structureSet = structureSet1;
            Structure[] newS = structureSet.Structures.ToArray();


            foreach(var xx in newS)
            {
                LSTBX.Items.Add(xx.Name);
            }
            
            foreach(var xxx in abcd.Items)
            {
                vs1.Add(xxx.ToString());
            }               
            
            


           
        }
        

        
        private void PtvAdd_Click(object sender, RoutedEventArgs e)
        {
            selected_ptv.Text = LSTBX.SelectedItem.ToString();           

        }

        private void OarAdd_Click(object sender, RoutedEventArgs e)
        {
            abcd.Items.Add(LSTBX.SelectedItem);
        }
        
        private void clear_ptv_Click(object sender, RoutedEventArgs e)
        {
            selected_ptv.Clear();
        }

        private void clear_oar_Click(object sender, RoutedEventArgs e)
        {
            abcd.Items.Clear();
        }

        private void dup_button_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (var xxx in abcd.Items)
            {
                vs1.Add(xxx.ToString());
            }
            vs1.Add(selected_ptv.Text);
            Dup_struc(vs1, structureSet);
            MessageBox.Show("Congrats! Duplication complete.");
            foreach(var x1 in vs2)
            {
                if (x1 != selected_ptv.Text + "_d") { dup_oar.Items.Add(x1); }
                else { dup_ptv.Text = selected_ptv.Text + "_d"; }
                
            }           

        }

        private void crop_button_Click(object sender, RoutedEventArgs e)
        {
            foreach(var i in abcd.Items)
            {
                Crop_OAR(i.ToString());
            }

            Crop_PTV(selected_ptv.Text);
            MessageBox.Show("Duplicated structures cropped !!");
        }

        private void rmv_struc_Click(object sender, RoutedEventArgs e)
        {
            foreach (Structure x in structureSet.Structures)
            {
                if (x.IsEmpty)  {  vs1.Add(x.Name); }
            }
            string yy = "";
            foreach (string y in vs1)
            {
                Structure S = structureSet.Structures.FirstOrDefault(s => s.Name.Equals(y));
                structureSet.RemoveStructure(S);
                yy = yy + y + "\n\n";
            }
            int c = vs1.Count;
            MessageBox.Show("Following " + c.ToString() + " empty structures were removed:\n\n" + yy);
        }






        // Following are the functions:



        public void Dup_struc(List<string> vs, StructureSet set)
        {
            foreach (var xxxx in vs)
            {                
                foreach (var y in set.Structures.ToArray())
                {
                    if (y.Name != "BODY" && y.Name == xxxx)
                    {                        
                        string yy = y.Name + "_d";
                        set.AddStructure(y.DicomType, yy);
                        vs2.Add(yy);
                    }
                    
                }
            }
            
            
        }         
        public void Crop_OAR(String stru_name)
        {
            double x;
            x = 3;

            Structure SS0 = structureSet.Structures.FirstOrDefault(s => s.Name.Equals(stru_name));            
            structureSet.AddStructure(SS0.DicomType,"dummy1");
            structureSet.AddStructure(SS0.DicomType,"dummy2");            
            Structure SS1 = structureSet.Structures.FirstOrDefault(s => s.Name.Equals("dummy1"));
            Structure SS2 = structureSet.Structures.FirstOrDefault(s => s.Name.Equals("dummy2"));
            Structure SS3 = structureSet.Structures.FirstOrDefault(s => s.Name.Equals(selected_ptv.Text));            
            SS1.SegmentVolume = SS3.SegmentVolume.Margin(x);
            SS2.SegmentVolume = SS1.SegmentVolume.Not();
            Structure SS = structureSet.Structures.FirstOrDefault(s => s.Name.Equals(stru_name+"_d"));
            SS.SegmentVolume = SS2.SegmentVolume.And(SS0.SegmentVolume);
            structureSet.RemoveStructure(SS1);
            structureSet.RemoveStructure(SS2);

        }

        public void Crop_PTV(string stru_name)
        {
            double x;
            x = -3;

            Structure SS0 = structureSet.Structures.FirstOrDefault(s => s.Name.Equals("BODY"));
            structureSet.AddStructure("Organ", "dummy1");
            structureSet.AddStructure("Organ", "dummy2");
            Structure SS1 = structureSet.Structures.FirstOrDefault(s => s.Name.Equals("dummy1"));
            Structure SS2 = structureSet.Structures.FirstOrDefault(s => s.Name.Equals("dummy2"));
            SS1.SegmentVolume = SS0.SegmentVolume;
            SS2.SegmentVolume = SS1.SegmentVolume.Margin(x);
            Structure SS = structureSet.Structures.FirstOrDefault(s => s.Name.Equals(selected_ptv.Text));
            Structure S = structureSet.Structures.FirstOrDefault(s => s.Name.Equals(dup_ptv.Text));
            S.SegmentVolume = SS2.SegmentVolume.And(SS.SegmentVolume);
            structureSet.RemoveStructure(SS1);
            structureSet.RemoveStructure(SS2);

        }

        
    }
}
