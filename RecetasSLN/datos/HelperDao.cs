using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RecetasSLN.dominio;

namespace RecetasSLN.datos
{
    public class HelperDao 
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private static HelperDao instancia;

        public HelperDao()
        {
            conn = new SqlConnection(@"Data Source=DESKTOP-5H084HJ\SQLEXPRESS;Initial Catalog=recetas_db;Integrated Security=True");
            cmd = new SqlCommand();
        }

        public static HelperDao ObtenerInstancia() // singleton
        {
            if (instancia == null)
            {
                instancia = new HelperDao();
            }
            return instancia;
        }

        public int ObtenerProximoID(string nombreSP, string nombreParametro)
        {
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = nombreSP;
            SqlParameter pOut = new SqlParameter();
            pOut.ParameterName = nombreParametro;
            pOut.DbType = DbType.Int32;
            pOut.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pOut);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            conn.Close();
            return (int)pOut.Value;
        }

        public DataTable cargarCombo(string nombreSP)
        {
            DataTable dt = new DataTable();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = nombreSP;
            cmd.Parameters.Clear();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }

        public bool grabarReceta(Receta oReceta, string MaestroSP, string DetalleSP)
        {
            bool ok = false;
            SqlTransaction sqlTransaction = null;
            try
            {
                conn.Open();
                sqlTransaction = conn.BeginTransaction();
                SqlCommand comandoMaestro = new SqlCommand(MaestroSP, conn, sqlTransaction);
                comandoMaestro.CommandType = CommandType.StoredProcedure;
                comandoMaestro.Parameters.AddWithValue("@nombre", oReceta.Nombre);
                comandoMaestro.Parameters.AddWithValue("@cheff", oReceta.Chef);
                comandoMaestro.Parameters.AddWithValue("@tipo_receta", oReceta.TipoReceta);
                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "receta_nro";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                comandoMaestro.Parameters.Add(pOut);
                comandoMaestro.ExecuteNonQuery();

                int numeroReceta = (int)pOut.Value;
                SqlCommand comandoDetalle = null;

                foreach (DetalleReceta detalle in oReceta.DetallesRecetas)
                {
                    comandoDetalle = new SqlCommand(DetalleSP, conn, sqlTransaction);
                    comandoMaestro.Parameters.AddWithValue("@id_receta", numeroReceta);
                    comandoMaestro.Parameters.AddWithValue("@id_ingrediente", detalle.Ingrediente.IdIngrediente);
                    comandoMaestro.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                }
                sqlTransaction.Commit();
                ok = true;
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
                ok = false;
            } 
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return ok;
        }
    }
}
