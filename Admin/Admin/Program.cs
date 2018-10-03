﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Admin.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Admin
{
    public class Program

    {
		private readonly static IQuestionServices _questionService;


		
		public static void Main(string[] args)
        {
            try 
            {
                DotNetEnv.Env.Load("./machine_config/machine.env");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine(System.Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4"));
			
			ConceptMapListener conceptMapListener = new ConceptMapListener(_questionService);
			
			conceptMapListener.CreateQuestionConceptMap();

			CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
