using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Task_1
{
    public partial class Task1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument wsResponseXmlDoc = new XmlDocument();

            //http://api.worldweatheronline.com/premium/v1/weather.ashx?key=****&q=London&format=xml&num_of_days=5
            //id=jipx(spacetime0)
            UriBuilder url = new UriBuilder();
            url.Scheme = "https";// Same as "http: but its secured//"

            url.Host = "api.worldweatheronline.com"; //website
            url.Path = "premium/v1/weather.ashx"; //pathing
            url.Query = "q=china&format=xml&num_of_days=5&key=ba2f060cd70b480a98a00206210701"; //query

            //Make a HTTP request to the global weather web service
            wsResponseXmlDoc = MakeRequest(url.ToString());
            if (wsResponseXmlDoc != null)
            {
                //To display the XML response for user
                String xmlString = wsResponseXmlDoc.InnerXml;
                Response.ContentType = "text/xml";
                Response.Write(xmlString);

                // Save the document to a file and auto-indent the output.
                XmlTextWriter writer = new XmlTextWriter(Server.MapPath("xmlweather.xml"), null);
                writer.Formatting = Formatting.Indented;
                wsResponseXmlDoc.Save(writer);

                writer.Close();
            }
            else
            { //error code for when access is unavailable
                Response.ContentType = "text/html";
                Response.Write("<h2> error accessing web service </h2>");
            }
        }

        public static XmlDocument MakeRequest(string requestUrl) //time out for request
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;


            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}