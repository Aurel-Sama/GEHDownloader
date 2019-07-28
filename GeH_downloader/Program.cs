using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeH_downloader.GeHentai;
using System.Threading;

namespace GeH_downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Driver driver;
            driver = new Driver();
            try
            {


                while (true)
                {


                    Console.WriteLine("Paste the url of the gallery you want to download(input '1' if you want to quit) :");
                    string url = Console.ReadLine();

                    string[] token = url.Split('/');
                    int zero = 0;
                    int.TryParse(url, out zero);
                    if (zero == 1)
                    { break; }
                    if (token.Length < 7)
                    { Console.WriteLine("invalid url lenght! If url is valid (https://e-hentai.org/g/xxxxx/xxxxx/) ,please contact the developer "); }
                    else
                    {
                        if (!token[0].Trim().Equals("https:"))
                        {
                            Console.WriteLine("protocol error , protocol must be https !");
                        }
                        else
                        {
                            if (!token[2].Trim().Equals("e-hentai.org")&&token[2].Trim()!= "exhentai.org")
                            {
                                Console.WriteLine("wrong site ! pls this Application only work for 'e-hentai.org' !");
                            }
                            else
                            {

                                Gedownload(url, driver);
                            }

                        }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }

        public static void Gedownload(string url, Driver driver)
        {
            
            try
            {
                driver.GoToGe(url);
                driver.gettofirstpage();
                while (driver.Checkifnext())
                {
                    try
                    {
                        if (driver.downloadcurrentimage())
                        {
                         
                          

                        }
                        else
                        {
                            Console.WriteLine("failure :( ");
                        }
                        driver.GotoNextPage();

                        if (driver.downloadcurrentimage())
                        {
                          
                           

                        }
                        else
                        {
                            Console.WriteLine("failure :( ");
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();

                    }
                }
            }
            catch (Exception e)
            { }
        }
    }
}

