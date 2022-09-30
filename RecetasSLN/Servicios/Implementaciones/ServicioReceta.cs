using RecetasSLN.datos.Implementaciones;
using RecetasSLN.datos.Interfaces;
using RecetasSLN.dominio;
using RecetasSLN.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.Servicios.Implementaciones
{
    public class ServicioReceta : IServicio
    {
        private IRecetaDao dao;
        public ServicioReceta()
        {
            dao = new RecetaDao();
        }

        public bool ConfirmarReceta(Receta nueva)
        {
            return dao.Save(nueva);
        }

        public List<Ingrediente> ObtenerIngredientes()
        {
            return dao.ToGetIngrediente();
        }

        public int ProximaReceta()
        {
            return dao.ObtenerProximoID();
        }
    }
}
