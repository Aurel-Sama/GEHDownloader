using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Net.Http;
using HtmlAgilityPack;

namespace GeH_downloader.GeHentai
{
	public class Driver
	{
		string galleryurl;
		string gallerynumber;
        HtmlDocument doc = new HtmlDocument();
        string siteheader;
		public Driver()
		{
			
		}
      
        string Credential;

        public void navigate(string url)
        {
            WebRequest req = WebRequest.Create(url);
            //req.Headers["Cookie"] = coockie;
            WebResponse resp = req.GetResponse();
            doc.Load(resp.GetResponseStream());
        }
        public void GoToGe(string ge)
		{
            
			

            WebRequest req = WebRequest.Create(ge);
           // req.Headers["Cookie"] = coockie;
           WebResponse resp =  req.GetResponse();
            doc = new HtmlDocument();
            //time
            doc.Load(resp.GetResponseStream());

                try
                {
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div/h1").Attributes["Text"].Equals("Content Warning"))
                    {
                    WebRequest req2 = WebRequest.Create(doc.DocumentNode.SelectSingleNode("/html/body/div/p[3]/a[1]").Attributes["href"].Value);
                  //  req2.Headers["Cookie"] = coockie;
                    WebResponse resp2 = req.GetResponse();
                    doc.Load(resp2.GetResponseStream());
                      
                    }
                }
                catch (Exception e)
                { }

                finally
                {
                    galleryurl = ge;
                    string[] token = ge.Split('/');
                    gallerynumber = token[4] + "-" + token[5];
                }
		}

		public void gettofirstpage()
		{
            try
            {
               
                string url = doc.GetElementbyId("gdt").Elements("div").FirstOrDefault().Elements("div").FirstOrDefault().Elements("a").FirstOrDefault().Attributes["href"].Value;
                WebRequest req = WebRequest.Create(url);
               // req.Headers["Cookie"] = coockie;
                WebResponse resp = req.GetResponse();
                doc.Load(resp.GetResponseStream());
            }
            catch (Exception e)
            { }
		}
		private string getfoldername()
		{
			string retour = doc.GetElementbyId("i1").Elements("h1").FirstOrDefault().InnerText+"_"+gallerynumber;
			return retour;

		}
		public bool downloadcurrentimage()
		{
			string folder = getfoldername();
            HtmlNode elem = doc.GetElementbyId("img");
            HtmlNode elem2 = doc.GetElementbyId("i2").Elements("div").FirstOrDefault().Elements("div").FirstOrDefault().Elements("span").FirstOrDefault();
			
			string fullpath = elem.Attributes["src"].Value;
			string extention = "."+fullpath.Split('.')[fullpath.Split('.').Length - 1];
			using (WebClient client = new WebClient())
			{
				string path = @".\folders\" +CleanFilename(CleanFoldername((folder ).Trim()))+ "\\";
				
				
			
				Directory.CreateDirectory(path.Trim());
				string fullpathbis = path+CleanFilename(CleanFoldername((elem2.InnerText + extention).Trim()));
				
				client.DownloadFile(new Uri(elem.Attributes["src"].Value),fullpathbis);
				
				return true;
			}
			return false;
		}

		public bool Checkifnext()
		{
			int nopage;
			int totalpage;
			int.TryParse(doc.DocumentNode.SelectSingleNode("//*[@id=\"i2\"]/div[1]/div/span[1]").InnerText, out nopage);
			int.TryParse(doc.DocumentNode.SelectSingleNode("//*[@id=\"i2\"]/div[1]/div/span[2]").InnerText, out totalpage);
			Console.WriteLine("downloading page: " + nopage + "/" + totalpage + "  !");
			if (nopage < totalpage)
			{
				return true;
			}
			else
			{ return false; }

		}

		public bool GotoNextPage()
		{
            navigate(doc.DocumentNode.SelectSingleNode("//*[@id=\"i3\"]/a").Attributes["href"].Value);
            
			
			
			
			return true;
			
		}

		public string CleanFilename(string filename)
		{

			return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));

		}
		public string CleanFoldername(string filename)
		{

			return string.Join("_", filename.Split(Path.GetInvalidPathChars()));

		}

	}
}
