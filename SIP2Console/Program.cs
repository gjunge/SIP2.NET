// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
using (SIP2.SipConnection con = new SIP2.SipConnection("SOMEDOMAIN", "SOMEPORT", "", ""))
{

    con.Open();

    var foos = con.ItemInformation( "SOMEBARCODE");
}