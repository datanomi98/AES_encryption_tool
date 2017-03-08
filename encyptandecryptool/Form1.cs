using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace encyptandecryptool
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();


        }
        public byte[] AES_Salaus(byte[] bytesencrypted, byte[] salasanabytes)
        {
            byte[] encyptedbytes = null;

            //the salt bytes must be at least 8 bytes
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(salasanabytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesencrypted, 0, bytesencrypted.Length);
                        cs.Close();

                    }
                    encyptedbytes = ms.ToArray();
                }
                return encyptedbytes;
            }
        }
        public byte[] AES_salauksenpurku(byte[] bytesDecrypted, byte[] salasanabytes)
        {

            byte[] decryptedbytes = null;


            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(salasanabytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesDecrypted, 0, bytesDecrypted.Length);
                        cs.Close();

                    }
                    decryptedbytes = ms.ToArray();
                }

            }
            return decryptedbytes;
        }
        public void EncryptFile(string inputFile, string outputFile)
        {
            try
            {


                //Password for the file
                string salasana = textBox2.Text;

                byte[] bytestoencrypted = File.ReadAllBytes(inputFile);
                byte[] salasanabytes = Encoding.UTF8.GetBytes(salasana);

                //hash the password with sha256
                salasanabytes = SHA256.Create().ComputeHash(salasanabytes);

                byte[] bytessalattu = AES_Salaus(bytestoencrypted, salasanabytes);

                File.WriteAllBytes(outputFile, bytessalattu);
                richTextBox1.Text = "salauksen teko onnistui\n encryption succeeded";

            }
            catch (Exception e)
            {
                richTextBox1.Text = "Jotain meni pileen\n something went wrong";
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog avaaTiedosto = new OpenFileDialog();

            if (avaaTiedosto.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = avaaTiedosto.FileName;
            }

        }

        private void DecryptFile(string inputFile, string outputFile)
        {
            try
            {



                string password = textBox2.Text;

                byte[] bytesToBeDecrypted = File.ReadAllBytes(outputFile);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES_salauksenpurku(bytesToBeDecrypted, passwordBytes);


                File.WriteAllBytes(outputFile, bytesDecrypted);
                richTextBox1.Text = "salauksen purku onnistui\n Decryption succeeded";


            }
            catch (Exception e)
            {
                richTextBox1.Text = "Jotain meni pileen\n something went wrong";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string inputpath = textBox1.Text;
            string outputpath = inputpath;
            EncryptFile(inputpath, outputpath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string inputpath = textBox1.Text;
            string outputpath = inputpath;
            DecryptFile(inputpath, outputpath);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }



        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        class globvar
        {
            public static string folder2;

        }
        public void button4_Click_1(object sender, EventArgs e)
        {
          
        

        
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folder = folderBrowserDialog1.SelectedPath;
            string[] kek = Directory.GetFileSystemEntries(folder);
              
                foreach (string kek2 in kek)
                {

                    globvar.folder2 = kek2;
                    
                         
                }
              
            }
    
}
      
        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folder = folderBrowserDialog1.SelectedPath;
                string[] kek = Directory.GetFileSystemEntries(folder);

                foreach (string kek2 in kek)
                {

                    EncryptFile(kek2, kek2);
                  

                }

            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folder = folderBrowserDialog1.SelectedPath;
                string[] kek = Directory.GetFileSystemEntries(folder);

                foreach (string kek2 in kek)
                {

                    DecryptFile(kek2, kek2);

                }

            }

        }
    }

}
   

        

       

       
    
    
     

