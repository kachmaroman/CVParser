using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aspose.Words;
using Aspose.Words.Saving;

namespace CVParser.Helpers
{
    public static class Convert
    {
        public static Stream ToPdfStream(Stream stream)
        {
            byte[] byteContent;

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byteContent = ms.ToArray();
            }

            Document loadedFromBytes = new Document(new MemoryStream(byteContent));
            MemoryStream pdfStream = new MemoryStream();

            loadedFromBytes.Save(pdfStream, new PdfSaveOptions{SaveFormat = SaveFormat.Pdf, Compliance = PdfCompliance.PdfA1b});
            byte[] pdfBytes = pdfStream.ToArray();

            return new MemoryStream(pdfBytes);
        }
    }
}
