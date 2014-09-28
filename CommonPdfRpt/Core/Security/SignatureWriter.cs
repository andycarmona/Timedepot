﻿using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.Security
{
    /// <summary>
    /// CustomPdfReader to be able to work with streams.
    /// </summary>
    public class CustomPdfReader : PdfReader
    {

        /// <summary>
        /// CustomPdfReader to be able to work with streams.
        /// </summary>
        public CustomPdfReader(Stream isp, X509Certificate certificate, ICipherParameters certificateKey)
        {
            this.certificate = certificate;
            this.certificateKey = certificateKey;
            tokens = new PRTokeniser(new RandomAccessFileOrArray(isp));
            ReadPdf();
        }
    }

    /// <summary>
    /// Applies a digital signature to a document
    /// </summary>
    public class SignatureWriter
    {
        #region Fields (2)

        ICipherParameters _asymmetricKeyParameter;
        X509Certificate[] _chain;

        #endregion Fields

        #region Properties (1)

        /// <summary>
        /// Digital signature's info
        /// </summary>
        public Signature SignatureData { set; get; }

        #endregion Properties

        #region Methods (22)

        // Public Methods (6) 

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>
        /// <param name="inputPdfStream">Input/Output pdf file's stream</param>        
        /// <param name="ownerPassword">the password to read the document</param>
        public void SignPdf(Stream inputPdfStream, byte[] ownerPassword)
        {
            validateInputs();
            readCertificate();
            addSignature(inputPdfStream, ownerPassword);
        }

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>
        /// <param name="inputPdfStream">Input/Output pdf file's stream</param>        
        /// <param name="pfxData">The Personal Information Exchange File Info which is used to encrypt the file.</param>
        public void SignPdf(Stream inputPdfStream, PfxData pfxData)
        {
            validateInputs();
            readCertificate();
            addSignature(inputPdfStream, pfxData);
        }

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>        
        /// <param name="inputPdfPath">Input pdf file's path</param>
        /// <param name="outputPdfPath">Output/Signed pdf file's path</param>
        /// <param name="ownerPassword">the password to read the document</param>
        public void SignPdf(string inputPdfPath, string outputPdfPath, byte[] ownerPassword)
        {
            validateInputs();
            readCertificate();
            addSignature(inputPdfPath, outputPdfPath, ownerPassword);
        }

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>
        /// <param name="inputPdfStream">Input pdf file's stream</param>
        /// <param name="outputPdfStream">Output/Signed pdf file's stream</param>
        /// <param name="ownerPassword">the password to read the document</param>
        public void SignPdf(Stream inputPdfStream, Stream outputPdfStream, byte[] ownerPassword)
        {
            validateInputs();
            readCertificate();
            addSignature(inputPdfStream, outputPdfStream, ownerPassword);
        }

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>        
        /// <param name="inputPdfPath">Input pdf file's path</param>
        /// <param name="outputPdfPath">Output/Signed pdf file's path</param>
        /// <param name="pfxData">The Personal Information Exchange File Info which is used to encrypt the file.</param>
        public void SignPdf(string inputPdfPath, string outputPdfPath, PfxData pfxData)
        {
            validateInputs();
            readCertificate();
            addSignature(inputPdfPath, outputPdfPath, pfxData);
        }

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>
        /// <param name="inputPdfStream">Input pdf file's stream</param>
        /// <param name="outputPdfStream">Output/Signed pdf file's stream</param>
        /// <param name="pfxData">The Personal Information Exchange File Info which is used to encrypt the file.</param>
        public void SignPdf(Stream inputPdfStream, Stream outputPdfStream, PfxData pfxData)
        {
            validateInputs();
            readCertificate();
            addSignature(inputPdfStream, outputPdfStream, pfxData);
        }
        // Private Methods (16) 

        private void addSignature(Stream inputPdfStream, byte[] ownerPassword)
        {
            byte[] buffer;
            using (var outputPdfStream = new MemoryStream())
            {
                using (var stamper = PdfStamper.CreateSignature(new PdfReader(inputPdfStream, ownerPassword), outputPdfStream, '\0', null, SignatureData.CertificateFile.AppendSignature))
                {
                    tryAddSignature(stamper);
                }
                buffer = outputPdfStream.GetBuffer();
            }

            inputPdfStream = inputPdfStream.ReopenForWriting();
            using (var signedStream = new MemoryStream(buffer))
            {
                signedStream.WriteTo(inputPdfStream);
            }
        }

        private void addSignature(Stream inputPdfStream, PfxData pfxData)
        {
            byte[] buffer;
            using (var outputPdfStream = new MemoryStream())
            {
                using (var stamper = PdfStamper.CreateSignature(new CustomPdfReader(inputPdfStream, pfxData.X509PrivateKeys[0], pfxData.PublicKey), outputPdfStream, '\0', null, SignatureData.CertificateFile.AppendSignature))
                {
                    tryAddSignature(stamper);
                }
                buffer = outputPdfStream.GetBuffer();
            }

            inputPdfStream.Position = 0;
            inputPdfStream.SetLength(0);
            using (var signedStream = new MemoryStream(buffer))
            {
                signedStream.WriteTo(inputPdfStream);
            }
        }

        private void addSignature(Stream inputPdfStream, Stream outputPdfStream, byte[] ownerPassword)
        {
            using (var stamper = PdfStamper.CreateSignature(new PdfReader(inputPdfStream, ownerPassword), outputPdfStream, '\0', null, SignatureData.CertificateFile.AppendSignature))
            {
                tryAddSignature(stamper);
            }
        }

        private void addSignature(string inputPdfPath, string outputPdfPath, byte[] ownerPassword)
        {
            if (string.IsNullOrEmpty(inputPdfPath) || !File.Exists(inputPdfPath))
                throw new FileNotFoundException("Please specify a valid InputPdfPath");

            if (string.IsNullOrEmpty(outputPdfPath) || !File.Exists(outputPdfPath))
                throw new FileNotFoundException("Please specify a valid OutputPdfPath");

            using (var stream = new FileStream(outputPdfPath, FileMode.Create, FileAccess.Write))
            {
                using (var stamper = PdfStamper.CreateSignature(new PdfReader(inputPdfPath, ownerPassword), stream, '\0', null, SignatureData.CertificateFile.AppendSignature))
                {
                    tryAddSignature(stamper);
                }
            }
        }

        private void addSignature(Stream inputPdfStream, Stream outputPdfStream, PfxData pfxData)
        {
            using (var stamper = PdfStamper.CreateSignature(new CustomPdfReader(inputPdfStream, pfxData.X509PrivateKeys[0], pfxData.PublicKey), outputPdfStream, '\0', null, SignatureData.CertificateFile.AppendSignature))
            {
                tryAddSignature(stamper);
            }
        }

        private void addSignature(string inputPdfPath, string outputPdfPath, PfxData pfxData)
        {
            if (string.IsNullOrEmpty(inputPdfPath) || !File.Exists(inputPdfPath))
                throw new FileNotFoundException("Please specify a valid InputPdfPath");

            if (string.IsNullOrEmpty(outputPdfPath) || !File.Exists(outputPdfPath))
                throw new FileNotFoundException("Please specify a valid OutputPdfPath");

            using (var stream = new FileStream(outputPdfPath, FileMode.Create, FileAccess.Write))
            {
                using (var stamper = PdfStamper.CreateSignature(new PdfReader(inputPdfPath, pfxData.X509PrivateKeys[0], pfxData.PublicKey), stream, '\0', null, SignatureData.CertificateFile.AppendSignature))
                {
                    tryAddSignature(stamper);
                }
            }
        }

        private void addVisibleSignature(PdfSignatureAppearance signAppearance, PdfStamper stamper)
        {
            if (SignatureData.VisibleSignature == null) return;

            signAppearance.Image = string.IsNullOrEmpty(SignatureData.VisibleSignature.ImagePath) ? null : Image.GetInstance(SignatureData.VisibleSignature.ImagePath);
            signAppearance.Layer2Text = SignatureData.VisibleSignature.CustomText;

            if (SignatureData.VisibleSignature.RunDirection == null)
                SignatureData.VisibleSignature.RunDirection = PdfRunDirection.LeftToRight;

            signAppearance.RunDirection = (int)SignatureData.VisibleSignature.RunDirection;
            signAppearance.Layer2Font = SignatureData.VisibleSignature.Font.Fonts[0];
            var pageNumber = SignatureData.VisibleSignature.UseLastPageToShowSignature ? stamper.Reader.NumberOfPages : SignatureData.VisibleSignature.PageNumberToShowSignature;
            signAppearance.SetVisibleSignature(SignatureData.VisibleSignature.Position, pageNumber, null);
        }

        private void readCertificate()
        {
            var certs = PfxReader.ReadCertificate(SignatureData.CertificateFile.PfxPath, SignatureData.CertificateFile.PfxPassword);
            _asymmetricKeyParameter = certs.PublicKey;
            _chain = certs.X509PrivateKeys;
        }

        private PdfSignatureAppearance setSignAppearance(PdfStamper stamper)
        {
            var signAppearance = stamper.SignatureAppearance;
            signAppearance.Reason = SignatureData.SigningInfo.Reason;
            signAppearance.Contact = SignatureData.SigningInfo.Contact;
            signAppearance.Location = SignatureData.SigningInfo.Location;

            addVisibleSignature(signAppearance, stamper);

            var dic = new PdfSignature(PdfName.ADOBE_PPKLITE, new PdfName("adbe.pkcs7.detached"))
                {
                    Reason = signAppearance.Reason,
                    Location = signAppearance.Location,
                    Contact = signAppearance.Contact,
                    Date = new PdfDate(signAppearance.SignDate)
                };
            signAppearance.CryptoDictionary = dic;
            return signAppearance;
        }

        private void signUsingTimeStampAuthority(PdfStamper stamper)
        {
            if (string.IsNullOrEmpty(SignatureData.TsaClient.Url))
                throw new InvalidOperationException("Please specify the TimestampAuthorityClientUrl.");

            var signAppearance = setSignAppearance(stamper);
            addTsa(signAppearance);
        }

        private void addTsa(PdfSignatureAppearance signAppearance)
        {
            var es = new PrivateKeySignature(_asymmetricKeyParameter, "SHA-256");
            var tsc = new TSAClientBouncyCastle(SignatureData.TsaClient.Url, SignatureData.TsaClient.UserName, SignatureData.TsaClient.Password);
            MakeSignature.SignDetached(signAppearance, es, _chain, null, null, tsc, 0, CryptoStandard.CMS);
        }

        private void signWithoutTimeStampAuthority(PdfStamper stamper)
        {
            var signAppearance = stamper.SignatureAppearance;
            signAppearance.Reason = SignatureData.SigningInfo.Reason;
            signAppearance.Contact = SignatureData.SigningInfo.Contact;
            signAppearance.Location = SignatureData.SigningInfo.Location;
            addVisibleSignature(signAppearance, stamper);
            signDetached(signAppearance);
        }

        private void signDetached(PdfSignatureAppearance signAppearance)
        {
            signAppearance.CertificationLevel = PdfSignatureAppearance.CERTIFIED_NO_CHANGES_ALLOWED;
            var es = new PrivateKeySignature(_asymmetricKeyParameter, "SHA-256");
            MakeSignature.SignDetached(signAppearance, es, _chain, null, null, null, 0, CryptoStandard.CMS);
        }

        private void tryAddSignature(PdfStamper stamper)
        {
            if (SignatureData.TsaClient == null)
            {
                signWithoutTimeStampAuthority(stamper);
            }
            else
            {
                signUsingTimeStampAuthority(stamper);
            }
        }

        private void validateInputs()
        {
            if (SignatureData.CertificateFile == null)
                throw new InvalidOperationException("Please specify the CertificateFile");

            if (string.IsNullOrEmpty(SignatureData.CertificateFile.PfxPassword))
                throw new InvalidOperationException("Please specify the PfxPassword");

            if (string.IsNullOrEmpty(SignatureData.CertificateFile.PfxPath) || !File.Exists(SignatureData.CertificateFile.PfxPath))
                throw new FileNotFoundException("Please specify a valid PfxPath");

            if (SignatureData.SigningInfo == null)
                throw new InvalidOperationException("Please specify the SigningInfo");

            if (string.IsNullOrEmpty(SignatureData.SigningInfo.Reason))
                throw new InvalidOperationException("Please specify the SigningReason");

            if (string.IsNullOrEmpty(SignatureData.SigningInfo.Contact))
                throw new InvalidOperationException("Please specify the SigningContact");

            if (string.IsNullOrEmpty(SignatureData.SigningInfo.Location))
                throw new InvalidOperationException("Please specify the SigningLocation");
        }

        #endregion Methods
    }
}
