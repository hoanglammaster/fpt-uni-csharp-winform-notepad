using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotepadApplication
{
    public partial class Notepad : Form
    {
        const string DEFAULT_FILE_NAME = "Untitled";
        string filePath = DEFAULT_FILE_NAME; 
        
        public Notepad()
        {
            InitializeComponent();
        }
        //
        //Change color
        //
        private void changeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.ForeColor = colorDialog.Color;
            }
        }
        //
        //New
        //
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!filePath.Equals(DEFAULT_FILE_NAME))
            {
                try
                {
                    StreamReader sd = new StreamReader(filePath);
                    string textInFile = sd.ReadToEnd();
                    if (!textInFile.Equals(textBox1.Text))
                    {
                        confirmDialog(filePath);
                    }
                    else
                    {
                        clearPathAndTextBox();
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                if (textBox1.Text.Equals(""))
                {
                    clearPathAndTextBox();
                }
                else 
                {
                    confirmDialog(filePath);
                }
                
                
            }
        }
        //
        //Open
        //
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!filePath.Equals(DEFAULT_FILE_NAME))
            {
                try
                {
                    StreamReader sd = new StreamReader(filePath);
                    string textInFile = sd.ReadToEnd();
                    if (!textInFile.Equals(textBox1.Text))
                    {
                        confirmDialog(filePath);
                    }
                    openFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                if (!textBox1.Text.Equals(""))
                {
                    openFile();
                }
            }
        }
        //
        //Save
        //
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filePath.Equals(DEFAULT_FILE_NAME))
            {
                saveFileUseDialog();
            }
            else
            {
                writeTextToFile(filePath);
            }
        }
        //
        //Save as...
        //
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileUseDialog();
        }
        //
        //Exit
        //
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!filePath.Equals(DEFAULT_FILE_NAME))
            {
                StreamReader sd  = null;
                try
                {
                    sd = new StreamReader(filePath);
                    string textInFile = sd.ReadToEnd();
                    if (!textInFile.Equals(textBox1.Text))
                    {
                        confirmDialog(filePath);
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sd.Close();
                    this.Dispose();
                }
            }
            else
            {
                if (!textBox1.Text.Equals(""))
                {
                    confirmDialog(filePath);
                }
                this.Dispose();

            }
        }


        //
        //Customer code
        //
        private void saveFileUseDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "E:\\";
            saveFileDialog.Filter = "Text Document (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;
                writeTextToFile(saveFileDialog.FileName);
            }
            saveFileDialog.Dispose();
        }
        private void writeTextToFile(string fileName)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine(textBox1.Text);
                    sw.Close();
                }
                if (filePath.Equals(DEFAULT_FILE_NAME))
                {
                    fileName = "*Untitled";
                }
                else
                {
                    fileName = Path.GetFileName(filePath);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            this.Text = fileName + " - Notepad";
        }
        //
        //Change Background
        //
        private void changeBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.BackColor = colorDialog.Color;
            }
        }

        private void textInTextBox_Change(object sender, EventArgs e)
        {
            string fileName;
            if (filePath.Equals(DEFAULT_FILE_NAME))
            {
                fileName = "*Untitled";
            }
            else
            {
                fileName = Path.GetFileName(filePath);
            }

            this.Text = fileName + " - Notepad";
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripComboBox stripComboBox = (System.Windows.Forms.ToolStripComboBox)sender;
            
            textBox1.Font = new Font(textBox1.Font.FontFamily, Int32.Parse(stripComboBox.SelectedItem.ToString()));
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripItem toolStripItem = (System.Windows.Forms.ToolStripItem)sender;
            textBox1.Font = new Font(toolStripItem.Text, textBox1.Font.Size);
        }
        private void confirmDialog(string path)
        {
            string title = "Notepad";
            string message = "Do you want save changes to " + path;
            if (!MessageBoxManager.Abort.Equals("Save"))
            {
                MessageBoxManager.Abort = "Save";
                MessageBoxManager.Retry = "Don'tSave";
                MessageBoxManager.Ignore = "Cancel";
                MessageBoxManager.Register();
            }
            MessageBoxButtons buttons = MessageBoxButtons.AbortRetryIgnore;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Abort)
            {
                if (path.Equals(DEFAULT_FILE_NAME))
                {
                    saveFileUseDialog();
                }
                else
                {
                    writeTextToFile(Path.GetFileName(path));
                }

                clearPathAndTextBox();
            }
            else if (result == DialogResult.Retry)
            {
                clearPathAndTextBox();
            }
            else
            {
                return;
            }
        }
        private void clearPathAndTextBox()
        {
            filePath = DEFAULT_FILE_NAME;
            textBox1.Text = "";
        }
        private void openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "E:\\";
            openFileDialog.Filter = "Sql File(*.sql)|*.sql|Text File (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                var fileStream = openFileDialog.OpenFile();
                try
                {
                    System.IO.StreamReader reader = new StreamReader(fileStream);
                    textBox1.Text = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private System.Windows.Forms.ToolStripItem[] genarateFontFamily()
        {
            System.Windows.Forms.ToolStripItem[] listToolStripMenuItem;
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            FontFamily[] fontFamilies = installedFontCollection.Families;
            listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem[fontFamilies.Length];
            for (int i = 0; i < fontFamilies.Length; i++)
            {
                listToolStripMenuItem[i] = new System.Windows.Forms.ToolStripMenuItem();
                listToolStripMenuItem[i].Name = "newListToolStripMenuItem" + i;
                listToolStripMenuItem[i].Size = new System.Drawing.Size(163, 22);
                listToolStripMenuItem[i].Text = fontFamilies[i].Name;
                listToolStripMenuItem[i].Click += new System.EventHandler(this.changeFontToolStripMenuItem_Click);
            }

            return listToolStripMenuItem;
        }
        private object[] genarateFontSize()
        {
            return new object[] {"8","9","10","11","12","14","16","18","20","22","24","26","28","30","32","34","36","38","40","62"};
        }
    }
}
