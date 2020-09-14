using Business.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Data.Database
{
    public class UsuarioAdapter:Adapter
    {
        #region DatosEnMemoria
        // Esta regi�n solo se usa en esta etapa donde los datos se mantienen en memoria.
        // Al modificar este proyecto para que acceda a la base de datos esta ser� eliminada
        private static List<Usuario> _Usuarios;

        private static List<Usuario> Usuarios
        {
            get
            {
                if (_Usuarios == null)
                {
                    _Usuarios = new List<Business.Entities.Usuario>();
                    Business.Entities.Usuario usr;
                    usr = new Business.Entities.Usuario();
                    usr.ID = 1;
                    usr.State = Business.Entities.BusinessEntity.States.Unmodified;
                    usr.Nombre = "Casimiro";
                    usr.Apellido = "Cegado";
                    usr.NombreUsuario = "casicegado";
                    usr.Clave = "miro";
                    usr.Email = "casimirocegado@gmail.com";
                    usr.Habilitado = true;
                    _Usuarios.Add(usr);

                    usr = new Business.Entities.Usuario();
                    usr.ID = 2;
                    usr.State = Business.Entities.BusinessEntity.States.Unmodified;
                    usr.Nombre = "Armando Esteban";
                    usr.Apellido = "Quito";
                    usr.NombreUsuario = "aequito";
                    usr.Clave = "carpintero";
                    usr.Email = "armandoquito@gmail.com";
                    usr.Habilitado = true;
                    _Usuarios.Add(usr);

                    usr = new Business.Entities.Usuario();
                    usr.ID = 3;
                    usr.State = Business.Entities.BusinessEntity.States.Unmodified;
                    usr.Nombre = "Alan";
                    usr.Apellido = "Brado";
                    usr.NombreUsuario = "alanbrado";
                    usr.Clave = "abrete sesamo";
                    usr.Email = "alanbrado@gmail.com";
                    usr.Habilitado = true;
                    _Usuarios.Add(usr);

                }
                return _Usuarios;
            }
        }
        #endregion


        public bool ValidarUser(string nomuser , string clave)
        {

            try
            {
                this.OpenConnection();

                SqlCommand cmdUsuarios = new SqlCommand("select * from usuarios where nombre_usuario = @nomuser and clave = @clave", sqlConn);
                cmdUsuarios.Parameters.Add("@nomuser", SqlDbType.VarChar).Value = nomuser;
                cmdUsuarios.Parameters.Add("@clave", SqlDbType.VarChar).Value = clave;

                SqlDataReader drUsuarios = cmdUsuarios.ExecuteReader();

                if (drUsuarios.Read())
                {
                    drUsuarios.Close();
                    return true;
                }
                else
                {
                    drUsuarios.Close();
                    return false;
                }
            }
            catch (Exception Ex)
            {
                Exception ExcepcionManejada =
                    new Exception("Error al validar el usuario", Ex);

                throw ExcepcionManejada;
            }
            finally
            {
                this.CloseConnection();
            }

            

            
        }

        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {            
            this.OpenConnection();

            SqlCommand cmdUsuarios = new SqlCommand("select * from usuarios", sqlConn);

            SqlDataReader drUsuarios = cmdUsuarios.ExecuteReader();

            while (drUsuarios.Read())
            {
                Usuario usr = new Usuario();

                usr.ID = (int)drUsuarios["id_usuario"];
                usr.NombreUsuario = (string)drUsuarios["nombre_usuario"];
                usr.Clave = (string)drUsuarios["clave"];
                usr.Habilitado = (bool)drUsuarios["habilitado"];
                usr.Nombre = (string)drUsuarios["nombre"];
                usr.Apellido = (string)drUsuarios["apellido"];
                usr.Email = (string)drUsuarios["email"];

                usuarios.Add(usr);
            }

            drUsuarios.Close();
            }
            catch (Exception Ex)
            {
                Exception ExcepcionManejada =
                    new Exception("Error al recuperar lista de usuarios", Ex);

                throw ExcepcionManejada;
            }
            finally
            {
                this.CloseConnection();
            }

            return usuarios;
        }

        public Business.Entities.Usuario GetOne(int ID)
        {
            Usuario usr = new Usuario();
            try
            {

            
                this.OpenConnection();
    
                SqlCommand cmdUsuarios = new SqlCommand("select * from usuarios where id_usuario = @id", sqlConn);
                cmdUsuarios.Parameters.Add("@id", SqlDbType.Int).Value = ID;
                SqlDataReader drUsuarios = cmdUsuarios.ExecuteReader();

                if (drUsuarios.Read())
                {
                    usr.ID = (int)drUsuarios["id_usuario"];
                    usr.NombreUsuario = (string)drUsuarios["nombre_usuario"];
                    usr.Clave = (string)drUsuarios["clave"];
                    usr.Habilitado = (bool)drUsuarios["habilitado"];
                    usr.Nombre = (string)drUsuarios["nombre"];
                    usr.Apellido = (string)drUsuarios["apellido"];
                    usr.Email = (string)drUsuarios["email"];
                }
                drUsuarios.Close();
            }
            catch (Exception Ex)
            {
                Exception ExcepcionManejada = new Exception("Error al recuperar datos de usuario", Ex);
                throw ExcepcionManejada;
            }
            finally
            {
                this.CloseConnection();
            }

            return usr;
            //return Usuarios.Find(delegate (Usuario u) { return u.ID == ID; });
        }

        public void Delete(int ID)
        {
            try
            {
                this.OpenConnection();

                SqlCommand cmdDelete =
                    new SqlCommand("delete usuarios where id_usuario = @id", sqlConn);
                cmdDelete.Parameters.Add("@id", SqlDbType.Int).Value = ID;
                cmdDelete.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                Exception ExcepcionManejada =
                    new Exception("Error al eliminar usuario", Ex);
                throw ExcepcionManejada;
            }
            finally
            {
                this.CloseConnection();
            }

            //Usuarios.Remove(this.GetOne(ID));
        }

        protected void Update(Usuario usuario)
        {
            try
            {
                this.OpenConnection();
                //CAPAS QUE HAY ERROR ACA NO ESTA REVISADO
                SqlCommand cmdSave = new SqlCommand(
                    "UPDATE usuarios SET nombre_usuario = @nombre_usuario, clave = @clave, "+
                    "habilitado = @habilitado, nombre = @nombre , apellido = @apellido, email = @email "+
                    "WHERE id_usuario = @id", sqlConn);

                cmdSave.Parameters.Add("@id", SqlDbType.Int).Value = usuario.ID;
                cmdSave.Parameters.Add("@nombre_usuario", SqlDbType.VarChar, 50).Value = usuario.NombreUsuario;
                cmdSave.Parameters.Add("@clave", SqlDbType.VarChar, 50).Value = usuario.Clave;
                cmdSave.Parameters.Add("@habilitado", SqlDbType.Bit).Value = usuario.Habilitado;
                cmdSave.Parameters.Add("@nombre", SqlDbType.VarChar, 50).Value = usuario.Nombre;
                cmdSave.Parameters.Add("@apellido", SqlDbType.VarChar, 50).Value = usuario.Apellido;
                cmdSave.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = usuario.Email;
                cmdSave.ExecuteNonQuery();

            }
            catch (Exception Ex)
            {
                Exception ExcepcionManejada =
                    new Exception("Error al modificar datos del usuario", Ex);
                throw ExcepcionManejada;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        protected void Insert(Usuario usuario)
        {
            try
            {
                this.OpenConnection();
                // PUEDE HABER ERRORES ACA , HAY QUE REVISAR
                SqlCommand cmdSave = new SqlCommand(
                    "insert into usuarios(nombre_usuario,clave,habilitado,nombre,apellido,email)" +
                    "values (@nombre_usuario,@clave,@habilitado,@nombre,@apellido,@email)" +
                    "select @@identity",
                    sqlConn);

                cmdSave.Parameters.Add("@nombre_usuario", SqlDbType.VarChar, 50).Value = usuario.NombreUsuario;
                cmdSave.Parameters.Add("@clave", SqlDbType.VarChar, 50).Value = usuario.Clave;
                cmdSave.Parameters.Add("@habilitado", SqlDbType.Bit).Value = usuario.Habilitado;
                cmdSave.Parameters.Add("@nombre", SqlDbType.VarChar, 50).Value = usuario.Nombre;
                cmdSave.Parameters.Add("@apellido", SqlDbType.VarChar, 50).Value = usuario.Apellido;
                cmdSave.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = usuario.Email;
                usuario.ID = Decimal.ToInt32((decimal)cmdSave.ExecuteScalar());
                //Asi se obtiene el id desde la base de datos
            }
            catch  (Exception Ex)
            {
                Exception ExcepcionManejada =
                    new Exception("Error al crear usuario", Ex);
                throw ExcepcionManejada;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        public void Save(Usuario usuario)
        {
            if (usuario.State == BusinessEntity.States.Deleted )
            {
                this.Delete(usuario.ID);
            }
            else if (usuario.State == BusinessEntity.States.New)
            {
                this.Insert(usuario);
            }
            else if (usuario.State == BusinessEntity.States.Modified)
            {
                this.Update(usuario);
            }
            usuario.State = BusinessEntity.States.Unmodified;

            // VIEJO SAVE
            /*
            if (usuario.State == BusinessEntity.States.New)
            {
                int NextID = 0;
                foreach (Usuario usr in Usuarios)
                {
                    if (usr.ID > NextID)
                    {
                        NextID = usr.ID;
                    }
                }
                usuario.ID = NextID + 1;
                Usuarios.Add(usuario);
            }
            else if (usuario.State == BusinessEntity.States.Deleted)
            {
                this.Delete(usuario.ID);
            }
            else if (usuario.State == BusinessEntity.States.Modified)
            {
                Usuarios[Usuarios.FindIndex(delegate(Usuario u) { return u.ID == usuario.ID; })]=usuario;
            }
            */
        }
    }
}