using RecetasSLN.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.Servicios
{
    abstract class AbstractFactoryServicio 
    {
        public abstract IServicio crearServicio();
    }
}
