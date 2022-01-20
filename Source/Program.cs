using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace XmlSignatureValidation
{
    class Program
    {
        private static string success_signature_assertion_app = ".\\success_signature_assertion_app.xml";
        private static string failed_signature_assertion = ".\\failed_signature_assertion.xml";
        private static string failed_signature_response = ".\\failed_signature_response.xml";

        private static string failed_metadata_encryption_cer = ".\\failed_metadata_encryption.cer";
        private static string failed_metadata_cer = ".\\failed_metadata.cer";
        private static string failed_response_cer = ".\\failed_response.cer";
        private static string success_cer = ".\\success.cer";

        static void Main(string[] args)
        {
            VerifySignedXml(success_signature_assertion_app, success_cer);

            VerifySignedXml(failed_signature_response, failed_response_cer);
            VerifySignedXml(failed_signature_response, failed_metadata_cer);
            VerifySignedXml(failed_signature_response, failed_metadata_encryption_cer);

            VerifySignedXml(failed_signature_assertion, failed_response_cer);
            VerifySignedXml(failed_signature_assertion, failed_metadata_cer);
            VerifySignedXml(failed_signature_assertion, failed_metadata_encryption_cer);
        }

        public static void VerifySignedXml(string responsePath, string certificatePath)
        {
            Console.WriteLine($"File: {Path.GetFileName(responsePath)}, certificate: {Path.GetFileName(certificatePath)}");
            var responseDoc = new XmlDocument();
            responseDoc.Load(responsePath);
            var cert = new X509Certificate2(certificatePath);

            VerifySignedXml(cert, responseDoc);
        }

        private static void VerifySignedXml(X509Certificate2 theirPublicKey, XmlDocument doc)
        {
            XmlElement signature = (XmlElement)doc.GetElementsByTagName("Signature")[0];

            SignedXml sig = new SignedXml(doc);
            sig.LoadXml(signature);

            Console.WriteLine("----------------------");
            var checkWithProvidedCert = sig.CheckSignature(theirPublicKey, true);
            Console.WriteLine($"checkWithProvidedCert: {checkWithProvidedCert}");


            var checkWithKeyInTheSignature = sig.CheckSignature();
            Console.WriteLine($"checkWithKeyInTheSignature: {checkWithKeyInTheSignature}");


            Console.WriteLine("----------------------");
        }
    }
}
