using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Words;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Path = System.IO.Path;

namespace CVParser.Helpers
{
    public class CVReader
	{
		private const int WatermarkSize = 14593;

		public static string[] GetContentFromPdf(Stream stream)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (PdfReader pdfReader = new PdfReader(stream))
            {
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    string thePage = PdfTextExtractor.GetTextFromPage(pdfReader, i, new SimpleTextExtractionStrategy());
                    string[] theLines = thePage.Split("\n");

                    foreach (string theLine in theLines)
                    {
                        if (!string.IsNullOrEmpty(theLine) && !string.IsNullOrWhiteSpace(theLine))
                        {
                            stringBuilder.AppendLine(theLine.Trim());
                        }
                    }
                }
            }

            return stringBuilder.ToString().Split("\r\n");
        }

        public static string[] GetContentFromDocAndTxt(Stream stream)
        {
            Stream pdfStream = Convert.ToPdfStream(stream);
            string[] lines = GetContentFromPdf(pdfStream);
            int indexOfWatermark = Array.FindIndex(lines, x => x.Contains("Pty Ltd", StringComparison.InvariantCultureIgnoreCase));
            List<string> listOfLines = lines.ToList();
            listOfLines.RemoveRange(0, indexOfWatermark + 1);

            return listOfLines.ToArray();
        }

        public static byte[] GetPictureFromPdf(Stream stream)
        {
            byte[] imageData = new byte[0];
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "wwwroot", "CV Parser", "Pictures");

            using (PdfReader pdfReader = new PdfReader(stream))
            {
                for (int i = 0; i < pdfReader.XrefSize; i++)
                {
                    PdfObject po = pdfReader.GetPdfObject(i);

                    if (po == null || !po.IsStream()) //object not found so continue
                        continue;

                    PRStream pst = (PRStream)po;
                    PdfObject type = pst.Get(PdfName.SUBTYPE); //get the object type
                                                               //check if the object is the image type object
                    if (type != null && type.ToString().Equals(PdfName.IMAGE.ToString()))
                    {
                        PdfImageObject pio = new PdfImageObject(pst);

                        int imageLength = pio.GetImageAsBytes().Length;

                        if (imageLength != WatermarkSize && imageLength > imageData.Length)
                        {
                            imageData = pio.GetImageAsBytes();
                        }
                    }
                }
            }

            return imageData;
        }
    }
}
