// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Net;
UnlockChilkat();
Console.WriteLine("Hello, World!");
Chilkat.Cert cert = new Chilkat.Cert();
cert.LoadFromFile("anto-cns-cert.cer");
var url = "https://fseservicetest.sanita.finanze.it/FseInsServicesWeb/services/fseRecuperoInformativa";
var httpRequest = (HttpWebRequest)WebRequest.Create(url);
httpRequest.Method = "POST";
httpRequest.ContentType = "application/soap+xml";
//httpRequest.Headers.Add(HttpRequestHeader.Authorization, "Basic UFJPVkFYMDBYMDBYMDAwWTpTYWx2ZTEyMw==");
httpRequest.Host = "fseservicetest.sanita.finanze.it";
httpRequest.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
var data = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:rec=""http://recuperoinformativarichiesta.xsd.fse.ini.finanze.it"" xmlns:tip=""http://tipodatirecuperoinformativa.xsd.fse.ini.finanze.it"">
   <soapenv:Header/>
   <soapenv:Body>
      <rec:RecuperoInformativaRichiesta>
         <rec:IdentificativoUtente>PROVAX00X00X000Y</rec:IdentificativoUtente>
         <!--Optional:-->
         <rec:pinCode>LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=</rec:pinCode>
         <rec:IdentificativoOrganizzazione>150</rec:IdentificativoOrganizzazione>
         <!--Optional:-->
         <rec:StrutturaUtente>201123456</rec:StrutturaUtente>
         <!--Optional:-->
         <rec:RuoloUtente>DRS</rec:RuoloUtente>
         <!--Optional:-->
         <rec:TipoAttivita>READ</rec:TipoAttivita>
         <rec:IdentificativoInformativa>150^last</rec:IdentificativoInformativa>
         <!--Zero or more repetitions:-->
         <rec:OpzioniRequest>
            <tip:chiave></tip:chiave>
            <tip:valore></tip:valore>
            <!--Optional:-->
            <tip:tipo>?</tip:tipo>
         </rec:OpzioniRequest>
      </rec:RecuperoInformativaRichiesta>
   </soapenv:Body>
</soapenv:Envelope>";

//https://tools.chilkat.io/xmlCreate.cshtml --> To generate code for chilkat 
Chilkat.Xml xml = new Chilkat.Xml();
xml.Tag = "soapenv:Envelope";
xml.AddAttribute("xmlns:soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
xml.AddAttribute("xmlns:rec", "http://recuperoinformativarichiesta.xsd.fse.ini.finanze.it");
xml.AddAttribute("xmlns:tip", "http://tipodatirecuperoinformativa.xsd.fse.ini.finanze.it");
xml.UpdateChildContent("soapenv:Header", "");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:IdentificativoUtente", "PROVAX00X00X000Y");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:pinCode", "LsQiYtf7FcpMYVKvf+51V6t1BSUk+E/dGOB2vmwNl0DhirZ8QzvTI2Ay04p6+t+eH+DjzkJpXrlEEZvKRz6wKVNOt7uYSQUYKBIFcbcEQJnqT7zTgtz7jV3BK+QaEphfKRsOP1Iejv+vKvJ/3te2xNMHPkNYZIAjxEQHftw9Swk=");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:IdentificativoOrganizzazione", "150");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:StrutturaUtente", "201123456");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:RuoloUtente", "DRS");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:TipoAttivita", "READ");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:IdentificativoInformativa", "150^last");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:OpzioniRequest|tip:chiave", "");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:OpzioniRequest|tip:valore", "");
xml.UpdateChildContent("soapenv:Body|rec:RecuperoInformativaRichiesta|rec:OpzioniRequest|tip:tipo", "?");

Chilkat.XmlDSigGen gen = new Chilkat.XmlDSigGen();

// Indicate where the Signature will be inserted.
//gen.SigLocation = "SOAP-ENV:Envelope|SOAP-ENV:Header|wsse:Security";
gen.SigLocation = "SOAP-ENV:Envelope";

// Add a reference to the fragment of the XML to be signed.

// Note: "Body" refers to the XML element having an "id" equal to "Body", where "id" is case insensitive
// and where any namespace might qualify the attribute.  In this case, the SOAP-ENV:Body fragment is signed
// NOT because the tag = "Body", but because it has SOAP-SEC:id="Body"
gen.AddSameDocRef("Body", "sha1", "EXCL_C14N", "", "");
// (You can read about the SignedInfoPrefixList in the online reference documentation.  It's optional..)
gen.SignedInfoPrefixList = "wsse SOAP-ENV";
// Provide the private key for signing via the certificate, and indicate that
// we want the base64 of the certificate embedded in the KeyInfo.
gen.KeyInfoType = "X509Data";
gen.X509Type = "Certificate";
var success = gen.SetX509Cert(cert, true);
if (success != true)
{
    Debug.WriteLine(gen.LastErrorText);
    Console.ReadLine();
    return;
}
// Everything's specified.  Now create and insert the Signature
Chilkat.StringBuilder sbXml = new Chilkat.StringBuilder();
xml.EmitCompact = false;
xml.GetXmlSb(sbXml);
gen.CreateXmlDSigSb(sbXml);
Console.WriteLine(sbXml.GetAsString());

using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
{
    streamWriter.Write(sbXml.GetAsString());
}

var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
{
    var result = streamReader.ReadToEnd();
    Console.WriteLine(result);
}

Console.WriteLine(httpResponse.StatusCode);
Console.ReadLine();

void UnlockChilkat()
{
    Chilkat.Global glob = new Chilkat.Global();
    bool success = glob.UnlockBundle("Anything for 30-day trial");
    if (success != true)
    {
        Debug.WriteLine(glob.LastErrorText);
        return;
    }

    int status = glob.UnlockStatus;
    if (status == 2)
    {
        Debug.WriteLine("Unlocked using purchased unlock code.");
    }
    else
    {
        Debug.WriteLine("Unlocked in trial mode.");
    }

    // The LastErrorText can be examined in the success case to see if it was unlocked in
    // trial more, or with a purchased unlock code.
    Debug.WriteLine(glob.LastErrorText);
}