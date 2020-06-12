//using System.DrawingCore;

namespace WebAppCoreBlazorServer.Common
{
    public class PdfHelper
    {

        //public List<string> CreateImageFromPdf(string url)
        //{
        //    string[] fileName = new string[1000];
        //    SautinSoft.PdfFocus pdf = new SautinSoft.PdfFocus();
        //    pdf.OpenPdf(url);
        //    if (pdf.PageCount > 0) {
        //        var folder = Path.GetDirectoryName(url);
        //        //var filePath = Path.Combine(uploads, file.FileName);
        //        if (!Directory.Exists(folder)) {
        //            Directory.CreateDirectory(folder);
        //        }
        //        var name = Path.GetFileNameWithoutExtension(url);
        //        pdf.ToImage(folder, name, out fileName);
        //    }
        //    if (fileName.Length == 0) {
        //        return null;
        //    }
        //    return fileName.Where(x => !string.IsNullOrEmpty(x)).ToList();
        //}

        //public void ExtractJpeg(string file,string fileNameReplace="")
        //{
        //    try {
        //        string nameOld = "";
        //        if (!string.IsNullOrEmpty(fileNameReplace)) {
        //            fileNameReplace = fileNameReplace.Trim(',');
        //            fileNameReplace = Path.GetFileName(fileNameReplace);
        //            fileNameReplace = fileNameReplace.Replace(".pdf", ".jpg");
        //            if (fileNameReplace.Length>3) {
        //                var tempName = Path.GetFileNameWithoutExtension(fileNameReplace);
        //                nameOld = tempName.Substring(0,tempName.Length-4)+".jpg";
        //            }

        //        }
        //        var dir1 = Path.GetDirectoryName(file);
        //        var fn = string.IsNullOrEmpty(fileNameReplace)? Path.GetFileNameWithoutExtension(file): Path.GetFileNameWithoutExtension(nameOld);
        //        var dir2 = Path.Combine(dir1, fn);
        //        if (!Directory.Exists(dir2))
        //            Directory.CreateDirectory(dir2);
        //        using (var pdf = new PdfReader(file)) {
        //            int n = pdf.NumberOfPages;
        //            for (int i = 1; i <= n; i++) {
        //                var pg = pdf.GetPageN(i);
        //                var res = PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES)) as PdfDictionary;
        //                var xobj = PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT)) as PdfDictionary;
        //                if (xobj == null) continue;

        //                var keys = xobj.Keys;
        //                if (keys.Count == 0) continue;

        //                var obj = xobj.Get(keys.ElementAt(0));
        //                if (!obj.IsIndirect()) continue;

        //                var tg = PdfReader.GetPdfObject(obj) as PdfDictionary;
        //                var type = PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE)) as PdfName;
        //                if (!PdfName.IMAGE.Equals(type)) continue;
        //                int XrefIndex = (obj as PRIndirectReference).Number;
        //                var pdfStream = pdf.GetPdfObject(XrefIndex) as PRStream;
        //                var data = PdfReader.GetStreamBytesRaw(pdfStream);

        //                var jpeg = Path.Combine(dir2, !string.IsNullOrEmpty(fileNameReplace) ? fileNameReplace : string.Format("{0}_{1}.jpg", Path.GetFileNameWithoutExtension(file), i.ToString("D3")));
        //                File.WriteAllBytes(jpeg, data);
        //            }
        //        }
        //    }
        //    catch (Exception e) {

        //    }
        //}
        //public void ExtractImage(string file)
        //{

        //}
        //public void SplitePDF(string filepath)
        //{
        //    iTextSharp.text.pdf.PdfReader reader = null;
        //    int currentPage = 1;
        //    int pageCount = 0;
        //    //string filepath_New = filepath + "\\PDFDestination\\";

        //    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //    //byte[] arrayofPassword = encoding.GetBytes(ExistingFilePassword);
        //    reader = new iTextSharp.text.pdf.PdfReader(filepath);
        //    reader.RemoveUnusedObjects();
        //    pageCount = reader.NumberOfPages;
        //    string ext = System.IO.Path.GetExtension(filepath);
        //    for (int i = 1; i <= pageCount; i++) {
        //        iTextSharp.text.pdf.PdfReader reader1 = new iTextSharp.text.pdf.PdfReader(filepath);
        //        string outfile = filepath.Replace((System.IO.Path.GetFileName(filepath)), (System.IO.Path.GetFileName(filepath).Replace(".pdf", "") + "_" + i.ToString()) + ext);
        //        reader1.RemoveUnusedObjects();
        //        iTextSharp.text.Document doc = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(currentPage));
        //        iTextSharp.text.pdf.PdfCopy pdfCpy = new iTextSharp.text.pdf.PdfCopy(doc, new System.IO.FileStream(outfile, System.IO.FileMode.Create));
        //        doc.Open();
        //        for (int j = 1; j <= 1; j++) {
        //            iTextSharp.text.pdf.PdfImportedPage page = pdfCpy.GetImportedPage(reader1, currentPage);
        //            //pdfCpy.SetFullCompression();
        //            pdfCpy.AddPage(page);
        //            currentPage += 1;
        //        }
        //        doc.Close();
        //        pdfCpy.Close();
        //        reader1.Close();
        //        reader.Close();

        //    }
        //}
        //public void ConvertImageToPdf(string srcFilename, string dstFilename)
        //{
        //    iTextSharp.text.Rectangle pageSize = null;

        //    using (var srcImage = new Bitmap(srcFilename)) {
        //        pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
        //    }
        //    using (var ms = new MemoryStream()) {
        //        var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
        //        iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
        //        document.Open();
        //        var image = iTextSharp.text.Image.GetInstance(srcFilename);
        //        document.Add(image);
        //        document.Close();
        //        File.WriteAllBytes(dstFilename, ms.ToArray());
        //    }
        //}
    }
}
