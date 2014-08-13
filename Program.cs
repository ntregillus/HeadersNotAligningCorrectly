using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf.Facades;

namespace HeadersNotAligningCorrectly
{
    class Program
    {

        static void Main(string[] args)
        {
            const string pathToTempPdfFile = "..\\..\\App_Data\\7fbcb632-e54c-4ee0-a76b-4dc4b0e8379f_nohdr.pdf";
            var target = new TestImplementationToShowExample();
            if (args.Any())
            {
                Console.WriteLine("attempting to writing new PDF here: " + args[0]);
                target.PathToPdf = args[0];
            }
            else
            {
                Console.WriteLine("please provide path:");
                target.PathToPdf = Console.ReadLine();
                Console.WriteLine("writing file");
            }
           
            target.AddHeaderAndFooter(pathToTempPdfFile);
            Console.WriteLine("completed writing file");

        }

    }

    #region helper classes to spoof our internal domain classes
    internal class Study
    {
        public string VisitLabelSingular
        {
            get { return "Visit"; }
        }
    }

    internal class Subject
    {
        public string FormatMedrioSubjectID(string returnValue)
        {
            return returnValue;
        }
    }
    #endregion

    public class TestImplementationToShowExample
    {
        public string PathToPdf { get; set; }
        #region filler varaibles to make method work
        private const bool FONT_EMBEDED = false;
        private const EncodingType FONT_ENCODING = EncodingType.Identity_h;
        private const string FONT_STYLE = "Arial Unicode MS";
        private const int FONT_SIZE = 8;
        private const string SubjectIdentifier = "0001";
        private const string MedrioSubjectID = "0001";
        private const bool CustomSubjectID = false;
        private const bool IsMedrioIDShown = true;
        private const string SiteName = "Site A";
        private const string VisitName = "VisitName";
        private const int HEADER_LABELWIDTH = 40;
        private const string GroupName = "Group A";
        private const string FormName = "CDASH - Demographics";
        private const string StudyTitle = "Aspose Upgrade";
        private const string TimeZoneFormatForCurrentUser = "Pacific Standard Time";
        private Study Study { get { return new Study(); } }
        private Subject Subject { get { return new Subject(); } }
        private const string ExportDateFormat = "dd-MMM-yyyy";
        #endregion


        /// <summary>
        /// adds the header and footer details to the given pathTotempPdf, and creates the offical pdf location.
        /// </summary>
        public void AddHeaderAndFooter(string pathToTempPdfFile)
        {
            PdfFileStamp fileStamp = null;
            try
            {
                fileStamp = new PdfFileStamp();
                fileStamp.BindPdf(pathToTempPdfFile);
                FormattedText ftSubjectID = null;
                FormattedText ftMedrioID = null;
                System.Drawing.Color fontColor = System.Drawing.Color.Black;

                if (CustomSubjectID)
                {
                    ftSubjectID = new FormattedText(string.Format("Subject Identifier: {0}", SubjectIdentifier)
                                                    , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);
                }

                if (IsMedrioIDShown)
                {
                    ftMedrioID = new FormattedText(string.Format("Medrio ID: {0}", Subject.FormatMedrioSubjectID(MedrioSubjectID))
                        , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);
                }

                FormattedText ftSite = new FormattedText(HeaderItemText("Site: ", SiteName, HEADER_LABELWIDTH)
                        , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);
                FormattedText ftVisit = new FormattedText(HeaderItemText(Study.VisitLabelSingular + ": ", VisitName, HEADER_LABELWIDTH)
                        , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);
                FormattedText ftGroup = new FormattedText(HeaderItemText("Group: ", GroupName, HEADER_LABELWIDTH)
                        , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);
                FormattedText ftForm = new FormattedText(HeaderItemText("Form: ", FormName, HEADER_LABELWIDTH)
                        , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);
                FormattedText ftStudy = new FormattedText(string.Format("{0}", StudyTitle)
                        , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);

                //add header
                int medrioIdTopMargin = 20;
                if (null != ftSubjectID)
                {
                    fileStamp.AddHeader(ftSubjectID, 20, 25, 0);
                    medrioIdTopMargin = 30;
                }
                if (null != ftMedrioID)
                {
                    fileStamp.AddHeader(ftMedrioID, medrioIdTopMargin, 25, 0);
                }
                fileStamp.AddHeader(ftSite, 20, fileStamp.PageWidth / 2, fileStamp.PageWidth / 2);
                fileStamp.AddHeader(ftVisit, 30, fileStamp.PageWidth / 2, fileStamp.PageWidth / 2);
                fileStamp.AddHeader(ftGroup, 20, 0, 50);
                fileStamp.AddHeader(ftForm, 30, 0, 50);

                TimeZoneInfo tzi = null;
                try
                {
                    tzi = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneFormatForCurrentUser);
                }
                catch
                {
                    tzi = TimeZoneInfo.Local;
                }

                DateTime dt = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tzi);

                FormattedText ftDatetime = new FormattedText(string.Format("{0} {1:HH:mm} ({2})", dt.ToString(ExportDateFormat), dt, TimeZoneFormatForCurrentUser)
                        , fontColor, FONT_STYLE, FONT_ENCODING, FONT_EMBEDED, FONT_SIZE);

                // add footer
                fileStamp.AddFooter(ftStudy, 20, fileStamp.PageWidth / 2, fileStamp.PageWidth / 2);
                fileStamp.AddFooter(ftDatetime, 20, 25, 0);
                fileStamp.Save(PathToPdf);
            }
            finally
            {
                if (fileStamp != null)
                {
                    //close
                    fileStamp.Dispose();
                }
            }
        }

        /// <summary>
        /// formats the header item with elipses (...) if the name is too long
        /// </summary>
        private string HeaderItemText(string headerItem, string strToTruncate, int indexToTruncateAt)
        {
            string result = headerItem + strToTruncate;
            if (result.Length >= indexToTruncateAt)
                result = result.Substring(0, indexToTruncateAt) + "...";

            return result;
        }       
    }
}


