using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.datos.Interfaces
{
    public interface IRecetaDao
    {
        int ObtenerProximoID();
        bool Save(Receta nueva);
        List<Ingrediente> ToGetIngrediente();
    }
}
