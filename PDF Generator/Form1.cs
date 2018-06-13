using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Rectangle = iTextSharp.text.Rectangle;

namespace PDF_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const char RightToLeftEmbedding = (char)0x202B;
        const char PopDirectionalFormatting = (char)0x202C;
        readonly int cellHeight = 35;
        private iTextSharp.text.Font _myFont;
        private iTextSharp.text.Font _myFontBold;
        private static string FixWeakCharacters(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;
            var weakCharacters = new[] { @"\", "/", "+", "-", "=", ";", "$" };
            foreach (var weakCharacter in weakCharacters)
            {
                data = data.Replace(weakCharacter, RightToLeftEmbedding + weakCharacter + PopDirectionalFormatting);
            }

            return data;
        }

        private static iTextSharp.text.Font GetFont(string fontName, int fontSize)
        {
            if (!FontFactory.IsRegistered(fontName))

            {
                FontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\" + fontName +
                                     ".ttf");
            }

            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED, fontSize);
        }

        private class CustomDashedLineSeparator : DottedLineSeparator
        {
            private float _dash = 5;
            private float _phase = 2.5f;

            public float GetDash()
            {
                return _dash;
            }

            public float GetPhase()
            {
                return _phase;
            }

            public void SetDash(float dash)
            {
                this._dash = dash;
            }

            public void SetPhase(float phase)
            {
                this._phase = phase;
            }

            public void DrawLine(PdfContentByte canvas, float llx, float lly, float urx, float ury, float y)
            {
                canvas.SaveState();
                canvas.SetLineWidth(lineWidth);
                canvas.SetLineDash(_dash, gap, _phase);
                DrawLine(canvas, llx, urx, y);
                canvas.RestoreState();
            }
        }

        private void Btn_PDFGen_Click(object sender, EventArgs e)
        {
            //var m = new ITModel.ITModelContainer();
            //var list = (from pp in m.PERSONNELs
            //    select pp).ToList();

            using (Document pdfDoc = new Document(PageSize.A4))
            {
                _myFont = GetFont("Parastoo-Print", 14);
                _myFontBold = GetFont("Parastoo-Print", 16);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream("Test.pdf", FileMode.Create));
                var readPassword = Encoding.UTF8.GetBytes(""); //it can be null.
                var editPassword = Encoding.UTF8.GetBytes("456");
                int permissions = PdfWriter.ALLOW_PRINTING | PdfWriter.ALLOW_COPY;
                pdfWriter.SetEncryption(readPassword, editPassword, permissions, PdfWriter.STRENGTH128BITS);


                //pdfWriter.PageEvent = new PageEvents();

                pdfDoc.Open();

                pdfDoc.AddAuthor("AddAuthor");
                pdfDoc.AddCreator("AddCreator");
                pdfDoc.AddTitle("AddTitle");
                pdfDoc.AddSubject("AddSubject");
                pdfDoc.AddKeywords("iTextSharp,C#");

                float[] widths = new float[] { 1.5f, 5f, 1.5f };

                PdfPTable headerTb = new PdfPTable(widths)
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    WidthPercentage = 100,
                    DefaultCell = { Border = Rectangle.NO_BORDER }
                };
                //headerTb.DefaultCell.Border = Rectangle.NO_BORDER;

                #region Det

                PdfPTable formDetail = new PdfPTable(2) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };


                formDetail.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("تاریخ:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER

                });
                formDetail.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("[date]"), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER
                });


                formDetail.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("شماره:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER

                });
                formDetail.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("[number]"), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER

                });


                formDetail.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("پیوست:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER

                });
                formDetail.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("[index]"), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER

                });


                headerTb.AddCell(formDetail);


                #endregion

                #region Header

                headerTb.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("بسمه تعالی"), _myFontBold),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_BOTTOM,
                    Border = Rectangle.NO_BORDER

                });

                #endregion

                #region Logo

                iTextSharp.text.Image logoPng = iTextSharp.text.Image.GetInstance("logo.png");

                logoPng.ScaleToFit(50f, 50f);
                logoPng.Alignment = Element.ALIGN_LEFT;



                headerTb.AddCell(new PdfPCell()
                {
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Image = logoPng,
                    Border = Rectangle.NO_BORDER
                });

                #endregion

                #region Parageraph


                string firstParaStr = "CompanyName" + " بیمه‌گذارِ بیمه‌نامه شماره " + "InsuranceNumber" +
                                      " اینجانب " + "ExpertName" +
                                      " نمایندگی " + "Namayandegi" +
                                      " کد " + "CodeNamayandegi" +
                                      " به اتفاق نماینده صاحب کالا  به محل بارگیری محل بارگیری به نشانی " + "Address" +
                                      " مراجعه و از کالای آماده حمل با مشخصات زیر بازدید نمودم که نتیجه بازدید و نظر اینجانب به شرح ذیل می‌باشد:";


                PdfPTable matnTb = new PdfPTable(1)
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    WidthPercentage = 100
                };


                matnTb.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters("گزارش بازدید اولیه، بیمه مسئولیت متصدیان حمل و نفل داخلی"), GetFont("Parastoo-Print", 16)),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_TOP,
                    BorderColorBottom = BaseColor.WHITE,
                    Border = Rectangle.NO_BORDER

                });
                matnTb.AddCell(new PdfPCell()
                {
                    Phrase = new Phrase(FixWeakCharacters(" بازگشت به درخواست مورخ " + "date" + " شرکت حمل و نقل "),
                        GetFont("Parastoo-Print", 15)),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_TOP,
                    Border = Rectangle.NO_BORDER

                });


                matnTb.AddCell(
                    new PdfPCell(
                        new Paragraph(new Chunk(firstParaStr, _myFont).setLineHeight(25)))
                    {
                        Border = Rectangle.NO_BORDER,
                        MinimumHeight = 150
                    });




                #endregion


                #region CheckList

                PdfPTable checkListTb = new PdfPTable(2)
                {
                    ExtendLastRow = true,
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    WidthPercentage = 100,

                };


                checkListTb.AddCell(new PdfPCell()
                {

                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("نوع محموله:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = 40

                });
                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("نام راننده:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });



                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("نوع بسته‌بندی:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });
                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("شماره گواهینامه راننده:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });



                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("وسیله حمل:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });
               
                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("شماره شهربانی:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });



                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("مسیر حرکت:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });

                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("محل بارگیری:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });

                CustomDashedLineSeparator separator = new CustomDashedLineSeparator();
                separator.SetDash(100);
                separator.Gap=7;
                separator.LineWidth=3;
                separator.Percentage = 100f;
                Chunk linebreak = new Chunk(separator);

                
                checkListTb.AddCell(new PdfPCell()
                {
                    FixedHeight = 10 ,
                    Colspan = 2,
                    Border = Rectangle.NO_BORDER

                });
                PdfContentByte cb = pdfWriter.DirectContent;
                cb.SetLineDash(3f, 3f);
                cb.MoveTo(30, pdfDoc.PageSize.Height / 3);
                cb.LineTo(pdfDoc.PageSize.Width - 30, pdfDoc.PageSize.Height / 3);
                cb.Stroke();
                checkListTb.AddCell(new PdfPCell()
                {
                    FixedHeight = 10,
                    Colspan = 2,
                    Border = Rectangle.NO_BORDER

                });
                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("شماره بارنامه:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });

                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("تاریخ صدور بارنامه:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });


                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("مبداء:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });

                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("مقصد:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });


                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("ساعت حرکت:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });

                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("ارزش کالا (طبق اظهار بیمه‌گذار):  "),
                        _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });


                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("آدرس و تلفن راننده:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Colspan = 2,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight

                });


                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("صحت و سلامت محموله تایید می‌گردد /  تایید نمی‌گردد.  "),
                        _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Colspan = 2,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight

                });

                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(FixWeakCharacters("محل درج امضاء:  "), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Colspan = 2,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight

                });


                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(
                        FixWeakCharacters("مراقبت فوق تایید می‌گردد." + Environment.NewLine +
                                          "امضاء فرستنده یا متصدی حمل"), _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });

                checkListTb.AddCell(new PdfPCell()
                {
                    RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                    Phrase = new Phrase(
                        FixWeakCharacters("مراقبت فوق تایید می‌گردد." + Environment.NewLine + "امضاء راننده"),
                        _myFont),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.NO_BORDER,
                    FixedHeight = cellHeight
                });

                #endregion





                pdfDoc.Add(headerTb);
                pdfDoc.Add(matnTb);
                pdfDoc.Add(checkListTb);



                iTextSharp.text.Image wattermarkImg = iTextSharp.text.Image.GetInstance("EppadCo.jpg");
                wattermarkImg.SetAbsolutePosition(pdfDoc.PageSize.Left + 30, pdfDoc.PageSize.Bottom + 30);
                pdfDoc.Add(wattermarkImg);

            }



            Process.Start("Test.pdf");

        }

        

    }
}
