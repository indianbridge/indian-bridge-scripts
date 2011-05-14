using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Google.GData.Client;

namespace Upload_To_Google_Sites
{
    class Upload_To_Google_Sites
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage is Upload_To_Google_Sites sitename username password [debug_flag]. Press any key to exit...");
                Console.ReadLine();
                return;
            }
            try
            {
                Boolean debug_flag = false;
                if (args.Length > 3) debug_flag = Boolean.Parse(args[4]);
                SitesAPI sa = new SitesAPI(args[0],args[1],args[2],debug_flag);
                sa.uploadDirectory(@"C:\Temp\bfame_test", "/test");
            }
            catch (GDataRequestException e)
            {
                Console.WriteLine("GdataException : "+e.ResponseString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : "+e.ToString());
            }
            Console.WriteLine("Finished all tasks. Press a key to exit...");
            Console.ReadLine();


        }
    }
}
