using RYDEL.modelView;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Xml.Serialization;

namespace bSide.NMP.RYDEL.App_Code
{
    /// <summary>
    /// Clase de utilerias
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Obtiene el objeto 'partidaSaldos' de la sesión
        /// </summary>
        /// <returns></returns>
        internal static PartidaSaldos GetPartidaSaldosFromSession()
        {
					PartidaSaldos pSaldos = new PartidaSaldos();
					try
					{
						if (HttpContext.Current.Session["partidaSaldos"] == null || string.IsNullOrEmpty(HttpContext.Current.Session["partidaSaldos"].ToString()))
						{
							return null;
						}

						pSaldos = HttpContext.Current.Session["partidaSaldos"] as PartidaSaldos;
					} catch (Exception ex) {
						Debug.WriteLine("Error: " + ex.Message);
					}
					return pSaldos;
        }

        /// <summary>
        /// Guarda el objeto 'Partida Saldos' en sesión
        /// </summary>
        /// <param name="partidaSaldos"></param>
        internal static void SetPartidaSaldosToSession(PartidaSaldos partidaSaldos)
        {
            HttpContext.Current.Session["partidaSaldos"] = partidaSaldos;
        }

        /// <summary>
        /// Refresca el objeto 'partidaSaldos' en sesión
        /// </summary>
        internal static void ActualizaInfoPartida()
        {
            var ps = Utils.GetPartidaSaldosFromSession();
            if(ps != null)
                Utils.SetPartidaSaldosToSession(Services.GetPartidaCliente(ps.folio));
        }

        /// <summary>
        /// Obtiene el ID operacion (referencia) de la sesión
        /// </summary>
        /// <returns></returns>
        internal static long GetIdOperacionFromSession()
        {
            long idOp = 0;
            long.TryParse(HttpContext.Current.Session["reference"].ToString().Substring(0, 3), out idOp);
            return idOp;
        }
        
        /// <summary>
        /// Crea mascarilla con el nombre del cliente
        /// </summary>
        /// <param name="nombreCliente"></param>
        /// <returns></returns>
        internal static string GetMascaraNombreCliente(string nombreCliente)
        {
            StringBuilder mask = new StringBuilder();
            foreach (var item in nombreCliente.Split(' '))
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                mask.Append(item.Substring(0, 1));
                for (int i = 1; i < item.Length; i++)
                {
                    mask.Append("*");
                }
                mask.Append(" ");
            }

            return mask.ToString();
        }

        /// <summary>
        /// Crea imagen utilizada en el componente 'Captcha'
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        internal static byte[] CrearImagen(string cadena)
        {
            //string cadena = context.Request.QueryString["captcha"];

            int iHeight = 80;
            int iWidth = 410;
            Random oRandom = new Random();

            int[] aBackgroundNoiseColor = new int[] { 150, 150, 150 };
            int[] aTextColor = new int[] { 0, 0, 0 };
            int[] aFontEmSizes = new int[] { 15, 20, 25, 30, 35 };

            string[] aFontNames = new string[]
{
 "Comic Sans MS",
 "Arial",
 "Times New Roman",
 "Georgia",
 "Verdana",
 "Geneva"
};
            FontStyle[] aFontStyles = new FontStyle[]
{
 FontStyle.Bold,
 FontStyle.Italic,
 FontStyle.Regular,
 FontStyle.Strikeout,
 FontStyle.Underline
};
            HatchStyle[] aHatchStyles = new HatchStyle[]
{
 HatchStyle.BackwardDiagonal, HatchStyle.Cross,
    HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
 HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
    HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
 HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid,
    HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
 HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard,
    HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
 HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal,
    HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
 HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal,
    HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
 HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,
    HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
 HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis,
    HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
 HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
};

            //Get Captcha in Session
            string sCaptchaText = cadena;// context.Session["Captcha"].ToString();

            //Creates an output Bitmap
            Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Create a Drawing area
            RectangleF oRectangleF = new RectangleF(0, 0, iWidth, iHeight);
            Brush oBrush = default(Brush);

            //Draw background (Lighter colors RGB 100 to 255)
            oBrush = new HatchBrush(aHatchStyles[oRandom.Next
                (aHatchStyles.Length - 1)], Color.FromArgb((oRandom.Next(100, 255)),
                (oRandom.Next(100, 255)), (oRandom.Next(100, 255))), Color.White);
            oGraphics.FillRectangle(oBrush, oRectangleF);

            System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
            int i = 0;
            for (i = 0; i <= sCaptchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;
                int x = iWidth / (iChars + 1) * i;
                int y = iHeight / 2;

                //Rotate text Random
                oMatrix.RotateAt(oRandom.Next(-40, 40), new PointF(x, y));
                oGraphics.Transform = oMatrix;

                //Draw the letters with Random Font Type, Size and Color
                oGraphics.DrawString
                (
                //Text
                sCaptchaText.Substring(i, 1),
                //Random Font Name and Style
                new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                   aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                   aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                //Random Color (Darker colors RGB 0 to 100)
                new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
                   oRandom.Next(0, 100), oRandom.Next(0, 100))),
                x,
                oRandom.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }

            MemoryStream oMemoryStream = new MemoryStream();
            oOutputBitmap.Save(oMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] oBytes = oMemoryStream.GetBuffer();

            oOutputBitmap.Dispose();
            oMemoryStream.Close();

            //context.Response.BinaryWrite(oBytes);
            //context.Response.End();
            return oBytes;
        }
    }
}
