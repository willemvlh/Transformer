﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using ScintillaNET;

namespace Transformer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void setScintillaStyle(Scintilla scintilla)
        {
            // Set the XML Lexer
            scintilla.Lexer = Lexer.Xml;

            // Show line numbers
            scintilla.Margins[0].Width = 30;

            // Enable folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");
            scintilla.SetProperty("fold.html", "1");
            scintilla.SetProperty("read.only", "1");

            // Use Margin 2 for fold markers
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Reset folder markers
            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Style the folder markers
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;

            // Set the Styles
            scintilla.StyleResetDefault();
            // I like fixed font for XML
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();
            scintilla.Styles[Style.Xml.Attribute].ForeColor = Color.Red;
            scintilla.Styles[Style.Xml.Entity].ForeColor = Color.Red;
            scintilla.Styles[Style.Xml.Comment].ForeColor = Color.Green;
            scintilla.Styles[Style.Xml.Tag].ForeColor = Color.Blue;
            scintilla.Styles[Style.Xml.TagEnd].ForeColor = Color.Blue;
            scintilla.Styles[Style.Xml.DoubleString].ForeColor = Color.DeepPink;
            scintilla.Styles[Style.Xml.SingleString].ForeColor = Color.DeepPink;
        }

        public string chooseFile(string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = filter;
            ofd.InitialDirectory = "C:\\";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }

            return "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Scintilla[] sc = new Scintilla[] { scintilla1, scintilla2, scintilla3 };
            for (int i = 0; i < sc.Length; i++)
            {
                setScintillaStyle(sc[i]);
            }
        }

        private void scintilla1_Click(object sender, EventArgs e)
        {

        }

        private void scintilla2_Click(object sender, EventArgs e)
        {
            
        }

        private void printOutput(MemoryStream stream, Scintilla sc)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string output = reader.ReadToEnd();
            sc.Text = output;
        }

        private void callTransform()
        {
            TextReader source = new StringReader(scintilla2.Text);
            TextReader xsl = new StringReader(scintilla1.Text);
            Console.Write(source.ToString(), xsl.ToString());
            Transformer xformer = new Transformer();
          
                MemoryStream stream = new MemoryStream();
                StreamWriter writer =  xformer.createWriter(stream); // writer will write to the stream
            if (xformer.Transform(XmlReader.Create(source), XmlReader.Create(xsl), writer))
            {
                // print output to tabpage
                printOutput(stream, scintilla3);

                //show msgbox
                //MessageBox.Show("Success!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabControl1.SelectTab(2);
            }
            

               
            }

            

        private void transformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            callTransform();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 about = new Form2();
            about.Show();
        }

        private void loadStylesheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chooseFile("XSLT Stylesheets (*.xsl;*.xslt)|*.xsl;*.xslt") ;
        }

        private void loadSourceXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chooseFile("XML Documents (*.xml)|*.xml");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            options.Show();
        }
    }

}
