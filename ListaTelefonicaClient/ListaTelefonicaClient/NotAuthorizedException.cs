using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTelefonicaClient
{
    /// <summary>
    /// Exception para controle do Unauthorized
    /// <!--NÃO ERA MINHA VONTADE, MAS O HTTPCLIENT.HEADER.AUTHORIZATION...-->
    /// </summary>
    public class NotAuthorizedException : Exception { }
}
