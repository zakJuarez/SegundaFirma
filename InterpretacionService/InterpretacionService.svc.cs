using FUJIFILM.InterpretacionesDF.DataAccess;
using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace InterpretacionService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1 : IInterpretacionService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool getValidacionProcedeBloqueo(string NumEstudio, string userID)
        {
            bool valida = false;
            try
            {
                valida = (new InterpretacionDA()).getValidacionProcedeBloqueo(NumEstudio, userID);
            }
            catch (Exception egV)
            {
                throw egV;
            }
            return valida;
        }

        public List<vBuscaEnRevision> getInterpretacionList()
        {
            List<vBuscaEnRevision> _lstRtn = new List<vBuscaEnRevision>();
            try
            {
                _lstRtn = (new InterpretacionDA()).getInterpretacionList();
            }
            catch (Exception)
            {
                _lstRtn = null;
            }
            return _lstRtn;
        }

        public opeUsuario getUsuario(String user)
        {
            opeUsuario _mdl = new opeUsuario();
            try
            {
                _mdl = (new InterpretacionDA()).getUsuario(user);
            }
            catch(Exception)
            {
                _mdl = null;
            }
            return _mdl;
        }
    }
}
