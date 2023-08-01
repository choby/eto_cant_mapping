using Swashbuckle.AspNetCore.SwaggerUI;

namespace Evo.Scm.Swagger;

public static class EndpointCopyable
{
    public static void SwaggerEndpointCopyable(this SwaggerUIOptions options)
    {
        options.HeadContent += @"<script  
        src='https://code.jquery.com/jquery-3.6.1.slim.min.js' 
        integrity='sha256-w8CvhFs7iHNVUtnSP0YKEg00p9Ih13rlL9zGqvLdePA='
        crossorigin='anonymous'></script>
        <script  type='text/javascript'>
            function getUrl(btn){
                let url=$(btn).find('.opblock-summary-path').attr('data-path');
                console.log(url);
                navigator.clipboard.writeText(url);
            }

                    
            function init(){
                $('body').on('dblclick','.opblock-summary-control', function(e){
                         getUrl(this); e.stopPropagation();
                });
            }
                    
            window.addEventListener('load',init,false);
        </script>";
    }
}