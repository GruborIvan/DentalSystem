﻿using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Ordinacija.Features.MedicalReports.Models;
using Ordinacija.Features.Patients.Models;
using System.Diagnostics;

namespace Ordinacija.Features.ReportPrint.Repository.Implementation
{
    public class PdfPrintService : IPdfPrintService
    {
        public PdfPrintService() 
        { 
        
        }

        public void PrintMedicalReport(MedicalReport medicalReport, Patient patient)
        {
            string title = "Nalaz Specijaliste";

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory; // Get the app's directory
            string defaultPdfPath = System.IO.Path.Combine(appDirectory, $"NalazSpecijaliste-{Guid.NewGuid()}.pdf");

            using PdfWriter writer = new PdfWriter(defaultPdfPath);
            using PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Add the title
            Paragraph titleParagraph = new Paragraph(title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SimulateBold();
            document.Add(titleParagraph);

            // Add a horizontal line
            document.Add(new LineSeparator(new SolidLine()));

            // Add some space (2 blank lines)
            document.Add(new Paragraph("\n\n"));

            // Add patient information without table borders
            Table patientInfoTable = new Table(2); // 2 columns
            patientInfoTable.SetWidth(UnitValue.CreatePercentValue(50)); // Set table width to 50% of the page
            patientInfoTable.SetHorizontalAlignment(HorizontalAlignment.CENTER); // Center the table
            patientInfoTable.SetBorder(Border.NO_BORDER); // Remove table borders

            Cell cellStyle = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPadding(0);

            // Add "Ime & Prezime" and its value
            patientInfoTable.AddCell(cellStyle.Clone(true).Add(new Paragraph("Prezime:").SimulateBold()));
            patientInfoTable.AddCell(cellStyle.Clone(true).Add(new Paragraph(patient.LastName)));

            patientInfoTable.AddCell(cellStyle.Clone(true).Add(new Paragraph("Ime:").SimulateBold()));
            patientInfoTable.AddCell(cellStyle.Clone(true).Add(new Paragraph(patient.FirstName)));

            document.Add(patientInfoTable);

            document.Add(new Paragraph("\n\n"));

            // Add the medical report content (left-aligned)
            Paragraph reportContent = new Paragraph(medicalReport.Anamneza)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
            document.Add(reportContent);

            // Add some space (1 blank line)
            document.Add(new Paragraph("\n"));

            // Add additional fields (DG, TH, Kontrola, DateOfReport) with proper alignment
            Paragraph additionalFields = new Paragraph()
                .Add($"DG:        {medicalReport.DG}\n\n") // Add space between fields
                .Add($"TH:        {medicalReport.TH}\n\n")
                .Add($"Kontrola:  {medicalReport.Control}\n\n")
                .Add($"Datum:     {medicalReport.DateOfReport.ToString("dd/MM/yyyy")}\n\n")
                .Add($"{medicalReport.DoctorName}")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
            document.Add(additionalFields);

            // Close the document
            document.Close();

            // Open the PDF with the default program (e.g., Adobe Reader)
            Process.Start(new ProcessStartInfo(defaultPdfPath) { UseShellExecute = true });
        }

        public void PrintUZ(string UZContent)
        {
            string title = "UZ ABDOMENA I BUBREGA";

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string defaultPdfPath = System.IO.Path.Combine(appDirectory, $"UZAbdomenaIBubrega-{Guid.NewGuid()}.pdf");

            using PdfWriter writer = new PdfWriter(defaultPdfPath);
            using PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            // Add the title
            Paragraph titleParagraph = new Paragraph(title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SimulateBold();
            document.Add(titleParagraph);

            // Add a horizontal line
            document.Add(new LineSeparator(new SolidLine()));

            // Add some space (2 blank lines)
            document.Add(new Paragraph("\n"));

            // Add the UZ content (left-aligned)
            Paragraph contentParagraph = new Paragraph(UZContent)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(12);
            document.Add(contentParagraph);

            // Close the document
            document.Close();

            // Open the PDF with the default program (e.g., Adobe Reader)
            Process.Start(new ProcessStartInfo(defaultPdfPath) { UseShellExecute = true });
        }

        public void PrintPreSchoolApproval(string textBody)
        {
            string title = "Poliklinika „Velisavljev“";
            string address = "Novi Sad, J.Boškoviča 6";
            string confirmationTitle = "POTVRDA ZA PREDŠKOLSKU USTANOVU";

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string defaultPdfPath = System.IO.Path.Combine(appDirectory, $"Potvrda-{Guid.NewGuid()}.pdf");

            using PdfWriter writer = new PdfWriter(defaultPdfPath);
            using PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            string fontPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            // Add header
            document.Add(new Paragraph(title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetFont(font)
                .SimulateBold());

            document.Add(new Paragraph(address)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(14)
                .SetFont(font));

            document.Add(new Paragraph("\n\n"));

            document.Add(new Paragraph(confirmationTitle)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetFont(font)
                .SimulateBold());

            document.Add(new Paragraph("\n"));

            // Add the complete body text
            document.Add(new Paragraph(textBody)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(14)
                .SetFont(font));

            document.Close();
            Process.Start(new ProcessStartInfo(defaultPdfPath) { UseShellExecute = true });
        }

        public void PrintDoctorsExemption(string textBody)
        {
            string title = "POLIKLINIKA „VELISAVLJEV“";
            string address = "Novi Sad, Jovana Boškoviča 6";
            string phone = "021/457-417";
            string confirmationTitle = "LEKARSKO OPRAVDANJE";

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string defaultPdfPath = System.IO.Path.Combine(appDirectory, $"LekarskoOpravdanje-{Guid.NewGuid()}.pdf");

            using PdfWriter writer = new PdfWriter(defaultPdfPath);
            using PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            string fontPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            // Add header
            document.Add(new Paragraph(title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20)
                .SetFont(font)
                .SimulateBold());

            document.Add(new Paragraph(address)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(14)
                .SetFont(font));

            document.Add(new Paragraph(phone)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(14)
                .SetFont(font));

            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph(confirmationTitle)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetFont(font)
                .SimulateBold());

            document.Add(new Paragraph("\n"));

            // Add the complete body text
            document.Add(new Paragraph(textBody)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(14)
                .SetFont(font));

            document.Close();
            Process.Start(new ProcessStartInfo(defaultPdfPath) { UseShellExecute = true });
        }

        public void PrintAlergyConfirmation(string textBody)
        {
            string title = "Preporuke za ishranu u vrtićima za decu sa alergijom na hranu";

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string defaultPdfPath = System.IO.Path.Combine(appDirectory, $"Iskljucenje-{Guid.NewGuid()}.pdf");

            using PdfWriter writer = new PdfWriter(defaultPdfPath);
            using PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            string fontPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            // Add header
            document.Add(new Paragraph(title)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16)
                .SetFont(font)
                .SimulateBold());

            document.Add(new Paragraph("\n"));

            // Add the complete body text
            document.Add(new Paragraph(textBody)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(14)
                .SetFont(font));

            document.Close();
            Process.Start(new ProcessStartInfo(defaultPdfPath) { UseShellExecute = true });
        }
    }
}
