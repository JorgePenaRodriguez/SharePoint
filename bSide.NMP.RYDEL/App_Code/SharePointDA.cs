using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace bSide.NMP.RYDEL.App_Code
{
    /// <summary>
    /// Clase que se utiliza para interactuar con la información guardada en las listas de SharePoint
    /// </summary>
    class SharePointDA
    {
        /// <summary>
        /// Obtiene mensaje de la lista "Mensajes" de SharePoint
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns></returns>
        public static string GetMensaje(string titulo)
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;
            string mensaje = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                //RydelLog.LogMessage("Elevando privilegios");
                using (SPSite site = new SPSite(siteId))
                {
                    //RydelLog.LogMessage("Usando site " + site.Url);
                    using (SPWeb web = site.OpenWeb())
                    {
                        site.AllowUnsafeUpdates = true;
                        web.AllowUnsafeUpdates = true;

                        //RydelLog.LogMessage("Usando web " + web.Url + " usuario: " + web.CurrentUser.LoginName);
                        //RydelLog.LogMessage("listas: " + web.Lists.Count.ToString());
                        SPList lst = web.Lists.TryGetList(Constantes.listConfiguracionMensajes.nombrelista);
                        //RydelLog.LogMessage("Utilizando lista " + lst != null? lst.Title : "na");
                        if (lst != null)
                        {
                            SPQuery qry = new SPQuery();
                            qry.Query = "<Where><Eq><FieldRef Name = '" + Constantes.listConfiguracionMensajes.Campos.Titulo + "' /><Value Type='Text'>" + titulo + "</Value></Eq></Where>";
                            qry.RowLimit = 1;
                            var items = lst.GetItems(qry);
                            if (items != null && items.Count > 0)
                            {
                                mensaje = items[0][items[0].Fields.GetFieldByInternalName(Constantes.listConfiguracionMensajes.Campos.Valor).Id].ToString();
                            }
                        }
                    }
                }
            });

            return mensaje;
        }

        /// <summary>
        /// Obtiene los valores de la lista "Configuración Pago" de SharePoint
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetParametrosPago()
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            Dictionary<string, string> dict = new Dictionary<string, string>();

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        SPList lst = web.Lists.TryGetList(Constantes.listConfiguracionPago.nombrelista);

                        if (lst != null)
                        {
                            foreach (SPListItem item in lst.GetItems())
                            {
                                dict.Add(item[item.Fields.GetFieldByInternalName(Constantes.listConfiguracionPago.Campos.Titulo).Id].ToString(),
                                    item[item.Fields.GetFieldByInternalName(Constantes.listConfiguracionPago.Campos.Valor).Id] != null ?
                                    item[item.Fields.GetFieldByInternalName(Constantes.listConfiguracionPago.Campos.Valor).Id].ToString() : string.Empty);
                            }
                        }
                    }
                }
            });

            return dict;
        }

        /// <summary>
        /// Obtiene valor del parámetro, a partir de un diccionario de parámetros de la lista "Configuración Pago"
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static string GetParametro(string key, Dictionary<string, string> parametros)
        {
            string val = string.Empty;
            parametros.TryGetValue(key, out val);
            return val;
        }

        /// <summary>
        /// Obtiene valor del parámetro, a partir del campo llave, de la lista "Configuración Pago"
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetParametro(string key)
        {
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;
            string val = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        SPList lst = web.Lists.TryGetList(Constantes.listConfiguracionPago.nombrelista);

                        if (lst != null)
                        {
                            SPQuery query = new SPQuery();
                            query.Query = "<Where><Eq><FieldRef Name = '" + Constantes.listConfiguracionPago.Campos.Titulo
                                + "' /><Value Type='Text'>" + key + "</Value></Eq></Where>";
                            query.RowLimit = 1;
                            var items = lst.GetItems(query);

                            if (items != null && items.Count == 1 && items[0][Constantes.listConfiguracionPago.Campos.Valor] != null)
                                val = items[0][Constantes.listConfiguracionPago.Campos.Valor].ToString();
                        }
                    }
                }
            });

            return val;
        }

        /// <summary>
        /// Indica si la hora actual se encuentra dentro de los parámetros de configuración de horario de operación
        /// </summary>
        /// <returns></returns>
        public static bool IsHorarioOperacion()
        {
            DateTime horarioInicioOp;
            DateTime horarioFinOp;

            try
            {
                if (DateTime.TryParse(GetParametro(Constantes.listConfiguracionPago.Registros.horarioInicioOp),
                    out horarioInicioOp) &&
                    DateTime.TryParse(GetParametro(Constantes.listConfiguracionPago.Registros.horarioFinOp),
                    out horarioFinOp))
                {
                    return (DateTime.Now.TimeOfDay >= horarioInicioOp.TimeOfDay &&
                        DateTime.Now.TimeOfDay < horarioFinOp.TimeOfDay);
                }
            }
            catch(Exception ex)
            {
                RydelLog.LogError(ex, "Error al validar horario operacional.");
            }
            return true;
        }
    }
}
