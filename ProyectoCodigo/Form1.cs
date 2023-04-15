using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//AÑADIR LA REFERENCIA BARCODELIB
using BarcodeLib;

namespace ProyectoCodigo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //1.- CREAR UNA CLASE PARA RELLENAR LOS ITEMS DEL COMBO BOX
        public class OpcionCombo {
            public int Valor { get; set; }
            public string Texto { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //2.- CARGAR EL COMBO CON LOS TIPO DE BARCODE

            int indice = 0;
            foreach (var nombre in Enum.GetNames(typeof(BarcodeLib.TYPE))) 
            {
                comboBox1.Items.Add(new OpcionCombo() { Valor = indice, Texto = nombre });
                indice++;
            }

            comboBox1.DisplayMember = "Texto";
            comboBox1.ValueMember = "Valor";
            comboBox1.SelectedIndex = 0;

        }

        private void btnver_Click(object sender, EventArgs e)
        {
            Image imagenCodigo;


            int indice = (comboBox1.SelectedItem as OpcionCombo).Valor;
            BarcodeLib.TYPE tipoCodigo = (BarcodeLib.TYPE)indice;


            Barcode codigo = new Barcode();
            codigo.IncludeLabel = true;
            codigo.LabelPosition = LabelPositions.BOTTOMCENTER;

            //AQUI PASALE EL TEXTO DEL TXT  "CODIGO"
            imagenCodigo = codigo.Encode(tipoCodigo, textBox2.Text.Trim(), Color.Black, Color.White, 300, 100);




            //EXTRA Y AL ULTIMO
            Bitmap imagenTitulo = convertirTextoImagen(textBox1.Text.Trim(),300,Color.White);

            int alto_imagen_nuevo = imagenCodigo.Height + imagenTitulo.Height;

            Bitmap imagenNueva = new Bitmap(300, alto_imagen_nuevo);
            Graphics dibujar = Graphics.FromImage(imagenNueva);

            dibujar.DrawImage(imagenTitulo, new Point(0, 0));
            dibujar.DrawImage(imagenCodigo, new Point(0, imagenTitulo.Height));


           
            //pictureBox1.BackgroundImage = imagenCodigo;
            pictureBox1.BackgroundImage = imagenNueva;
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            Image imagen_codigo = pictureBox1.BackgroundImage.Clone() as Image;

            SaveFileDialog ventana_dialogo = new SaveFileDialog();
            ventana_dialogo.FileName = string.Format("{0}.png", textBox2.Text.Trim());
            ventana_dialogo.Filter = "Imagen|*.png";

            if (ventana_dialogo.ShowDialog() == DialogResult.OK)
            {
                imagen_codigo.Save(ventana_dialogo.FileName, ImageFormat.Png);
                MessageBox.Show("Codigo generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }


        public static Bitmap convertirTextoImagen(string texto,int ancho, Color color)
        {
            //creamos el objeto imagen Bitmap
            Bitmap objBitmap = new Bitmap(1, 1);
            int Width = 0;
            int Height = 0;
            //formateamos la fuente (tipo de letra, tamaño)
            System.Drawing.Font objFont = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            //creamos un objeto Graphics a partir del Bitmap
            Graphics objGraphics = Graphics.FromImage(objBitmap);

            //establecemos el tamaño según la longitud del texto
            Width = ancho;
            Height = (int)objGraphics.MeasureString(texto, objFont).Height + 5;
            objBitmap = new Bitmap(objBitmap, new Size(Width, Height));

            objGraphics = Graphics.FromImage(objBitmap);

            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            
            StringFormat drawFormat = new StringFormat();
            objGraphics.Clear(color);

            drawFormat.Alignment = StringAlignment.Center;
            objGraphics.DrawString(texto, objFont, new SolidBrush(Color.Black), new RectangleF(0, (objBitmap.Height / 2) - (objBitmap.Height - 10), objBitmap.Width, objBitmap.Height), drawFormat);
            objGraphics.Flush();
            

            return objBitmap;
        }



    }
}
