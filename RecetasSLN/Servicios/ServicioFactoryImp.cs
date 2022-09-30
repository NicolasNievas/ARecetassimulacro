using RecetasSLN.Servicios.Implementaciones;
using RecetasSLN.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.Servicios
{
    class ServicioFactoryImp : AbstractFactoryServicio
    {
        public override IServicio crearServicio()
        {
            return new ServicioReceta();
        }
    }
}
