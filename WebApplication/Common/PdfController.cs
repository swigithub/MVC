using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using RazorEngine;
using Encoding = System.Text.Encoding;
using iTextSharp.text.pdf.parser;
using Library.SWI.Survey.Model;
using System.Text.RegularExpressions;
using System.Net;

namespace SWI.AirView.Common
{
    public class CustomImageTagProcessor : iTextSharp.tool.xml.html.Image
    {
        public override IList<IElement> End(IWorkerContext ctx, Tag tag, IList<IElement> currentContent)
        {
            IDictionary<string, string> attributes = tag.Attributes;
            string src;
            if (!attributes.TryGetValue(HTML.Attribute.SRC, out src))
                return new List<IElement>(1);

            if (string.IsNullOrEmpty(src))
                return new List<IElement>(1);

            if (src.StartsWith("data:image/", StringComparison.InvariantCultureIgnoreCase))
            {
                // data:[<MIME-type>][;charset=<encoding>][;base64],<data>
                var base64Data = src.Substring(src.IndexOf(",") + 1);
                var imagedata = Convert.FromBase64String(base64Data);
                var image = iTextSharp.text.Image.GetInstance(imagedata);

                var list = new List<IElement>();
                var htmlPipelineContext = GetHtmlPipelineContext(ctx);
                list.Add(GetCssAppliers().Apply(new Chunk((iTextSharp.text.Image)GetCssAppliers().Apply(image, tag, htmlPipelineContext), 0, 0, true), tag, htmlPipelineContext));
                return list;
            }
            else
            {
                return base.End(ctx, tag, currentContent);
            }
        }
    }
    public class PdfController : Controller
    {
        public FileContentResult PDFView<T>(T model, string viewPath,string Page="Potrait")
        {
            TSS_SurveyDocument TempModel = model as TSS_SurveyDocument;
            var Title = TempModel.SurveyTitle + " " + TempModel.SiteCode.ToString();
            var html = WebUtility.HtmlDecode(GetHtml(model, viewPath));
            
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                    using (var doc = new Document())
                    {
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {
                            doc.Open();
                            try
                            {
                                var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                                tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                                tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                                CssFilesImpl cssFiles = new CssFilesImpl();
                                cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                                var cssResolver = new StyleAttrCSSResolver(cssFiles);
                                cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                                var charset = Encoding.UTF8;
                                var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                                hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                                var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, writer));
                                var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                                var worker = new XMLWorker(pipeline, true);
                                var xmlParser = new XMLParser(true, worker, charset);

                                xmlParser.Parse(new StringReader(html));
                            }
                        catch (Exception ex)
                        {
                            var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                            tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                            tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                            CssFilesImpl cssFiles = new CssFilesImpl();
                            cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                            var cssResolver = new StyleAttrCSSResolver(cssFiles);
                            cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                            var charset = Encoding.UTF8;
                            var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                            hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                            var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, writer));
                            var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                            var worker = new XMLWorker(pipeline, true);
                            var xmlParser = new XMLParser(true, worker, charset);
                            xmlParser.Parse(new StringReader("<p>Some Content Failed to Load!</p>"));
                        }
                            finally
                            {
                                doc.Close();
                            }
                        }
                }
                    bytes = ms.ToArray();

            }


            using (var reader = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader, ms))
                    {
                        
                        int PageCount = reader.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase(String.Format("Page {0} of {1}", i, PageCount), new Font(Font.FontFamily.HELVETICA, 8)), 560, 10, 0);
                         
                        }
                    }
                    bytes = ms.ToArray();
                }
            }
            return new FileContentResult(bytes, "application/pdf");
        }
     
        public byte[] PDFViewBytes<T>(T model, string viewPath)
        {
            var html = GetHtml(model, viewPath);
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        try
                        {
                            using (var msHtml = new MemoryStream(Encoding.UTF8.GetBytes(html)))
                            {
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance()
                                    .ParseXHtml(writer, doc, msHtml, Encoding.UTF8);
                            }
                        }
                        finally
                        {
                            doc.Close();
                        }
                    }
                }
                bytes = ms.ToArray();
            }
            return bytes;
        }
        public System.Drawing.Image GetImageFromBytes(byte[] bytes)
        {
            System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(bytes));
               return img;
        }

        private string GetHtml<T>(T model, string viewPath)
        {
            try
            {
                var physicalPath = Server.MapPath(viewPath);
                var html = System.IO.File.ReadAllText(physicalPath);
                return Razor.Parse(html, model);
            }
            catch(Exception ex){

                return "<html><body><div style='width:100%;text-align:center'><h1>Report Load Failed.Something Went Wrong!</h1></div></body></html>";
            }
        }

       

        public FileContentResult PDFViewMultiple<T>(List<T> models, string viewPath, string Page = "Potrait")
        {
            List<byte[]> listBytes=new List<byte[]>();
            int CheckModels = 0;
            
            TSS_SurveyDocument GetFirstSurvey = models[0] as TSS_SurveyDocument;
            string SiteSubCategory = GetFirstSurvey.SubCategory;
          
            float marginTop = 220;
            foreach (var model in models)
            {
                CheckModels++;
                TSS_SurveyDocument TempModel = model as TSS_SurveyDocument;

                var Title = TempModel.SurveyTitle + " " + TempModel.SiteCode.ToString();
                TempModel.ChecklistCount = CheckModels;
                var html = WebUtility.HtmlDecode(GetHtml(TempModel, viewPath));
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    using (var doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, marginTop, 10))
                    {
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {
                            doc.Open();
                            try
                            {
                                doc.NewPage();
                                doc.SetPageSize(PageSize.A4.Rotate());
                                var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                                tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                                tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                                CssFilesImpl cssFiles = new CssFilesImpl();
                                cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                                var cssResolver = new StyleAttrCSSResolver(cssFiles);
                                cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                                var charset = Encoding.UTF8;
                                var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                                hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                                var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, writer));
                                var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                                var worker = new XMLWorker(pipeline, true);
                                var xmlParser = new XMLParser(true, worker, charset);

                                xmlParser.Parse(new StringReader(html));
                            }
                            catch (Exception Ex)
                            {
                                doc.NewPage();
                                doc.SetPageSize(PageSize.A4.Rotate());
                                var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                                tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                                tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                                CssFilesImpl cssFiles = new CssFilesImpl();
                                cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                                var cssResolver = new StyleAttrCSSResolver(cssFiles);
                                cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                                var charset = Encoding.UTF8;
                                var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                                hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                                var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, writer));
                                var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                                var worker = new XMLWorker(pipeline, true);
                                var xmlParser = new XMLParser(true, worker, charset);
                                xmlParser.Parse(new StringReader("<p>Content Load Failed!</p>"));
                            }
                            finally
                            {
                                doc.Close();
                            }
                        }
                    }

                    bytes = ms.ToArray();

                }


                using (var reader = new PdfReader(bytes))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var stamper = new PdfStamper(reader, ms))
                        {

                            int PageCount = reader.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                            {
                                if (SiteSubCategory.ToUpper() == "CRAN" || SiteSubCategory.ToUpper() == "AIRSCALE")
                                {
                                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Content/Images/ClientLogo/verizon_logo_mini.png"));
                                    var pdfContentByte = stamper.GetOverContent(i);
                                    image.SetAbsolutePosition(305, 495);
                                    pdfContentByte.AddImage(image);
                                }
                                else
                                {
                                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Content/Images/ClientLogo/rsz_amp-slide-bkg.jpg"));
                                    var pdfContentByte = stamper.GetOverContent(i);
                                    image.SetAbsolutePosition(285, 465);
                                    pdfContentByte.AddImage(image);
                                }

                               
                                if (listBytes.Count == 0)
                                {
                                    if (i != 1 )
                                    {
                                        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER,
                                        //    new Phrase(Title, new Font(Font.FontFamily.UNDEFINED, 15, Font.BOLD)), 420, 430, 0);
                                    }
                                    else
                                    {
                                        if (SiteSubCategory.ToUpper() == "MICROWAVE" || SiteSubCategory.ToUpper()=="CORRECTION" || SiteSubCategory.ToUpper()=="MAINTENANCE" || SiteSubCategory.ToUpper() == "LOS COP" || SiteSubCategory.ToUpper()== "FASTBACK" )
                                        {
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER,
                                              new Phrase("Completed by AMP Communications LLC", new Font(Font.FontFamily.UNDEFINED, 15, Font.BOLD)), 430, 20, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER,
                                    //       new Phrase(Title, new Font(Font.FontFamily.UNDEFINED, 15, Font.BOLD)), 420, 430, 0);
                                }
                                //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase(String.Format("Page {0} of {1}", i, PageCount), new Font(Font.FontFamily.HELVETICA, 8)), 760, 10, 0);
                            }
                        }
                        listBytes.Add(ms.ToArray());
                    }
                }
            }
            byte[] Multiplebytes = CombineMultipleByteArrays(listBytes);

            return new FileContentResult(Multiplebytes, "application/pdf");
        }
        public static byte[] CombineMultipleByteArrays(List<byte[]> lstByteArray)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        foreach (var p in lstByteArray)
                        {
                            using (var reader = new PdfReader(p))
                            {
                                copy.AddDocument(reader);
                            }
                        }

                        doc.Close();
                    }
                }
                return ms.ToArray();
            }
        }
    }
   

}