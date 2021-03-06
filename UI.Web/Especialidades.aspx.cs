﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Entities;
using Business.Logic;

namespace UI.Web
{
    public partial class Especialidades : System.Web.UI.Page
    {        
        #region Atributos

        private Especialidad Entity;

        #endregion

        #region Propiedades

        EspecialidadLogic _logic;

        private EspecialidadLogic Logic
        {
            get
            {
                if (_logic == null)
                {
                    _logic = new EspecialidadLogic();
                }
                return _logic;
            }
        }
        public enum FormModes
        {
            Alta,
            Baja,
            Modificacion
        }

        public FormModes FormMode
        {
            get { return (FormModes)this.ViewState["FormMode"]; }
            set { this.ViewState["FormMode"] = value; }
        }
        private int SelectedID
        {
            get
            {
                if (this.ViewState["SelectedID"] != null)
                {
                    return (int)this.ViewState["SelectedID"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.ViewState["SelectedID"] = value;
            }
        }

        private bool IsEntitySelected
        {
            get
            {
                return (this.SelectedID != 0);
            }
        }

        #endregion

        #region Metodos

        private void LoadGrid()
        {
            this.gridView.DataSource = this.Logic.GetAll();
            this.gridView.DataBind();
        }

        private void LoadForm(int id)
        {
            this.Entity = this.Logic.GetOne(id);
            idTextBox.Text = Entity.ID.ToString();
            this.descripcionTextBox.Text = this.Entity.Descripcion;
        }

        private void LoadEntity(Especialidad especialidad)
        {
            especialidad.Descripcion = descripcionTextBox.Text;           

        }

        private void EnableForm(bool enable)
        {
            descripcionTextBox.Enabled = enable;
            idTextBox.Enabled = enable;
        }

        private void ClearForm()
        {
            descripcionTextBox.Text = string.Empty;
            idTextBox.Text = string.Empty;
        }

        private void DeleteEntity(int id)
        {
            Logic.Delete(id);
        }

        private void SaveEntity(Especialidad especialidad)
        {
            Logic.Save(especialidad);
        }
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGrid();
            }

            Usuario usuario = (Usuario)Session["UsuarioActual"];
            UsuarioLogic usuarioLogic = new UsuarioLogic();
            try
            {

                ModuloUsuario moduloUsuario = usuarioLogic.GetModuloUsuario("Personas", usuario.ID);
                if (moduloUsuario.IDModulo != 0)
                {
                    editarLinkButton.Enabled = moduloUsuario.PermiteModificacion;
                    eliminarLinkButton.Enabled = moduloUsuario.PermiteBaja;
                    nuevoLinkButton.Enabled = moduloUsuario.PermiteAlta;
                    gridView.Enabled = moduloUsuario.PermiteConsulta;
                    if (moduloUsuario.PermiteConsulta)
                    {
                        LoadGrid();
                    }
                }
                else
                {
                    gridPanel.Visible = false;
                    formPanel.Visible = false;
                    gridActionsPanel.Visible = false;
                    Page.Response.Write("Usuario sin permisos");
                }
            }
            catch (Exception ex)
            {
                Exception ExcepcionManejada = new Exception("Error al recuperar Modulo", ex);

                throw ExcepcionManejada;
            }
        }

        protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedID = (int)this.gridView.SelectedValue;
        }

        protected void aceptarLinkButton_Click(object sender, EventArgs e)
        {
            switch (this.FormMode)
            {
                case FormModes.Alta:
                    this.Entity = new Especialidad();
                    this.LoadEntity(this.Entity);
                    this.SaveEntity(this.Entity);
                    this.LoadGrid();
                    break;
                case FormModes.Baja:
                    this.DeleteEntity(this.SelectedID);
                    this.LoadGrid();
                    break;
                case FormModes.Modificacion:
                    this.Entity = new Especialidad();
                    this.Entity.ID = this.SelectedID;
                    this.Entity.State = BusinessEntity.States.Modified;
                    this.LoadEntity(this.Entity);
                    this.SaveEntity(this.Entity);
                    this.LoadGrid();
                    break;
                default:
                    break;
            }
            this.formPanel.Visible = false;
            gridActionsPanel.Visible = true;
        }

        protected void cancelarLinkButton_Click(object sender, EventArgs e)
        {
            formPanel.Visible = false;
            gridActionsPanel.Visible = true;
            LoadGrid();
        }

        protected void editarLinkButton_Click(object sender, EventArgs e)
        {
            if (this.IsEntitySelected)
            {
                gridActionsPanel.Visible = false;
                this.formPanel.Visible = true;
                this.FormMode = FormModes.Modificacion;
                this.LoadForm(this.SelectedID);
            }
        }

        protected void eliminarLinkButton_Click(object sender, EventArgs e)
        {
            if (this.IsEntitySelected)
            {
                gridActionsPanel.Visible = false;
                this.formPanel.Visible = true;
                this.FormMode = FormModes.Baja;
                this.EnableForm(false);
                this.LoadForm(this.SelectedID);
            }
        }

        protected void nuevoLinkButton_Click(object sender, EventArgs e)
        {
            gridActionsPanel.Visible = false;
            this.formPanel.Visible = true;
            this.FormMode = FormModes.Alta;
            this.ClearForm();
            this.EnableForm(true);
        }

        #endregion
    }
}