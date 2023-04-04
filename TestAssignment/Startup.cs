using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace TestAssignment
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            HttpClient httpClient = new HttpClient();

            app.Run(async (context) =>
            {
                string path = context.Request.Path;
                
                HttpResponseMessage response = await httpClient.GetAsync($"https://reddit.com{path}");
                
                string body = await response.Content.ReadAsStringAsync();
                
                if (response.Content.Headers.ContentType.MediaType == "text/html")
                {
                    body += @"
<script>
function replaceText(regex, replacement) {
  var walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
  while (walker.nextNode()) {
    var node = walker.currentNode;
    node.textContent = node.textContent.replace(regex, replacement);
  }
}
setInterval(() => replaceText(/\b(?!\w*™)\w{6}\b/g, ""$&™""), 500);
</script>
";
                }
                await context.Response.WriteAsync(body);
            });
        }
    }
}
