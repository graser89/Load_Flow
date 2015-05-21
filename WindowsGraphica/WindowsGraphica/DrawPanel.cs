using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Drawing;


namespace WindowsGraphica
{                        
    class DrawPanel:Panel,IXmlControl
    {
        private string xmlNameElement = "DrawPanel";

        public string XmlNameElement
        {
            get { return xmlNameElement; }
        }


        public new bool DoubleBuffered
        {
            get 
            {
                return base.DoubleBuffered;
            }
            set 
            {
                base.DoubleBuffered = value;
            }
        }
        private float holst_width;

        public float Holst_width
        {
            get { return holst_width; }
            set { holst_width = value; }
        }
        private float holst_height;

        public float Holst_height
        {
            get { return holst_height; }
            set { holst_height = value; }
        }
        
        public  void Scale(float scale)
        {
            base.Size = new System.Drawing.Size((int)(Holst_width * scale), (int)(Holst_width * scale));
            //base.Scale(new System.Drawing.SizeF(Holst_width*scale,Holst_width*scale));            
        }


        public void SaveToXml(XmlTextWriter XmlOut)
        {

            XmlOut.WriteStartElement(XmlNameElement);
            XmlOut.WriteAttributeString("holst_width", holst_width.ToString());
            XmlOut.WriteAttributeString("holst_height", holst_height.ToString());
            XmlOut.WriteAttributeString("BackColor", BackColor.ToArgb().ToString());
            XmlOut.WriteAttributeString("X", Location.X.ToString());
            XmlOut.WriteAttributeString("Y", Location.Y.ToString());
            XmlOut.WriteAttributeString("Name", Name.ToString());
            XmlOut.WriteAttributeString("TabIndex", TabIndex.ToString());
            XmlOut.WriteAttributeString("AutoSize",AutoSize.ToString());

            XmlOut.WriteEndElement();
        }


        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                holst_width = (float)Convert.ToDecimal(xmlIn.GetAttribute("holst_width"));
                holst_height = (float)Convert.ToDecimal(xmlIn.GetAttribute("holst_height"));                                
                BackColor = Color.FromArgb(Convert.ToInt32(xmlIn.GetAttribute("BackColor")));
                int x=  Convert.ToInt32(xmlIn.GetAttribute("X"));
                int y = Convert.ToInt32(xmlIn.GetAttribute("Y"));
                Location = new Point(x, y);
                Name = Convert.ToString(xmlIn.GetAttribute("Name"));
                TabIndex = Convert.ToInt32(xmlIn.GetAttribute("TabIndex"));
                AutoSize = Convert.ToBoolean(xmlIn.GetAttribute("AutoSize"));
            }
            catch (Exception)
            { }
        }

    }
}
