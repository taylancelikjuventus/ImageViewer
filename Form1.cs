using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewerApplication
{
    public partial class frm : Form
    {
        public frm()
        {
            InitializeComponent();
        }

        /*******To store fullpaths of selected image files from file dialog*********/
        public static List<string> filenames = new List<string>();

        private Image img ;

       


        /**********if Open is clicked*************/
        private void btnOpen_Click(object sender, EventArgs e)
        {


            //Filter fileDialog tan sadece .jpg uzantılı imajların görünmesini saglar.
            //multiselection = true birden fazla seçim yapılabilmesini saglar.
            // using(OpenFileDialog ofd = new OpenFileDialog (){Multiselect=true,ValidateNames=true,Filter="JPEG|*.jpg"})

            //Filter : Filter="Image Files|*.jpg" ,Here "Image Files|" is the text you want to show on Extension Combobox of file Dialog. 
            //Extensions must be given like this  Filter="Image Files|*.jpg;*.png;*.bmp;*.gif" ,
             // For all files Filter="*.*" , here first '*' is filename and socund '*' is any type of extension.
            using (OpenFileDialog ofd = new OpenFileDialog (){Multiselect=true,ValidateNames=true,Filter="Image Files|*.jpg;*.gif;*.png"}) //we can add much more by seperating extensions with ";" .
            {
               if(ofd.ShowDialog() == DialogResult.OK)
                {
                    //clear list and listview Items
                    filenames.Clear();
                    listView1.Items.Clear();

                    // if user wants to open multiple files once
                    //We get those selected files from ofd.FileNames !
                    //ofd.FileNames seçilen klasörün içindeki tüm dosyaları jpg,png,txt
                    //indexte tutuyor!BU KISMI DUZELT
                    foreach ( string filename in ofd.FileNames )
                    {
                        //get Info about file
                        FileInfo finfo = new FileInfo(filename);

                        //Check
                        Console.WriteLine(filename);

                       int  startIndex = listView1.Items.Count; ;

                        //add each selected file to filenames list
                        filenames.Add(finfo.FullName); //adds full path of selected file
                                                           //we add each file name to listView
                        listView1.Items.Add(finfo.Name, 0); //list view da sadece dosya ismi var
                                                            //fullpath leri filenames List inde saklıyoruz          
                                                            //listView1 daki index sırasıyla filenames teki
                                                            //index sırası AYNIDIR.
                        
                    }


                }


            }
        }

        /********if user clickes on any item in listview*********/
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if(listView1.FocusedItem != null) //if any item is focused
            {
                //open a new Viewer by using 

                using (viewer viewer = new viewer(listView1.FocusedItem.Index))
                {
                    //listView1 deki dosya isminin indexi ile filenames(List<string>) deki 
                    // fullpath 'in indexi aynıdır.
                    img = Image.FromFile( filenames[listView1.FocusedItem.Index] );
                    

                    //set image to pictureBox via using our created SETTER
                    viewer.ImageBox = img;

                    //viewer.Show(); //Pencereyi acar
                    viewer.ShowDialog(); // pencereyi Modal olarak açar.Yani bu pencere kapatılmadanda
                        //altındaki pencere kullanılabilir.
                }



            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            img.Dispose();
           
            List<string> namelist = new List<string>();

            if(listView1.SelectedItems.Count > 0)
            {

                //LISTVIEW dan item silerken BU SEKILDE SIL!!!
                //listView1.SelectedItems tüm seçilmiş itemleri tutar.
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    item.Remove(); 
                }

                Console.WriteLine();
            }


        }

        private void frm_Load(object sender, EventArgs e)
        {
            //Allow multi selection for listView1
            listView1.MultiSelect = true;
        }


    }
}
