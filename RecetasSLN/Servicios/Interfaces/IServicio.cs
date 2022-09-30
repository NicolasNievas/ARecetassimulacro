using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.Servicios.Interfaces
{
    public interface IServicio
    {
        int ProximaReceta();
        List<Ingrediente> ObtenerIngredientes();
        bool ConfirmarReceta(Receta nueva);
    }
}
