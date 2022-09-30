using RecetasSLN.datos.Interfaces;
using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.datos.Implementaciones
{
    public class RecetaDao : IRecetaDao
    {
        public int ObtenerProximoID()
        {
            return HelperDao.ObtenerInstancia().ObtenerProximoID("SP_PROXIMO_ID", "@next");
        }

        public bool Save(Receta nueva)
        {
            return HelperDao.ObtenerInstancia().grabarReceta(nueva, "SP_INSERTAR_RECETA", "SP_INSERTAR_DETALLE");
        }

        public List<Ingrediente> ToGetIngrediente()
        {
            List<Ingrediente> lstIngredientes = new List<Ingrediente>();
            DataTable dt = HelperDao.ObtenerInstancia().cargarCombo("SP_CONSULTAR_INGREDIENTES");
            foreach (DataRow dr in dt.Rows)
            {
                Ingrediente ingrediente = new Ingrediente();
                ingrediente.IdIngrediente = Convert.ToInt32(dr["id_ingrediente"]);
                ingrediente.Nombre = dr["n_ingrediente"].ToString();
                lstIngredientes.Add(ingrediente);
            }
            return lstIngredientes;
        }
    }
}
